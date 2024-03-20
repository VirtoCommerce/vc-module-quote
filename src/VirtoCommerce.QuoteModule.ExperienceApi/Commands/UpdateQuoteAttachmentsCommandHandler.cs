using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteAttachmentsCommandHandler : UpdateQuoteAttachmentsCommandHandlerBase<UpdateQuoteAttachmentsCommand>
{
    public UpdateQuoteAttachmentsCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
    }

    protected override Task UpdateQuoteAsync(QuoteRequest quote, UpdateQuoteAttachmentsCommand request)
    {
        return UpdateAttachmentsAsync(quote, request.Urls);
    }
}
