using VirtoCommerce.Platform.Core.ChangeLog;

namespace VirtoCommerce.QuoteModule.Data.Jobs
{
    /// <summary>
    /// Payload of the background job that persists quote request change-log entries.
    /// </summary>
    public class LogEntityChangesJobPayload
    {
        public OperationLog[] OperationLogs { get; set; }
    }
}
