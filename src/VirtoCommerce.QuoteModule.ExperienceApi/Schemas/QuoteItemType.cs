using System.Linq;
using GraphQL.DataLoader;
using GraphQL.Resolvers;
using GraphQL.Types;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.XDigitalCatalog;
using VirtoCommerce.XDigitalCatalog.Queries;
using VirtoCommerce.XDigitalCatalog.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteItemType : ExtendableGraphType<QuoteItemAggregate>
{
    public QuoteItemType(IMediator mediator, IDataLoaderContextAccessor dataLoader)
    {
        Field(x => x.Model.Id, nullable: false);
        Field(x => x.Model.Sku, nullable: false);
        Field(x => x.Model.ProductId, nullable: false);
        Field(x => x.Model.CatalogId, nullable: false);
        Field(x => x.Model.CategoryId, nullable: true);
        Field(x => x.Model.Name, nullable: false);
        Field(x => x.Model.Comment, nullable: true);
        Field(x => x.Model.ImageUrl, nullable: true);
        Field(x => x.Model.TaxType, nullable: true);

        Field<NonNullGraphType<MoneyType>>(nameof(QuoteItem.ListPrice), resolve: context => context.Source.Model.ListPrice.ToMoney(context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteItem.SalePrice), resolve: context => context.Source.Model.SalePrice.ToMoney(context.Source.Quote.Currency));

        ExtendableField<QuoteTierPriceType>(nameof(QuoteItem.SelectedTierPrice), resolve: context => context.Source.SelectedTierPrice);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteTierPriceType>>>>(nameof(QuoteItem.ProposalPrices), resolve: context => context.Source.ProposalPrices);

        var productField = new FieldType
        {
            Name = "product",
            Type = GraphTypeExtenstionHelper.GetActualType<ProductType>(),
            Resolver = new FuncFieldResolver<QuoteItemAggregate, IDataLoaderResult<ExpProduct>>(context =>
            {
                var loader = dataLoader.Context.GetOrAddBatchLoader<string, ExpProduct>("quote_lineItem_products", async ids =>
                {
                    var quote = context.Source.Quote.Model;

                    var request = new LoadProductsQuery
                    {
                        StoreId = quote.StoreId,
                        CurrencyCode = quote.Currency,
                        ObjectIds = ids.ToArray(),
                        IncludeFields = context.SubFields.Values.GetAllNodesPaths(context).ToArray(),
                    };

                    var response = await mediator.Send(request);

                    return response.Products.ToDictionary(x => x.Id);
                });

                return loader.LoadAsync(context.Source.Model.ProductId);
            })
        };

        AddField(productField);
    }
}
