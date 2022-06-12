﻿using SqlSugar;
using XinjingdailyBot.Enums;

namespace XinjingdailyBot.Models
{
    [SugarTable("user", TableDescription = "用户表")]
    [SugarIndex("index_userid", nameof(UserID), OrderByType.Asc, true)]
    [SugarIndex("index_username", nameof(UserName), OrderByType.Asc)]
    internal sealed class Users
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户名@
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        /// <summary>
        /// 是否封禁
        /// </summary>
        public bool IsBan { get; set; }
        /// <summary>
        /// 默认开启匿名模式
        /// </summary>
        public bool PerferAnymouse { get; set; }
        /// <summary>
        /// 通过的稿件数量
        /// </summary>
        public int AcceptCount { get; set; }
        /// <summary>
        /// 被拒绝的稿件数量
        /// </summary>
        public int RejetCount { get; set; }
        /// <summary>
        /// 投稿数量
        /// </summary>
        public int PostCount { get; set; }
        /// <summary>
        /// 审核数量
        /// </summary>
        public int ReviewCount { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public long Experience { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public UserRights Right { get; set; } = UserRights.None;
        /// <summary>
        /// 用户组ID
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyAt { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{UserID} @{UserName} / {FirstName} {LastName}";
        }
    }
}
