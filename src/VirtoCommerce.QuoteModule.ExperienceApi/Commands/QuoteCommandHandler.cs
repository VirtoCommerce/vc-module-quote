using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using QuoteSettings = VirtoCommerce.QuoteModule.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class QuoteCommandHandler<TCommand> : IRequestHandler<TCommand, QuoteAggregate>
    where TCommand : QuoteCommand
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;
    private readonly ISettingsManager _settingsManager;

    protected QuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
        _settingsManager = settingsManager;
    }

    public async Task<QuoteAggregate> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var quote = (await _quoteRequestService.GetByIdsAsync(request.QuoteId)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        var defaultStatus = await _settingsManager.GetValueAsync<string>(QuoteSettings.DefaultStatus);

        if (quote.Status != defaultStatus)
        {
            throw new ExecutionError($"Quote status is not '{defaultStatus}'") { Code = Constants.ValidationErrorCode };
        }

        UpdateQuote(quote, request);
        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        return await _quoteAggregateRepository.GetById(quote.Id);
    }

    protected abstract void UpdateQuote(QuoteRequest quote, TCommand request);
}
