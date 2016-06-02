using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.QuoteModule.Client.Client;
using VirtoCommerce.QuoteModule.Client.Model;

namespace VirtoCommerce.QuoteModule.Client.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IVirtoCommerceQuoteApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>QuoteRequest</returns>
        QuoteRequest QuoteModuleCalculateTotals(QuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        ApiResponse<QuoteRequest> QuoteModuleCalculateTotalsWithHttpInfo(QuoteRequest quoteRequest);
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>QuoteRequest</returns>
        QuoteRequest QuoteModuleCreate(QuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        ApiResponse<QuoteRequest> QuoteModuleCreateWithHttpInfo(QuoteRequest quoteRequest);
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        void QuoteModuleDelete(List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleDeleteWithHttpInfo(List<string> ids);
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>QuoteRequest</returns>
        QuoteRequest QuoteModuleGetById(string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        ApiResponse<QuoteRequest> QuoteModuleGetByIdWithHttpInfo(string id);
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>List&lt;ShipmentMethod&gt;</returns>
        List<ShipmentMethod> QuoteModuleGetShipmentMethods(string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of List&lt;ShipmentMethod&gt;</returns>
        ApiResponse<List<ShipmentMethod>> QuoteModuleGetShipmentMethodsWithHttpInfo(string id);
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>QuoteRequestSearchResult</returns>
        QuoteRequestSearchResult QuoteModuleSearch(QuoteRequestSearchCriteria criteria);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of QuoteRequestSearchResult</returns>
        ApiResponse<QuoteRequestSearchResult> QuoteModuleSearchWithHttpInfo(QuoteRequestSearchCriteria criteria);
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        void QuoteModuleUpdate(QuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleUpdateWithHttpInfo(QuoteRequest quoteRequest);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of QuoteRequest</returns>
        System.Threading.Tasks.Task<QuoteRequest> QuoteModuleCalculateTotalsAsync(QuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo(QuoteRequest quoteRequest);
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of QuoteRequest</returns>
        System.Threading.Tasks.Task<QuoteRequest> QuoteModuleCreateAsync(QuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo(QuoteRequest quoteRequest);
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleDeleteAsync(List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> QuoteModuleDeleteAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of QuoteRequest</returns>
        System.Threading.Tasks.Task<QuoteRequest> QuoteModuleGetByIdAsync(string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;ShipmentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<ShipmentMethod>> QuoteModuleGetShipmentMethodsAsync(string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;ShipmentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo(string id);
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of QuoteRequestSearchResult</returns>
        System.Threading.Tasks.Task<QuoteRequestSearchResult> QuoteModuleSearchAsync(QuoteRequestSearchCriteria criteria);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (QuoteRequestSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<QuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo(QuoteRequestSearchCriteria criteria);
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleUpdateAsync(QuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> QuoteModuleUpdateAsyncWithHttpInfo(QuoteRequest quoteRequest);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class VirtoCommerceQuoteApi : IVirtoCommerceQuoteApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceQuoteApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="apiClient">An instance of ApiClient.</param>
        /// <returns></returns>
        public VirtoCommerceQuoteApi(ApiClient apiClient)
        {
            ApiClient = apiClient;
            Configuration = apiClient.Configuration;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the API client object
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>QuoteRequest</returns>
        public QuoteRequest QuoteModuleCalculateTotals(QuoteRequest quoteRequest)
        {
             ApiResponse<QuoteRequest> localVarResponse = QuoteModuleCalculateTotalsWithHttpInfo(quoteRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        public ApiResponse<QuoteRequest> QuoteModuleCalculateTotalsWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleCalculateTotals");

            var localVarPath = "/api/quote/requests/recalculate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of QuoteRequest</returns>
        public async System.Threading.Tasks.Task<QuoteRequest> QuoteModuleCalculateTotalsAsync(QuoteRequest quoteRequest)
        {
             ApiResponse<QuoteRequest> localVarResponse = await QuoteModuleCalculateTotalsAsyncWithHttpInfo(quoteRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleCalculateTotals");

            var localVarPath = "/api/quote/requests/recalculate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }
        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>QuoteRequest</returns>
        public QuoteRequest QuoteModuleCreate(QuoteRequest quoteRequest)
        {
             ApiResponse<QuoteRequest> localVarResponse = QuoteModuleCreateWithHttpInfo(quoteRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        public ApiResponse<QuoteRequest> QuoteModuleCreateWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleCreate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of QuoteRequest</returns>
        public async System.Threading.Tasks.Task<QuoteRequest> QuoteModuleCreateAsync(QuoteRequest quoteRequest)
        {
             ApiResponse<QuoteRequest> localVarResponse = await QuoteModuleCreateAsyncWithHttpInfo(quoteRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleCreate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }
        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        public void QuoteModuleDelete(List<string> ids)
        {
             QuoteModuleDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> QuoteModuleDeleteWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceQuoteApi->QuoteModuleDelete");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleDeleteAsync(List<string> ids)
        {
             await QuoteModuleDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> QuoteModuleDeleteAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceQuoteApi->QuoteModuleDelete");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>QuoteRequest</returns>
        public QuoteRequest QuoteModuleGetById(string id)
        {
             ApiResponse<QuoteRequest> localVarResponse = QuoteModuleGetByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of QuoteRequest</returns>
        public ApiResponse<QuoteRequest> QuoteModuleGetByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceQuoteApi->QuoteModuleGetById");

            var localVarPath = "/api/quote/requests/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of QuoteRequest</returns>
        public async System.Threading.Tasks.Task<QuoteRequest> QuoteModuleGetByIdAsync(string id)
        {
             ApiResponse<QuoteRequest> localVarResponse = await QuoteModuleGetByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (QuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<QuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceQuoteApi->QuoteModuleGetById");

            var localVarPath = "/api/quote/requests/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequest)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequest)));
            
        }
        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>List&lt;ShipmentMethod&gt;</returns>
        public List<ShipmentMethod> QuoteModuleGetShipmentMethods(string id)
        {
             ApiResponse<List<ShipmentMethod>> localVarResponse = QuoteModuleGetShipmentMethodsWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of List&lt;ShipmentMethod&gt;</returns>
        public ApiResponse<List<ShipmentMethod>> QuoteModuleGetShipmentMethodsWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceQuoteApi->QuoteModuleGetShipmentMethods");

            var localVarPath = "/api/quote/requests/{id}/shipmentmethods";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ShipmentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ShipmentMethod>)ApiClient.Deserialize(localVarResponse, typeof(List<ShipmentMethod>)));
            
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;ShipmentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<ShipmentMethod>> QuoteModuleGetShipmentMethodsAsync(string id)
        {
             ApiResponse<List<ShipmentMethod>> localVarResponse = await QuoteModuleGetShipmentMethodsAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;ShipmentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<ShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceQuoteApi->QuoteModuleGetShipmentMethods");

            var localVarPath = "/api/quote/requests/{id}/shipmentmethods";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ShipmentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ShipmentMethod>)ApiClient.Deserialize(localVarResponse, typeof(List<ShipmentMethod>)));
            
        }
        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>QuoteRequestSearchResult</returns>
        public QuoteRequestSearchResult QuoteModuleSearch(QuoteRequestSearchCriteria criteria)
        {
             ApiResponse<QuoteRequestSearchResult> localVarResponse = QuoteModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of QuoteRequestSearchResult</returns>
        public ApiResponse<QuoteRequestSearchResult> QuoteModuleSearchWithHttpInfo(QuoteRequestSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommerceQuoteApi->QuoteModuleSearch");

            var localVarPath = "/api/quote/requests/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequestSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequestSearchResult)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequestSearchResult)));
            
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of QuoteRequestSearchResult</returns>
        public async System.Threading.Tasks.Task<QuoteRequestSearchResult> QuoteModuleSearchAsync(QuoteRequestSearchCriteria criteria)
        {
             ApiResponse<QuoteRequestSearchResult> localVarResponse = await QuoteModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (QuoteRequestSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<QuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo(QuoteRequestSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommerceQuoteApi->QuoteModuleSearch");

            var localVarPath = "/api/quote/requests/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<QuoteRequestSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (QuoteRequestSearchResult)ApiClient.Deserialize(localVarResponse, typeof(QuoteRequestSearchResult)));
            
        }
        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        public void QuoteModuleUpdate(QuoteRequest quoteRequest)
        {
             QuoteModuleUpdateWithHttpInfo(quoteRequest);
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> QuoteModuleUpdateWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleUpdate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleUpdateAsync(QuoteRequest quoteRequest)
        {
             await QuoteModuleUpdateAsyncWithHttpInfo(quoteRequest);

        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.QuoteModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> QuoteModuleUpdateAsyncWithHttpInfo(QuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling VirtoCommerceQuoteApi->QuoteModuleUpdate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    }
}
