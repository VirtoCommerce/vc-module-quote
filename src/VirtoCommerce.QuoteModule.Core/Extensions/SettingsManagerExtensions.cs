using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using QuoteSettings = VirtoCommerce.QuoteModule.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.QuoteModule.Core.Extensions;

public static class SettingsManagerExtensions
{
    public static Task<string> GetDefaultQuoteStatusAsync(this ISettingsManager settingsManager)
    {
        return settingsManager.GetValueAsync<string>(QuoteSettings.DefaultStatus);
    }
}
