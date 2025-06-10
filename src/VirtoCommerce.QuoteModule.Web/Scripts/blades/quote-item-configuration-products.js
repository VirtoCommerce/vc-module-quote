angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemConfigurationProductsController', [
        '$scope', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
        function ($scope, uiGridHelper, bladeNavigationService) {
            var blade = $scope.blade;
            blade.title = 'quotes.blades.quote-item-configuration.menu.products.title';
            blade.headIcon = 'fas fa-box';

            blade.toolbarCommands = [
                {
                    name: "platform.navigation.back",
                    icon: 'fas fa-arrow-left',
                    canExecuteMethod: function () { return true; },
                    executeMethod: function () {
                        var newBlade = {
                            id: "quoteItemConfiguration",
                            controller: 'virtoCommerce.quoteModule.quoteItemConfigurationController',
                            template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-item-configuration.html',
                            currentEntity: blade.currentEntity,
                        };
                        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                    },
                }
            ];

            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    $scope.gridApi = gridApi;
                });
            };

            $scope.selectNode = function (item) {
                $scope.selectedNodeId = item.id;

                var newBlade = {
                    id: "listItemDetail",
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html',
                    title: item.name,
                    itemId: item.productId
                };
                bladeNavigationService.showBlade(newBlade, blade);
            }

            function initialize() {
                blade.isLoading = false;
                blade.items = blade.currentEntity.configurationItems.filter(x => x.type === 'Product');
            }

            initialize();
        }]);
