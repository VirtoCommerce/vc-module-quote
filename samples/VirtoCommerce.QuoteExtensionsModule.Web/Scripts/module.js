// Call this to register your module to main application
var moduleName = 'QuoteExtensionsModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.metaFormsService',
        function (metaFormsService) {
            //extend quote via metaForms service
            metaFormsService.registerMetaFields("quoteDetails",
                [
                    {
                        name: "shippingCost",
                        title: "Total Shipping Cost",
                        valueType: "Decimal"
                    }
                ]);
        }
    ]);
