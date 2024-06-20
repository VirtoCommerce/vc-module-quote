using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Helpers;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.Xapi.Core.Services;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteType : ExtendableGraphType<QuoteAggregate>
{
    public QuoteType(IDynamicPropertyResolverService dynamicPropertyResolverService)
    {
        Field(x => x.Model.CancelledDate, nullable: true);
        Field(x => x.Model.CancelReason, nullable: true);
        Field(x => x.Model.ChannelId, nullable: true);
        Field(x => x.Model.Comment, nullable: true);
        Field(x => x.Model.Coupon, nullable: true);
        Field(x => x.Model.CustomerId, nullable: true);
        Field(x => x.Model.CustomerName, nullable: true);
        Field(x => x.Model.CreatedBy, nullable: true);
        Field(x => x.Model.CreatedDate, nullable: false);
        Field(x => x.Model.EmployeeId, nullable: true);
        Field(x => x.Model.EmployeeName, nullable: true);
        Field(x => x.Model.EnableNotification, nullable: false);
        Field(x => x.Model.ExpirationDate, nullable: true);
        Field(x => x.Model.Id, nullable: false);
        Field(x => x.Model.InnerComment, nullable: true);
        Field(x => x.Model.IsAnonymous, nullable: false);
        Field(x => x.Model.IsCancelled, nullable: false);
        Field(x => x.Model.IsLocked, nullable: false);
        Field(x => x.Model.LanguageCode, nullable: true);
        Field(x => x.Model.ModifiedBy, nullable: true);
        Field(x => x.Model.ModifiedDate, nullable: true);
        Field(x => x.Model.Number, nullable: false);
        Field(x => x.Model.ObjectType, nullable: true);
        Field(x => x.Model.OrganizationId, nullable: true);
        Field(x => x.Model.OrganizationName, nullable: true);
        Field(x => x.Model.ReminderDate, nullable: true);
        Field(x => x.Model.Status, nullable: true);
        Field(x => x.Model.StoreId, nullable: false);
        Field(x => x.Model.Tag, nullable: true);

        Field<NonNullGraphType<CurrencyType>>(nameof(QuoteRequest.Currency),
            resolve: context => context.Source.Currency);
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequest.ManualRelDiscountAmount),
            resolve: context => context.Source.Model.ManualRelDiscountAmount.ToMoney(context.Source.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequest.ManualShippingTotal),
            resolve: context => context.Source.Model.ManualShippingTotal.ToMoney(context.Source.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequest.ManualSubTotal),
            resolve: context => context.Source.Model.ManualSubTotal.ToMoney(context.Source.Currency));

        ExtendableField<NonNullGraphType<QuoteTotalsType>>(nameof(QuoteRequest.Totals),
            resolve: context => context.Source.Totals);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteItemType>>>>(nameof(QuoteRequest.Items),
            resolve: context => context.Source.Items);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteAddressType>>>>(nameof(QuoteRequest.Addresses),
            resolve: context => context.Source.Model.Addresses);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteAttachmentType>>>>(nameof(QuoteRequest.Attachments),
            resolve: context => context.Source.Model.Attachments);
        ExtendableField<QuoteShipmentMethodType>(nameof(QuoteRequest.ShipmentMethod), resolve: context => context.Source.ShipmentMethod);
        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<QuoteTaxDetailType>>>>(nameof(QuoteRequest.TaxDetails), resolve: context => context.Source.TaxDetails);

        ExtendableField<NonNullGraphType<ListGraphType<NonNullGraphType<DynamicPropertyValueType>>>>(
            nameof(QuoteRequest.DynamicProperties),
            "Quote dynamic property values",
            QueryArgumentPresets.GetArgumentForDynamicProperties(),
            context => dynamicPropertyResolverService.LoadDynamicPropertyValues(context.Source.Model, context.GetArgumentOrValue<string>("cultureName")));
    }
}
