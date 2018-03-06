using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Platform.Core.Common;

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
      
        public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<QuoteItemEntity> Items { get; set; }
		public virtual ObservableCollection<AttachmentEntity> Attachments { get; set; }

        #endregion

        public virtual QuoteRequest ToModel(QuoteRequest quoteRequest)
        {
          if (quoteRequest == null)
            throw new ArgumentNullException(nameof(quoteRequest));
       
          quoteRequest.Id = this.Id;
          quoteRequest.Number = this.Number;
          quoteRequest.StoreId = this.StoreId;
          quoteRequest.ChannelId = this.ChannelId;
          quoteRequest.OrganizationId = this.OrganizationId;
          quoteRequest.OrganizationName = this.OrganizationName;
          quoteRequest.IsAnonymous = this.IsAnonymous;
          quoteRequest.CustomerId = this.CustomerId;
          quoteRequest.CustomerName = this.CustomerName;
          quoteRequest.EmployeeId = this.EmployeeId;
          quoteRequest.EmployeeName = this.EmployeeName;
          quoteRequest.ExpirationDate = this.ExpirationDate;
          quoteRequest.ReminderDate = this.ReminderDate;
          quoteRequest.EnableNotification = this.EnableNotification;
          quoteRequest.IsLocked = this.IsLocked;
          quoteRequest.Status = this.Status;
          quoteRequest.Comment = this.Comment;
          quoteRequest.InnerComment = this.InnerComment;
          quoteRequest.Tag = this.Tag;
          quoteRequest.Currency = this.Currency;
          quoteRequest.LanguageCode = this.LanguageCode;
          quoteRequest.Coupon = this.Coupon;
          quoteRequest.ShipmentMethod= new ShipmentMethod
          {
            OptionName = this.ShipmentMethodOption,
            ShipmentMethodCode = this.ShipmentMethodCode
          };
          quoteRequest.IsCancelled = this.IsCancelled;
          quoteRequest.CancelledDate = this.CancelledDate;
          quoteRequest.CancelReason = this.CancelReason;
          quoteRequest.ManualSubTotal = this.ManualSubTotal;
          quoteRequest.ManualRelDiscountAmount = this.ManualRelDiscountAmount;
          quoteRequest.ManualShippingTotal = this.ManualShippingTotal;

          quoteRequest.CreatedDate = this.CreatedDate;
          quoteRequest.ModifiedDate = this.ModifiedDate;
          quoteRequest.CreatedBy = this.CreatedBy;
          quoteRequest.ModifiedBy = this.ModifiedBy;

          quoteRequest.Addresses = new ObservableCollection<Address>(this.Addresses.Select(x => x.ToModel(AbstractTypeFactory<Address>.TryCreateInstance())));
          quoteRequest.Items = new ObservableCollection<QuoteItem>(this.Items.Select(x => x.ToModel(AbstractTypeFactory<QuoteItem>.TryCreateInstance())));
          quoteRequest.Attachments = new ObservableCollection<QuoteAttachment>(this.Attachments.Select(x => x.ToModel(AbstractTypeFactory<QuoteAttachment>.TryCreateInstance())));
       
          return quoteRequest;
        }
       
        public virtual QuoteRequestEntity FromModel(QuoteRequest quoteRequest, PrimaryKeyResolvingMap pkMap)
        {
          if (quoteRequest == null)
            throw new ArgumentNullException(nameof(quoteRequest));

          pkMap.AddPair(quoteRequest, this);
      
          this.Id = quoteRequest.Id;
          this.Number = quoteRequest.Number;
          this.StoreId = quoteRequest.StoreId;
          this.ChannelId = quoteRequest.ChannelId;
          this.OrganizationId = quoteRequest.OrganizationId;
          this.OrganizationName = quoteRequest.OrganizationName;
          this.IsAnonymous = quoteRequest.IsAnonymous;
          this.CustomerId = quoteRequest.CustomerId;
          this.CustomerName = quoteRequest.CustomerName;
          this.EmployeeId = quoteRequest.EmployeeId;
          this.EmployeeName = quoteRequest.EmployeeName;
          this.ExpirationDate = quoteRequest.ExpirationDate;
          this.ReminderDate = quoteRequest.ReminderDate;
          this.EnableNotification = quoteRequest.EnableNotification;
          this.IsLocked = quoteRequest.IsLocked;
          this.Status = quoteRequest.Status;
          this.Comment = quoteRequest.Comment;
          this.InnerComment = quoteRequest.InnerComment;
          this.Tag = quoteRequest.Tag;
          this.Currency = quoteRequest.Currency;
          this.LanguageCode = quoteRequest.LanguageCode;
          this.Coupon = quoteRequest.Coupon;
          if (quoteRequest.ShipmentMethod != null)
          {
            this.ShipmentMethodCode = quoteRequest.ShipmentMethod.ShipmentMethodCode;
            this.ShipmentMethodOption = quoteRequest.ShipmentMethod.OptionName;
          }
          this.IsCancelled = quoteRequest.IsCancelled;
          this.CancelledDate = quoteRequest.CancelledDate;
          this.CancelReason = quoteRequest.CancelReason;
          this.ManualSubTotal = quoteRequest.ManualSubTotal;
          this.ManualRelDiscountAmount = quoteRequest.ManualRelDiscountAmount;
          this.ManualShippingTotal = quoteRequest.ManualShippingTotal;

          this.CreatedDate  = quoteRequest.CreatedDate;
          this.ModifiedDate = quoteRequest.ModifiedDate;
          this.CreatedBy    = quoteRequest.CreatedBy;
          this.ModifiedBy   = quoteRequest.ModifiedBy;

            if (quoteRequest.Addresses != null)
          {
            this.Addresses = new ObservableCollection<AddressEntity>(quoteRequest.Addresses.Select(x => AbstractTypeFactory<AddressEntity>.TryCreateInstance().FromModel(x)));
          }
          if (quoteRequest.Items != null)
          {
            this.Items = new ObservableCollection<QuoteItemEntity>(quoteRequest.Items.Select(x => AbstractTypeFactory<QuoteItemEntity>.TryCreateInstance().FromModel(x, pkMap)));
          }
          if (quoteRequest.Attachments != null)
          {
            this.Attachments = new ObservableCollection<AttachmentEntity>(quoteRequest.Attachments.Select(x => AbstractTypeFactory<AttachmentEntity>.TryCreateInstance().FromModel(x, pkMap)));
          }
       
          return this;
        }
       
        public virtual void Patch(QuoteRequestEntity target)
        {
          target.Number = this.Number;
          target.StoreId = this.StoreId;
          target.StoreName = this.StoreName;
          target.ChannelId = this.ChannelId;
          target.OrganizationId = this.OrganizationId;
          target.OrganizationName = this.OrganizationName;
          target.IsAnonymous = this.IsAnonymous;
          target.CustomerId = this.CustomerId;
          target.CustomerName = this.CustomerName;
          target.EmployeeId = this.EmployeeId;
          target.EmployeeName = this.EmployeeName;
          target.ExpirationDate = this.ExpirationDate;
          target.ReminderDate = this.ReminderDate;
          target.EnableNotification = this.EnableNotification;
          target.IsLocked = this.IsLocked;
          target.Status = this.Status;
          target.Comment = this.Comment;
          target.InnerComment = this.InnerComment;
          target.Tag = this.Tag;
          target.IsSubmitted = this.IsSubmitted;
          target.Currency = this.Currency;
          target.LanguageCode = this.LanguageCode;
          target.Coupon = this.Coupon;
          target.ShipmentMethodCode = this.ShipmentMethodCode;
          target.ShipmentMethodOption = this.ShipmentMethodOption;
          target.IsCancelled = this.IsCancelled;
          target.CancelledDate = this.CancelledDate;
          target.CancelReason = this.CancelReason;
          target.ManualSubTotal = this.ManualSubTotal;
          target.ManualRelDiscountAmount = this.ManualRelDiscountAmount;
          target.ManualShippingTotal = this.ManualShippingTotal;

          target.CreatedDate = this.CreatedDate;
          target.ModifiedDate = this.ModifiedDate;
          target.CreatedBy = this.CreatedBy;
          target.ModifiedBy = this.ModifiedBy;

            if (!this.Addresses.IsNullCollection())
          {
            this.Addresses.Patch(target.Addresses, (sourceAddresses, targetAddresses) => sourceAddresses.Patch(targetAddresses));
          }
          if (!this.Items.IsNullCollection())
          {
            this.Items.Patch(target.Items, (sourceItems, targetItems) => sourceItems.Patch(targetItems));
          }
          if (!this.Attachments.IsNullCollection())
          {
            this.Attachments.Patch(target.Attachments, (sourceAttachments, targetAttachments) => sourceAttachments.Patch(targetAttachments));
          }
        }
  }    
}
