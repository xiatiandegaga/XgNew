namespace Cloud.Caching
{
    public interface ICacheConfig<T> where T : class
    {
        CachingExpireType CachingExpirationType { get; set; }

        string GetCacheKeyOfEntity(object primaryKey);

        string GetListCacheKey();
    }
}
