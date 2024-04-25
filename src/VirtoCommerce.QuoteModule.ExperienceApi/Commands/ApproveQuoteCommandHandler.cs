using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ApproveQuoteCommandHandler(
        ICustomerOrderBuilder customerOrderBuilder,
        IQuoteRequestService quoteRequestService,
        IQuoteTotalsCalculator totalsCalculator)
    : IRequestHandler<ApproveQuoteCommand, ApproveQuoteResult>
{
    public async Task<ApproveQuoteResult> Handle(ApproveQuoteCommand request, CancellationToken cancellationToken)
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

        var totals = await totalsCalculator.CalculateTotalsAsync(quote);

        quote.Status = QuoteStatus.Ordered;
        var cart = quote.ToCartModel(new ShoppingCart(), totals);

        foreach (var cartItem in cart.Items)
        {
            cartItem.ListPrice = cartItem.SalePrice;
        }

        var order = await customerOrderBuilder.PlaceCustomerOrderFromCartAsync(cart);
        await quoteRequestService.SaveChangesAsync(new[] { quote });

        return new ApproveQuoteResult
        {
            Id = quote.Id,
            OrderId = order.Id,
        };
    }
}
