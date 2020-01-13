using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace DesktopApp.Common.Util
{
    public static class MemoryCacheUtil
    {
        private static ObjectCache Cache => MemoryCache.Default;

        public static void SetItem(string key, object item, int keepTime = 1)
        {
            if (Cache.Contains(key))
            {
                throw new ArgumentException("缓存KEY已经存在");
            }

            Cache.Set(key, item, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(keepTime),
                RemovedCallback = c =>
                {
#if DEBUG

                    Debug.WriteLine($"缓存{c.CacheItem.Key}被移除,原因：{c.RemovedReason}");

#endif
                }
            });
        }

        public static T GetItem<T>(string key)
        {
            if (Cache.Get(key) is T o)
            {
                return o;
            }

            return default(T);
        }
    }
}