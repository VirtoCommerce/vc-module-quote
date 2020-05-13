using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Common.Events;
using VirtoCommerce.Domain.Quote.Events;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.QuoteModule.Data.Converters;
using VirtoCommerce.QuoteModule.Data.Repositories;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class QuoteRequestServiceImpl : ServiceBase, IQuoteRequestService
    {
        private readonly Func<IQuoteRepository> _repositoryFactory;
        private readonly IUniqueNumberGenerator _uniqueNumberGenerator;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IChangeLogService _changeLogService;
        private readonly IStoreService _storeService;

        public QuoteRequestServiceImpl(Func<IQuoteRepository> quoteRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator, IDynamicPropertyService dynamicPropertyService, IEventPublisher eventPublisher, IChangeLogService changeLogService, IStoreService storeService)
        {
            _repositoryFactory = quoteRepositoryFactory;
            _uniqueNumberGenerator = uniqueNumberGenerator;
            _dynamicPropertyService = dynamicPropertyService;
            _eventPublisher = eventPublisher;
            _changeLogService = changeLogService;
            _storeService = storeService;
        }

        #region IQuoteRequestService Members

        public IEnumerable<QuoteRequest> GetByIds(params string[] ids)
        {
            var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();
            using (var repository = _repositoryFactory())
            {
                var retVal = repository.GetQuoteRequestByIds(ids).Select(x => x.ToCoreModel()).ToArray();
                foreach (var quote in retVal)
                {
                    _dynamicPropertyService.LoadDynamicPropertyValues(quote);
                    _changeLogService.LoadChangeLogs(quote);
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(quote, quote, EntryState.Unchanged));
                }
                _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
                return retVal;
            }
        }

        public IEnumerable<QuoteRequest> SaveChanges(QuoteRequest[] quoteRequests)
        {
            if (quoteRequests == null)
            {
                throw new ArgumentNullException("quoteRequests");
            }

            //Generate Number
            EnsureThatQuoteHasNumber(quoteRequests);
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();
            using (var repository = _repositoryFactory())
            {
                var ids = quoteRequests.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();

                var origDbQuotes = repository.GetQuoteRequestByIds(ids);
                using (var changeTracker = GetChangeTracker(repository))
                {
                    //Update
                    foreach (var origDbQuote in origDbQuotes)
                    {
                        var changedQuote = quoteRequests.First(x => x.Id == origDbQuote.Id);
                        // Do business logic on  quote request
                        changedEntries.Add(new GenericChangedEntry<QuoteRequest>(changedQuote, GetByIds(new[] { origDbQuote.Id }).First(), EntryState.Modified));

                        var changedDbQuote = changedQuote.ToDataModel(pkMap);
                        changeTracker.Attach(origDbQuote);
                        changedDbQuote.Patch(origDbQuote);
                    }

                    //Create
                    var newQuotes = quoteRequests.Where(x => !origDbQuotes.Any(y => y.Id == x.Id));
                    foreach (var newQuote in newQuotes)
                    {
                        // Do business logic on  quote request
                        changedEntries.Add(new GenericChangedEntry<QuoteRequest>(newQuote, newQuote, EntryState.Added));
                        var newDbQuote = newQuote.ToDataModel(pkMap);
                        repository.Add(newDbQuote);

                    }
                    repository.UnitOfWork.Commit();
                    //Copy generated id from dbEntities to model
                    pkMap.ResolvePrimaryKeys();
                    _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
                }

                //Save dynamic properties
                foreach (var quoteRequest in quoteRequests)
                {
                    _dynamicPropertyService.SaveDynamicPropertyValues(quoteRequest);
                }
                return quoteRequests;
            }
        }

        public GenericSearchResult<QuoteRequest> Search(QuoteRequestSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<QuoteRequest>();
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

                if (criteria.Number != null)
                {
                    query = query.Where(x => x.Number == criteria.Number);
                }
                else if (criteria.Keyword != null)
                {
                    query = query.Where(x => x.Number.Contains(criteria.Keyword));
                }

                if (criteria.Tag != null)
                {
                    query = query.Where(x => x.Tag == criteria.Tag);
                }
                if (criteria.Status != null)
                {
                    query = query.Where(x => x.Status == criteria.Status);
                }
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<QuoteRequest>(x => x.CreatedDate), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id);

                retVal.TotalCount = query.Count();

                var ids = query.Select(x => x.Id).Skip(criteria.Skip).Take(criteria.Take).ToArray();

                var quotes = GetByIds(ids);
                retVal.Results = quotes.AsQueryable<QuoteRequest>().OrderBySortInfos(sortInfos).ToList();
            }
            return retVal;
        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var changedEntries = new List<GenericChangedEntry<QuoteRequest>>();
                var dbQuotes = repository.GetQuoteRequestByIds(ids);
                var quotes = GetByIds(ids);
                foreach (var dbQuote in dbQuotes)
                {
                    changedEntries.Add(new GenericChangedEntry<QuoteRequest>(null, quotes.First(x => x.Id == dbQuote.Id), EntryState.Deleted));
                    repository.Remove(dbQuote);
                }
                repository.UnitOfWork.Commit();
                _eventPublisher.Publish(new QuoteRequestChangeEvent(changedEntries));
            }
        }
        #endregion


        private void EnsureThatQuoteHasNumber(QuoteRequest[] quoteRequests)
        {
            var stores = _storeService.GetByIds(quoteRequests.Select(x => x.StoreId).Distinct().ToArray());
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

    }
}
