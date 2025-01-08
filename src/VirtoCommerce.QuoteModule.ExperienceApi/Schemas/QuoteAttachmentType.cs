using GraphQL.Types;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteAttachmentType : ExtendableGraphType<QuoteAttachment>
{
    public QuoteAttachmentType()
    {
        Field(x => x.Name, nullable: false);
        Field(x => x.Url, nullable: false);
        Field<StringGraphType>("ContentType").Resolve(context => context.Source.MimeType);
        Field(x => x.Size, nullable: false);

        Field<StringGraphType>(nameof(QuoteAttachment.MimeType))
            .Resolve(context => context.Source.MimeType)
            .DeprecationReason("Use ContentType");
    }
}
