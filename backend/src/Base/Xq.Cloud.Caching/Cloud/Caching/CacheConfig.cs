using Cloud.Utilities;

namespace Cloud.Caching
{
    public class CacheConfig<T> : ICacheConfig<T> where T : class
    {


        public CacheConfig()
        {
            TypeHashID = EncryptionUtility.MD5_16(typeof(T).FullName);
            CachingExpirationType = CachingExpireType.Invariable;
        }

        public CacheConfig(CachingExpireType cachingExpirationType) : this()
        {
            CachingExpirationType = cachingExpirationType;
        }


        /// <summary>
        /// 完整名称md5-16
        /// </summary>
        public string TypeHashID { get; private set; }

        /// <summary>
        /// 缓存过期类型
        /// </summary>
        public CachingExpireType CachingExpirationType { get; set; }


        #region GetCacheKey

        /// <summary>
        /// 获取实体缓存的cacheKey
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>实体的CacheKey</returns>
        public string GetCacheKeyOfEntity(object primaryKey) => $"ClientCache:{TypeHashID}:{primaryKey}";


        /// <summary>
        /// 获取实体List缓存CacheKey
        /// </summary>
        /// <returns></returns>
        public string GetListCacheKey() => $"ClientCache:List:{TypeHashID}";

        #endregion
    }
}
