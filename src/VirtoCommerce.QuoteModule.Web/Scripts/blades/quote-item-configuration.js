angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemConfigurationController', [
        '$scope', 'platformWebApp.bladeNavigationService',
        function ($scope, bladeNavigationService) {
            var blade = $scope.blade;
            blade.title = 'quotes.blades.quote-item-configuration.title';
            blade.headIcon = 'fas fa-sliders';

            $scope.showProducts = function() {
                var newBlade = {
                    id: "quoteConfigurationProducts",
                    controller: 'virtoCommerce.quoteModule.quoteItemConfigurationProductsController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-item-configuration-products.html',
                    currentEntity: blade.currentEntity,
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
            }

            $scope.showTexts = function () {
                var newBlade = {
                    id: "сonfigurationProducts",
                    controller: 'virtoCommerce.quoteModule.quoteItemConfigurationTextsController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-item-configuration-texts.html',
                    currentEntity: blade.currentEntity,
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
            }

            $scope.showFiles = function () {
                var newBlade = {
                    id: "сonfigurationFiles",
                    controller: 'virtoCommerce.quoteModule.quoteItemConfigurationFilesController',
                    template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-item-configuration-files.html',
                    currentEntity: blade.currentEntity,
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
            }

            blade.isLoading = false;
        }]);
