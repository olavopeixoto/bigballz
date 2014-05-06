using System;
using System.Diagnostics;
using System.Web.Caching;
using BigBallz.Core.Helper;

namespace BigBallz.Core.Caching
{
    public class WebCacheWrapper : ICache
    {
        private readonly System.Web.Caching.Cache _cache;

        public WebCacheWrapper(System.Web.Caching.Cache cache)
        {
            _cache = cache;
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _cache.Count;
            }
        }

        public void Clear()
        {
            var enumerator = _cache.GetEnumerator();
            while(enumerator.MoveNext())
            {
                _cache.Remove(Convert.ToString(enumerator.Key));
            }
        }

        public bool Contains(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _cache.Get(key) != null;
        }

        public T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return (T) _cache.Get(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            value = default(T);

            if (Contains(key))
            {
                var existingValue = _cache.Get(key);

                if (existingValue != null)
                {
                    value = (T) existingValue;

                    return true;
                }
            }

            return false;
        }

        public void Set<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            RemoveIfExists(key);

            _cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            _cache.Add(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            _cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.Default, null);
        }

        public void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _cache.Remove(key);
        }

        internal void RemoveIfExists(string key)
        {
            if (Contains(key))
            {
                _cache.Remove(key);
            }
        }
    }
}