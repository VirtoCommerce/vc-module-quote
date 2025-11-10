using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.FileExperienceApi.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Extensions;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteAggregateRepository : IQuoteAggregateRepository
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteTotalsCalculator _totalsCalculator;
    private readonly ICurrencyService _currencyService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IStoreService _storeService;
    private readonly IItemService _productService;

    private const string _attachmentsUrlPrefix = "/api/files/";
    private readonly StringComparer _ignoreCase = StringComparer.OrdinalIgnoreCase;

    public QuoteAggregateRepository(
        IQuoteRequestService quoteRequestService,
        IQuoteTotalsCalculator totalsCalculator,
        ICurrencyService currencyService,
        IFileUploadService fileUploadService,
        IStoreService storeService,
        IItemService productService)
    {
        _quoteRequestService = quoteRequestService;
        _totalsCalculator = totalsCalculator;
        _currencyService = currencyService;
        _fileUploadService = fileUploadService;
        _storeService = storeService;
        _productService = productService;
    }

    public async Task<QuoteAggregate> GetById(string id)
    {
        var quote = (await _quoteRequestService.GetByIdsAsync(id)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        var aggregates = await ToQuoteAggregates([quote], quote.LanguageCode);

        return aggregates.FirstOrDefault();
    }

    [Obsolete("Use ToQuoteAggregates(IEnumerable<QuoteRequest> quotes, string cultureName) instead. CultureName argument can be null.", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual async Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes)
    {
        var result = new List<QuoteAggregate>();

        var currencies = (await _currencyService.GetAllCurrenciesAsync()).ToList();

        foreach (var quote in quotes)
        {
            quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);

            var currency = currencies.GetCurrencyForLanguage(quote.Currency, quote.LanguageCode);
            var store = await _storeService.GetByIdAsync(quote.StoreId);
            var aggregate = ToQuoteAggregate(quote, currency, store);

            result.Add(aggregate);
        }

        return result;
    }

    public virtual async Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes, string cultureName)
    {
        var result = new List<QuoteAggregate>();

        var currencies = (await _currencyService.GetAllCurrenciesAsync()).ToList();
        var products = await _productService.GetNoCloneAsync(
                                    quotes.SelectMany(x => x.Items).Select(x => x.ProductId).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray(),
                                    ItemResponseGroup.ItemInfo.ToString());

        foreach (var quote in quotes)
        {
            // Actualize Cart Language From Context
            if (!string.IsNullOrEmpty(cultureName) && quote.LanguageCode != cultureName)
            {
                quote.LanguageCode = cultureName;
            }

            quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);

            var currency = currencies.GetCurrencyForLanguage(quote.Currency, quote.LanguageCode);
            var store = await _storeService.GetByIdAsync(quote.StoreId);
            var aggregate = ToQuoteAggregate(quote, currency, store, products);

            result.Add(aggregate);
        }

        return result;
    }

    public virtual async Task UpdateQuoteAttachmentsAsync(QuoteRequest quote, IList<string> urls)
    {
        var oldUrls = quote.Attachments.Select(x => x.Url).ToHashSet(_ignoreCase);
        var allFiles = await GetFiles(urls, oldUrls);

        var filesByUrls = allFiles
            .Where(x => x.Scope == ModuleConstants.QuoteAttachmentsScope &&
                (x.OwnerIsEmpty() || x.OwnerIs(nameof(QuoteRequest), quote.Id)))
            .ToDictionary(x => GetFileUrl(x.Id), _ignoreCase);

        var changedFiles = new List<File>();

        // Remove attachments
        var attachmentsToDelete = quote.Attachments.Where(x => !urls.Contains(x.Url, _ignoreCase)).ToList();

        foreach (var attachment in attachmentsToDelete)
        {
            quote.Attachments.Remove(attachment);

            if (filesByUrls.TryGetValue(attachment.Url, out var file))
            {
                file.OwnerEntityId = null;
                file.OwnerEntityType = null;
                changedFiles.Add(file);
            }
        }

        // Add attachments
        var urlsToAdd = urls.Except(oldUrls, _ignoreCase).ToList();

        foreach (var url in urlsToAdd)
        {
            if (filesByUrls.TryGetValue(url, out var file))
            {
                quote.Attachments.Add(ConvertToAttachment(file));

                file.OwnerEntityId = quote.Id;
                file.OwnerEntityType = nameof(QuoteRequest);
                changedFiles.Add(file);
            }
        }

        // Update owner entity in files
        if (changedFiles.Count != 0)
        {
            await _fileUploadService.SaveChangesAsync(changedFiles);
        }
    }

    [Obsolete("Use ToQuoteAggregate(QuoteRequest model, Currency currency, Store store, IList<CatalogProduct> products) instead. Products can be null or empty.", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual QuoteAggregate ToQuoteAggregate(QuoteRequest model, Currency currency, Store store)
    {
        var aggregate = AbstractTypeFactory<QuoteAggregate>.TryCreateInstance();

        aggregate.Store = store;
        aggregate.Model = model;
        aggregate.Currency = currency;

        aggregate.Totals = model.Totals?.Convert(x => ToQuoteTotalsAggregate(x, aggregate));
        aggregate.Items = model.Items?.Select(x => ToQuoteItemAggregate(x, aggregate)).ToList();
        aggregate.ShipmentMethod = model.ShipmentMethod?.Convert(x => ToQuoteShipmentMethodAggregate(x, aggregate));
        aggregate.TaxDetails = model.TaxDetails?.Select(x => ToQuoteTaxDetailAggregate(x, aggregate)).ToList();

        return aggregate;
    }

    protected virtual QuoteAggregate ToQuoteAggregate(QuoteRequest model, Currency currency, Store store, IList<CatalogProduct> products)
    {
        var aggregate = AbstractTypeFactory<QuoteAggregate>.TryCreateInstance();

        aggregate.Store = store;
        aggregate.Model = model;
        aggregate.Currency = currency;

        aggregate.Totals = model.Totals?.Convert(x => ToQuoteTotalsAggregate(x, aggregate));
        aggregate.Items = model.Items?.Select(x => ToQuoteItemAggregate(x, aggregate, products?.FirstOrDefault(y => y.Id == x.ProductId))).ToList();
        aggregate.ShipmentMethod = model.ShipmentMethod?.Convert(x => ToQuoteShipmentMethodAggregate(x, aggregate));
        aggregate.TaxDetails = model.TaxDetails?.Select(x => ToQuoteTaxDetailAggregate(x, aggregate)).ToList();

        return aggregate;
    }

    protected virtual QuoteTotalsAggregate ToQuoteTotalsAggregate(QuoteRequestTotals model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteTotalsAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Quote = quote;

        return aggregate;
    }

    [Obsolete("Use ToQuoteItemAggregate(QuoteItem model, QuoteAggregate quote, CatalogProduct product) instead. Product can be null.", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual QuoteItemAggregate ToQuoteItemAggregate(QuoteItem model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteItemAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Quote = quote;

        aggregate.SelectedTierPrice = model.SelectedTierPrice?.Convert(x => ToQuoteTierPriceAggregate(x, quote));
        aggregate.ProposalPrices = model.ProposalPrices?.Select(x => ToQuoteTierPriceAggregate(x, quote)).ToList();

        return aggregate;
    }

    protected virtual QuoteItemAggregate ToQuoteItemAggregate(QuoteItem model, QuoteAggregate quote, CatalogProduct product)
    {
        var aggregate = AbstractTypeFactory<QuoteItemAggregate>.TryCreateInstance();

        aggregate.Model = model;

        if (product?.LocalizedName != null)
        {
            var localizedName = product.LocalizedName.GetValue(quote.Model.LanguageCode);
            aggregate.Model.Name = !string.IsNullOrEmpty(localizedName) ? localizedName : aggregate.Model.Name;
        }

        aggregate.Quote = quote;

        aggregate.SelectedTierPrice = model.SelectedTierPrice?.Convert(x => ToQuoteTierPriceAggregate(x, quote));
        aggregate.ProposalPrices = model.ProposalPrices?.Select(x => ToQuoteTierPriceAggregate(x, quote)).ToList();

        return aggregate;
    }

    protected virtual QuoteTierPriceAggregate ToQuoteTierPriceAggregate(TierPrice model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteTierPriceAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Quote = quote;

        return aggregate;
    }

    protected virtual QuoteShipmentMethodAggregate ToQuoteShipmentMethodAggregate(ShipmentMethod model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteShipmentMethodAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Quote = quote;

        return aggregate;
    }

    protected virtual QuoteTaxDetailAggregate ToQuoteTaxDetailAggregate(TaxDetail model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteTaxDetailAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Quote = quote;

        return aggregate;
    }

    protected virtual async Task<IList<File>> GetFiles(IEnumerable<string> newUrls, IEnumerable<string> oldUrls)
    {
        var ids = newUrls
            .Concat(oldUrls)
            .Distinct(_ignoreCase)
            .Select(GetFileId)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        var files = await _fileUploadService.GetAsync(ids);

        return files;
    }

    protected virtual QuoteAttachment ConvertToAttachment(File file)
    {
        var attachment = AbstractTypeFactory<QuoteAttachment>.TryCreateInstance();

        attachment.Name = file.Name;
        attachment.Url = GetFileUrl(file.Id);
        attachment.MimeType = file.ContentType;
        attachment.Size = file.Size;

        return attachment;
    }

    protected static string GetFileUrl(string id)
    {
        return $"{_attachmentsUrlPrefix}{id}";
    }

    protected static string GetFileId(string url)
    {
        return url != null && url.StartsWith(_attachmentsUrlPrefix)
            ? url[_attachmentsUrlPrefix.Length..]
            : null;
    }
}
