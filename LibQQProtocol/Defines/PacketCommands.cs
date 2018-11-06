using System;

namespace LibQQProtocol.Defines {
    public enum PacketCommands : ushort {
        /// <summary>
        ///     保持在线状态
        /// </summary>
        Message0X0002 = 0x0002,

        /// <summary>
        ///     登录Ping
        /// </summary>
        Login0X0825 = 0x0825,

        /// <summary>
        ///     登录校验
        /// </summary>
        Login0X0836 = 0x0836,
        Login0X0839 = 0x0839,

        /// <summary>
        ///     取SessionKey
        /// </summary>
        Login0X0828 = 0x0828,

        /// <summary>
        ///     改变在线状态
        /// </summary>
        Login0X00Ec = 0x00EC,

        /// <summary>
        ///     Token请求
        /// </summary>
        Interactive0X00Ae = 0x00AE,

        /// <summary>
        ///     验证码提交
        /// </summary>
        Login0X00Ba = 0x00BA,

        /// <summary>
        ///     请求一些操作需要的密钥，比如文件中转，视频也有可能  目前用来获取Skey
        /// </summary>
        Data0X001D = 0x001D,

        /// <summary>
        ///     获取基本资料
        /// </summary>
        Data0X005C = 0x005C,

        /// <summary>
        ///     获取群分组
        /// </summary>
        Data0X0195 = 0x0195,

        /// <summary>
        ///     查询黑名单
        /// </summary>
        Data0X01A5 = 0x01A5,
        Data0X019B = 0x019B,

        /// <summary>
        ///     获取好友和群列表
        /// </summary>
        Data0X0134 = 0x0134,
        Data0X01C4 = 0x01C4,
        Data0X01C5 = 0x01C5,
        Data0X0126 = 0x0126,

        /// <summary>
        ///     天气预报
        /// </summary>
        Data0X00A6 = 0x00A6,

        /// <summary>
        ///     PM2.5浓度
        /// </summary>
        Data0X0397 = 0x0397,

        /// <summary>
        ///     问问个人中心API地址
        /// </summary>
        Data0X00D8 = 0x00D8,

        /// <summary>
        ///     群消息
        /// </summary>
        Message0X0017 = 0x0017,

        /// <summary>
        ///     群消息查看确认
        /// </summary>
        Message0X0360 = 0x0360,
        Message0X01C0 = 0x01C0,

        /// <summary>
        ///     好友消息
        /// </summary>
        Message0X00Ce = 0x00CE,

        /// <summary>
        ///     消息查看确认
        /// </summary>
        Message0X0319 = 0x0319,

        /// <summary>
        ///     发送好友消息
        /// </summary>
        Message0X00Cd = 0x00CD,

        /// <summary>
        ///     获取Ukey
        /// </summary>
        Message0X0352 = 0x0352,

        /// <summary>
        ///     获取Ukey
        /// </summary>
        Message0X0388 = 0x0388,

        /// <summary>
        ///     心跳包
        /// </summary>
        Message0X0058 = 0x0058,

        /// <summary>
        ///     点赞
        /// </summary>
        Interactive0X03E3 = 0x03E3,

        /// <summary>
        ///     未知包
        /// </summary>
        Unknown = 0xFFFF
    }
}
