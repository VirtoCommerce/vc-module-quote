using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Data.Handlers;
using VirtoCommerce.QuoteModule.Data.Jobs;
using Xunit;

namespace VirtoCommerce.QuoteModule.Tests.Handlers;

// Any other test class that enqueues through the static BackgroundJob facade must join this collection:
// the facade has no reset API (Initialize rejects null), so Dispose leaves a DISPOSED provider behind in
// the static, and a class racing this one would see ObjectDisposedException from it.
[Collection(nameof(LogChangesEventHandlerTests))]
public class LogChangesEventHandlerTests
{
    [Fact]
    public async Task Handle_ModifiedQuoteRequest_EnqueuesOneJobWithAllLogs()
    {
        // Arrange
        using var capture = new EnqueueCapture();
        var handler = new LogChangesEventHandler(Mock.Of<IChangeLogService>());

        var original = new QuoteRequest { Id = "quote1", Status = "Draft" };
        var modified = new QuoteRequest { Id = "quote1", Status = "Processing" };

        // Act
        await handler.Handle(new QuoteRequestChangeEvent([new GenericChangedEntry<QuoteRequest>(modified, original, EntryState.Modified)]));

        // Assert
        Assert.Equal(1, capture.EnqueueCount);
        Assert.Equal(2, capture.Payload.OperationLogs.Length);
        Assert.All(capture.Payload.OperationLogs, x =>
        {
            Assert.Equal("quote1", x.ObjectId);
            Assert.Equal(nameof(QuoteRequest), x.ObjectType);
            Assert.Equal(EntryState.Modified, x.OperationType);
        });
    }

    [Fact]
    public async Task Handle_NoLogsCollected_DoesNotEnqueue()
    {
        // Arrange
        using var capture = new EnqueueCapture();
        var handler = new LogChangesEventHandler(Mock.Of<IChangeLogService>());

        var quoteRequest = new QuoteRequest { Id = "quote1" };

        // Act
        await handler.Handle(new QuoteRequestChangeEvent([new GenericChangedEntry<QuoteRequest>(quoteRequest, EntryState.Added)]));

        // Assert
        Assert.Equal(0, capture.EnqueueCount);
    }

    [Fact]
    public async Task LogEntityChangesJobHandler_SavesThePayloadLogs()
    {
        // Arrange
        var operationLogs = new[] { AbstractTypeFactory<OperationLog>.TryCreateInstance() };
        var changeLogServiceMock = new Mock<IChangeLogService>();

        var handler = new LogEntityChangesJobHandler(changeLogServiceMock.Object);

        // Act
        await handler.Execute(new LogEntityChangesJobPayload { OperationLogs = operationLogs }, context: null,
            TestContext.Current.CancellationToken);

        // Assert
        changeLogServiceMock.Verify(x => x.SaveChangesAsync(operationLogs), Times.Once);
    }

    [Fact]
    public void LogEntityChangesInBackground_StillSavesForLegacyHangfireJobs()
    {
        // Arrange
        // Hangfire stores a queued job as type + method name + parameter types + serialized args, so a store
        // written by an earlier version can still invoke this method. Deleting it would strand those as Failed.
        var operationLogs = new List<OperationLog> { AbstractTypeFactory<OperationLog>.TryCreateInstance() };
        var changeLogServiceMock = new Mock<IChangeLogService>();

        var handler = new LogChangesEventHandler(changeLogServiceMock.Object);

        // Act
#pragma warning disable VC0015
        handler.LogEntityChangesInBackground(operationLogs);
#pragma warning restore VC0015

        // Assert
        changeLogServiceMock.Verify(x => x.SaveChangesAsync(It.Is<OperationLog[]>(logs => logs.SequenceEqual(operationLogs))), Times.Once);
    }

    // Captures what the handler enqueued through the static BackgroundJob facade. IBackgroundJob is registered
    // Scoped here exactly as the engine module registers it, so this also proves the facade's per-call scope
    // resolves it - the handler itself is root-resolved and must never hold it.
    private sealed class EnqueueCapture : IDisposable
    {
        private readonly ServiceProvider _provider;

        public EnqueueCapture()
        {
            BackgroundJobMock
                .Setup(x => x.Enqueue<LogEntityChangesJobHandler>(It.IsAny<object>(), It.IsAny<EnqueueOptions>(), It.IsAny<CancellationToken>()))
                .Callback<object, EnqueueOptions, CancellationToken>((payload, _, _) =>
                {
                    Payload = (LogEntityChangesJobPayload)payload;
                    EnqueueCount++;
                })
                .ReturnsAsync("job-id");

            var services = new ServiceCollection();
            services.AddScoped(_ => BackgroundJobMock.Object);
            _provider = services.BuildServiceProvider(validateScopes: true);

            BackgroundJob.Initialize(_provider);
        }

        public Mock<IBackgroundJob> BackgroundJobMock { get; } = new();

        public LogEntityChangesJobPayload Payload { get; private set; }

        public int EnqueueCount { get; private set; }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
