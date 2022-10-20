using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteQuery : Query<QuoteAggregate>
{
    public string Id { get; set; }
    public string StoreId { get; set; }
    public string UserId { get; set; }
    public string CurrencyCode { get; set; }
    public string CultureName { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        yield return Argument<StringGraphType>(nameof(Id));
        yield return Argument<StringGraphType>(nameof(StoreId));
        yield return Argument<StringGraphType>(nameof(UserId));
        yield return Argument<StringGraphType>(nameof(CurrencyCode));
        yield return Argument<StringGraphType>(nameof(CultureName));
    }

    public override void Map(IResolveFieldContext context)
    {
        Id = context.GetArgument<string>(nameof(Id));
        StoreId = context.GetArgument<string>(nameof(StoreId));
        UserId = context.GetArgument<string>(nameof(UserId));
        CurrencyCode = context.GetArgument<string>(nameof(CurrencyCode));
        CultureName = context.GetArgument<string>(nameof(CultureName));
    }
}
