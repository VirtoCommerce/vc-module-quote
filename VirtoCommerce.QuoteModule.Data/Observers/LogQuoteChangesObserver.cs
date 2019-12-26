using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Quote.Events;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Platform.Core.ChangeLog;

namespace VirtoCommerce.QuoteModule.Data.Observers
{
    public class LogQuoteChangesObserver : IObserver<QuoteRequestChangeEvent>
    {
        private readonly IChangeLogService _changeLogService;
        public LogQuoteChangesObserver(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        #region IObserver<QuoteRequestChangeEvent> Members

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(QuoteRequestChangeEvent value)
        {
            var changedEntries = value.ChangedEntries;
            foreach (var item in changedEntries)
            {
                var origQuote = item.OldEntry;
                var modifiedQuote = item.NewEntry;

                if (item.EntryState == Platform.Core.Common.EntryState.Modified)
                {
                    var operationLog = new OperationLog
                    {
                        ObjectId = item.NewEntry.Id,
                        ObjectType = typeof(QuoteRequest).Name,
                        OperationType = item.EntryState
                    };

                    if (origQuote.Status != modifiedQuote.Status)
                    {
                        operationLog.Detail += String.Format("status changed from {0} -> {1} ", origQuote.Status ?? "undef", modifiedQuote.Status ?? "undef");
                    }
                    if (origQuote.Comment != modifiedQuote.Comment)
                    {
                        operationLog.Detail += String.Format("comment changed ");
                    }
                    if (origQuote.IsLocked != modifiedQuote.IsLocked)
                    {
                        operationLog.Detail += modifiedQuote.IsLocked ? "lock " : "unlock ";
                    }
                    _changeLogService.SaveChanges(operationLog);
                }
            }

        }

        #endregion

    }
}
