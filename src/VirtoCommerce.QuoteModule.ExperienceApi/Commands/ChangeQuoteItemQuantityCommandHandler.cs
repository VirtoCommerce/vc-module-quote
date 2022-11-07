using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteItemQuantityCommandHandler : QuoteCommandHandler<ChangeQuoteItemQuantityCommand>
{
    public ChangeQuoteItemQuantityCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override void UpdateQuote(QuoteRequest quote, ChangeQuoteItemQuantityCommand request)
    {
        var item = quote.Items.FirstOrDefault(x => x.Id.EqualsInvariant(request.LineItemId));

        if (item == null)
        {
            return;
        }

        if (request.Quantity > 0)
        {
            item.SelectedTierPrice.Quantity = request.Quantity;
        }
        else
        {
            quote.Items.Remove(item);
        }
    }
}
