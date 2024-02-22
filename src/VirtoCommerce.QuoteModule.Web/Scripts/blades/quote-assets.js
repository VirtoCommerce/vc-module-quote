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

                // After selecting files, but before uploading them
                uploader.onAfterAddingAll = function (items) {
                    clearErrors();
                };

                uploader.onSuccessItem = function (item, response, status, headers) {
                    angular.forEach(response, function (result) {
                        if (result.succeeded === false) {
                            addError(item._file.name, result.errorMessage, result.errorCode);
                        }
                        else {
                            result.mimeType = result.contentType;

                            //ADD uploaded asset
                            blade.currentEntities.push(result);
                        }
                    });
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    addError(item._file.name, response.message, status);
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
                window.prompt('Copy to clipboard: Ctrl+C, Enter', data.url);
            };

            const uploadError = {
                status: 'Error',
                statusText: 'File upload failed',
                data: {
                    errors: [],
                },
            };

            function clearErrors() {
                uploadError.data.errors.length = 0;
                bladeNavigationService.clearError(blade);
            }

            function addError(fileName, message, status) {
                const error = `${fileName} failed: ${message || status}`;
                uploadError.data.errors.push(error);
                bladeNavigationService.setError(uploadError, blade);
            }

            initialize();
        }]);
