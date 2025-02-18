using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// 
    ///</summary>
    public class UserConstInfoDto : BaseEntity<long>
    {
        /// <summary>
        /// 用户收货地址 
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        /// 用户邮箱 
        ///</summary>
        public string Email { get; set; }
        /// <summary>
        /// 性别 1为男 2为女 0未知 
        ///</summary>
        public int Sex { get; set; }
        /// <summary>
        /// 生日 
        ///</summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 用户表id 
        ///</summary>
        public long? UserId { get; set; }
    }
}
