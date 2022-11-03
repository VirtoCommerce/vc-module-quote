using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using QuoteSettings = VirtoCommerce.QuoteModule.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class SubmitQuoteCommandHandler : IRequestHandler<SubmitQuoteCommand, QuoteAggregate>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;
    private readonly ISettingsManager _settingsManager;

    public SubmitQuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISettingsManager settingsManager)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
        _settingsManager = settingsManager;
    }

    public async Task<QuoteAggregate> Handle(SubmitQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = (await _quoteRequestService.GetByIdsAsync(request.QuoteId)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        var defaultStatus = await _settingsManager.GetValueByDescriptorAsync<string>(QuoteSettings.DefaultStatus);

        if (quote.Status != defaultStatus)
        {
            throw new ExecutionError($"Quote status is not '{defaultStatus}'") { Code = Constants.ValidationErrorCode };
        }

        quote.Status = QuoteStatus.Processing;

        if (!string.IsNullOrEmpty(request.Comment))
        {
            quote.Comment = request.Comment;
        }

        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        return await _quoteAggregateRepository.GetById(quote.Id);
    }
}
