using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Data.Resources;

namespace VirtoCommerce.QuoteModule.Data.Handlers
{
    public class LogChangesEventHandler : IEventHandler<QuoteRequestChangeEvent>
    {
        private readonly IChangeLogService _changeLogService;

        public LogChangesEventHandler(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        public virtual async Task Handle(QuoteRequestChangeEvent message)
        {
            var operationLogs = new List<OperationLog>();
            foreach (var changedEntry in message.ChangedEntries)
            {
                operationLogs.AddRange(await GetChangedEntryOperationLogsAsync(changedEntry));
            }

            if (!operationLogs.IsNullOrEmpty())
            {
                //Background task is used here for performance reasons
                BackgroundJob.Enqueue(() => LogEntityChangesInBackground(operationLogs));
            }
        }

        protected virtual Task<IEnumerable<OperationLog>> GetChangedEntryOperationLogsAsync(GenericChangedEntry<QuoteRequest> changedEntry)
        {
            var result = new List<OperationLog>();

            var original = changedEntry.OldEntry;
            var modified = changedEntry.NewEntry;

            if (changedEntry.EntryState == EntryState.Modified)
            {
                result.Add(GetLogRecord(modified.Id, QuoteResources.Updated));

                if (original.Status != modified.Status)
                {
                    result.Add(GetLogRecord(modified.Id, QuoteResources.StatusChanged, original.Status, modified.Status));
                }
                if (original.Comment != modified.Comment)
                {
                    result.Add(GetLogRecord(modified.Id, QuoteResources.CommentChanged, original.Comment, modified.Comment));
                }
                if (original.IsLocked != modified.IsLocked)
                {
                    result.Add(GetLogRecord(modified.Id, QuoteResources.IsLockedChanged, original.IsLocked, modified.IsLocked));
                }
            }

            return Task.FromResult<IEnumerable<OperationLog>>(result);
        }

        protected virtual OperationLog GetLogRecord(string id, string template, params object[] parameters)
        {
            var result = new OperationLog
            {
                ObjectId = id,
                ObjectType = typeof(QuoteRequest).Name,
                OperationType = EntryState.Modified,
                Detail = string.Format(template, parameters)
            };
            return result;
        }

        [DisableConcurrentExecution(10)]
        // "DisableConcurrentExecutionAttribute" prevents to start simultaneous job payloads.
        // Should have short timeout, because this attribute implemented by following manner: newly started job falls into "processing" state immediately.
        // Then it tries to receive job lock during timeout. If the lock received, the job starts payload.
        // When the job is awaiting desired timeout for lock release, it stucks in "processing" anyway. (Therefore, you should not to set long timeouts (like 24*60*60), this will cause a lot of stucked jobs and performance degradation.)
        // Then, if timeout is over and the lock NOT acquired, the job falls into "scheduled" state (this is default fail-retry scenario).
        // Failed job goes to "Failed" state (by default) after retries exhausted.
        public void LogEntityChangesInBackground(List<OperationLog> operationLogs)
        {
            _changeLogService.SaveChangesAsync(operationLogs.ToArray()).GetAwaiter().GetResult();
        }
    }
}
