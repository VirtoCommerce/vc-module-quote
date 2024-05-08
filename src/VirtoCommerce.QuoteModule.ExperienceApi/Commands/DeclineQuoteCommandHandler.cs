using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeclineQuoteCommandHandler(
    IQuoteRequestService quoteRequestService,
    IQuoteAggregateRepository quoteAggregateRepository)
    : IRequestHandler<DeclineQuoteCommand, QuoteAggregate>
{
    public async Task<QuoteAggregate> Handle(DeclineQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = (await quoteRequestService.GetByIdsAsync(request.QuoteId)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        if (quote.Status != QuoteStatus.ProposalSent)
        {
            throw new ExecutionError($"Quote status is not '{QuoteStatus.ProposalSent}'") { Code = Constants.ValidationErrorCode };
        }

        quote.Status = QuoteStatus.Declined;
        quote.Comment = request.Comment;

        await quoteRequestService.SaveChangesAsync(new[] { quote });

        return await quoteAggregateRepository.GetById(quote.Id);
    }
}
