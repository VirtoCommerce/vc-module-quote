using System.Collections.Generic;
using GraphQL.Types;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteAddressesCommand : QuoteCommand
{
    public IList<Address> Addresses { get; set; }
}

public class UpdateQuoteAddressesCommandType : QuoteCommandType<UpdateQuoteAddressesCommand>
{
    public UpdateQuoteAddressesCommandType()
    {
        Field<NonNullGraphType<ListGraphType<InputQuoteAddressType>>>(nameof(UpdateQuoteAddressesCommand.Addresses));
    }
}
