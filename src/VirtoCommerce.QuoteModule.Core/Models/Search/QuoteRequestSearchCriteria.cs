using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteRequestSearchCriteria : SearchCriteriaBase
    {
        public string Number { get; set; }
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
    }
}
