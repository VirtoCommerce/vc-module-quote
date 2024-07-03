using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Extensions;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, QuoteAggregate>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;
    private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
    private readonly IMemberService _memberService;
    private readonly ISettingsManager _settingsManager;

    public CreateQuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        Func<UserManager<ApplicationUser>> userManagerFactory,
        IMemberService memberService,
        ISettingsManager settingsManager)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
        _userManagerFactory = userManagerFactory;
        _memberService = memberService;
        _settingsManager = settingsManager;
    }

    public async Task<QuoteAggregate> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = AbstractTypeFactory<QuoteRequest>.TryCreateInstance();

        quote.StoreId = request.StoreId;
        quote.CustomerId = request.UserId;
        quote.Currency = request.CurrencyCode;
        quote.LanguageCode = request.CultureName;
        quote.Status = await _settingsManager.GetDefaultQuoteStatusAsync();

        var contact = await GetContact(request.UserId);
        quote.CustomerName = contact?.Name;
        // todo: get organization from another contact
        quote.OrganizationId = contact?.Organizations?.FirstOrDefault();

        var organization = await GetOrganization(quote.OrganizationId);
        quote.OrganizationName = organization?.Name;

        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        return await _quoteAggregateRepository.GetById(quote.Id);
    }

    protected async Task<Contact> GetContact(string userId)
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

    protected async Task<Organization> GetOrganization(string organizationId)
    {
        if (string.IsNullOrEmpty(organizationId))
        {
            return null;
        }

        return await _memberService.GetByIdAsync(organizationId) as Organization;
    }
}
