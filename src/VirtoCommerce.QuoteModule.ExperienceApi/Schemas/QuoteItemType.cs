using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Resolvers;
using GraphQL.Types;
using MediatR;
using StackExchange.Redis;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Helpers;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.XCatalog.Core.Models;
using VirtoCommerce.XCatalog.Core.Queries;
using VirtoCommerce.XCatalog.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteItemType : ExtendableGraphType<QuoteItemAggregate>
{
    public QuoteItemType(IMediator mediator, IDataLoaderContextAccessor dataLoader)
    {
        Field(x => x.Model.Id, nullable: false);
        Field(x => x.Model.Sku, nullable: true);
        Field(x => x.Model.ProductId, nullable: true);
        Field(x => x.Model.CatalogId, nullable: true);
        Field(x => x.Model.CategoryId, nullable: true);
        Field(x => x.Model.Name, nullable: false);
        Field(x => x.Model.Comment, nullable: true);
        Field(x => x.Model.ImageUrl, nullable: true);
        Field(x => x.Model.TaxType, nullable: true);
        Field(x => x.Model.Quantity, nullable: false);

        Field<NonNullGraphType<MoneyType>>(nameof(QuoteItem.ListPrice)).Resolve(context => context.Source.Model.ListPrice.ToMoney(context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteItem.SalePrice)).Resolve(context => context.Source.Model.SalePrice.ToMoney(context.Source.Quote.Currency));

        ExtendableField<QuoteTierPriceType>(nameof(QuoteItem.SelectedTierPrice), resolve: context => context.Source.SelectedTierPrice);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteTierPriceType>>>>(nameof(QuoteItem.ProposalPrices), resolve: context => context.Source.ProposalPrices);

        var productField = new FieldType
        {
            Name = "product",
            Type = GraphTypeExtensionHelper.GetActualType<ProductType>(),
            Resolver = new FuncFieldResolver<QuoteItemAggregate, IDataLoaderResult<ExpProduct>>(context =>
            {
                var loader = dataLoader.Context.GetOrAddBatchLoader<string, ExpProduct>("quote_lineItem_products", async ids =>
                {
                    var quoteAggregate = context.Source.Quote;
                    var quote = quoteAggregate.Model;

                    var request = new LoadProductsQuery
                    {
                        StoreId = quote.StoreId,
                        CurrencyCode = quote.Currency,
                        ObjectIds = ids.ToArray(),
                        IncludeFields = context.SubFields.Values.GetAllNodesPaths(context).ToArray(),
                    };


                    var cultureName = context.GetArgumentOrValue<string>("cultureName") ?? quote.LanguageCode;
                    context.UserContext.TryAdd("currencyCode", quote.Currency);
                    context.UserContext.TryAdd("storeId", quote.StoreId);
                    context.UserContext.TryAdd("store", quoteAggregate.Store);
                    context.UserContext.TryAdd("cultureName", cultureName);

                    var response = await mediator.Send(request);

                    return response.Products.ToDictionary(x => x.Id);
                });

                return context.Source.Model.ProductId != null
                    ? loader.LoadAsync(context.Source.Model.ProductId)
                    : new DataLoaderResult<ExpProduct>(Task.FromResult<ExpProduct>(null));
            })
        };

        AddField(productField);
    }
}
