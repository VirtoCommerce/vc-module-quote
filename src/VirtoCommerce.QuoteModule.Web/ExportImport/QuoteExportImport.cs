using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<QuoteRequest> QuoteRequests { get; set; }
    }

    public sealed class QuoteExportImport
    {
        private readonly IQuoteRequestService _quoteRequestService;

        public QuoteExportImport(IQuoteRequestService quoteRequestService)
        {
            _quoteRequestService = quoteRequestService;
        }

        public async Task DoExportAsync(Stream outStream, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var progressInfo = new ExportImportProgressInfo { Description = "Quotes are loading" };
            progressCallback(progressInfo);

            var backupObject = await GetBackupObject(progressCallback);
            backupObject.SerializeJson(outStream);
        }

        public async Task DoImportAsync(Stream inputStream, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var backupObject = inputStream.DeserializeJson<BackupObject>();

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = $"{backupObject.QuoteRequests.Count} RFQs importing";
            progressCallback(progressInfo);
            await _quoteRequestService.SaveChangesAsync(backupObject.QuoteRequests.ToArray());
        }


        private async Task<BackupObject> GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            var retVal = new BackupObject();
            var progressInfo = new ExportImportProgressInfo();

            var searchResponse = await _quoteRequestService.SearchAsync(new QuoteRequestSearchCriteria { Take = int.MaxValue });

            progressInfo.Description = $"{searchResponse.Results.Count()} RFQs loading";
            progressCallback(progressInfo);

            retVal.QuoteRequests = (await _quoteRequestService.GetByIdsAsync(searchResponse.Results.Select(x => x.Id).ToArray())).ToList();

            return retVal;
        }

    }
}