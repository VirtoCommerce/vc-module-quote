using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuotesQuery : SearchQuery<QuoteAggregateSearchResult>
{
    public string CustomerId { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        foreach (var argument in base.GetArguments())
        {
            yield return argument;
        }

        yield return Argument<StringGraphType>(nameof(CustomerId));
    }

    public override void Map(IResolveFieldContext context)
    {
        base.Map(context);

        CustomerId = context.GetArgument<string>(nameof(CustomerId));
    }
}
