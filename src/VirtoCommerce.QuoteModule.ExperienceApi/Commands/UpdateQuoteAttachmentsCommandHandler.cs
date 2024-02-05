using System.Threading.Tasks;
using VirtoCommerce.FileExperienceApi.Core.Services;
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
        ISettingsManager settingsManager,
        IFileUploadService fileUploadService)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager, fileUploadService)
    {
    }

    protected override Task UpdateQuoteAsync(QuoteRequest quote, UpdateQuoteAttachmentsCommand request)
    {
        return UpdateAttachmentsAsync(quote, request.Urls);
    }
}
