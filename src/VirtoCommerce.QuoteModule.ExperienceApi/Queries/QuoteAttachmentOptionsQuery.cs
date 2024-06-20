using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.FileExperienceApi.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteAttachmentOptionsQuery : Query<FileUploadScopeOptions>
{
    public override IEnumerable<QueryArgument> GetArguments()
    {
        yield break;
    }

    public override void Map(IResolveFieldContext context)
    {
    }
}
