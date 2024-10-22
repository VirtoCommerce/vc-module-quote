using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Introspection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas
{
    public class QuoteSchemaFactory : SchemaFactory
    {
        public QuoteSchemaFactory(IEnumerable<ISchemaBuilder> schemaBuilders, IServiceProvider services, ISchemaFilter schemaFilter)
            : base(schemaBuilders, services, schemaFilter)
        {
        }

        protected override List<ISchemaBuilder> GetSchemaBuilders()
        {
            var schemaBuilders = base.GetSchemaBuilders();

            // find all builders with SubSchemaNameAttribute with name "Quote"
            var subSchemaBuilders = schemaBuilders
                .Where(p => p.GetType().GetCustomAttributes(typeof(SubSchemaNameAttribute), false)
                .OfType<SubSchemaNameAttribute>()
                .Any(x => "Quote".EqualsInvariant(x.Name))).ToList();

            return subSchemaBuilders;
        }
    }
}
