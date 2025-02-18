namespace Identity.Shared
{
    /// <summary>
    /// 公共常量
    /// </summary>
    public static class IdentityCommonConst
    {
        #region 用户表类型（1：前台用户 2：后台管理系统用户）
        /// <summary>
        /// 前台用户
        /// </summary>
        public const int UserCategory_Front = 1;
        /// <summary>
        /// 后台管理系统用户
        /// </summary>
        public const int UserCategory_Backend = 2;
        #endregion

        #region 菜单表类型（1：菜单 2：按钮）
        /// <summary>
        /// 菜单
        /// </summary>
        public const int MenuCategory_Menu = 1;
        /// <summary>
        /// 按钮
        /// </summary>
        public const int MenuCategory_Button = 2;
        #endregion

    }
}
