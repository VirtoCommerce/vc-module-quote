using GraphQL.Types;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteConfigurationItemType : ExtendableGraphType<ConfigurationItem>
{
    public QuoteConfigurationItemType()
    {
        Field(x => x.Id, nullable: false).Description("Configuration item ID");
        Field(x => x.Name, nullable: true).Description("Configuration item name");
        Field(x => x.Type, nullable: false).Description("Configuration item type. Possible values: 'Product', 'Text', 'File'");
        Field(x => x.CustomText, nullable: true).Description("Configuration item custom text");

        ExtendableField<ListGraphType<QuoteConfigurationItemFileType>>(nameof(ConfigurationItem.Files),
            resolve: context => context.Source.Files,
            description: "List of files for 'File' configuration item section");
    }
}
