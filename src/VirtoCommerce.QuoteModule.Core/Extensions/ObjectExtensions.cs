using System;

namespace VirtoCommerce.QuoteModule.Core.Extensions;

public static class ObjectExtensions
{
    public static TResult Convert<TSource, TResult>(this TSource source, Func<TSource, TResult> convert)
    {
        return convert(source);
    }
}
