using LibQQProtocol.Util;
using System;
using System.Collections.Generic;
using System.Net;

namespace LibQQProtocol.Defines {
    public class TXProtocol {
        public Byte[] BufTgtgtKey { get; set; } = ByteHelper.RandomKey();
        public Byte[] BufTgtgt { get; set; }

        public Byte[] BufTgt { get; set; }

        public Byte[] BufComputerId { get; set; } = { 0x43, 0x04, 0x21, 0x7D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public Byte[] BufComputerIdEx { get; set; } = { 0x77, 0x98, 0x00, 0x0B, 0xAB, 0x5D, 0x4F, 0x3D, 0x30, 0x50, 0x65, 0x2C, 0x4A, 0x2A, 0xF8, 0x65 };

        public Byte[] BufDeviceId { get; set; } = { 0x0f, 0xab, 0xbe, 0x21, 0x04, 0xa7, 0x2a, 0xf1, 0xe1, 0x9d, 0xa1, 0x95, 0x6a, 0x36, 0x3d, 0xf0, 0x7b, 0x22, 0xff, 0x2e, 0xc2, 0xca, 0xc9, 0x2b, 0xa8, 0xd6, 0xda, 0x45, 0x9d, 0x31, 0xa9, 0x60 };

        public Byte[] BufSigPic { get; set; }
        public Byte[] PngToken { get; set; }
        public Byte[] PngKey { get; set; }
        public Byte[] BufTgtGtKey { get; set; }
        public Byte[] Buf16BytesGtKeySt { get; set; }
        public Byte[] BufServiceTicket { get; set; }
        public Byte[] Buf16BytesGtKeyStHttp { get; set; }
        public Byte[] BufServiceTicketHttp { get; set; }
        public Byte[] BufGtKeyTgtPwd { get; set; }
        public Byte[] BufSessionKey { get; set; }
        public Byte[] BufSigSession { get; set; }
        public Byte[] BufPwdForConn { get; set; }
        public Byte[] SessionKey { get; set; }
        public Byte[] ClientKey { get; set; }
        public Byte[] BufSigClientAddr { get; set; }

        public Byte[] BufDhPublicKey { get; set; } = { 0x02, 0x78, 0x28, 0x16, 0x7C, 0x9E, 0xF3, 0xB7, 0x5A, 0x7B, 0x5A, 0xEF, 0xA2, 0x30, 0x10, 0xEC, 0x0C, 0x46, 0x87, 0x70, 0x76, 0x31, 0xA7, 0x88, 0xEA };

        public Byte[] BufDhShareKey { get; set; } = { 0x60, 0x42, 0x3B, 0x51, 0xC3, 0xB1, 0xF6, 0x0F, 0x67, 0xE8, 0x9C, 0x00, 0xF0, 0xA7, 0xBD, 0xA3 };

        public Byte[] BufMachineInfoGuid { get; set; }

        public Byte[] BufMacGuid { get; set; } = { 0x21, 0x4B, 0x1A, 0x04, 0x09, 0xED, 0x19, 0x70, 0x98, 0x75, 0x51, 0xBB, 0x2D, 0x3A, 0x7E, 0x0A };


        public Byte BRememberPwdLogin { get; set; } = 0x00;

        public Byte CPingType { get; set; } = 0x01;

        public List<Byte[]> RedirectIP { get; set; } = new List<Byte[]>();

        public String BufComputerName { get; set; } = Dns.GetHostName();

        public Byte CMainVer = 0x37;

        public Byte CSubVer = 0x09;

        public Byte[] XxooA = { 0x03, 0x00, 0x00 };

        public Byte[] XxooD = { 0x30, 0x00, 0x00, 0x00 };
        public Byte XxooB = 0x01;

        public Byte[] DwClientType = { 0x00, 0x01, 0x01, 0x01 };

        public Byte[] DwPubNo = { 0x00, 0x00, 0x68, 0x1C };

        public UInt16 CQdProtocolVer = 0x0063;
        public Int64 DwQdVerion = 0x02040404;
        public UInt16 WQdCsCmdNo = 0x0004;
        public Byte CQdCcSubNo = 0x00;

        internal Byte COsType = 0x03;

        internal Byte BIsWow64 = 0x01;

        public Int64 DwDrvVersionInfo = 0x01020000;

        public Byte[] BufVersionTsSafeEditDat = { 0x07, 0xdf, 0x00, 0x0a, 0x00, 0x0c, 0x00, 0x01 };

        public Byte[] BufVersionQScanEngineDll = { 0x00, 0x04, 0x00, 0x03, 0x00, 0x04, 0x20, 0x5c };

        public Byte[] QdSufFix = { 0x68 };
        public Byte[] QdPreFix = { 0x3E };


        public Byte[] BufQdKey = { 0x77, 0x45, 0x37, 0x5e, 0x33, 0x69, 0x6d, 0x67, 0x23, 0x69, 0x29, 0x25, 0x68, 0x31, 0x32, 0x5d };


        public UInt32 DwSsoVersion { get; set; } = 0x00000453;

        public UInt32 DwServiceId { get; set; } = 0x00000001;

        public UInt32 DwClientVer { get; set; } = 0x00001585;

        public UInt32 DwIsp { get; set; }
        public UInt32 DwIdc { get; set; }
        public Int64 TimeDifference { get; set; }

        public Byte[] BufSid { get; set; } = { 0x1E, 0xC1, 0x25, 0x71, 0xB2, 0x4C, 0xEA, 0x91, 0x9A, 0x6E, 0x8D, 0xE6, 0x95, 0x4E, 0xCE, 0x06 };

        public Byte[] QqexeMD5 { get; set; }

        public UInt16 WClientPort { get; set; }
        public String DwClientIP { get; set; }
        public DateTime DwServerTime { get; set; }

        public UInt16 WRedirectCount { get; set; }

        public String DwServerIP { get; set; } = NetworkHelper.GetRandomServerAsString();
        public String DwRedirectIP { get; set; }
        public UInt16 WRedirectPort { get; set; }
        public UInt16 WServerPort { get; set; } = 8000;
        public UInt16 SubVer { get; set; } = 0x0001;
        public UInt16 EcdhVer { get; set; } = 0x0102;
        public Byte[] QdData { get; set; }
    }
}
