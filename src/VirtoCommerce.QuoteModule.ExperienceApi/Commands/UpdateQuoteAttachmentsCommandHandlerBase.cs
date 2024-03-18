using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class UpdateQuoteAttachmentsCommandHandlerBase<TCommand> : QuoteCommandHandler<TCommand>
    where TCommand : QuoteCommand
{
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    protected UpdateQuoteAttachmentsCommandHandlerBase(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
        _quoteAggregateRepository = quoteAggregateRepository;
    }

    protected Task UpdateAttachmentsAsync(QuoteRequest quote, IList<string> urls)
    {
        return _quoteAggregateRepository.UpdateQuoteAttachmentsAsync(quote, urls);
    }

    protected override void UpdateQuote(QuoteRequest quote, TCommand request)
    {
        // Empty implementation of obsolete abstract method
    }
}
