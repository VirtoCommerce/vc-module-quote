using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Data.Model;
using Xunit;

namespace VirtoCommerce.QuoteModule.Tests
{
    [Trait("Category", "Unit")]
    public class QuoteEntityMappingTests
    {
        private static QuoteItem Item(int selectedTier)
        {
            TierPrice[] tiers = [new() { Price = 10, Quantity = 1 }, new() { Price = 9, Quantity = 10 }, new() { Price = 8, Quantity = 100 }];

            return new QuoteItem
            {
                Id = "item1",
                Currency = "USD",
                Name = "Item",
                ProposalPrices = tiers,
                // a separate instance, as it arrives from the API
                SelectedTierPrice = new TierPrice { Price = tiers[selectedTier].Price, Quantity = tiers[selectedTier].Quantity },
            };
        }

        private static QuoteItemEntity ToEntity(QuoteItem item) => new QuoteItemEntity().FromModel(item, new PrimaryKeyResolvingMap());

        [Fact]
        public void SelectedTierPrice_PostedAfterProposalPrices_DoesNotOverwriteTheFirstTier()
        {
            var item = JsonConvert.DeserializeObject<QuoteItem>(
                "{\"proposalPrices\":[{\"quantity\":1,\"price\":120},{\"quantity\":10,\"price\":110}],\"selectedTierPrice\":{\"quantity\":10,\"price\":110}}");

            Assert.Equal([(1L, 120m), (10L, 110m)], item.ProposalPrices.Select(x => ((long)x.Quantity, x.Price)));
            Assert.Equal(10, item.SelectedTierPrice.Quantity);
        }

        [Fact]
        public void SelectedTierPrice_SurvivesSaveAndLoad()
        {
            var loaded = ToEntity(Item(selectedTier: 1)).ToModel(new QuoteItem());

            Assert.Equal(10, loaded.SelectedTierPrice.Quantity);
            Assert.Equal(9, loaded.SelectedTierPrice.Price);
            // the same instance as in ProposalPrices (ChangeQuoteItemQuantityCommandHandler changes it in place)
            Assert.Same(loaded.ProposalPrices.ElementAt(1), loaded.SelectedTierPrice);
        }

        [Fact]
        public void SelectedTierPrice_ChangedOnUpdate_IsPatched()
        {
            var stored = ToEntity(Item(selectedTier: 0));

            ToEntity(Item(selectedTier: 2)).Patch(stored);

            Assert.Equal(100, stored.ToModel(new QuoteItem()).SelectedTierPrice.Quantity);
        }

        [Fact]
        public void AddressIsDefault_SurvivesSaveAndUpdate()
        {
            var stored = new AddressEntity().FromModel(new Address { Key = "a1", IsDefault = false });
            Assert.False(stored.ToModel(new Address()).IsDefault);

            new AddressEntity().FromModel(new Address { Key = "a1", IsDefault = true }).Patch(stored);

            Assert.True(stored.ToModel(new Address()).IsDefault);
        }
    }
}
