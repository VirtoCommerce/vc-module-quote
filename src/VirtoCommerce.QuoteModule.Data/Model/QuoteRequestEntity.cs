using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class QuoteRequestEntity : AuditableEntity
    {
        public QuoteRequestEntity()
        {
            Addresses = new NullCollection<AddressEntity>();
            Items = new NullCollection<QuoteItemEntity>();
            Attachments = new NullCollection<AttachmentEntity>();
        }
        [Required]
        [StringLength(64)]
        public string Number { get; set; }

        [Required]
        [StringLength(64)]
        public string StoreId { get; set; }
        [StringLength(255)]
        public string StoreName { get; set; }
        [StringLength(64)]
        public string ChannelId { get; set; }

        [StringLength(64)]
        public string OrganizationId { get; set; }
        [StringLength(255)]
        public string OrganizationName { get; set; }

        public bool IsAnonymous { get; set; }
        [StringLength(64)]
        public string CustomerId { get; set; }
        [StringLength(255)]
        public string CustomerName { get; set; }

        [StringLength(64)]
        public string EmployeeId { get; set; }
        [StringLength(255)]
        public string EmployeeName { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public DateTime? ReminderDate { get; set; }

        public bool EnableNotification { get; set; }
        public bool IsLocked { get; set; }

        [StringLength(64)]
        public string Status { get; set; }

        public string Comment { get; set; }
        public string InnerComment { get; set; }

        [StringLength(128)]
        public string Tag { get; set; }

        public bool IsSubmitted { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [StringLength(5)]
        public string LanguageCode { get; set; }

        [StringLength(64)]
        public string Coupon { get; set; }

        [StringLength(64)]
        public string ShipmentMethodCode { get; set; }
        [StringLength(64)]
        public string ShipmentMethodOption { get; set; }

        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        [StringLength(2048)]
        public string CancelReason { get; set; }

        [Column(TypeName = "Money")]
        public decimal ManualSubTotal { get; set; }
        public decimal ManualRelDiscountAmount { get; set; }
        [Column(TypeName = "Money")]
        public decimal ManualShippingTotal { get; set; }

        #region Navigation properties
        public ObservableCollection<AddressEntity> Addresses { get; set; }
        public ObservableCollection<QuoteItemEntity> Items { get; set; }
        public ObservableCollection<AttachmentEntity> Attachments { get; set; }
        public virtual ObservableCollection<QuoteDynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; set; }
            = new NullCollection<QuoteDynamicPropertyObjectValueEntity>();
        #endregion


        public virtual QuoteRequest ToModel(QuoteRequest target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var _ = this;
            target.Id = _.Id;
            target.CreatedBy = _.CreatedBy;
            target.CreatedDate = _.CreatedDate;
            target.ModifiedBy = _.ModifiedBy;
            target.ModifiedDate = _.ModifiedDate;

            target.Number = _.Number;
            target.StoreId = _.StoreId;
            target.ChannelId = _.ChannelId;
            target.IsAnonymous = _.IsAnonymous;
            target.CustomerId = _.CustomerId;
            target.CustomerName = _.CustomerName;
            target.OrganizationName = _.OrganizationName;
            target.OrganizationId = _.OrganizationId;
            target.EmployeeId = _.EmployeeId;
            target.EmployeeName = _.EmployeeName;
            target.ExpirationDate = _.ExpirationDate;
            target.ReminderDate = _.ReminderDate;
            target.EnableNotification = _.EnableNotification;
            target.IsLocked = _.IsLocked;
            target.Status = _.Status;
            target.Tag = _.Tag;
            target.Comment = _.Comment;
            target.InnerComment = _.InnerComment;
            target.Currency = _.Currency;
            target.Coupon = _.Coupon;
            target.ManualShippingTotal = _.ManualShippingTotal;
            target.ManualSubTotal = _.ManualSubTotal;
            target.ManualRelDiscountAmount = _.ManualRelDiscountAmount;
            target.LanguageCode = _.LanguageCode;
            target.IsCancelled = _.IsCancelled;
            target.CancelledDate = _.CancelledDate;
            target.CancelReason = _.CancelReason;

            // target.Totals     = default(QuoteRequestTotals)               ;

            if (ShipmentMethodCode != null)
            {
                target.ShipmentMethod = new ShipmentMethod
                {
                    OptionName = ShipmentMethodOption,
                    ShipmentMethodCode = ShipmentMethodCode
                };
            }
            target.Addresses = Addresses.Select(x => x.ToModel(AbstractTypeFactory<Address>.TryCreateInstance())).ToList();
            target.Attachments = Attachments.Select(x => x.ToModel(AbstractTypeFactory<QuoteAttachment>.TryCreateInstance())).ToList();
            target.Items = Items.Select(x => x.ToModel(AbstractTypeFactory<QuoteItem>.TryCreateInstance())).ToList();
            target.DynamicProperties = DynamicPropertyObjectValues.GroupBy(g => g.PropertyId).Select(x =>
            {
                var property = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();
                property.Id = x.Key;
                property.Name = x.FirstOrDefault()?.PropertyName;
                property.Values = x.Select(v => v.ToModel(AbstractTypeFactory<DynamicPropertyObjectValue>.TryCreateInstance())).ToArray();
                return property;
            }).ToArray();

            return target;
        }

        public QuoteRequestEntity FromModel(QuoteRequest _, PrimaryKeyResolvingMap pkMap)
        {
            if (_ == null)
                throw new ArgumentNullException(nameof(_));

            pkMap.AddPair(_, this);

            var target = this;
            target.Id = _.Id;
            target.CreatedBy = _.CreatedBy;
            target.CreatedDate = _.CreatedDate;
            target.ModifiedBy = _.ModifiedBy;
            target.ModifiedDate = _.ModifiedDate;

            target.Number = _.Number;
            target.StoreId = _.StoreId;
            target.ChannelId = _.ChannelId;
            target.IsAnonymous = _.IsAnonymous;
            target.CustomerId = _.CustomerId;
            target.CustomerName = _.CustomerName;
            target.OrganizationName = _.OrganizationName;
            target.OrganizationId = _.OrganizationId;
            target.EmployeeId = _.EmployeeId;
            target.EmployeeName = _.EmployeeName;
            target.ExpirationDate = _.ExpirationDate;
            target.ReminderDate = _.ReminderDate;
            target.EnableNotification = _.EnableNotification;
            target.IsLocked = _.IsLocked;
            target.Status = _.Status;
            target.Tag = _.Tag;
            target.Comment = _.Comment;
            target.InnerComment = _.InnerComment;
            target.Currency = _.Currency;
            target.Coupon = _.Coupon;
            target.ManualShippingTotal = _.ManualShippingTotal;
            target.ManualSubTotal = _.ManualSubTotal;
            target.ManualRelDiscountAmount = _.ManualRelDiscountAmount;
            target.LanguageCode = _.LanguageCode;
            target.IsCancelled = _.IsCancelled;
            target.CancelledDate = _.CancelledDate;
            target.CancelReason = _.CancelReason;

            // target. QuoteRequestTotals Totals     =               ;

            if (_.ShipmentMethod != null)
            {
                ShipmentMethodCode = _.ShipmentMethod.ShipmentMethodCode;
                ShipmentMethodOption = _.ShipmentMethod.OptionName;
            }
            if (_.Addresses != null)
            {
                Addresses = new ObservableCollection<AddressEntity>(_.Addresses.Select(x => AbstractTypeFactory<AddressEntity>.TryCreateInstance().FromModel(x)));
            }
            if (_.Attachments != null)
            {
                Attachments = new ObservableCollection<AttachmentEntity>(_.Attachments.Select(x => AbstractTypeFactory<AttachmentEntity>.TryCreateInstance().FromModel(x)));
            }
            if (_.Items != null)
            {
                Items = new ObservableCollection<QuoteItemEntity>(_.Items.Select(x => AbstractTypeFactory<QuoteItemEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }
            if (_.DynamicProperties != null)
            {
                DynamicPropertyObjectValues = new ObservableCollection<QuoteDynamicPropertyObjectValueEntity>(_.DynamicProperties.SelectMany(p => p.Values
                    .Select(v => AbstractTypeFactory<QuoteDynamicPropertyObjectValueEntity>.TryCreateInstance().FromModel(v, _, p))).OfType<QuoteDynamicPropertyObjectValueEntity>());
            }
            return this;
        }

        public void Patch(QuoteRequestEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var _ = this;
            target.Number = _.Number;
            target.StoreId = _.StoreId;
            target.ChannelId = _.ChannelId;
            target.IsAnonymous = _.IsAnonymous;
            target.CustomerId = _.CustomerId;
            target.CustomerName = _.CustomerName;
            target.OrganizationName = _.OrganizationName;
            target.OrganizationId = _.OrganizationId;
            target.EmployeeId = _.EmployeeId;
            target.EmployeeName = _.EmployeeName;
            target.ExpirationDate = _.ExpirationDate;
            target.ReminderDate = _.ReminderDate;
            target.EnableNotification = _.EnableNotification;
            target.IsLocked = _.IsLocked;
            target.Status = _.Status;
            target.Tag = _.Tag;
            target.Comment = _.Comment;
            target.InnerComment = _.InnerComment;
            target.Currency = _.Currency;
            target.Coupon = _.Coupon;
            target.ManualShippingTotal = _.ManualShippingTotal;
            target.ManualSubTotal = _.ManualSubTotal;
            target.ManualRelDiscountAmount = _.ManualRelDiscountAmount;
            target.LanguageCode = _.LanguageCode;
            target.IsCancelled = _.IsCancelled;
            target.CancelledDate = _.CancelledDate;
            target.CancelReason = _.CancelReason;

            target.StoreName = _.StoreName;
            target.ShipmentMethodCode = _.ShipmentMethodCode;
            target.ShipmentMethodOption = _.ShipmentMethodOption;
            target.IsSubmitted = _.IsSubmitted;

            if (!Addresses.IsNullCollection())
            {
                Addresses.Patch(target.Addresses, (sourceAddress, targetAddress) => { });
            }
            if (!Attachments.IsNullCollection())
            {
                Attachments.Patch(target.Attachments, (sourceAttachment, targetAttachment) => { });
            }
            if (!Items.IsNullCollection())
            {
                Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
            }
            if (!DynamicPropertyObjectValues.IsNullCollection())
            {
                DynamicPropertyObjectValues.Patch(target.DynamicPropertyObjectValues, (sourceDynamicPropertyObjectValues, targetDynamicPropertyObjectValues) => sourceDynamicPropertyObjectValues.Patch(targetDynamicPropertyObjectValues));
            }
        }

    }
}
