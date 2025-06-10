angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemConfigurationTextsController', [
        '$scope', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
        function ($scope, uiGridHelper, bladeNavigationService) {
            var blade = $scope.blade;
            blade.title = 'quotes.blades.quote-item-configuration.menu.texts.title';
            blade.headIcon = 'fa fa-font';

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

            function initialize() {
                blade.isLoading = false;
                blade.items = blade.currentEntity.configurationItems.filter(x => x.type === 'Text');
            }

            initialize();
        }]);
