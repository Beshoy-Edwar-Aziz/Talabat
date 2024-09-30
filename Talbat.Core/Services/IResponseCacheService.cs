using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface IResponseCacheService
    {
        Task ResponseCacheAsync(string CacheKey, object Response, TimeSpan TimeToLive);
        Task<string?> GetResponseCacheAsync(string CacheKey);
     }
}
