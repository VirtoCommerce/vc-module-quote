using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteAttachmentType : ExtendableGraphType<QuoteAttachment>
{
    public QuoteAttachmentType()
    {
        Field(x => x.Name, nullable: true);
        Field(x => x.Url, nullable: false);
        Field(x => x.MimeType, nullable: true);
        Field(x => x.Size, nullable: false);
    }
}
