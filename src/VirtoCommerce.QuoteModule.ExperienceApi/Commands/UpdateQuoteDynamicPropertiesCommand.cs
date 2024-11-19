using System.Collections.Generic;
using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Models;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteDynamicPropertiesCommand : QuoteCommand
{
    public List<DynamicPropertyValue> DynamicProperties { get; set; } = default!;
}

public class UpdateQuoteDynamicPropertiesCommandType : QuoteCommandType<UpdateQuoteDynamicPropertiesCommand>
{
    public UpdateQuoteDynamicPropertiesCommandType()
    {
        Field<NonNullGraphType<ListGraphType<InputDynamicPropertyValueType>>>(
            "dynamicProperties",
            "Dynamic properties"
        );
    }
}
