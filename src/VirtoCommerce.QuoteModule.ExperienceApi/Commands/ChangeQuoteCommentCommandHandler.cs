using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteCommentCommandHandler : QuoteCommandHandler<ChangeQuoteCommentCommand>
{
    public ChangeQuoteCommentCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override void UpdateQuote(QuoteRequest quote, ChangeQuoteCommentCommand request)
    {
        quote.Comment = request.Comment;
    }
}
