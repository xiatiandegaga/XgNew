using Cloud.Domain.Entities;

namespace Identity.Shared.Dto.Admin.General
{
    public class UserRoleDto : BaseEntity<long>
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
