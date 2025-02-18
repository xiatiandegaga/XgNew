using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Input
{
    /// <summary>
    /// 登录请求的用户
    /// </summary>
    public class AdminRequestLoginUserInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }

    public class AdminUserChangePwdInput
    {
        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 用户表
    /// </summary>
    public class AdminUserCreateOrEditInput : BaseEntity<long>
    {
        /// <summary>
	    /// 账号
	    /// </summary>
        public string Account { get; set; }
        /// <summary>
	    /// 密码
	    /// </summary>
        public string Password { get; set; }
        /// <summary>
	    /// 真实姓名
	    /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
	    /// 联系电话
	    /// </summary>
        public string Mobile { get; set; }
        /// <summary>
	    /// 用户头像
	    /// </summary>
        public string Img { get; set; }
        /// <summary>
	    /// 会员积分
	    /// </summary>
        public int MemberPoint { get; set; }
        /// <summary>
	    /// 验证手机状态（1：已验证 2：未验证）
	    /// </summary>
        public int VerifyMobileStatus { get; set; }
        /// <summary>
        /// 验证手机时间
        /// </summary>
        public DateTime? VerifyMobileTime { get; set; }
        /// <summary>
	    /// 创建日期
	    /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserId { get; set; }
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
        /// </summary>
        public string ThirdpartId { get; set; }

        /// <summary>
        /// 用户表类型（1：前台用户 2：后台管理系统用户）
        /// </summary>
        public int UserCategory { get; set; }

        /// <summary>
        /// 角色id集合
        /// </summary>
        public virtual List<long> UserRoleIds { get; set; }
    }
}
