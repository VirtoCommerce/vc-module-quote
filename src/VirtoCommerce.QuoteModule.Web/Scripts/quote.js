//Call this to register our module to main application
var moduleName = "virtoCommerce.quoteModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(
        ['$stateProvider', function ($stateProvider) {
            $stateProvider
                .state('workspace.quoteModule', {
                    url: '/quotes',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var blade = {
                                id: 'quote',
                                title: 'quotes.blades.quotes-list.title',
                                controller: 'virtoCommerce.quoteModule.quotesListController',
                                template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quotes-list.tpl.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(blade);
                            //Need for isolate and prevent conflict module css to another modules 
                            //it value included in bladeContainer as ng-class='moduleName'
                            $scope.moduleName = "vc-quote";
                        }
                    ]
                });
        }]
    )
    .run(
        [
            '$state', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.metaFormsService',
            function ($state, mainMenuService, widgetService, metaFormsService) {
                //Register module in main menu
                var menuItem = {
                    path: 'browse/quote',
                    icon: 'fa fa-file-text-o',
                    title: 'quotes.main-menu-title',
                    priority: 95,
                    action: function () { $state.go('workspace.quoteModule'); },
                    permission: 'quote:access'
                };
                mainMenuService.addMenuItem(menuItem);

                //Register widgets in quote details
                widgetService.registerWidget({
                    size: [2, 1],
                    controller: 'virtoCommerce.quoteModule.quoteAddressWidgetController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/widgets/quote-address-widget.tpl.html'
                }, 'quoteDetail');
                widgetService.registerWidget({
                    size: [2, 1],
                    controller: 'virtoCommerce.quoteModule.quoteItemsWidgetController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/widgets/quote-totals-widget.tpl.html'
                    //template: 'Modules/$(VirtoCommerce.Quote)/Scripts/widgets/quote-items-widget.tpl.html'
                }, 'quoteDetail');
                widgetService.registerWidget({
                    controller: 'virtoCommerce.quoteModule.quoteAssetWidgetController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/widgets/quote-asset-widget.tpl.html'
                }, 'quoteDetail');
                widgetService.registerWidget({
                    controller: 'platformWebApp.dynamicPropertyWidgetController',
                    template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
                }, 'quoteDetail');
                widgetService.registerWidget({
                    controller: 'platformWebApp.changeLog.operationsWidgetController',
                    template: '$(Platform)/Scripts/app/changeLog/widgets/operations-widget.tpl.html'
                }, 'quoteDetail');

                widgetService.registerWidget({
                    controller: 'virtoCommerce.quoteModule.quoteItemConfigurationWidgetController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/widgets/quote-item-configuration-widget.html'
                }, 'quoteItemDetail');

                metaFormsService.registerMetaFields('QuoteAddressDetails', [
                {
                    name: 'description',
                    templateUrl: 'description.html',
                    priority: 0
                },
                {
                    templateUrl: 'addressTypeSelector.html',
                    priority: 1
                },
                {
                    name: 'firstName',
                    title: 'orders.blades.address-details.labels.first-name',
                    placeholder: 'orders.blades.address-details.placeholders.first-name',
                    valueType: 'ShortText',
                    isRequired: false,
                    priority: 2
                },
                {
                    name: 'middleName',
                    title: 'orders.blades.address-details.labels.middle-name',
                    placeholder: 'orders.blades.address-details.placeholders.middle-name',
                    valueType: 'ShortText',
                    isRequired: false,
                    priority: 3
                },
                {
                    name: 'lastName',
                    title: 'orders.blades.address-details.labels.last-name',
                    placeholder: 'orders.blades.address-details.placeholders.last-name',
                    valueType: 'ShortText',
                    isRequired: false,
                    priority: 4
                },
                {
                    templateUrl: 'countrySelector.html',
                    priority: 5
                },
                {
                    templateUrl: 'countryRegionSelector.html',
                    priority: 6
                },
                {
                    name: 'city',
                    title: 'orders.blades.address-details.labels.city',
                    placeholder: 'orders.blades.address-details.placeholders.city',
                    valueType: 'ShortText',
                    isRequired: true,
                    priority: 7
                },
                {
                    name: 'line1',
                    title: 'orders.blades.address-details.labels.address1',
                    placeholder: 'orders.blades.address-details.placeholders.address1',
                    valueType: 'ShortText',
                    isRequired: true,
                    priority: 8
                },
                {
                    name: 'line2',
                    title: 'orders.blades.address-details.labels.address2',
                    placeholder: 'orders.blades.address-details.placeholders.address2',
                    valueType: 'ShortText',
                    priority: 9
                },
                {
                    name: 'postalCode',
                    title: 'orders.blades.address-details.labels.zip-code',
                    placeholder: 'orders.blades.address-details.placeholders.zip-code',
                    valueType: 'ShortText',
                    isRequired: true,
                    priority: 10
                },
                {
                    name: 'email',
                    title: 'orders.blades.address-details.labels.email',
                    placeholder: 'orders.blades.address-details.placeholders.email',
                    valueType: 'Email',
                    priority: 11
                },
                {
                    name: 'phone',
                    title: 'orders.blades.address-details.labels.phone',
                    placeholder: 'orders.blades.address-details.placeholders.phone',
                    valueType: 'ShortText',
                    priority: 12
                }
            ]);
        }]);
