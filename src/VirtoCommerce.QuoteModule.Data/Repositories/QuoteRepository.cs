using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Repositories
{
    public class QuoteRepository : DbContextRepositoryBase<QuoteDbContext>, IQuoteRepository
    {
        public QuoteRepository(QuoteDbContext dbContext) : base(dbContext)
        {
        }

        #region IQuoteRepository Members
        public virtual async Task<QuoteRequestEntity[]> GetQuoteRequestByIdsAsync(params string[] ids)
        {
            var result = await QuoteRequests.Where(x => ids.Contains(x.Id)).ToArrayAsync();

            ids = result.Select(x => x.Id).ToArray();
            if (!ids.IsNullOrEmpty())
            {
                await Addresses.Where(x => ids.Contains(x.QuoteRequestId)).LoadAsync();

                await Attachments.Where(x => ids.Contains(x.QuoteRequestId)).LoadAsync();

                var quoteItems = await QuoteItems.Include(x => x.ProposalPrices)
                                    .Where(x => ids.Contains(x.QuoteRequestId)).ToArrayAsync();

                var configurationItemIds = quoteItems.Where(x => x.IsConfigured).Select(x => x.Id).ToArray();
                if (configurationItemIds.Length > 0)
                {
                    await ConfigurationItems
                        .Where(x => configurationItemIds.Contains(x.QuoteItemId))
                        .Include(x => x.Files)
                        .AsSplitQuery()
                        .LoadAsync();
                }

                await DynamicPropertyObjectValues.Where(x => ids.Contains(x.ObjectId)).LoadAsync();
            }

            return result;
        }

        public IQueryable<AddressEntity> Addresses => DbContext.Set<AddressEntity>();

        public IQueryable<AttachmentEntity> Attachments => DbContext.Set<AttachmentEntity>();

        public IQueryable<QuoteItemEntity> QuoteItems => DbContext.Set<QuoteItemEntity>();

        public IQueryable<QuoteRequestEntity> QuoteRequests => DbContext.Set<QuoteRequestEntity>();

        public IQueryable<QuoteConfigurationItemEntity> ConfigurationItems => DbContext.Set<QuoteConfigurationItemEntity>();

        public IQueryable<QuoteConfigurationItemFileEntity> ConfigurationItemFiles => DbContext.Set<QuoteConfigurationItemFileEntity>();
        #endregion

        protected IQueryable<QuoteDynamicPropertyObjectValueEntity> DynamicPropertyObjectValues => DbContext.Set<QuoteDynamicPropertyObjectValueEntity>();
    }
}
