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
            //var origQuote = value.OrigQuote;
            var origQuote = value.ChangedEntries.Select(x => x.OldEntry);

            var modifiedQuote = value.ChangedEntries.Select(x => x.NewEntry);
            //var modifiedQuote = value.ModifiedQuote;

            if (value.ChangedEntries.Select(x => x.EntryState).Equals(Platform.Core.Common.EntryState.Modified)
            {
                var operationLog = new OperationLog
                {
                    ObjectId = value.ChangedEntries.Select(x => x.NewEntry.Id).ToString(),
                    ObjectType = typeof(QuoteRequest).Name,
                    OperationType = value.ChangedEntries.Select(x => x.EntryState)
                };

                if (value.ChangedEntries.Select(x => x.OldEntry.Status).ToString() != value.ChangedEntries.Select(x => x.NewEntry.Status).ToString())
                {
                    operationLog.Detail += String.Format("status changed from {0} -> {1} ", origQuote.Select(x => x.Status).ToString() ?? "undef", modifiedQuote.Select(x => x.Status).ToString() ?? "undef");
                }
                if (value.ChangedEntries.Select(x => x.OldEntry.Comment).ToString() != value.ChangedEntries.Select(x => x.NewEntry.Comment).ToString())
                {
                    operationLog.Detail += String.Format("comment changed ");
                }
                if (Convert.ToBoolean(value.ChangedEntries.Select(x => x.OldEntry.IsLocked)) != Convert.ToBoolean(value.ChangedEntries.Select(x => x.NewEntry.IsLocked)))
                {
                    operationLog.Detail += Convert.ToBoolean(modifiedQuote.Select(x => x.IsLocked)) ? "lock " : "unlock ";
                }
                _changeLogService.SaveChanges(operationLog);
            }

            //if(value.ChangeState == Platform.Core.Common.EntryState.Modified)
            //{
            //    var operationLog = new OperationLog
            //    {
            //        ObjectId = value.ModifiedQuote.Id,
            //        ObjectType = typeof(QuoteRequest).Name,
            //        OperationType = value.ChangeState
            //    };

            //    if (origQuote.Status != modifiedQuote.Status)
            //    {
            //        operationLog.Detail += String.Format("status changed from {0} -> {1} ", origQuote.Status ?? "undef", modifiedQuote.Status ?? "undef");
            //    }
            //    if (origQuote.Comment != modifiedQuote.Comment)
            //    {
            //        operationLog.Detail += String.Format("comment changed ");
            //    }

            //    if(origQuote.IsLocked != modifiedQuote.IsLocked)
            //    {
            //        operationLog.Detail += modifiedQuote.IsLocked ? "lock " : "unlock ";
            //    }

            //    _changeLogService.SaveChanges(operationLog);
            //}

        }

        #endregion

    }
}
