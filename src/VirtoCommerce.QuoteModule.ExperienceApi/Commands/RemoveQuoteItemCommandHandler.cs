using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class RemoveQuoteItemCommandHandler : QuoteCommandHandler<RemoveQuoteItemCommand>
{
    public RemoveQuoteItemCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override void UpdateQuote(QuoteRequest quote, RemoveQuoteItemCommand request)
    {
        var item = quote.Items.FirstOrDefault(x => x.Id.EqualsIgnoreCase(request.LineItemId));

        if (item != null)
        {
            quote.Items.Remove(item);
        }
    }
}
