using LibQQProtocol.Defines;
using LibQQProtocol.Structs;
using LibQQProtocol.TlvLib.Tlvs;
using System;

namespace LibQQProtocol.Packets.Out.Login {
    public class ServerTouchPacket : OutPacket {
        public ServerTouchPacket(QQAccount user, Boolean redirect) : base(user) {
            this.Sequence = GetNextSeq();
            this.Redirect = redirect;
            this.SecretKey = !redirect ? this.Account.QQPacket0825Key : this.Account.QQPacketRedirectionkey;
            this.Command = PacketCommands.Login0X0825;

        }

        private Boolean Redirect { get; }

        protected override void PutHeader() {
            base.PutHeader();
            this.SendPACKET_FIX();
            this.PacketWriter.Write(this.SecretKey);
        }

        protected override void PutBody() {
            this.BodyWriter.Write(new TLV0018().Get_Tlv(this.Account));
            this.BodyWriter.Write(new TLV0309().Get_Tlv(this.Account));
            this.BodyWriter.Write(new TLV0036().Get_Tlv(this.Account));
            this.BodyWriter.Write(new TLV0114().Get_Tlv(this.Account));
        }
    }
}
