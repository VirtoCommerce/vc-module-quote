using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.FileExperienceApi.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class UpdateQuoteAttachmentsCommandHandlerBase<TCommand> : QuoteCommandHandler<TCommand>
    where TCommand : QuoteCommand
{
    private const string _urlPrefix = "/api/files/";
    private readonly IFileUploadService _fileUploadService;

    protected UpdateQuoteAttachmentsCommandHandlerBase(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager,
        IFileUploadService fileUploadService)
        : base(quoteRequestService, quoteAggregateRepository, settingsManager)
    {
        _fileUploadService = fileUploadService;
    }

    protected async Task UpdateAttachmentsAsync(QuoteRequest quote, IList<string> urls)
    {
        var fileIds = urls.Select(GetFileId).Where(x => !string.IsNullOrEmpty(x)).ToList();
        var files = await _fileUploadService.GetAsync(fileIds);

        // Allow only file uploaded in the quote attachments scope
        files = files.Where(x => x.Scope == ModuleConstants.QuoteAttachmentsScope).ToList();

        // Delete attachments
        var newFileIds = files.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var newUrls = newFileIds.Select(GetFileUrl).ToList();
        var attachmentsToDelete = quote.Attachments.Where(x => !newUrls.Contains(x.Url)).ToList();

        foreach (var attachment in attachmentsToDelete)
        {
            quote.Attachments.Remove(attachment);
        }

        // Add attachments
        var oldUrls = quote.Attachments.Select(x => x.Url).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var filesToAdd = files.Where(x => !oldUrls.Contains(GetFileUrl(x.Id))).ToList();

        foreach (var file in filesToAdd)
        {
            quote.Attachments.Add(ConvertToAttachment(file));
        }
    }

    protected override void UpdateQuote(QuoteRequest quote, TCommand request)
    {
        // Empty implementation of abstract method
    }

    protected virtual QuoteAttachment ConvertToAttachment(File file)
    {
        var attachment = AbstractTypeFactory<QuoteAttachment>.TryCreateInstance();

        attachment.Name = file.Name;
        attachment.Url = GetFileUrl(file.Id);
        attachment.MimeType = file.ContentType;
        attachment.Size = file.Size;

        return attachment;
    }

    protected static string GetFileUrl(string id)
    {
        return $"{_urlPrefix}{id}";
    }

    protected static string GetFileId(string url)
    {
        return url != null && url.StartsWith(_urlPrefix)
            ? url[_urlPrefix.Length..]
            : null;
    }
}
