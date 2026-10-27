using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Data.Jobs;
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
                var payload = AbstractTypeFactory<LogEntityChangesJobPayload>.TryCreateInstance();
                payload.OperationLogs = operationLogs.ToArray();

                //Background task is used here for performance reasons.
                //The static facade, not an injected IBackgroundJob: RegisterEventHandler resolves this handler once
                //from the root provider and holds it for the process lifetime, so it must not capture a Scoped dependency.
                await BackgroundJob.Enqueue<LogEntityChangesJobHandler>(payload);
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

        /// <summary>
        /// Kept for background jobs enqueued by an earlier version, which reference this method by name.
        /// New work goes through <see cref="LogEntityChangesJobHandler"/>; remove this once no such job
        /// can still be pending.
        /// </summary>
        // Signature is byte-identical on purpose: Hangfire persists a queued job as type name + method name +
        // parameter types + serialized args, so changing any of them would strand already-queued entries as Failed.
        [Obsolete("Enqueued indirectly by legacy Hangfire jobs only; new work uses LogEntityChangesJobHandler.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public void LogEntityChangesInBackground(List<OperationLog> operationLogs)
        {
            _changeLogService.SaveChangesAsync(operationLogs.ToArray()).GetAwaiter().GetResult();
        }
    }
}
