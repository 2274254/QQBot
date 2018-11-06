using System;

namespace LibQQProtocol.TlvLib.Tlvs {
    public class TlvTagAttribute : Attribute {
        public TlvTagAttribute(TlvTags tag) {
            this.Tag = tag;
        }

        public TlvTags Tag { get; }
    }
}