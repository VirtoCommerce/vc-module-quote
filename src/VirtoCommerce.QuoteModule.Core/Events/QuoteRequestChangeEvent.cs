using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Core.Events
{
    public class QuoteRequestChangeEvent : GenericChangedEntryEvent<QuoteRequest>
    {
        public QuoteRequestChangeEvent(IEnumerable<GenericChangedEntry<QuoteRequest>> changedEntries)
         : base(changedEntries)
        {
        }
    }
}
