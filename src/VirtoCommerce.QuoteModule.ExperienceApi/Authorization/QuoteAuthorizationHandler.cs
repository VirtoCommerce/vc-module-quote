using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.ExperienceApiModule.Core;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Queries;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Authorization;

public class QuoteAuthorizationRequirement : IAuthorizationRequirement
{
}

public class QuoteAuthorizationHandler : AuthorizationHandler<QuoteAuthorizationRequirement>
{
    private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
    private readonly IMemberService _memberService;

    public QuoteAuthorizationHandler(
        Func<UserManager<ApplicationUser>> userManagerFactory,
        IMemberService memberService)
    {
        _userManagerFactory = userManagerFactory;
        _memberService = memberService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, QuoteAuthorizationRequirement requirement)
    {
        var result = context.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

        if (!result)
        {
            var currentUserId = GetUserId(context);

            switch (context.Resource)
            {
                case QuoteAggregate quote:
                    result = quote.Model.CustomerId == currentUserId ||
                        await IsOrganizationMaintainer(context.User, currentUserId, quote.Model.OrganizationId);
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
