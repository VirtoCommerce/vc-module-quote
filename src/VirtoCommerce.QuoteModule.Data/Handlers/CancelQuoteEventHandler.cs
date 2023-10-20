using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Data.Handlers
{
    public class CancelQuoteEventHandler : IEventHandler<QuoteRequestChangeEvent>
    {
        private readonly IQuoteRequestService _service;

        public CancelQuoteEventHandler(IQuoteRequestService service)
        {
            _service = service;
        }

        public async Task Handle(QuoteRequestChangeEvent message)
        {
            var changes = message.ChangedEntries.Where(x =>
                x.NewEntry.IsCancelled && !x.OldEntry.IsCancelled && !x.NewEntry.CancelledDate.HasValue).ToList();
            if (changes.Any())
            {
                var entries = changes.Select(x => x.NewEntry).ToArray();
                foreach (var entry in entries)
                {
                    entry.CancelledDate = DateTime.UtcNow;
                }

                await _service.SaveChangesAsync(entries);
            }
        }
    }
}
