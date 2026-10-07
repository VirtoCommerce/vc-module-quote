using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Internal;
using FluentValidation.Results;
using GraphQL;
using Moq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Services;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Commands;
using VirtoCommerce.XCart.Core;
using VirtoCommerce.XCart.Core.Services;
using VirtoCommerce.XCart.Core.Validators;
using Xunit;
using Address = VirtoCommerce.CartModule.Core.Model.Address;
using Store = VirtoCommerce.StoreModule.Core.Model.Store;

namespace VirtoCommerce.QuoteModule.Tests.Handlers;

public class CreateQuoteFromCartCommandHandlerTests
{
    /// <summary>
    /// A derived aggregate can post-process validation results in a ValidateAsync(string) override (VCST-6089).
    /// The obsolete CartValidationErrors mirror, which GetValidationErrors() reads, only ever holds the base result,
    /// so an error the override adds never reached the old gate, and the invalid quote was created.
    /// </summary>
    [Fact]
    public async Task Handle_ValidateAsyncOverrideAddsError_ExceptionThrown()
    {
        // Arrange
        var extraError = new ValidationFailure("Cart", "Rejected by a project rule") { ErrorCode = "PROJECT_RULE" };
        var cartAggregate = new ExtraErrorCartAggregate(GetValidationContextFactory(), GetValidatorRegistry(), extraError);
        cartAggregate.GrabCart(GetEmptyCart(), new Store(), new Contact(), new Currency());

        var quoteRequestService = new Mock<IQuoteRequestService>();
        var handler = GetHandler(cartAggregate, quoteRequestService.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ExecutionError>(() => handler.Handle(new CreateQuoteFromCartCommand { CartId = cartAggregate.Cart.Id }, CancellationToken.None));

        // Assert
        exception.Data.Contains("PROJECT_RULE").Should().BeTrue();
        quoteRequestService.Verify(x => x.SaveChangesAsync(It.IsAny<QuoteRequest[]>()), Times.Never);
    }

    /// <summary>
    /// The reverse direction: an override that filters out an error the base validation found must let the quote
    /// through. The mirror still holds the unfiltered base result, so the old gate refused this valid quote.
    /// </summary>
    [Fact]
    public async Task Handle_ValidateAsyncOverrideRemovesError_QuoteCreated()
    {
        // Arrange
        var baseError = new ValidationFailure("Cart", "Waived by a project rule") { ErrorCode = "WAIVED_RULE" };
        var cartAggregate = new FilteringCartAggregate(GetValidationContextFactory(), GetValidatorRegistry(baseError), baseError.ErrorCode);
        cartAggregate.GrabCart(GetEmptyCart(), new Store(), new Contact(), new Currency());

        var quoteRequestService = new Mock<IQuoteRequestService>();
        var handler = GetHandler(cartAggregate, quoteRequestService.Object);

        // Act
        await handler.Handle(new CreateQuoteFromCartCommand { CartId = cartAggregate.Cart.Id }, CancellationToken.None);

        // Assert
        quoteRequestService.Verify(x => x.SaveChangesAsync(It.IsAny<QuoteRequest[]>()), Times.Once);
    }

    /// <summary>
    /// Errors recorded by cart operations (OperationValidationErrors) refuse the quote even when the "default"
    /// ruleSet itself is clean: the gate adds them to the ValidateAsync result, as GetValidationErrors() did.
    /// </summary>
    [Fact]
    public async Task Handle_OperationValidationErrors_ExceptionThrown()
    {
        // Arrange
        var cartAggregate = GetCartAggregate(GetValidatorRegistry());
        cartAggregate.OperationValidationErrors.Add(new ValidationFailure("LineItem", "The product is no longer available") { ErrorCode = "OPERATION_ERROR" });

        var quoteRequestService = new Mock<IQuoteRequestService>();
        var handler = GetHandler(cartAggregate, quoteRequestService.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ExecutionError>(() => handler.Handle(new CreateQuoteFromCartCommand { CartId = cartAggregate.Cart.Id }, CancellationToken.None));

        // Assert
        exception.Data.Contains("OPERATION_ERROR").Should().BeTrue();
        quoteRequestService.Verify(x => x.SaveChangesAsync(It.IsAny<QuoteRequest[]>()), Times.Never);
    }

    /// <summary>
    /// The handler's own line-item check (a product that no longer exists) is part of the same combined error list,
    /// so it still refuses the quote when the "default" ruleSet is clean.
    /// </summary>
    [Fact]
    public async Task Handle_LineItemProductDeleted_ExceptionThrown()
    {
        // Arrange
        var cartAggregate = GetCartAggregate(GetValidatorRegistry());
        cartAggregate.Cart.Items.Add(new LineItem { Id = "line-1", ProductId = "deleted-product" });

        var quoteRequestService = new Mock<IQuoteRequestService>();
        var handler = GetHandler(cartAggregate, quoteRequestService.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ExecutionError>(() => handler.Handle(new CreateQuoteFromCartCommand { CartId = cartAggregate.Cart.Id }, CancellationToken.None));

        // Assert
        exception.Data.Contains("CART_PRODUCT_UNAVAILABLE").Should().BeTrue();
        quoteRequestService.Verify(x => x.SaveChangesAsync(It.IsAny<QuoteRequest[]>()), Times.Never);
    }

    private static ShoppingCart GetEmptyCart()
    {
        return new ShoppingCart
        {
            Id = "cart-1",
            Name = "default",
            Currency = "USD",
            CustomerId = Guid.NewGuid().ToString(),
            Items = new List<LineItem>(),
            Shipments = new List<Shipment>(),
            Payments = new List<Payment>(),
            Addresses = new List<Address>(),
            Coupons = new List<string>(),
        };
    }

    private static CartAggregate GetCartAggregate(ICartValidatorRegistry validatorRegistry)
    {
        var cartAggregate = new CartAggregate(null, null, null, null, null, null, null, null, null, GetValidationContextFactory(), Mock.Of<ICartItemBuilder>(), validatorRegistry);
        cartAggregate.GrabCart(GetEmptyCart(), new Store(), new Contact(), new Currency());

        return cartAggregate;
    }

    private static ICartValidatorRegistry GetValidatorRegistry(params ValidationFailure[] errors)
    {
        var validatorRegistry = new Mock<ICartValidatorRegistry>();
        validatorRegistry
            .Setup(x => x.ValidateAsync(It.IsAny<CartValidationContext>(), It.IsAny<Action<ValidationStrategy<CartValidationContext>>>()))
            .ReturnsAsync(new List<ValidationFailure>(errors));

        return validatorRegistry.Object;
    }

    private static ICartValidationContextFactory GetValidationContextFactory()
    {
        var validationContextFactory = new Mock<ICartValidationContextFactory>();
        validationContextFactory
            .Setup(x => x.CreateValidationContextAsync(It.IsAny<CartAggregate>()))
            .ReturnsAsync(new CartValidationContext());

        return validationContextFactory.Object;
    }

    private static CreateQuoteFromCartCommandHandler GetHandler(CartAggregate cartAggregate, IQuoteRequestService quoteRequestService)
    {
        var cartService = new Mock<IShoppingCartService>();
        cartService
            .Setup(x => x.GetAsync(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(new List<ShoppingCart> { cartAggregate.Cart });

        var cartRepository = new Mock<ICartAggregateRepository>();
        cartRepository
            .Setup(x => x.GetCartForShoppingCartAsync(It.IsAny<ShoppingCart>(), It.IsAny<string>()))
            .ReturnsAsync(cartAggregate);

        var quoteConverter = new Mock<IQuoteConverter>();
        quoteConverter
            .Setup(x => x.ConvertFromCart(It.IsAny<ShoppingCart>()))
            .ReturnsAsync(new QuoteRequest { Id = "quote-1" });

        return new CreateQuoteFromCartCommandHandler(
            cartService.Object,
            cartRepository.Object,
            GetValidationContextFactory(),
            quoteConverter.Object,
            quoteRequestService,
            Mock.Of<IQuoteAggregateRepository>());
    }

    /// <summary>
    /// Post-processes validation the way a project's derived aggregate would: the list it returns has one more
    /// error than the base result, which is all the obsolete CartValidationErrors mirror ever holds.
    /// </summary>
    private sealed class ExtraErrorCartAggregate : CartAggregate
    {
        private readonly ValidationFailure _extraError;

        public ExtraErrorCartAggregate(ICartValidationContextFactory validationContextFactory, ICartValidatorRegistry validatorRegistry, ValidationFailure extraError)
            : base(null, null, null, null, null, null, null, null, null, validationContextFactory, Mock.Of<ICartItemBuilder>(), validatorRegistry)
        {
            _extraError = extraError;
        }

        public override async Task<IList<ValidationFailure>> ValidateAsync(string ruleSet)
        {
            var errors = await base.ValidateAsync(ruleSet);

            // A new list: the one base returns is the cached result, so it must not be changed in place.
            return new List<ValidationFailure>(errors) { _extraError };
        }
    }

    /// <summary>
    /// The opposite post-processing: drops the errors with one code from the base result and returns the rest
    /// as a new list, while the mirror keeps the unfiltered base result.
    /// </summary>
    private sealed class FilteringCartAggregate : CartAggregate
    {
        private readonly string _waivedErrorCode;

        public FilteringCartAggregate(ICartValidationContextFactory validationContextFactory, ICartValidatorRegistry validatorRegistry, string waivedErrorCode)
            : base(null, null, null, null, null, null, null, null, null, validationContextFactory, Mock.Of<ICartItemBuilder>(), validatorRegistry)
        {
            _waivedErrorCode = waivedErrorCode;
        }

        public override async Task<IList<ValidationFailure>> ValidateAsync(string ruleSet)
        {
            var errors = await base.ValidateAsync(ruleSet);

            return errors.Where(x => x.ErrorCode != _waivedErrorCode).ToList();
        }
    }
}
