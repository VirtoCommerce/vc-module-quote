using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteRequestSearchCriteria : SearchCriteriaBase
    {
        private string[] _statuses;

        // strict number search
        public string Number { get; set; }

        // search number by contains keyword
        public string NumberKeyword { get; set; }

        public string CustomerId { get; set; }
        public string[] OrganizationIds { get; set; }
        public string StoreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Tag { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }

        // don't change type into a generic enumerable (yet)
        public string[] Statuses
        {
            get
            {
                if (_statuses == null && !string.IsNullOrEmpty(Status))
                {
                    _statuses = new[] { Status };
                }
                return _statuses;
            }
            set
            {
                _statuses = value;
            }
        }
    }
}
