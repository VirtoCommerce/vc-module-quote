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
using VirtoCommerce.QuoteModule.Core;
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


        public QuoteRequestService(
            Func<IQuoteRepository> quoteRepositoryFactory,
            IUniqueNumberGenerator uniqueNumberGenerator,
            IEventPublisher eventPublisher,
            IStoreService storeService,
            IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = quoteRepositoryFactory;
            _uniqueNumberGenerator = uniqueNumberGenerator;
            _eventPublisher = eventPublisher;
            _storeService = storeService;
            _platformMemoryCache = platformMemoryCache;
        }

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

            // Generate Number
            await EnsureThatQuoteHasNumber(quoteRequests);

            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();
            var changedEntities = new List<QuoteRequestEntity>();

            using (var repository = _repositoryFactory())
            {
                var ids = quoteRequests.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();
                var existingEntities = await repository.GetQuoteRequestByIdsAsync(ids);

                foreach (var model in quoteRequests)
                {
                    var originalEntity = existingEntities.FirstOrDefault(x => x.Id == model.Id);
                    var modifiedEntity = AbstractTypeFactory<QuoteRequestEntity>.TryCreateInstance().FromModel(model, pkMap);

                    if (originalEntity != null)
                    {
                        repository.TrackModifiedAsAddedForNewChildEntities(originalEntity);

                        var originalModel = originalEntity.ToModel(AbstractTypeFactory<QuoteRequest>.TryCreateInstance());
                        modifiedEntity.Patch(originalEntity);

                        changedEntries.Add(new GenericChangedEntry<QuoteRequest>(model, originalModel, EntryState.Modified));
                        changedEntities.Add(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);

                        changedEntries.Add(new GenericChangedEntry<QuoteRequest>(model, EntryState.Added));
                        changedEntities.Add(modifiedEntity);
                    }
                }

                await repository.UnitOfWork.CommitAsync();
            }

            pkMap.ResolvePrimaryKeys();
            ClearCache(quoteRequests);

            foreach (var (changedEntry, i) in changedEntries.Select((x, i) => (x, i)))
            {
                changedEntry.NewEntry = changedEntities[i].ToModel(AbstractTypeFactory<QuoteRequest>.TryCreateInstance());
            }

            await _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
        }

        public virtual Task<QuoteRequestSearchResult> SearchAsync(QuoteRequestSearchCriteria criteria)
        {
            var result = new QuoteRequestSearchResult();
            var cacheKey = CacheKey.With(GetType(), nameof(SearchAsync), criteria.GetCacheKey());
            return _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(QuoteSearchCacheRegion.CreateChangeToken());
                using var repository = _repositoryFactory();

                var query = BuildQuery(repository, criteria);
                var sortInfos = BuildSortExpression(criteria);

                result.TotalCount = await query.CountAsync();
                if (criteria.Take > 0)
                {
                    var ids = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                        .Select(x => x.Id)
                        .Skip(criteria.Skip).Take(criteria.Take)
                        .ToArrayAsync();

                    result.Results = (await GetByIdsAsync(ids)).OrderBy(x => Array.IndexOf(ids, x.Id)).ToArray();
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
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(quotes.First(x => x.Id == dbQuote.Id), EntryState.Deleted));
                    repository.Remove(dbQuote);
                }
                repository.UnitOfWork.Commit();

                ClearCache(quotes);

                await _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
            }
        }

        protected virtual IQueryable<QuoteRequestEntity> BuildQuery(IQuoteRepository repository, QuoteRequestSearchCriteria criteria)
        {
            var query = repository.QuoteRequests;

            if (criteria.CustomerId != null)
            {
                query = query.Where(x => x.CustomerId == criteria.CustomerId);
            }

            if (criteria.OrganizationId != null)
            {
                query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
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
                query = GetKeywordFilters(query, criteria);
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

            return query;
        }

        protected virtual IQueryable<QuoteRequestEntity> GetKeywordFilters(IQueryable<QuoteRequestEntity> query,
            QuoteRequestSearchCriteria criteria)
        {
            return query.Where(x => x.Number.Contains(criteria.Keyword)
                                     || (x.CustomerName != null && x.CustomerName.Contains(criteria.Keyword))
                                     || (x.Status != null && x.Status.Contains(criteria.Keyword))
                                     || (x.Tag != null && x.Tag.Contains(criteria.Keyword)));
        }

        protected virtual IList<SortInfo> BuildSortExpression(QuoteRequestSearchCriteria criteria)
        {
            var data = criteria.SortInfos;
            if (data.IsNullOrEmpty())
            {
                data = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<QuoteRequest>(x => x.CreatedDate), SortDirection = SortDirection.Descending } };
            }

            return data;
        }

        protected virtual async Task EnsureThatQuoteHasNumber(QuoteRequest[] quoteRequests)
        {
            var stores = await _storeService.GetNoCloneAsync(quoteRequests.Select(x => x.StoreId).Distinct().ToList());
            foreach (var quoteRequest in quoteRequests)
            {
                if (string.IsNullOrEmpty(quoteRequest.Number))
                {
                    var store = stores.FirstOrDefault(x => x.Id == quoteRequest.StoreId);
                    var numberTemplate = (string)ModuleConstants.Settings.General.QuoteRequestNewNumberTemplate.DefaultValue;
                    if (store != null)
                    {
                        numberTemplate = store.Settings.GetValue<string>(ModuleConstants.Settings.General.QuoteRequestNewNumberTemplate);
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
