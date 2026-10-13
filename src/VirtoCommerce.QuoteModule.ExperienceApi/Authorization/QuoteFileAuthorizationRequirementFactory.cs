using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.FileExperienceApi.Core.Authorization;
using VirtoCommerce.FileExperienceApi.Core.Extensions;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using static VirtoCommerce.CatalogModule.Core.ModuleConstants;
using static VirtoCommerce.QuoteModule.Core.ModuleConstants;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Authorization;

public class QuoteFileAuthorizationRequirementFactory : IFileAuthorizationRequirementFactory
{
    private static readonly string[] _scopes =
    [
        QuoteAttachmentsScope,
        ConfigurationSectionFilesScope,
    ];

    public bool CanCreateRequirement(File file)
    {
        return _scopes.ContainsIgnoreCase(file.Scope) && file.OwnerTypeIs<QuoteRequest>();
    }

    public IAuthorizationRequirement Create(File file, string permission)
    {
        return new QuoteAuthorizationRequirement();
    }
}
