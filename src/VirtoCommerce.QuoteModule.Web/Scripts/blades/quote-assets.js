angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteAssetController', [
        '$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'FileUploader',
        function ($scope, bladeNavigationService, quotes, FileUploader) {
            var blade = $scope.blade;
            blade.updatePermission = 'quote:update';
            $scope.currentEntities = blade.currentEntities = blade.currentEntity.attachments;

            function initialize() {
                if ($scope.uploader || !blade.hasUpdatePermission()) {
                    blade.isLoading = false;
                    return;
                }

                // create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    url: 'api/assets?folderUrl=quote/' + blade.currentEntity.id,
                    method: 'POST',
                    autoUpload: true,
                    removeAfterUpload: true
                });

                uploader.onSuccessItem = function (fileItem, assets, status, headers) {
                    angular.forEach(assets, function (asset) {
                        asset.mimeType = asset.contentType;
                        //ADD uploaded asset
                        blade.currentEntities.push(asset);
                    });
                };

                uploader.onAfterAddingAll = function (addedItems) {
                    bladeNavigationService.setError(null, blade);
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };

                quotes.getAttachmentOptions(function (options) {
                    if (options.scope) {
                        uploader.url = `/api/files/${options.scope}`;
                    }

                    blade.isLoading = false;
                });
            }

            $scope.removeAction = function (asset) {
                var idx = blade.currentEntities.indexOf(asset);
                if (idx >= 0) {
                    blade.currentEntities.splice(idx, 1);
                }
            };

            $scope.copyUrl = function (data) {
                window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
            };

            initialize();
        }]);
