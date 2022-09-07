using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.QuoteExtensionsModule.Web.Models;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExtensionsModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // do nothing in here
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            var shippingCost = 100.0m;
            AbstractTypeFactory<QuoteRequest>.OverrideType<QuoteRequest, QuoteRequestExt>()
                .WithFactory(() => new QuoteRequestExt { ShippingCost = shippingCost });
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
