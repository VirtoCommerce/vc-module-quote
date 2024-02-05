using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.FileExperienceApi.Core.Services;
using VirtoCommerce.QuoteModule.Core;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteAttachmentOptionsQueryHandler : IQueryHandler<QuoteAttachmentOptionsQuery, FileUploadScopeOptions>
{
    private readonly IFileUploadService _fileUploadService;

    public QuoteAttachmentOptionsQueryHandler(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    public Task<FileUploadScopeOptions> Handle(QuoteAttachmentOptionsQuery request, CancellationToken cancellationToken)
    {
        return _fileUploadService.GetOptionsAsync(ModuleConstants.QuoteAttachmentsScope);
    }
}
