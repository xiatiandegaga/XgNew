//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using Cloud.Domain.Entities;

namespace Domain.Entity.Identity
{
    /// <summary>
	/// sys_userRole
	/// </summary>
    public class UserRole : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }

    }
}