namespace Domain.Enum
{
    /// <summary>
    /// 目标类型（直接给用户还是角色）
    /// </summary>
    public enum TargetType
    {
        User,
        Role
    }

    /// <summary>
    /// 权限类型（目前只有菜单）
    /// </summary>
    public enum PermissionType
    {
        Menu
    }

    /// <summary>
    /// 数据字典类型
    /// </summary>
    public enum GlobalDataType
    {
        /// <summary>
        /// 积分规则
        /// </summary>
        PointRule,
    }
}
