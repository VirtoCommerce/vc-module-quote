using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteRequestSearchResult : GenericSearchResult<QuoteRequest>
    {
        /// <summary>
        /// Gets the results for previous version compatibility.
        /// </summary>
        /// <value>
        /// The found RFQs.
        /// </value>
        [Obsolete("Will be removed in future versions. Use Results instead")]
        public IList<QuoteRequest> QuoteRequests => Results;
    }
}
