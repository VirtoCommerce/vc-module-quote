using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeleteQuoteAttachmentsCommandHandler : UpdateQuoteAttachmentsCommandHandlerBase<DeleteQuoteAttachmentsCommand>
{
    public DeleteQuoteAttachmentsCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override Task UpdateQuoteAsync(QuoteRequest quote, DeleteQuoteAttachmentsCommand request)
    {
        var urls = quote.Attachments
            .Select(x => x.Url)
            .Except(request.Urls, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return UpdateAttachmentsAsync(quote, urls);
    }
}
