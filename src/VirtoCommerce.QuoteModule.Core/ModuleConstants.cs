using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.QuoteModule.Core
{
    public static class QuoteStatus
    {
        public const string Draft = "Draft";
        public const string Processing = "Processing";
        public const string Cancelled = "Cancelled";
        public const string ProposalSent = "Proposal sent";
        public const string Ordered = "Ordered";
        public const string Declined = "Declined";
    }

    public static class ModuleConstants
    {
        public const string QuoteAttachmentsScope = "quote-attachments";

        public static class Security
        {
            public static class Permissions
            {
                public const string Read = "quote:read";
                public const string Create = "quote:create";
                public const string Access = "quote:access";
                public const string Update = "quote:update";
                public const string Delete = "quote:delete";

                public static string[] AllPermissions { get; } = { Read, Create, Access, Update, Delete };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor Status { get; } = new SettingDescriptor
                {
                    Name = "Quotes.Status",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    DefaultValue = "New",
                    AllowedValues = new object[] { QuoteStatus.Draft, "New", QuoteStatus.Processing, QuoteStatus.ProposalSent, QuoteStatus.Ordered, QuoteStatus.Cancelled, QuoteStatus.Declined }
                };

                public static SettingDescriptor DefaultStatus { get; } = new SettingDescriptor
                {
                    Name = "Quotes.DefaultStatus",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = QuoteStatus.Draft,
                };

                public static SettingDescriptor QuoteRequestNewNumberTemplate { get; } = new SettingDescriptor
                {
                    Name = "Quotes.QuoteRequestNewNumberTemplate",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "RFQ{0:yyMMdd}-{1:D5}"
                };

                public static SettingDescriptor EnableQuotes { get; } = new SettingDescriptor
                {
                    Name = "Quotes.EnableQuotes",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                    IsPublic = true,
                };

                public static SettingDescriptor FileUploadScopeName { get; } = new SettingDescriptor
                {
                    Name = "Quotes.FileUploadScopeName",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "quote-attachments",
                    IsPublic = true,
                };

            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    yield return General.EnableQuotes;
                    yield return General.Status;
                    yield return General.DefaultStatus;
                    yield return General.QuoteRequestNewNumberTemplate;
                    yield return General.FileUploadScopeName;
                }
            }

            public static IEnumerable<SettingDescriptor> StoreLevelSettings
            {
                get
                {
                    yield return General.QuoteRequestNewNumberTemplate;
                    yield return General.EnableQuotes;
                    yield return General.FileUploadScopeName;
                }
            }
        }
    }
}
