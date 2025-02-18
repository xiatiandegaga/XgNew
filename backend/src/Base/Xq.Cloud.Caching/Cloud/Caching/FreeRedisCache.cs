using Cloud.Extensions;
using Cloud.Utilities.Json;
using FreeRedis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FreeRedis.RedisClient;

namespace Cloud.Caching
{
    public class FreeRedisCache : ICache
    {
        private readonly Random _random;
        private readonly Dictionary<CachingExpireType, TimeSpan> cachingExpireTypeDictionary;
        private readonly IConfiguration _configuration;
        private readonly RedisClient _redisClient;
        public FreeRedisCache(IConfiguration configuration)
        {
            _random = new Random();
            cachingExpireTypeDictionary = new Dictionary<CachingExpireType, TimeSpan>
            {
                { CachingExpireType.OneYear, new TimeSpan(0, 0,(12 *30 * 24 * 60 * 60)) },
                { CachingExpireType.Invariable, new TimeSpan(0, 0,(3 *30 * 24 * 60 * 60)) },
                { CachingExpireType.Stable, new TimeSpan(0, 0,(24 * 60 * 60)) },
                { CachingExpireType.RelativelyStable, new TimeSpan(0, 0,(12 * 60 * 60)) },
                { CachingExpireType.UsualSingleObject, new TimeSpan(0, 0,(8 * 60 * 60)) },
                { CachingExpireType.UsualObjectCollection, new TimeSpan(0, 0,(6 * 60 * 60)) },
                { CachingExpireType.SingleObject, new TimeSpan(0, 0, (1 * 60 * 60)) },
                { CachingExpireType.MobileCodeVerify, new TimeSpan(0, 0,(10 * 60)) },
                { CachingExpireType.ObjectCollection, new TimeSpan(0, 0,(3 * 60)) }
            };
            _configuration = configuration;
            _redisClient = new RedisClient(_configuration["ConnectionStrings:Redis"]);
            //_redisClient.Notice += (s, e) => Console.WriteLine(e.Log); //print command log
            //_redisClient.UseClientSideCaching(new ClientSideCachingOptions
            //{

            //    Capacity = 0,
            //    //Filtering rules, which specify which keys can be cached locally
            //    KeyFilter = key => key.Contains("ClientCache:"),
            //    //Check long-term unused cache
            //    CheckExpired = (key, dt) => DateTime.Now.Subtract(dt) > TimeSpan.FromSeconds(3600)
            //});
        }

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _redisClient.Exists(key);
        }

        /// <summary>
        /// 验证缓存项是否存在(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            return await _redisClient.ExistsAsync(key);
        }

        /// <summary>
        ///添或更新缓存
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        public void Set(string cacheKey, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true)
        {
            Set(cacheKey, value, cachingExpireTypeDictionary[cachingExpirationType], isAddRandom);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="cacheKey">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="cachingExpirationType">缓存期限类型</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        public async Task SetAsync(string cacheKey, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true)
        {
             await SetAsync(cacheKey, value, cachingExpireTypeDictionary[cachingExpirationType], isAddRandom);
        }


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="timeSpan">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        private void Set(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                 _redisClient.Set(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                 _redisClient.Set(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="timeSpan">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        private async Task SetAsync(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                 await _redisClient.SetAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                 await _redisClient.SetAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _redisClient.Del(key);

            return !Exists(key);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            await _redisClient.DelAsync(key);
            return !await ExistsAsync(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="lstKey">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> lstKey)
        {
            if (lstKey == null)
            {
                throw new ArgumentNullException(nameof(lstKey));
            }
            lstKey.ToList().ForEach(item => Remove(item));
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="lstKey">缓存Key集合</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> lstKey)
        {
            if (lstKey == null)
            {
                throw new ArgumentNullException(nameof(lstKey));
            }
            await Task.Run(() => lstKey.ToList().ForEach(item => _redisClient.DelAsync(item)));
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheT = _redisClient.Get<byte[]>(key);
            if (cacheT == null)
            {
                return default;
            }
            return JsonUtility.DeserializeByte<T>(cacheT);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var cacheT = await _redisClient.GetAsync<byte[]>(key);
            if (cacheT == null)
            {
                return default;
            }
            return JsonUtility.DeserializeByte<T>(cacheT);

        }

        public void SetAll(Dictionary<string, object> dicKeyValues, CachingExpireType cachingExpirationType)
        {
            SetAll(dicKeyValues, cachingExpireTypeDictionary[cachingExpirationType]);
        }

        public async Task SetAllAsync(Dictionary<string, object> dicKeyValues, CachingExpireType cachingExpirationType)
        {
            await SetAllAsync(dicKeyValues, cachingExpireTypeDictionary[cachingExpirationType]);
        }

        private void SetAll(Dictionary<string, object> dicKeyValues, TimeSpan timeSpan)
        {
            if (dicKeyValues == null)
            {
                throw new ArgumentNullException(nameof(dicKeyValues));
            }
            var pipe = _redisClient.StartPipe();
            foreach (KeyValuePair<string, object> kvp in dicKeyValues)
            {
                pipe.Set(kvp.Key, JsonUtility.SerializeByte(kvp.Value), timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).Seconds);
            }
            pipe.EndPipe();
        }

        private async Task SetAllAsync(Dictionary<string, object> dicKeyValues, TimeSpan timeSpan)
        {
            if (dicKeyValues == null)
            {
                throw new ArgumentNullException(nameof(dicKeyValues));
            }
            var pipe = _redisClient.StartPipe();
            await Task.Run(() =>
            {
                foreach (KeyValuePair<string, object> kvp in dicKeyValues)
                {
                    pipe.Set(kvp.Key, JsonUtility.SerializeByte(kvp.Value), timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).Seconds);
                }
                pipe.EndPipe();
            });
        }

        public long IncrBy(string key, long value)
        {
            return _redisClient.IncrBy(key, value);
        }

        public async Task<long> IncrByAsync(string key, long value)
        {
            return await _redisClient.IncrByAsync(key, value);
        }

        /// <summary>
        /// 添加缓存NX 不存在才能添加，当成锁
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public bool SetNx(string key, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var timeSpan = cachingExpireTypeDictionary[cachingExpirationType];
            if (isAddRandom)
                return _redisClient.SetNx(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                return _redisClient.SetNx(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 添加缓存NX 不存在才能添加，当成锁（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync(string key, object value, CachingExpireType cachingExpirationType, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var timeSpan = cachingExpireTypeDictionary[cachingExpirationType];
            if (isAddRandom)
                return await _redisClient.SetNxAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                return await _redisClient.SetNxAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        public bool Expire(string key, CachingExpireType cachingExpirationType)
        {
            var timeSpan = cachingExpireTypeDictionary[cachingExpirationType];
            return _redisClient.Expire(key, timeSpan);
        }

        public async Task<bool> ExpireAsync(string key, CachingExpireType cachingExpirationType)
        {
            var timeSpan = cachingExpireTypeDictionary[cachingExpirationType];
            return await _redisClient.ExpireAsync(key, Convert.ToInt32(timeSpan.TotalSeconds));
        }

        public LockController Lock(string name, int timeoutSeconds, bool autoDelay = true) => _redisClient.Lock(name, timeoutSeconds, autoDelay);

        /// <summary>
        /// 添加缓存NX 不存在才能添加，当成锁，也可用于限购等功能
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public bool SetNx(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                return _redisClient.SetNx(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                return _redisClient.SetNx(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 添加缓存NX 不存在才能添加，当成锁，也可用于限购等功能（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                return await _redisClient.SetNxAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                return await _redisClient.SetNxAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        ///自由设置过期时间（商品限购场景）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public void SetFreeTime(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                 _redisClient.Set(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                 _redisClient.Set(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 自由设置过期时间异步方式（商品限购场景）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="cachingExpirationType">缓存失效时间</param>
        /// <param name="isAddRandom">是否增加缓存随机时间</param>
        /// <returns></returns>
        public async Task SetFreeTimeAsync(string key, object value, TimeSpan timeSpan, bool isAddRandom = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isAddRandom)
                 await _redisClient.SetAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.Add(new TimeSpan(0, 0, _random.Next(1, 600))).TotalSeconds);
            else
                 await _redisClient.SetAsync(key, JsonUtility.SerializeByte(value), (int)timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 添加set集合（比如视频点赞场景，用视频的id作为key-注意加上前缀，用户id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SAddAsync(string key, long value)
        {
            var ret= await _redisClient.SAddAsync(key,value);
            if (ret == 1)
                return true;
            return false; 
        }

        /// <summary>
        /// 移除set集合（比如视频取消点赞场景，用视频的id作为key-注意加上前缀，用户id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SRemAsync(string key, long value)
        {
            var ret = await _redisClient.SRemAsync(key,value);
            if (ret == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 获取set集合数量（比如视频点赞场景，获取点赞人数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SCardAsync(string key)
        {
            return await _redisClient.SCardAsync(key);
        }

        /// <summary>
        /// 获取set集合列表（比如视频点赞场景，获取点赞人列表id，慎用）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long[]> SMembersAsync(string key)
        {
            return await _redisClient.SMembersAsync<long>(key);
        }

        /// <summary>
        /// 判断元素是否在集合中（比如视频点赞场景，判断用户是否对视频点赞过）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SIsMemberAsync(string key,long member)
        {
            return await _redisClient.SIsMemberAsync(key,member);
        }

        /// <summary>
        /// 添加zset集合，score默认时间戳（比如我的点赞视频历史场景，用我的id作为key-注意加上前缀，视频id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ZAddByTsAsync(string key, string value)
        {
            //ZMember scoreMember =new (value.ToString(),TimeExtension.CurrentTimeMillis());
            var ret = await _redisClient.ZAddAsync(key, TimeExtension.CurrentTimeMillis(),value);
            if (ret == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 添加zset集合，score作为参数（比如分类视频根据浏览量排序场景，用视频分类的id做key-注意加上前缀，视频id作为值，浏览量作为score作为参数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> ZAddBySortAsync(string key, decimal score, string value)
        {
            var ret = await _redisClient.ZAddAsync(key, score, value);
            if (ret == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 获取元素的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<decimal> ZScoreAsync(string key,string member)
        {
           var ret=  await _redisClient.ZScoreAsync(key, member);
            if (ret == default) return 0;
            return Convert.ToDecimal(ret);
        }
        /// <summary>
        /// 移除zset集合（比如我的点赞视频历史场景-视频取消点赞，用我的id作为key-注意加上前缀，视频id作为值）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ZRemAsync(string key, string value)
        {
            var ret = await _redisClient.ZRemAsync(key, value);
            if (ret == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 获取zset集合数量（比我的点赞视频历史场景，获取我的点赞视频数）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ZCardAsync(string key)
        {
            return await _redisClient.ZCardAsync(key);
        }

        /// <summary>
        /// 获取zset集合列表（比我的点赞视频历史场景，根据时间戳从大到小获取我的点赞视频id）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string[]> ZRevRangeAsync(string key,decimal start=0, decimal stop =-1)
        {
            return await _redisClient.ZRevRangeAsync(key,start,stop);
        }

        /// <summary>
        /// 获取zset集合列表（比我的点赞视频历史场景，根据时间戳从小到大获取我的点赞视频id，索引stop-1展示所有，如果分页，索引start=0，索引stop=9，第二页，索引start=10，索引stop=19）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string[]> ZRangeAsync(string key, decimal start = 0, decimal stop = -1)
        {
            return await _redisClient.ZRangeAsync(key, start, stop);
        }

        /// <summary>
        /// zset批量删除
        /// </summary>
        /// <param name="dicKeyValues">删除的key+value</param>
        /// <param name="length">每次管道请求指令数，默认1000</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ZRemAll(Dictionary<string, string> dicKeyValues,int length=1000)
        {
            if(length<=0)
            {
                throw new ArgumentNullException(nameof(length));
            }
            if (dicKeyValues == null)
            {
                throw new ArgumentNullException(nameof(dicKeyValues));
            }
            if (dicKeyValues.Count <= length)
            {
                var pipe = _redisClient.StartPipe();
                foreach (KeyValuePair<string, string> kvp in dicKeyValues)
                {
                    pipe.ZRem(kvp.Key, kvp.Value);
                }
                pipe.EndPipe();
            }
            else
            {
                var dicList = dicKeyValues.Split((int)Math.Ceiling((double)dicKeyValues.Count / length)).ToList();
                dicList.ForEach(x =>
                {
                    var pipe = _redisClient.StartPipe();
                    foreach (KeyValuePair<string, string> kvp in x)
                    {
                        pipe.ZRem(kvp.Key, kvp.Value);
                    }
                    pipe.EndPipe();
                });

            }
        }
    }
}
