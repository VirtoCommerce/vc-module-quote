using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteAttachmentType : ExtendableGraphType<QuoteAttachment>
{
    public QuoteAttachmentType()
    {
        Field(x => x.Name, nullable: false);
        Field(x => x.Url, nullable: false);
        Field<StringGraphType>("ContentType", resolve: context => context.Source.MimeType);
        Field(x => x.Size, nullable: false);

        Field<StringGraphType>(nameof(QuoteAttachment.MimeType),
            resolve: context => context.Source.MimeType,
            deprecationReason: "Use ContentType");
    }
}
