using backend.Models;
using System.Runtime.Caching;

namespace backend
{
    public class SimpleCache
    {
        private readonly string _cacheKey;

        public SimpleCache(string cacheKey)
        {
            _cacheKey = cacheKey;
        }
     
        public void updateCache(IEnumerable<Calculation> items)
        {
            var cache = MemoryCache.Default;

            // keep data for the next hour
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

            cache.Add(_cacheKey, items, cacheItemPolicy);
        }

        public IEnumerable<Calculation>? getCached()
        {
            var cache = MemoryCache.Default;

            if (!cache.Contains(_cacheKey))
                return null;

            return (IEnumerable<Calculation>)cache.Get(_cacheKey);
        }

        public bool hasCache()
        {
            var cache = MemoryCache.Default;
            return cache.Contains(_cacheKey);
        }
    }
}
