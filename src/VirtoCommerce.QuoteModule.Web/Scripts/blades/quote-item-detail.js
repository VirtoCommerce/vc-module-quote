angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemDetailController',
        ['$scope', 'platformWebApp.bladeNavigationService',
            function ($scope, bladeNavigationService) {
                var blade = $scope.blade;
                blade.isLoading = false;

                blade.formScope = null;
                blade.metaFields = [
                    {
                        title: "quotes.blades.quote-item-configuration.labels.product-name",
                        templateUrl: "name.html"
                    }
                ];

                blade.refresh = function () {
                    blade.isLoading = false;
                };

                blade.setForm = function (form) {
                    blade.formScope = form;
                }

                blade.openItemDetail = function () {
                    var newBlade = {
                        id: "listItemDetail",
                        controller: 'virtoCommerce.catalogModule.itemDetailController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html',
                        title: blade.currentEntity.name,
                        itemId: blade.currentEntity.productId
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                blade.refresh();
            }]);
