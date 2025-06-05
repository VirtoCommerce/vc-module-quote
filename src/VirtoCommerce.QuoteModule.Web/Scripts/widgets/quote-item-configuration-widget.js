angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemConfigurationWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService',
            function ($scope, bladeNavigationService) {
                var blade = $scope.blade;

                $scope.openBlade = function () {
                    var newBlade = {
                        id: "quoteItemConfiguration",
                        controller: 'virtoCommerce.quoteModule.quoteItemConfigurationController',
                        template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-item-configuration.html',
                        currentEntity: blade.currentEntity,
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };
            }]);
