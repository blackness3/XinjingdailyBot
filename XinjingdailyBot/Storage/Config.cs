﻿namespace XinjingdailyBot.Storage
{
    public class Config
    {
        /// <summary>
        /// 调试模式
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// 机器人Token
        /// </summary>
        public string BotToken { get; set; } = "";

        /// <summary>
        /// Start命令显示的欢迎语
        /// </summary>
        public string Welecome { get; set; } = "欢迎使用 心惊报 @xinjingdaily 专用投稿机器人";

        /// <summary>
        /// 代理链接
        /// </summary>
        public string? Proxy { get; set; }

        /// <summary>
        /// MySQL连接设定
        /// </summary>
        public string DBHost { get; set; } = "127.0.0.1";
        public uint DBPort { get; set; } = 3306;
        public string DBName { get; set; } = "xjb_db";
        public string DBUser { get; set; } = "root";
        public string DBPassword { get; set; } = "123456";
        /// <summary>
        /// 是否生成数据库字段(数据库结构变动时需要打开)
        /// </summary>
        public bool DBGenerate { get; set; } = true;

        /// <summary>
        /// 超级管理员(覆盖数据库配置)
        /// </summary>
        public HashSet<long> SuperAdmins { get; set; } = new();

        /// <summary>
        /// 审核群组
        /// </summary>
        public string ReviewGroup { get; set; } = "";

        /// <summary>
        /// 频道评论区群组
        /// </summary>
        public string CommentGroup { get; set; } = "";

        /// <summary>
        /// 闲聊区群组
        /// </summary>
        public string SubGroup { get; set; } = "";

        /// <summary>
        /// 通过频道
        /// </summary>
        public string AcceptChannel { get; set; } = "";

        /// <summary>
        /// 拒稿频道
        /// </summary>
        public string RejectChannel { get; set; } = "";

        /// <summary>
        /// 自动退出未在配置文件中定义的群组和频道
        /// </summary>
        public bool AutoLeaveOtherGroup { get; set; } = false;
    }
}
