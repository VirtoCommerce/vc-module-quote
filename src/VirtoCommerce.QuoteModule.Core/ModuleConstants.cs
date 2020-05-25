using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.QuoteModule.Core
{
    public static class ModuleConstants
    {
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
                    AllowedValues = new object[] { "New", "Processing", "Proposal sent", "Ordered" }
                };
                public static SettingDescriptor QuoteRequestNewNumberTemplate { get; } = new SettingDescriptor
                {
                    Name = "Quotes.QuoteRequestNewNumberTemplate",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "RFQ{0:yyMMdd}-{1:D5}",
                    AllowedValues = new object[] { "New", "Processing", "Proposal sent", "Ordered" }
                };
                public static SettingDescriptor EnableQuotes { get; } = new SettingDescriptor
                {
                    Name = "Quotes.EnableQuotes",
                    GroupName = "Quotes|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    yield return General.Status;
                    yield return General.QuoteRequestNewNumberTemplate;
                    yield return General.EnableQuotes;
                }
            }

            public static IEnumerable<SettingDescriptor> StoreLevelSettings
            {
                get
                {
                    yield return General.QuoteRequestNewNumberTemplate;
                    yield return General.EnableQuotes;
                }
            }
        }
    }
}