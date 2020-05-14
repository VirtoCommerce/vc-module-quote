using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.QuoteModule.Data.Caching
{
    public class QuoteCacheRegion : CancellableCacheRegion<QuoteCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _quoteRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var changeTokens = new List<IChangeToken>() { CreateChangeToken() };
            foreach (var id in ids)
            {
                changeTokens.Add(new CancellationChangeToken(_quoteRegionTokenLookup.GetOrAdd(id, new CancellationTokenSource()).Token));
            }

            return new CompositeChangeToken(changeTokens);
        }

        public static void Expire(string id)
        {
            if (_quoteRegionTokenLookup.TryRemove(id, out CancellationTokenSource token))
            {
                token.Cancel();
            }
        }
    }
}
