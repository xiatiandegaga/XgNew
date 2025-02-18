using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FreeRedis.RedisClient;

namespace Cloud.Caching
{
    public interface ICache
    {
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 验证缓存项是否存在(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 添加或更新缓存
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        void Set(string cacheKey, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true);

        /// <summary>
        ///添加或更新缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        Task SetAsync(string key, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// 删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="lstKey">缓存Key集合</param>
        /// <returns></returns>
        void RemoveAll(IEnumerable<string> lstKey);

        /// <summary>
        /// 批量删除缓存（异步方式）
        /// </summary>
        /// <param name="lstKey">缓存Key集合</param>
        /// <returns></returns>
        Task RemoveAllAsync(IEnumerable<string> lstKey);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="dicKeyValues"></param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        void SetAll(Dictionary<string, object> dicKeyValues, CachingExpireType cachingExpirationType);

        /// <summary>
        /// 批量添加(异步)
        /// </summary>
        /// <param name="dicKeyValues"></param>
        /// <param name="cachingExpirationType"></param>
        /// <returns></returns>
        Task SetAllAsync(Dictionary<string, object> dicKeyValues, CachingExpireType cachingExpirationType);

        long IncrBy(string key, long value);

        Task<long> IncrByAsync(string key, long value);

        /// <summary>
        ///添加缓存NX 不存在才能添加，当成锁
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        bool SetNx(string cacheKey, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true);

        /// <summary>
        ///添加缓存NX 不存在才能添加，当成锁（异步方式）
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        Task<bool> SetNxAsync(string cacheKey, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true);

        bool Expire(string key, CachingExpireType cachingExpirationType);

        Task<bool> ExpireAsync(string key, CachingExpireType cachingExpirationType);

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="name">锁名称</param>
        /// <param name="timeoutSeconds">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时</param>
        /// <returns></returns>
        LockController Lock(string name, int timeoutSeconds, bool autoDelay = true);

        /// <summary>
        ///添加缓存NX 不存在才能添加，当成锁，也可用于限购等功能
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        bool SetNx(string cacheKey, object value, TimeSpan timeSpan, bool isAddRandom = true);

        /// <summary>
        ///添加缓存NX 不存在才能添加，当成锁，也可用于限购等功能（异步方式）
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        Task<bool> SetNxAsync(string cacheKey, object value, TimeSpan timeSpan, bool isAddRandom = true);


        /// <summary>
        ///添加缓存可用于限购等功能
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        void SetFreeTime(string cacheKey, object value, TimeSpan timeSpan, bool isAddRandom = true);

        /// <summary>
        ///添加缓存可用于限购等功能（异步方式）
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        Task SetFreeTimeAsync(string cacheKey, object value, TimeSpan timeSpan, bool isAddRandom = true);

        /// <summary>
        /// 添加set集合（比如视频点赞场景，用视频的id作为key-注意加上前缀，用户id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SAddAsync(string key, long value);

        /// <summary>
        /// 移除set集合（比如视频取消点赞场景，用视频的id作为key-注意加上前缀，用户id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SRemAsync(string key, long value);

        /// <summary>
        /// 获取set集合数量（比如视频点赞场景，获取点赞人数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> SCardAsync(string key);

        /// <summary>
        /// 获取set集合列表（比如视频点赞场景，获取点赞人列表id，慎用）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
         Task<long[]> SMembersAsync(string key);
        /// <summary>
        /// 判断元素是否在集合中（比如视频点赞场景，判断用户是否对视频点赞过）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<bool> SIsMemberAsync(string key, long member);
        /// <summary>
        /// 添加zset集合（比如我的点赞视频历史场景，用我的id作为key-注意加上前缀，视频id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ZAddByTsAsync(string key, string value);

        /// <summary>
        /// 添加zset集合，score作为参数（比如分类视频根据浏览量排序场景，用视频分类的id做key-注意加上前缀，视频id作为值，浏览量作为score作为参数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ZAddBySortAsync(string key, decimal score, string value);
        /// <summary>
        /// 获取元素的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<decimal> ZScoreAsync(string key, string member);
        /// <summary>
        /// 移除zset集合（比如我的点赞视频历史场景-视频取消点赞，用我的id作为key-注意加上前缀，视频id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ZRemAsync(string key, string value);

        /// <summary>
        /// 获取zset集合数量（比我的点赞视频历史场景，获取我的点赞视频数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
      Task<long> ZCardAsync(string key);

        /// <summary>
        /// 获取zset集合列表（比我的点赞视频历史场景，根据时间戳从大到小获取我的点赞视频id）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
       Task<string[]> ZRevRangeAsync(string key, decimal start = 0, decimal stop = -1);

        /// <summary>
        /// 获取zset集合列表（比我的点赞视频历史场景，根据时间戳从小到大获取我的点赞视频id，索引stop-1展示所有，如果分页，索引start=0，索引stop=9，第二页，索引start=10，索引stop=19）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
       Task<string[]> ZRangeAsync(string key, decimal start = 0, decimal stop = -1);

        /// <summary>
        /// zset批量删除
        /// </summary>
        /// <param name="dicKeyValues">删除的key+value</param>
        /// <param name="length">每次管道请求指令数，默认1000</param>
        /// <exception cref="ArgumentNullException"></exception>
        void ZRemAll(Dictionary<string, string> dicKeyValues, int length = 1000);
    }
}
