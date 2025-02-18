//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Identity
{
    /// <summary>
	/// 用户表
	/// </summary>
    public class User : BaseEntity<long>
    {
        /// <summary>
	    /// 账号
        ///</summary>
        public string Account { get; set; }
        /// <summary>
	    /// 密码
        ///</summary>
        public string Password { get; set; }
        /// <summary>
	    /// 真实姓名
        ///</summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        ///</summary>
        public string NickName { get; set; }

        /// <summary>
	    /// 联系电话
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
	    /// 用户头像
        ///</summary>
        public string Img { get; set; }
        /// <summary>
	    /// 会员积分
	    /// </summary>
        public int MemberPoint { get; set; }
        /// <summary>
	    /// 验证手机状态（1：已验证 2：未验证）
	    /// </summary>
        public int VerifyMobileStatus { get; set; } = 0;
        /// <summary>
        /// 验证手机时间
        /// </summary>
        public DateTime? VerifyMobileTime { get; set; }
        /// <summary>
        /// 上级用户id（转发人）
        /// </summary>
        public long Pid { get; set; }
        /// <summary>
        /// 推荐用户id
        /// </summary>

        public long ReferId { get; set; }

        /// <summary>
        /// ThirdpartId
        ///</summary>
        [MaxLength(200)]
        public string ThirdpartId { get; set; }

        /// <summary>
        /// 用户表类型（1：前台用户 2：后台管理系统用户）
        /// </summary>
        public int UserCategory { get; set; }
        /// <summary>
        /// 是否系统管理员（1是 0 否）
        /// </summary>
        public int IsSystem { get; set; } = 0;
    }
}