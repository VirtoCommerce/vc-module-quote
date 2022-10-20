using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.XPurchase;
using VirtoCommerce.XPurchase.Validators;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommandHandler : IRequestHandler<CreateQuoteFromCartCommand, QuoteAggregate>
{
    private readonly ICrudService<ShoppingCart> _cartService;
    private readonly ICartAggregateRepository _cartRepository;
    private readonly ICartValidationContextFactory _cartValidationContextFactory;
    private readonly IQuoteConverter _quoteConverter;
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    public CreateQuoteFromCartCommandHandler(
        ICrudService<ShoppingCart> cartService,
        ICartAggregateRepository cartRepository,
        ICartValidationContextFactory cartValidationContextFactory,
        IQuoteConverter quoteConverter,
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository)
    {
        _cartService = cartService;
        _cartRepository = cartRepository;
        _cartValidationContextFactory = cartValidationContextFactory;
        _quoteConverter = quoteConverter;
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
    }

    public async Task<QuoteAggregate> Handle(CreateQuoteFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetByIdAsync(request.CartId);

        if (cart == null)
        {
            return null;
        }

        await ValidateCart(cart);

        // Create quote
        var quote = _quoteConverter.ConvertFromCart(cart);
        quote.Comment = request.Comment;
        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        await _cartService.DeleteAsync(new[] { request.CartId }, softDelete: true);

        return await _quoteAggregateRepository.GetById(quote.Id);
    }

    protected virtual async Task ValidateCart(ShoppingCart cart)
    {
        var cartAggregate = await _cartRepository.GetCartForShoppingCartAsync(cart);
        var context = await _cartValidationContextFactory.CreateValidationContextAsync(cartAggregate);

        // do not validate items, shipments, payments; only basic validation
        await cartAggregate.ValidateAsync(context, "default");

        if (cartAggregate.ValidationErrors.Any() || cartAggregate.ValidationWarnings.Any())
        {
            var errors = cartAggregate.ValidationErrors
                .Union(cartAggregate.ValidationWarnings)
                .GroupBy(x => x.ErrorCode)
                .ToDictionary(x => x.Key, x => x.First().ErrorMessage);

            throw new ExecutionError("The cart has validation errors", errors) { Code = Constants.ValidationErrorCode };
        }
    }
}
