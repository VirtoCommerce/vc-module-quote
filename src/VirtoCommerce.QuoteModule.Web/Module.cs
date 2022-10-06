using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.PricingModule.Core.Model.Conditions;
using VirtoCommerce.QuoteModule.Core.Events;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.Data.Handlers;
using VirtoCommerce.QuoteModule.Data.Repositories;
using VirtoCommerce.QuoteModule.Data.Services;
using VirtoCommerce.QuoteModule.Web.ExportImport;
using VirtoCommerce.StoreModule.Core.Model;

namespace VirtoCommerce.QuoteModule.Web
{
    public class Module : IModule, IExportSupport, IImportSupport
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        private IApplicationBuilder _appBuilder;

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<QuoteDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString(ModuleInfo.Id) ?? configuration.GetConnectionString("VirtoCommerce"));
            });

            serviceCollection.AddTransient<IQuoteRepository, QuoteRepository>();
            serviceCollection.AddTransient<Func<IQuoteRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IQuoteRepository>());
            serviceCollection.AddTransient<IQuoteRequestService, QuoteRequestService>();
            serviceCollection.AddTransient<IQuoteConverter, QuoteConverter>();

            serviceCollection.AddSingleton<PriceConditionTreePrototype>();

            serviceCollection.AddTransient<IQuoteTotalsCalculator, DefaultQuoteTotalsCalculator>();
            serviceCollection.AddTransient<QuoteExportImport>();
            serviceCollection.AddTransient<LogChangesEventHandler>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            _appBuilder = appBuilder;

            var dynamicPropertyRegistrar = appBuilder.ApplicationServices.GetRequiredService<IDynamicPropertyRegistrar>();
            dynamicPropertyRegistrar.RegisterType<QuoteRequest>();

            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(Core.ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
            settingsRegistrar.RegisterSettingsForType(Core.ModuleConstants.Settings.StoreLevelSettings, nameof(Store));

            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(Core.ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission
                {
                    GroupName = "Quotes",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());

            var handlerRegistrar = appBuilder.ApplicationServices.GetRequiredService<IHandlerRegistrar>();
            handlerRegistrar.RegisterHandler<QuoteRequestChangeEvent>((message, _) => appBuilder.ApplicationServices.GetRequiredService<LogChangesEventHandler>().Handle(message));

            using var serviceScope = appBuilder.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<QuoteDbContext>();
            dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
            // Method intentionally left empty.
        }

        public async Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await _appBuilder.ApplicationServices.GetRequiredService<QuoteExportImport>().DoExportAsync(outStream,
                progressCallback, cancellationToken);
        }

        public async Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await _appBuilder.ApplicationServices.GetRequiredService<QuoteExportImport>().DoImportAsync(inputStream,
              progressCallback, cancellationToken);
        }
    }
}
