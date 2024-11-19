using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.ExperienceApiModule.Core.Services;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteDynamicPropertiesCommandHandler(
    IQuoteRequestService quoteRequestService,
    IQuoteAggregateRepository quoteAggregateRepository,
    ISettingsManager settingsManager,
    IDynamicPropertyUpdaterService dynamicPropertyUpdaterService
)
    : QuoteCommandHandler<UpdateQuoteDynamicPropertiesCommand>(
        quoteRequestService,
        quoteAggregateRepository,
        settingsManager
    )
{
    protected override Task UpdateQuoteAsync(QuoteRequest quote, UpdateQuoteDynamicPropertiesCommand request)
    {
        return dynamicPropertyUpdaterService.UpdateDynamicPropertyValues(quote, request.DynamicProperties);
    }

    protected override void UpdateQuote(QuoteRequest quote, UpdateQuoteDynamicPropertiesCommand request)
    {
        // Empty implementation of obsolete abstract method
    }
}
