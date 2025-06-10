using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using GraphQL;
using MediatR;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Validation;
using VirtoCommerce.Xapi.Core.Helpers;
using VirtoCommerce.XCart.Core.Services;
using VirtoCommerce.XCart.Core.Validators;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommandHandler : IRequestHandler<CreateQuoteFromCartCommand, QuoteAggregate>
{
    private readonly IShoppingCartService _cartService;
    private readonly ICartAggregateRepository _cartRepository;
    private readonly ICartValidationContextFactory _cartValidationContextFactory;
    private readonly IQuoteConverter _quoteConverter;
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    public CreateQuoteFromCartCommandHandler(
        IShoppingCartService cartService,
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
        var quote = await _quoteConverter.ConvertFromCart(cart);
        quote.Comment = request.Comment;
        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        await _cartService.DeleteAsync(new[] { request.CartId }, softDelete: true);

        return await _quoteAggregateRepository.GetById(quote.Id);
    }

    protected virtual async Task ValidateCart(ShoppingCart cart)
    {
        var cartAggregate = await _cartRepository.GetCartForShoppingCartAsync(cart);
        var context = await _cartValidationContextFactory.CreateValidationContextAsync(cartAggregate);

        // do not validate items, shipments, payments; only basic validation using default cart validator
        await cartAggregate.ValidateAsync(context, "default");

        // custom validate cart line items (for deleted products)
        var lineItemValidationErrors = new List<ValidationFailure>();
        var lineItemValidator = AbstractTypeFactory<CartToQuoteLineItemValidator>.TryCreateInstance();
        cartAggregate.Cart.Items?.Apply(item =>
        {
            var lineItemContext = new CartToQuoteLineItemValidationContext
            {
                LineItem = item,
                AllCartProducts = context.AllCartProducts ?? context.CartAggregate.CartProducts.Values
            };
            var result = lineItemValidator.Validate(lineItemContext);
            lineItemValidationErrors.AddRange(result.Errors);
        });

        // combine all errors
        if (cartAggregate.GetValidationErrors().Any() || lineItemValidationErrors.Any())
        {
            var errors = cartAggregate.GetValidationErrors()
                .Union(lineItemValidationErrors)
                .GroupBy(x => x.ErrorCode)
                .ToDictionary(x => x.Key, x => x.First().ErrorMessage);

            throw new ExecutionError("The cart has validation errors", errors) { Code = Constants.ValidationErrorCode };
        }
    }
}
