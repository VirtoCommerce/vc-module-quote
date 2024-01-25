using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.FileExperienceApi.Core.Authorization;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.QuoteModule.Core;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Authorization;

public class QuoteAuthorizationRequirementFactory : IFileAuthorizationRequirementFactory
{
    public string Scope => ModuleConstants.QuoteAttachmentsScope;

    public IAuthorizationRequirement Create(File file, string permission)
    {
        return new QuoteAuthorizationRequirement();
    }
}
