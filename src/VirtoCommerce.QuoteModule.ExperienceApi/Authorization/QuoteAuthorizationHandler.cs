using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.QuoteModule.Core.Extensions;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Queries;
using static VirtoCommerce.Xapi.Core.ModuleConstants;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Authorization;

public class QuoteAuthorizationRequirement : IAuthorizationRequirement
{
}

public class QuoteAuthorizationHandler : AuthorizationHandler<QuoteAuthorizationRequirement>
{
    private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
    private readonly IMemberService _memberService;
    private readonly IQuoteRequestService _quoteRequestService;

    public QuoteAuthorizationHandler(
        Func<UserManager<ApplicationUser>> userManagerFactory,
        IMemberService memberService,
        IQuoteRequestService quoteRequestService)
    {
        _userManagerFactory = userManagerFactory;
        _memberService = memberService;
        _quoteRequestService = quoteRequestService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, QuoteAuthorizationRequirement requirement)
    {
        var result = context.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

        if (!result)
        {
            var resource = context.Resource;

            if (resource is File file)
            {
                result = string.IsNullOrEmpty(file.OwnerEntityId);

                if (!result &&
                    file.OwnerEntityType.EqualsInvariant(nameof(QuoteRequest)) &&
                    !string.IsNullOrEmpty(file.OwnerEntityId))
                {
                    resource = await _quoteRequestService.GetByIdAsync(file.OwnerEntityId);
                }
            }

            var currentUserId = GetUserId(context);

            switch (resource)
            {
                case string userId when context.User.Identity?.IsAuthenticated == true:
                    result = userId == currentUserId;
                    break;
                case QuoteRequest quote:
                    result = await CanAccessQuote(context, quote);
                    break;
                case QuoteAggregate quoteAggregate:
                    result = await CanAccessQuote(context, quoteAggregate.Model);
                    break;
                case QuotesQuery query:
                    query.UserId = currentUserId;
                    result = query.UserId != null;
                    break;
            }
        }

        if (result)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    private async Task<bool> CanAccessQuote(AuthorizationHandlerContext context, QuoteRequest quote)
    {
        var currentUserId = GetUserId(context);

        return quote.CustomerId == currentUserId ||
               await IsOrganizationMaintainer(context.User, currentUserId, quote.OrganizationId);
    }

    // TODO: Use scoped permission
    private async Task<bool> IsOrganizationMaintainer(ClaimsPrincipal principal, string userId, string organizationId)
    {
        return !string.IsNullOrEmpty(organizationId) &&
            principal.HasGlobalPermission("xapi:my_organization:edit") &&
            ((await GetContact(userId))?.Organizations.Contains(organizationId, StringComparer.OrdinalIgnoreCase) ?? false);
    }

    private async Task<Contact> GetContact(string userId)
    {
        Contact contact = null;

        using var userManager = _userManagerFactory();
        var user = await userManager.FindByIdAsync(userId);

        if (!string.IsNullOrEmpty(user?.MemberId))
        {
            contact = await _memberService.GetByIdAsync(user.MemberId) as Contact;
        }

        return contact;
    }

    private static string GetUserId(AuthorizationHandlerContext context)
    {
        return
            context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            context.User.FindFirstValue("name") ??
            AnonymousUser.UserName;
    }
}
