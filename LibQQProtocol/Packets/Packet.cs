using LibAsyncUDP.Interfaces;
using LibQQProtocol.Defines;
using LibQQProtocol.Structs;
using System;

namespace LibQQProtocol.Packets {
    public class Packet : IPackage {
        public Byte[] PacketBuffer { get; protected set; } = new Byte[PacketDefines.QQPacketMaxSize];

        public QQAccount Account { get; private set; }

        public PacketCommands Command { get; protected set; }

        public Char Sequence { get; protected set; }

        public Byte[] BodyDecrypted { get; protected set; }

        public Byte[] BodyEcrypted { get; protected set; }

        public Byte[] SecretKey { get; protected set; }

        public Byte Header { get; protected set; }

        public Char Version { get; protected set; }

        public DateTime PacketTime { get; protected set; }

        public Byte[] PackageData => this.GetPackageData();

        public Int64 DataLength => this.GetPackageDataLength();

        protected virtual Byte[] GetPackageData() {
            return null;
        }

        protected virtual Int64 GetPackageDataLength() {
            return -1;
        }


        protected virtual Int32 GetCryptographStart() {
            return -1;
        }

        public Packet() {
            this.PacketTime = DateTime.Now;
        }

        protected Packet(QQAccount account) {
            this.Account = account;
        }

        public Packet(Byte[] byteBuffer, QQAccount user) : this(user) {
            this.PacketBuffer = byteBuffer;
        }

        public override Boolean Equals(Object obj) {
            if (obj is Packet packet) {
                return this.Header == packet.Header && this.Command == packet.Command && this.Sequence == packet.Sequence;
            }

            return base.Equals(obj);
        }

        public override Int32 GetHashCode() {
            return Hash(this.Sequence, this.Command);
        }

        public static Int32 Hash(Char sequence, PacketCommands command) {
            return (sequence << 16) | (UInt16)command;
        }
    }
}
