using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteAttachmentsCommandHandler : UpdateQuoteAttachmentsCommandHandlerBase<AddQuoteAttachmentsCommand>
{
    public AddQuoteAttachmentsCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override Task UpdateQuoteAsync(QuoteRequest quote, AddQuoteAttachmentsCommand request)
    {
        var urls = quote.Attachments
            .Select(x => x.Url)
            .Concat(request.Urls)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return UpdateAttachmentsAsync(quote, urls);
    }
}
