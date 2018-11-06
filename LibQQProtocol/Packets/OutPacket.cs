using LibQQProtocol.Defines;
using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;
using System.Threading;

namespace LibQQProtocol.Packets {

    public abstract class OutPacket : Packet {
        protected static volatile Int32 OutPacketCommonSeq = 0x3635;

        private Boolean IsInitized = false;

        public MemoryStream BodyStream;
        public BinaryWriter BodyWriter;

        public readonly BinaryWriter PacketWriter;

        public OutPacket(QQAccount account) : base(account) {
            this.PacketWriter = new BinaryWriter(new MemoryStream(this.PacketBuffer));
        }

        public Byte[] EncryptBody(Byte[] buf, Int32 offset, Int32 length, Byte[] key) {
            return QQTea.Encrypt(buf, offset, length, key);
        }

        protected virtual void PutHeader() {
            this.PacketWriter.Write(PacketMagics.PacketBegin);
            this.PacketWriter.Write(this.Account.TXProtocol.CMainVer);
            this.PacketWriter.Write(this.Account.TXProtocol.CSubVer);
            this.PacketWriter.BeWrite((UInt16)this.Command);
            this.PacketWriter.BeWrite(this.Sequence);
            this.PacketWriter.BeWrite(this.Account.QQ);
        }

        protected void SendPACKET_FIX() {
            this.PacketWriter.Write(this.Account.TXProtocol.XxooA);
            this.PacketWriter.Write(this.Account.TXProtocol.DwClientType);
            this.PacketWriter.Write(this.Account.TXProtocol.DwPubNo);
            this.PacketWriter.Write(this.Account.TXProtocol.XxooD);
        }

        protected static Char GetNextSeq() {
            var OldSeq = OutPacketCommonSeq;
            var NewSeq = OutPacketCommonSeq;

            do {
                OldSeq = OutPacketCommonSeq;
                NewSeq = (OutPacketCommonSeq + 1) & 0x00007FFF;
                if (NewSeq == 0) {
                    NewSeq = 1 & 0x00007FFF;
                }
            } while (OldSeq != Interlocked.CompareExchange(ref OutPacketCommonSeq, NewSeq, OldSeq));


            return (Char)OutPacketCommonSeq;
        }

        protected abstract void PutBody();

        protected virtual void PutTail() {
            this.PacketWriter.Write(PacketMagics.PacketEnd);
        }

        public Byte[] WriteData() {
            this.IsInitized = true;

            this.BodyStream = new MemoryStream();
            this.BodyWriter = new BinaryWriter(this.BodyStream);


            var correntPos = (Int32)this.PacketWriter.BaseStream.Position;
            this.PutHeader();
            this.PutBody();
            this.BodyDecrypted = this.BodyStream.ToArray();
            var enc = this.EncryptBody(this.BodyDecrypted, 0, this.BodyDecrypted.Length, this.SecretKey);
            this.PacketWriter.Write(enc);
            this.PutTail();
            this.PostFill(correntPos);

            return this.PacketBuffer;
        }

        public void PostFill(Int32 startPos) {
            if (!this.Account.IsUdp) {
                var len = (Int32)(this.PacketWriter.BaseStream.Length - startPos);
                var currentPos = this.PacketWriter.BaseStream.Position;
                this.PacketWriter.BaseStream.Position = startPos;
                this.PacketWriter.BeWrite((UInt16)len);
                this.PacketWriter.BaseStream.Position = currentPos;
            }
        }

        protected override Byte[] GetPackageData() {
            if (!this.IsInitized) {
                lock (this) {
                    if (!this.IsInitized) {
                        this.WriteData();
                    }
                }
            }

            return this.PacketBuffer;
        }

        protected override Int64 GetPackageDataLength() {
            if (!this.IsInitized) {
                lock (this) {
                    if (!this.IsInitized) {
                        this.WriteData();
                    }
                }
            }

            return this.PacketWriter.BaseStream.Position;
        }
    }
}
