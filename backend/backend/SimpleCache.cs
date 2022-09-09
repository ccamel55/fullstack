using backend.Models;
using System.Runtime.Caching;

namespace backend
{
    public class SimpleCache
    {
        private readonly string _cacheKey;
        private bool _forceRecache;

        public SimpleCache(string cacheKey)
        {
            _cacheKey = cacheKey;
            _forceRecache = false;
        }
     
        public void updateCache(IEnumerable<Calculation> items)
        {
            var cache = MemoryCache.Default;

            // keep data for the next hour
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.Priority = CacheItemPriority.NotRemovable;

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
        public void setRecahce(bool val)
        {
            _forceRecache = val;
        }

        public bool needRecahce()
        {
           return _forceRecache;
        }
    }
}
