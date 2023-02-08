using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Model;
using VirtoCommerce.ShippingModule.Core.Model.Search;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.StoreModule.Core.Model;
using OrderPermissions = VirtoCommerce.OrdersModule.Core.ModuleConstants.Security.Permissions;
using QuotePermissions = VirtoCommerce.QuoteModule.Core.ModuleConstants.Security.Permissions;
using ShipmentMethod = VirtoCommerce.QuoteModule.Core.Models.ShipmentMethod;

namespace VirtoCommerce.QuoteModule.Web.Controllers.Api
{
    [Route("api/quote/requests")]
    [Authorize(QuotePermissions.Read)]
    public class QuoteModuleController : Controller
    {
        private readonly IQuoteRequestService _quoteRequestService;
        private readonly IQuoteTotalsCalculator _totalsCalculator;
        private readonly ICrudService<Store> _storeService;
        private readonly ISearchService<ShippingMethodsSearchCriteria, ShippingMethodsSearchResult, ShippingMethod> _shippingMethodsSearchService;
        private readonly IQuoteConverter _quoteConverter;
        private readonly ICustomerOrderBuilder _customerOrderBuilder;

        public QuoteModuleController(
            IQuoteRequestService quoteRequestService,
            IQuoteTotalsCalculator totalsCalculator,
            ICrudService<Store> storeService,
            IShippingMethodsSearchService shippingMethodsSearchService,
            IQuoteConverter quoteConverter,
            ICustomerOrderBuilder customerOrderBuilder)
        {
            _quoteRequestService = quoteRequestService;
            _totalsCalculator = totalsCalculator;
            _storeService = storeService;
            _shippingMethodsSearchService = (ISearchService<ShippingMethodsSearchCriteria, ShippingMethodsSearchResult, ShippingMethod>)shippingMethodsSearchService;
            _quoteConverter = quoteConverter;
            _customerOrderBuilder = customerOrderBuilder;
        }

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<QuoteRequestSearchResult>> Search([FromBody] QuoteRequestSearchCriteria criteria)
        {
            var retVal = await _quoteRequestService.SearchAsync(criteria);
            return Ok(retVal);
        }

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>Return a single RFQ</remarks>
        /// <param name="id">RFQ id</param>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(QuoteRequest), StatusCodes.Status200OK)]
        public async Task<ActionResult<QuoteRequest>> GetById([FromRoute] string id)
        {
            var quote = (await _quoteRequestService.GetByIdsAsync(id)).FirstOrDefault();

            if (quote == null)
            {
                quote = (await _quoteRequestService.SearchAsync(new QuoteRequestSearchCriteria { Number = id })).Results.FirstOrDefault();
            }

            if (quote == null)
            {
                return NotFound();
            }

            quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);
            return Ok(quote);
        }

        /// <summary>
        ///  Create new RFQ
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        [HttpPost]
        [Route("")]
        [Authorize(QuotePermissions.Create)]
        public async Task<ActionResult<QuoteRequest>> Create([FromBody] QuoteRequest quoteRequest)
        {
            await _quoteRequestService.SaveChangesAsync(new[] { quoteRequest });
            return Ok(quoteRequest);
        }

        /// <summary>
        ///  Update a existing RFQ
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [Authorize(QuotePermissions.Update)]
        public async Task<ActionResult> Update([FromBody] QuoteRequest quoteRequest)
        {
            await _quoteRequestService.SaveChangesAsync(new[] { quoteRequest });
            return NoContent();
        }

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>Return totals for selected tier prices</remarks>
        /// <param name="quoteRequest">RFQ</param>
        [HttpPut]
        [Route("recalculate")]
        public async Task<ActionResult<QuoteRequest>> CalculateTotals([FromBody] QuoteRequest quoteRequest)
        {
            quoteRequest.Totals = await _totalsCalculator.CalculateTotalsAsync(quoteRequest);
            return Ok(quoteRequest);
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <param name="id">RFQ id</param>
        [HttpGet]
        [Route("{id}/shipmentmethods")]
        public async Task<ActionResult<ShipmentMethod[]>> GetShipmentMethods([FromRoute] string id)
        {
            var quote = (await _quoteRequestService.GetByIdsAsync(id)).FirstOrDefault();
            if (quote != null)
            {
                var store = await _storeService.GetByIdAsync(quote.StoreId);

                if (store != null)
                {
                    var cartFromQuote = quote.ToCartModel(AbstractTypeFactory<ShoppingCart>.TryCreateInstance());
                    var evalContext = new ShippingEvaluationContext(cartFromQuote);

                    var searchCriteria = AbstractTypeFactory<ShippingMethodsSearchCriteria>.TryCreateInstance();
                    searchCriteria.StoreId = quote.StoreId;
                    searchCriteria.IsActive = true;
                    searchCriteria.WithoutTransient = true;
                    var storeShippingMethods = await _shippingMethodsSearchService.SearchAsync(searchCriteria);
                    var rates = storeShippingMethods.Results
                        .SelectMany(x => x.CalculateRates(evalContext))
                        .ToArray();

                    var retVal = rates.Select(x => new ShipmentMethod
                    {
                        Currency = x.Currency,
                        Price = x.Rate,
                        OptionName = x.OptionName,
                        ShipmentMethodCode = x.ShippingMethod.Code,
                        TypeName = x.ShippingMethod.TypeName,
                        LogoUrl = x.ShippingMethod.LogoUrl
                    }).ToArray();

                    return Ok(retVal);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <param name="ids">The quotes ids.</param>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [Authorize(QuotePermissions.Delete)]
        public async Task<ActionResult> Delete([FromQuery] string[] ids)
        {
            await _quoteRequestService.DeleteAsync(ids);
            return NoContent();
        }

        /// <summary>
        /// Create new customer order based on quote request.
        /// </summary>
        /// <param name="id">Quote request ID</param>
        [HttpPost]
        [Route("{id}/order")]
        [Authorize(OrderPermissions.Create)]
        public async Task<ActionResult<CustomerOrder>> CreateOrderFromQuote([FromRoute] string id)
        {
            var quote = (await _quoteRequestService.GetByIdsAsync(id)).FirstOrDefault();

            if (quote == null)
            {
                return NotFound();
            }

            quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);

            var cart = _quoteConverter.ConvertToCart(quote);
            var order = await _customerOrderBuilder.PlaceCustomerOrderFromCartAsync(cart);

            return Ok(order);
        }

        /// <summary>
        /// Update quote request status.
        /// </summary>
        /// <param name="id">Quote request ID</param>
        /// <param name="status">New status</param>
        [HttpPut]
        [Route("{id}/status")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [Authorize(QuotePermissions.Update)]
        public async Task<ActionResult> UpdateStatus([FromRoute] string id, [FromQuery] string status)
        {
            var quote = (await _quoteRequestService.GetByIdsAsync(id)).FirstOrDefault();

            if (quote == null)
            {
                return NotFound();
            }

            quote.Status = status;
            await _quoteRequestService.SaveChangesAsync(new[] { quote });

            return NoContent();
        }
    }
}
