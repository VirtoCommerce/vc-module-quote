using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class SubmitQuoteCommandHandler : QuoteCommandHandler<SubmitQuoteCommand>
{
    public SubmitQuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override void UpdateQuote(QuoteRequest quote, SubmitQuoteCommand request)
    {
        quote.Status = QuoteStatus.Processing;
        quote.Comment = request.Comment;
    }
}
