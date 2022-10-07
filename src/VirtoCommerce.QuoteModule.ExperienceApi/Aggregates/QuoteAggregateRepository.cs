using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteAggregateRepository : IQuoteAggregateRepository
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteTotalsCalculator _totalsCalculator;
    private readonly ICurrencyService _currencyService;

    public QuoteAggregateRepository(
        IQuoteRequestService quoteRequestService,
        IQuoteTotalsCalculator totalsCalculator,
        ICurrencyService currencyService)
    {
        _quoteRequestService = quoteRequestService;
        _totalsCalculator = totalsCalculator;
        _currencyService = currencyService;
    }

    public async Task<QuoteAggregate> GetById(string id)
    {
        var quotes = await _quoteRequestService.GetByIdsAsync(id);
        var aggregates = await ToQuoteAggregates(quotes);

        return aggregates.FirstOrDefault();
    }

    public virtual async Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes)
    {
        var result = new List<QuoteAggregate>();

        var currencies = (await _currencyService.GetAllCurrenciesAsync()).ToList();

        foreach (var quote in quotes)
        {
            quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);

            var currency = currencies.GetCurrencyForLanguage(quote.Currency, quote.LanguageCode);
            var aggregate = ToQuoteAggregate(quote, currency);

            result.Add(aggregate);
        }

        return result;
    }


    protected virtual QuoteAggregate ToQuoteAggregate(QuoteRequest model, Currency currency)
    {
        var aggregate = AbstractTypeFactory<QuoteAggregate>.TryCreateInstance();

        aggregate.Model = model;
        aggregate.Currency = currency;

        aggregate.Totals = model.Totals?.Convert(x => ToQuoteTotalsAggregate(x, aggregate));
        aggregate.Items = model.Items?.Select(x => ToQuoteItemAggregate(x, aggregate)).ToList();
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

    protected virtual QuoteItemAggregate ToQuoteItemAggregate(QuoteItem model, QuoteAggregate quote)
    {
        var aggregate = AbstractTypeFactory<QuoteItemAggregate>.TryCreateInstance();

        aggregate.Model = model;
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
}
