using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Model.Search;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Web.Controllers.Api
{
    [Route("api/quote/requests")]
    [Authorize(Core.ModuleConstants.Security.Permissions.Read)]
    public class QuoteModuleController : Controller
    {
        private readonly IQuoteRequestService _quoteRequestService;
        private readonly IQuoteTotalsCalculator _totalsCalculator;
        private readonly IStoreService _storeService;
        private readonly IShippingMethodsSearchService _shippingMethodsSearchService;

        public QuoteModuleController(IQuoteRequestService quoteRequestService, IQuoteTotalsCalculator totalsCalculator, IStoreService storeService, IShippingMethodsSearchService shippingMethodsSearchService)
        {
            _quoteRequestService = quoteRequestService;
            _totalsCalculator = totalsCalculator;
            _storeService = storeService;
            _shippingMethodsSearchService = shippingMethodsSearchService;
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
        [ProducesResponseType(typeof(QuoteRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuoteRequest>> GetById(string id)
        {
            var quote = (await _quoteRequestService.GetByIdsAsync(new[] { id })).FirstOrDefault();

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
        [Authorize(Core.ModuleConstants.Security.Permissions.Create)]
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
        [Authorize(Core.ModuleConstants.Security.Permissions.Update)]
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
        public async Task<ActionResult<Core.Models.ShipmentMethod[]>> GetShipmentMethods(string id)
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
                    var storeShippingMethods = await _shippingMethodsSearchService.SearchShippingMethodsAsync(searchCriteria);
                    var rates = storeShippingMethods.Results
                        .SelectMany(x => x.CalculateRates(evalContext))
                        .ToArray();

                    var retVal = rates.Select(x => new Core.Models.ShipmentMethod
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
        [Authorize(Core.ModuleConstants.Security.Permissions.Delete)]
        public async Task<ActionResult> Delete([FromQuery] string[] ids)
        {
            await _quoteRequestService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
