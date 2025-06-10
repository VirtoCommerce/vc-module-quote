using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model;

public class QuoteConfigurationItemEntity : AuditableEntity, IDataEntity<QuoteConfigurationItemEntity, QuoteConfigurationItem>
{
    [StringLength(128)]
    public string QuoteItemId { get; set; }

    public QuoteItemEntity QuoteItem { get; set; }

    [StringLength(128)]
    public string ProductId { get; set; }

    [StringLength(1024)]
    public string Name { get; set; }

    [StringLength(128)]
    public string Sku { get; set; }

    public int Quantity { get; set; }

    [StringLength(1028)]
    public string ImageUrl { get; set; }

    [StringLength(128)]
    public string CatalogId { get; set; }

    [StringLength(128)]
    public string CategoryId { get; set; }

    [Required]
    [StringLength(64)]
    public string Type { get; set; }

    [StringLength(255)]
    public string CustomText { get; set; }

    #region Navigation Properties
    public virtual ObservableCollection<QuoteConfigurationItemFileEntity> Files { get; set; } = new NullCollection<QuoteConfigurationItemFileEntity>();
    #endregion

    public virtual QuoteConfigurationItem ToModel(QuoteConfigurationItem configurationItem)
    {
        ArgumentNullException.ThrowIfNull(configurationItem);

        configurationItem.Id = Id;
        configurationItem.CreatedBy = CreatedBy;
        configurationItem.CreatedDate = CreatedDate;
        configurationItem.ModifiedBy = ModifiedBy;
        configurationItem.ModifiedDate = ModifiedDate;

        configurationItem.LineItemId = QuoteItemId;
        configurationItem.ProductId = ProductId;
        configurationItem.Name = Name;
        configurationItem.Sku = Sku;
        configurationItem.Quantity = Quantity;
        configurationItem.ImageUrl = ImageUrl;
        configurationItem.CatalogId = CatalogId;
        configurationItem.CategoryId = CategoryId;
        configurationItem.Type = Type;
        configurationItem.CustomText = CustomText;

        configurationItem.Files = Files.Select(x => x.ToModel(AbstractTypeFactory<QuoteConfigurationItemFile>.TryCreateInstance())).ToList();

        return configurationItem;
    }

    public virtual QuoteConfigurationItemEntity FromModel(QuoteConfigurationItem configurationItem, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(configurationItem);

        pkMap.AddPair(configurationItem, this);

        Id = configurationItem.Id;
        CreatedBy = configurationItem.CreatedBy;
        CreatedDate = configurationItem.CreatedDate;
        ModifiedBy = configurationItem.ModifiedBy;
        ModifiedDate = configurationItem.ModifiedDate;

        QuoteItemId = configurationItem.LineItemId;
        ProductId = configurationItem.ProductId;
        Name = configurationItem.Name;
        Sku = configurationItem.Sku;
        Quantity = configurationItem.Quantity;
        ImageUrl = configurationItem.ImageUrl;
        CatalogId = configurationItem.CatalogId;
        CategoryId = configurationItem.CategoryId;
        Type = configurationItem.Type;
        CustomText = configurationItem.CustomText;

        if (configurationItem.Files != null)
        {
            Files = new ObservableCollection<QuoteConfigurationItemFileEntity>(configurationItem.Files.Select(x => AbstractTypeFactory<QuoteConfigurationItemFileEntity>.TryCreateInstance().FromModel(x, pkMap)));
        }

        return this;
    }

    public virtual void Patch(QuoteConfigurationItemEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.QuoteItemId = QuoteItemId;
        target.ProductId = ProductId;
        target.Name = Name;
        target.Sku = Sku;
        target.Quantity = Quantity;
        target.ImageUrl = ImageUrl;
        target.CatalogId = CatalogId;
        target.CategoryId = CategoryId;
        target.Type = Type;
        target.CustomText = CustomText;

        if (!Files.IsNullCollection())
        {
            Files.Patch(target.Files, (sourceImage, targetImage) => sourceImage.Patch(targetImage));
        }
    }
}
