angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteItemDetailController', ['$scope',
        'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.metaFormsService',
        'virtoCommerce.quoteModule.quotes', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.memberTypesResolverService',
        function ($scope, bladeNavigationService, settings, dialogService, metaFormsService, quotes, members, memberTypesResolverService) {
            var blade = $scope.blade;
            blade.isLoading = false;


        }]);
