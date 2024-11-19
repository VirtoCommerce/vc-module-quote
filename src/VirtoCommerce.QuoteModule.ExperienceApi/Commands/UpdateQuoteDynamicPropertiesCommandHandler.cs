using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Services;

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
    protected override async Task UpdateQuoteAsync(QuoteRequest quote, UpdateQuoteDynamicPropertiesCommand request)
    {
        await dynamicPropertyUpdaterService.UpdateDynamicPropertyValues(quote, request.DynamicProperties);
    }

    protected override void UpdateQuote(QuoteRequest quote, UpdateQuoteDynamicPropertiesCommand request)
    {
        // Empty implementation of obsolete abstract method
    }
}
