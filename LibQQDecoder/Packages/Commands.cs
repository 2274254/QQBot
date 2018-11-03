using System;
using System.Collections.Generic;
using System.Text;

namespace LibQQDecoder.Packages {
    public static class Commands {
        public static readonly Byte[] Touch = { 0x08, 0x25, 0x31, 0x01 };
        public static readonly Byte[] Redirect = { 0x08, 0x25, 0x31, 0x02 };
    }
}
