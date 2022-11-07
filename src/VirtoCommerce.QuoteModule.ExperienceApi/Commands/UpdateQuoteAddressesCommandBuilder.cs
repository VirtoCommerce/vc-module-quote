using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteAddressesCommandBuilder : QuoteCommandBuilder<UpdateQuoteAddressesCommand, UpdateQuoteAddressesCommandType>
{
    protected override string Name => "updateQuoteAddresses";

    public UpdateQuoteAddressesCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
