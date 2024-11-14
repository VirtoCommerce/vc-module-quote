using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteDynamicPropertiesCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : QuoteCommandBuilder<UpdateQuoteDynamicPropertiesCommand, UpdateQuoteDynamicPropertiesCommandType>(
        mediator,
        authorizationService
    )
{
    protected override string Name => "updateQuoteDynamicProperties";
}
