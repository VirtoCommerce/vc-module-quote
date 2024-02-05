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
using VirtoCommerce.QuoteModule.ExperienceApi.Extensions;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class UpdateQuoteAttachmentsCommandHandlerBase<TCommand> : QuoteCommandHandler<TCommand>
    where TCommand : QuoteCommand
{
    private const string _urlPrefix = "/api/files/";
    private readonly IFileUploadService _fileUploadService;
    private readonly StringComparer _ignoreCase = StringComparer.OrdinalIgnoreCase;

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
        var oldUrls = quote.Attachments.Select(x => x.Url).ToHashSet(_ignoreCase);
        var allFiles = await GetFiles(urls, oldUrls);

        var filesByUrls = allFiles
            .Where(x =>
                x.Scope == ModuleConstants.QuoteAttachmentsScope &&
                (x.OwnerIsEmpty() || x.OwnerIs(nameof(QuoteRequest), quote.Id)))
            .ToDictionary(x => GetFileUrl(x.Id), _ignoreCase);

        var changedFiles = new List<File>();

        // Remove attachments
        var attachmentsToDelete = quote.Attachments.Where(x => !urls.Contains(x.Url, _ignoreCase)).ToList();

        foreach (var attachment in attachmentsToDelete)
        {
            quote.Attachments.Remove(attachment);

            if (filesByUrls.TryGetValue(attachment.Url, out var file))
            {
                file.OwnerEntityId = null;
                file.OwnerEntityType = null;
                changedFiles.Add(file);
            }
        }

        // Add attachments
        var urlsToAdd = urls.Except(oldUrls, _ignoreCase).ToList();

        foreach (var url in urlsToAdd)
        {
            if (filesByUrls.TryGetValue(url, out var file))
            {
                quote.Attachments.Add(ConvertToAttachment(file));

                file.OwnerEntityId = quote.Id;
                file.OwnerEntityType = nameof(QuoteRequest);
                changedFiles.Add(file);
            }
        }

        // Update owner entity in files
        if (changedFiles.Count != 0)
        {
            await _fileUploadService.SaveChangesAsync(changedFiles);
        }
    }

    protected override void UpdateQuote(QuoteRequest quote, TCommand request)
    {
        // Empty implementation of obsolete abstract method
    }

    protected async Task<IList<File>> GetFiles(IEnumerable<string> newUrls, IEnumerable<string> oldUrls)
    {
        var ids = newUrls
            .Concat(oldUrls)
            .Distinct(_ignoreCase)
            .Select(GetFileId)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        var files = await _fileUploadService.GetAsync(ids);

        return files;
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
