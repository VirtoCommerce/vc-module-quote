using System;
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
			// Not used
		}

		public void OnError(Exception error)
		{
			// Not used
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

#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
					if (origQuote.Status != modifiedQuote.Status)
					{
						operationLog.Detail += $"status changed from {origQuote.Status ?? "undef"} -> {modifiedQuote.Status ?? "undef"} ";
					}
					if (origQuote.Comment != modifiedQuote.Comment)
					{
						operationLog.Detail += "comment changed ";
					}
					if (origQuote.IsLocked != modifiedQuote.IsLocked)
					{
						operationLog.Detail += modifiedQuote.IsLocked ? "lock " : "unlock ";
					}
					_changeLogService.SaveChanges(operationLog);
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
				}
			}
		}

		#endregion

	}
}
