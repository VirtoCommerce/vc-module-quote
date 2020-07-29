using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.QuoteModule.Data.Caching
{
    public class QuoteCacheRegion : CancellableCacheRegion<QuoteCacheRegion>
    {
        public static IChangeToken CreateChangeToken(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var changeTokens = new List<IChangeToken>() { CreateChangeToken() };
            foreach (var id in ids)
            {
                changeTokens.Add(CreateChangeTokenForKey(id));
            }

            return new CompositeChangeToken(changeTokens);
        }

        public static void Expire(string id)
        {
            ExpireTokenForKey(id);
        }
    }
}
