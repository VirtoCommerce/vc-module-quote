using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GraphQL.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
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
using VirtoCommerce.QuoteModule.Data.MySql;
using VirtoCommerce.QuoteModule.Data.PostgreSql;
using VirtoCommerce.QuoteModule.Data.Repositories;
using VirtoCommerce.QuoteModule.Data.Services;
using VirtoCommerce.QuoteModule.Data.SqlServer;
using VirtoCommerce.QuoteModule.ExperienceApi;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.Web.ExportImport;
using VirtoCommerce.StoreModule.Core.Model;

namespace VirtoCommerce.QuoteModule.Web
{
    public class Module : IModule, IExportSupport, IImportSupport, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        private IApplicationBuilder _appBuilder;
        public IConfiguration Configuration { get; set; }


        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<QuoteDbContext>((provider, options) =>
            {
                var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
                var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

                switch (databaseProvider)
                {
                    case "MySql":
                        options.UseMySqlDatabase(connectionString);
                        break;
                    case "PostgreSql":
                        options.UsePostgreSqlDatabase(connectionString);
                        break;
                    default:
                        options.UseSqlServerDatabase(connectionString);
                        break;
                }
            });

            serviceCollection.AddTransient<IQuoteRepository, QuoteRepository>();
            serviceCollection.AddTransient<Func<IQuoteRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IQuoteRepository>());
            serviceCollection.AddTransient<IQuoteRequestService, QuoteRequestService>();
            serviceCollection.AddTransient<IQuoteConverter, QuoteConverter>();

            serviceCollection.AddSingleton<PriceConditionTreePrototype>();

            serviceCollection.AddTransient<IQuoteTotalsCalculator, DefaultQuoteTotalsCalculator>();
            serviceCollection.AddTransient<QuoteExportImport>();
            serviceCollection.AddTransient<LogChangesEventHandler>();

            // GraphQL
            var assemblyMarker = typeof(AssemblyMarker);
            var graphQlBuilder = new CustomGraphQLBuilder(serviceCollection);
            graphQlBuilder.AddGraphTypes(assemblyMarker);
            serviceCollection.AddMediatR(assemblyMarker);
            serviceCollection.AddAutoMapper(assemblyMarker);
            serviceCollection.AddSchemaBuilders(assemblyMarker);
            serviceCollection.AddTransient<IQuoteAggregateRepository, QuoteAggregateRepository>();
            serviceCollection.AddSingleton<IAuthorizationHandler, QuoteAuthorizationHandler>();
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
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<QuoteDbContext>();
            if (databaseProvider == "SqlServer")
            {
                dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
            }
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
