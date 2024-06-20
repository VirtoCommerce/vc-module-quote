using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class ApproveQuoteResultType : ExtendableGraphType<ApproveQuoteResult>
{
    public ApproveQuoteResultType()
    {
        Field(x => x.Id, nullable: false);
        Field(x => x.OrderId, nullable: true);
    }
}
