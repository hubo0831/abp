using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Volo.Abp.Caching
{
    /// <summary>缓存扩展</summary>
    public static class CacheExtensions
    {
        /// <summary>生成缓存键</summary>
        public static string GetCacheKey<T>(this IMemoryCache cache, string key, Guid? tenantId = null)
        {
            var typeName = typeof(T).FullName;
            return GetCacheKey(typeName, key, tenantId);
        }
        /// <summary>生成缓存键</summary>
        public static string GetCacheKey(this IDistributedCache cache, string typeName, string key, Guid? tenantId = null)
        {
            return GetCacheKey(typeName, key, tenantId);
        }
        /// <summary>生成缓存键</summary>
        public static string GetCacheKey(string typeName, string key, Guid? tenantId = null)
        {
            var cacheKey = $"c:{typeName},k:{key}";
            if (tenantId.HasValue)
            {
                cacheKey = $"t:{tenantId.Value.ToString()}," + cacheKey;
            }
            return cacheKey;
        }
    }
}