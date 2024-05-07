using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ApproveQuoteCommandHandler : IRequestHandler<ApproveQuoteCommand, ApproveQuoteResult>
{
    private readonly ICustomerOrderBuilder _customerOrderBuilder;
    private readonly IQuoteConverter _quoteConverter;
    private readonly IQuoteRequestService _quoteRequestService;

    public ApproveQuoteCommandHandler(ICustomerOrderBuilder customerOrderBuilder,
        IQuoteConverter quoteConverter,
        IQuoteRequestService quoteRequestService)
    {
        _customerOrderBuilder = customerOrderBuilder;
        _quoteConverter = quoteConverter;
        _quoteRequestService = quoteRequestService;
    }

    public async Task<ApproveQuoteResult> Handle(ApproveQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = (await _quoteRequestService.GetByIdsAsync(request.QuoteId)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        if (quote.Status != QuoteStatus.ProposalSent)
        {
            throw new ExecutionError($"Quote status is not '{QuoteStatus.ProposalSent}'") { Code = Constants.ValidationErrorCode };
        }

        quote.Status = QuoteStatus.Ordered;
        var cart = await _quoteConverter.ConvertToCartWithTax(quote);
        var order = await _customerOrderBuilder.PlaceCustomerOrderFromCartAsync(cart);

        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        return new ApproveQuoteResult
        {
            Id = quote.Id,
            OrderId = order.Id,
        };
    }
}
