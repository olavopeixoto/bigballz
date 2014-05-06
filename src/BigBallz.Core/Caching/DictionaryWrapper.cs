using System;
using System.Collections;
using System.Diagnostics;

namespace BigBallz.Core.Caching
{
    public class DictionaryWrapper : ICache
    {
        private readonly IDictionary _cache;

        public DictionaryWrapper(IDictionary cache)
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
            _cache.Clear();
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key);
        }

        public T Get<T>(string key)
        {
            return (T) _cache[key];
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);

            if (Contains(key))
            {
                var existingValue = _cache[key];

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
            RemoveIfExists(key);

            _cache[key] = value;
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            RemoveIfExists(key);

            _cache[key] = value;
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            RemoveIfExists(key);

            _cache[key] = value;
        }

        public void Remove(string key)
        {
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