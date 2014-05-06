using System;

namespace BigBallz.Core.Caching
{
    public class NullCache : ICache
    {
        public int Count
        {
            get { return 0; }
        }

        public void Clear()
        {}

        public bool Contains(string key)
        {
            return false;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            return false;
        }

        public void Set<T>(string key, T value)
        {}

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {}

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {}

        public void Remove(string key)
        {}
    }
}