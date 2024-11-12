using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.XCatalog.Core.Models;
using VirtoCommerce.XCatalog.Core.Queries;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteItemsCommandHandler(
    IQuoteRequestService quoteRequestService,
    IQuoteAggregateRepository quoteAggregateRepository,
    ISettingsManager settingsManager,
    IMediator mediator
)
    : QuoteCommandHandler<AddQuoteItemsCommand>(quoteRequestService, quoteAggregateRepository, settingsManager)
{
    protected override async Task UpdateQuoteAsync(QuoteRequest quote, AddQuoteItemsCommand request)
    {
        var productIds = request.NewQuoteItems.Where(x => !string.IsNullOrEmpty(x.ProductId)).Select(x => x.ProductId).ToArray();

        var productsQuery = new LoadProductsQuery
        {
            UserId = quote.CustomerId,
            StoreId = quote.StoreId,
            CurrencyCode = quote.Currency,
            ObjectIds = productIds,
            IncludeFields =
            [
                "__object",
                "price",
                "images",
                "properties",
                "description",
                "slug",
                "outlines"
            ],
            EvaluatePromotions = false,
        };

        var productsResponse = await mediator.Send(productsQuery);

        AddQuoteItems(quote, request, productsResponse.Products);
    }

    // This method should be a part of QuoteAggregate, but it requires QuoteCommand to be refactored with breaking changes
    protected virtual void AddQuoteItems(QuoteRequest quote, AddQuoteItemsCommand request, ICollection<ExpProduct> products)
    {
        var productsByIds = products.ToDictionary(x => x.Id, x => x);

        foreach (var newQuoteItem in request.NewQuoteItems)
        {
            var quoteItem = AbstractTypeFactory<QuoteItem>.TryCreateInstance();

            quoteItem.ProductId = newQuoteItem.ProductId;
            quoteItem.Name = newQuoteItem.Name;
            quoteItem.Comment = newQuoteItem.Comment;
            quoteItem.Quantity = newQuoteItem.Quantity;
            quoteItem.Currency = quote.Currency;

            var product = productsByIds.GetValueSafe(newQuoteItem.ProductId);
            if (product != null)
            {
                quoteItem.Name = product.IndexedProduct.Name;
                quoteItem.Sku = product.IndexedProduct.Code;
                quoteItem.CatalogId = product.IndexedProduct.CatalogId;
                quoteItem.CategoryId = product.IndexedProduct.CategoryId;
                quoteItem.ImageUrl = product.IndexedProduct.ImgSrc;
                quoteItem.TaxType = product.IndexedProduct.TaxType;
            }

            var price = product?.AllPrices.FirstOrDefault();
            quoteItem.ListPrice = price?.ListPrice.InternalAmount ?? 0;
            quoteItem.SalePrice = price?.SalePrice.InternalAmount ?? 0;

            var tierPrice = AbstractTypeFactory<TierPrice>.TryCreateInstance();
            tierPrice.Price = newQuoteItem.Price ?? quoteItem.SalePrice;
            tierPrice.Quantity = newQuoteItem.Quantity;

            quoteItem.ProposalPrices = [tierPrice];

            quote.Items.Add(quoteItem);
        }
    }
}
