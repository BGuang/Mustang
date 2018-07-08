using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Mustang.NoSql.Redis
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _redis;
        private int _dbnum;
        public RedisCache():this(0,null)
        {           
        }
        public RedisCache(int dbnum) : this(dbnum, null)
        {
        }
        public RedisCache(int dbNum, string readWriteHosts)
        {
            _redis = string.IsNullOrWhiteSpace(readWriteHosts) ? RedisManager.Instance:RedisManager.GetConnectionMultiplexer(readWriteHosts);
            _db = _redis.GetDatabase(_dbnum);
        }

        public object Get(string key)
        {
            return Get<object>(key);
        }
        public T Get<T>(string key)
        {
            var cacheValue = _db.StringGet(key);
            var value = default(T);
            if (cacheValue.HasValue && !cacheValue.IsNull)
            {
                var cacheObject = JsonConvert.DeserializeObject<T>(cacheValue);
                value = cacheObject;
            }
            return value;
        }

        public void Insert(string key, object data)
        {
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData);
        }

        public void Insert(string key, object data, int expiry)
        {
            var timeSpan = TimeSpan.FromSeconds(expiry);
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData, timeSpan);
        }

        public void Insert(string key, object data, TimeSpan expiresAt)
        {
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData, expiresAt);
        }

        public void Insert<T>(string key, T data)
        {
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData);
        }

        public void Insert<T>(string key, T data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data, TimeSpan expiresAt)
        {
            var jsonData = SerializeContent(data);
            _db.StringSet(key, jsonData, expiresAt);
        }

        public bool Update<T>(string key, T value) where T : class
        {
            var stringContent = SerializeContent(value);
            return _db.StringSet(key, stringContent);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _db.KeyDelete(key, CommandFlags.HighPriority);
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        public bool Exists(string key)
        {
            return _db.KeyExists(key);
        }

        #region private
        private string SerializeContent(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private T DeserializeContent<T>(RedisValue myString)
        {
            return JsonConvert.DeserializeObject<T>(myString);
        }
        #endregion

    }
}
