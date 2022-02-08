using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.TaxModule.Core.Model;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteRequest : AuditableEntity, IHasTaxDetalization, ISupportCancellation, IHasDynamicProperties, IHasLanguageCode, IHasChangesHistory
    {
        public string Number { get; set; }
        public string StoreId { get; set; }
        public string ChannelId { get; set; }
        public bool IsAnonymous { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }

        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public DateTime? ReminderDate { get; set; }

        public bool EnableNotification { get; set; }
        public bool IsLocked { get; set; }
        public string Status { get; set; }

        public string Tag { get; set; }

        public string Comment { get; set; }
        public string InnerComment { get; set; }
        public string Currency { get; set; }

        public string Coupon { get; set; }

        /// <summary>
        /// Manual shipping total for quote request
        /// </summary>
        public decimal ManualShippingTotal { get; set; }
        /// <summary>
        /// Manual sub total for quote request
        /// </summary>
        public decimal ManualSubTotal { get; set; }
        /// <summary>
        /// Relative manual discount amount for quote request in percent
        /// </summary>
        public decimal ManualRelDiscountAmount { get; set; }

        public QuoteRequestTotals Totals { get; set; }

        public ShipmentMethod ShipmentMethod { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<QuoteItem> Items { get; set; }
        public ICollection<QuoteAttachment> Attachments { get; set; }

        #region IHasChangesHistory Members
        public ICollection<OperationLog> OperationsLog { get; set; }
        #endregion

        #region ILanguageSupport Members
        public string LanguageCode { get; set; }
        #endregion

        #region IHaveTaxDetalization Members
        public ICollection<TaxDetail> TaxDetails { get; set; }
        #endregion

        #region ISupportCancelation Members
        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancelReason { get; set; }
        #endregion

        #region IHasDynamicProperties Members
        public string ObjectType => typeof(QuoteRequest).FullName;
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
        #endregion

        public virtual ShoppingCart ToCartModel(ShoppingCart target)
        {
            var _ = this;
            target.Id = _.Id;
            target.CreatedBy = _.CreatedBy;
            target.CreatedDate = _.CreatedDate;
            target.ModifiedBy = _.ModifiedBy;
            target.ModifiedDate = _.ModifiedDate;

            target.StoreId = _.StoreId;
            target.ChannelId = _.ChannelId;
            target.IsAnonymous = _.IsAnonymous;
            target.CustomerId = _.CustomerId;
            target.CustomerName = _.CustomerName;
            target.OrganizationId = _.OrganizationId;
            target.Status = _.Status;
            target.Comment = _.Comment;
            target.Currency = _.Currency;
            target.Coupon = _.Coupon;
            target.LanguageCode = _.LanguageCode;

            if (Items != null)
            {
                target.Items = Items.Select(x => x.ToCartModel(AbstractTypeFactory<LineItem>.TryCreateInstance())).ToList();
            }

            if (ShipmentMethod != null)
            {
                var shipment = new Shipment
                {
                    Currency = Currency,
                    ShipmentMethodCode = ShipmentMethod.ShipmentMethodCode,
                    ShipmentMethodOption = ShipmentMethod.OptionName
                };
                target.Shipments = new List<Shipment>(new[] { shipment });
            }

            return target;
        }

        public virtual TaxEvaluationContext ToTaxEvalContext(TaxEvaluationContext target)
        {
            target.Id = Id;
            target.Code = Number;
            target.Currency = Currency;
            target.Address = Addresses?.FirstOrDefault()?.ToTaxModel(AbstractTypeFactory<TaxModule.Core.Model.Address>.TryCreateInstance());
            target.Type = GetType().Name;
            foreach (var quoteItem in Items)
            {
                var line = new TaxLine
                {
                    Id = quoteItem.Id,
                    Code = quoteItem.Sku,
                    Name = quoteItem.Name,
                    TaxType = quoteItem.TaxType,
                    Amount = quoteItem.SelectedTierPrice.Price * quoteItem.SelectedTierPrice.Quantity
                };
                target.Lines.Add(line);
            }
            return target;
        }
    }
}
