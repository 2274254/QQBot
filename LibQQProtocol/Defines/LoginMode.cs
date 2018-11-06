using System;
using System.Collections.Generic;
using System.Text;

namespace LibQQProtocol.Defines {
    public enum LoginMode : byte {
        /// <summary>
        ///     正常
        /// </summary>
        Normal = 0x0A,

        /// <summary>
        ///     隐身
        /// </summary>
        Hidden = 0x28
    }
}
