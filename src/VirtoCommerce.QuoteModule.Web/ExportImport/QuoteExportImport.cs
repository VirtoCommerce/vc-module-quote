using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Web.ExportImport
{
    public sealed class QuoteExportImport
    {
        private readonly IQuoteRequestService _quoteRequestService;
        private readonly JsonSerializer _serializer;
        private readonly int _batchSize = 50;

        public QuoteExportImport(IQuoteRequestService quoteRequestService, JsonSerializer serializer)
        {
            _quoteRequestService = quoteRequestService;
            _serializer = serializer;
        }

        public async Task DoExportAsync(Stream outStream, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var progressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(progressInfo);

            using (var sw = new StreamWriter(outStream, System.Text.Encoding.UTF8))
            using (var writer = new JsonTextWriter(sw))
            {
                await writer.WriteStartObjectAsync();
                await writer.WritePropertyNameAsync("QuoteRequests");

                await writer.SerializeArrayWithPagingAsync(_serializer, _batchSize, async (skip, take) =>
                        (GenericSearchResult<QuoteRequest>)await _quoteRequestService.SearchAsync(new QuoteRequestSearchCriteria { Skip = skip, Take = take })
                    , (processedCount, totalCount) =>
                    {
                        progressInfo.Description = $"{processedCount} of {totalCount} quote requests have been exported";
                        progressCallback(progressInfo);
                    }, cancellationToken);

                await writer.WriteEndObjectAsync();
                await writer.FlushAsync();
            }
        }

        public async Task DoImportAsync(Stream inputStream, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var progressInfo = new ExportImportProgressInfo { Description = "quote requests importing..." };
            progressCallback(progressInfo);

            using (var streamReader = new StreamReader(inputStream))
            using (var reader = new JsonTextReader(streamReader))
            {
                while (await reader.ReadAsync())
                {
                    if (reader.TokenType != JsonToken.PropertyName)
                    {
                        continue;
                    }

                    if (reader.Value.ToString() == "QuoteRequests")
                    {
                        await reader.DeserializeArrayWithPagingAsync<QuoteRequest>(_serializer, _batchSize, async items =>
                        {
                            await _quoteRequestService.SaveChangesAsync(items.ToArray());
                        }, processedCount =>
                        {
                            progressInfo.Description = $"{processedCount} Quote requests have been imported";
                            progressCallback(progressInfo);
                        }, cancellationToken);
                    }
                }
            }
        }
    }
}
