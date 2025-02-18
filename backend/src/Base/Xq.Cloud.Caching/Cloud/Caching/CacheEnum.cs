namespace Cloud.Caching
{
    public enum CachingExpireType
    {
        /// <summary>
        /// 永久不变的
        /// </summary>
        OneYear =-1,

        /// <summary>
        /// 永久不变的
        /// </summary>
        Invariable = 0,

        /// <summary>
        /// 稳定数据      
        /// </summary>
        Stable = 1,

        /// <summary>
        /// 相对稳定
        /// </summary>
        RelativelyStable = 2,

        /// <summary>
        /// 常用的单个对象
        /// </summary>

        UsualSingleObject = 3,

        /// <summary>
        /// 常用的对象集合
        /// </summary>
        UsualObjectCollection = 4,

        /// <summary>
        /// 单个对象
        /// </summary>
        SingleObject = 5,

        /// <summary>
        /// 核销码
        /// </summary>
        ObjectCollection = 6,

        /// <summary>
        /// 手机验证码
        /// </summary>
        MobileCodeVerify = 7


    }
}
