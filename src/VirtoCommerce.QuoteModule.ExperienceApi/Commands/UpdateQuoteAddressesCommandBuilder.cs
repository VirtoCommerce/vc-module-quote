using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class UpdateQuoteAddressesCommandBuilder : QuoteCommandBuilder<UpdateQuoteAddressesCommand, UpdateQuoteAddressesCommandType>
{
    protected override string Name => "updateQuoteAddresses";

    public UpdateQuoteAddressesCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
