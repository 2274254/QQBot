namespace LibQQProtocol.Defines {
    public enum MessageType {
        /// <summary>
        ///     普通文本
        /// </summary>
        Normal,

        /// <summary>
        ///     @他人
        /// </summary>
        At,

        /// <summary>
        ///     系统表情
        /// </summary>
        Emoji,

        /// <summary>
        ///     图片消息
        /// </summary>
        Picture,

        /// <summary>
        ///     Xml消息
        /// </summary>
        Xml,

        /// <summary>
        ///     Json消息
        /// </summary>
        Json,

        /// <summary>
        ///     抖动
        /// </summary>
        Shake,

        /// <summary>
        ///     音频
        /// </summary>
        Audio,

        /// <summary>
        ///     视频
        /// </summary>
        Video,

        /// <summary>
        ///     发送离线文件
        /// </summary>
        OfflineFile,

        /// <summary>
        ///     退群
        /// </summary>
        ExitGroup,

        /// <summary>
        ///     获取群信息
        /// </summary>
        GetGroupImformation,

        /// <summary>
        ///     加群
        /// </summary>
        AddGroup
    }
}
