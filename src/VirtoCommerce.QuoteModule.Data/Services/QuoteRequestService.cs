using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.Data.Caching;
using VirtoCommerce.QuoteModule.Data.Model;
using VirtoCommerce.QuoteModule.Data.Repositories;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class QuoteRequestService : IQuoteRequestService
    {
        private readonly Func<IQuoteRepository> _repositoryFactory;
        private readonly IUniqueNumberGenerator _uniqueNumberGenerator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStoreService _storeService;
        private readonly IPlatformMemoryCache _platformMemoryCache;


        public QuoteRequestService(Func<IQuoteRepository> quoteRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator, IEventPublisher eventPublisher, IStoreService storeService, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = quoteRepositoryFactory;
            _uniqueNumberGenerator = uniqueNumberGenerator;
            _eventPublisher = eventPublisher;
            _storeService = storeService;
            _platformMemoryCache = platformMemoryCache;
        }

        #region IQuoteRequestService Members

        public virtual async Task<IEnumerable<QuoteRequest>> GetByIdsAsync(params string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var retVal = new List<QuoteRequest>();
                using (var repository = _repositoryFactory())
                {
                    //It is so important to generate change tokens for all ids even for not existing members to prevent an issue
                    //with caching of empty results for non - existing objects that have the infinitive lifetime in the cache
                    //and future unavailability to create objects with these ids.
                    cacheEntry.AddExpirationToken(QuoteCacheRegion.CreateChangeToken(ids));
                    repository.DisableChangesTracking();

                    var dbQuotes = await repository.GetQuoteRequestByIdsAsync(ids);
                    foreach (var dbQuote in dbQuotes)
                    {
                        var quote = dbQuote.ToModel(AbstractTypeFactory<QuoteRequest>.TryCreateInstance());
                        retVal.Add(quote);
                    }
                }

                return retVal.ToArray();
            });
        }

        public virtual async Task SaveChangesAsync(QuoteRequest[] quoteRequests)
        {
            if (quoteRequests == null)
            {
                throw new ArgumentNullException(nameof(quoteRequests));
            }

            //Generate Number
            await EnsureThatQuoteHasNumber(quoteRequests);

            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();

            using (var repository = _repositoryFactory())
            {
                var ids = quoteRequests.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();
                var origDbQuotes = await repository.GetQuoteRequestByIdsAsync(ids);

                //Update
                foreach (var origDbQuote in origDbQuotes)
                {
                    repository.TrackModifiedAsAddedForNewChildEntities(origDbQuote);

                    var changedQuote = quoteRequests.First(x => x.Id == origDbQuote.Id);
                    // Do business logic on quote request
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(changedQuote, (await GetByIdsAsync(new[] { origDbQuote.Id })).First(), EntryState.Modified));

                    var changedDbQuote = AbstractTypeFactory<QuoteRequestEntity>.TryCreateInstance().FromModel(changedQuote, pkMap);
                    changedDbQuote.Patch(origDbQuote);
                }

                //Create
                var newQuotes = quoteRequests.Where(x => !origDbQuotes.Any(y => y.Id == x.Id));
                foreach (var newQuote in newQuotes)
                {
                    // Do business logic on quote request
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(newQuote, EntryState.Added));
                    var newDbQuote = AbstractTypeFactory<QuoteRequestEntity>.TryCreateInstance().FromModel(newQuote, pkMap);
                    repository.Add(newDbQuote);

                }
                await repository.UnitOfWork.CommitAsync();
                //Copy generated id from dbEntities to model
                pkMap.ResolvePrimaryKeys();

                ClearCache(quoteRequests);

                await _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
            }
        }

        public virtual async Task<QuoteRequestSearchResult> SearchAsync(QuoteRequestSearchCriteria criteria)
        {
            var result = new QuoteRequestSearchResult();
            var cacheKey = CacheKey.With(GetType(), nameof(SearchAsync), criteria.GetCacheKey());
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(QuoteSearchCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var query = repository.QuoteRequests;

                    if (criteria.CustomerId != null)
                    {
                        query = query.Where(x => x.CustomerId == criteria.CustomerId);
                    }

                    if (criteria.StoreId != null)
                    {
                        query = query.Where(x => x.StoreId == criteria.StoreId);
                    }

                    if (criteria.Currency != null)
                    {
                        query = query.Where(x => x.Currency == criteria.Currency);
                    }

                    if (criteria.LanguageCode != null)
                    {
                        query = query.Where(x => x.LanguageCode == criteria.LanguageCode);
                    }

                    if (criteria.Number != null)
                    {
                        query = query.Where(x => x.Number == criteria.Number);
                    }

                    else if (criteria.Keyword != null)
                    {
                        query = query.Where(x => x.Number.Contains(criteria.Keyword)
                            || (x.CustomerName != null && x.CustomerName.Contains(criteria.Keyword))
                            || (x.Status != null && x.Status.Contains(criteria.Keyword))
                            || (x.Tag != null && x.Tag.Contains(criteria.Keyword)));
                    }

                    else if (!string.IsNullOrEmpty(criteria.NumberKeyword))
                    {
                        query = query.Where(x => x.Number.Contains(criteria.NumberKeyword));
                    }

                    if (criteria.Tag != null)
                    {
                        query = query.Where(x => x.Tag == criteria.Tag);
                    }

                    if (!criteria.Statuses.IsNullOrEmpty())
                    {
                        query = query.Where(x => criteria.Statuses.Contains(x.Status));
                    }

                    if (criteria.StartDate != null)
                    {
                        query = query.Where(x => x.CreatedDate >= criteria.StartDate);
                    }

                    if (criteria.EndDate != null)
                    {
                        query = query.Where(x => x.CreatedDate <= criteria.EndDate);
                    }

                    var sortInfos = criteria.SortInfos;
                    if (sortInfos.IsNullOrEmpty())
                    {
                        sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<QuoteRequest>(x => x.CreatedDate), SortDirection = SortDirection.Descending } };
                    }

                    result.TotalCount = await query.CountAsync();
                    if (criteria.Take > 0)
                    {
                        var ids = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                                         .Select(x => x.Id)
                                         .Skip(criteria.Skip).Take(criteria.Take)
                                         .ToArrayAsync();

                        result.Results = (await GetByIdsAsync(ids)).OrderBy(x => Array.IndexOf(ids, x.Id)).ToList();
                    }
                }
                return result;
            });
        }

        public virtual async Task DeleteAsync(string[] ids)
        {
            var quotes = await GetByIdsAsync(ids);

            using (var repository = _repositoryFactory())
            {
                var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();
                var dbQuotes = await repository.GetQuoteRequestByIdsAsync(ids);
                foreach (var dbQuote in dbQuotes)
                {
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(null, quotes.First(x => x.Id == dbQuote.Id), EntryState.Deleted));
                    repository.Remove(dbQuote);
                }
                repository.UnitOfWork.Commit();

                ClearCache(quotes);

                await _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
            }
        }
        #endregion


        protected virtual async Task EnsureThatQuoteHasNumber(QuoteRequest[] quoteRequests)
        {
            var stores = await _storeService.GetByIdsAsync(quoteRequests.Select(x => x.StoreId).Distinct().ToArray());
            foreach (var quoteRequest in quoteRequests)
            {
                if (string.IsNullOrEmpty(quoteRequest.Number))
                {
                    var store = stores.FirstOrDefault(x => x.Id == quoteRequest.StoreId);
                    var numberTemplate = "RFQ{0:yyMMdd}-{1:D5}";
                    if (store != null)
                    {
                        numberTemplate = store.Settings.GetSettingValue("Quotes.QuoteRequestNewNumberTemplate", numberTemplate);
                    }
                    quoteRequest.Number = _uniqueNumberGenerator.GenerateNumber(numberTemplate);
                }
            }
        }

        protected virtual void ClearCache(IEnumerable<QuoteRequest> items)
        {
            QuoteSearchCacheRegion.ExpireRegion();

            foreach (var item in items.Where(x => !x.IsTransient()))
            {
                QuoteCacheRegion.Expire(item.Id);
            }
        }

    }
}
