angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteAssetController', [
        '$scope', '$translate', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'FileUploader',
        function ($scope, $translate, bladeNavigationService, quotes, FileUploader) {
            var blade = $scope.blade;
            blade.updatePermission = 'quote:update';
            $scope.currentEntities = blade.currentEntities = blade.currentEntity.attachments;

            var _canUpload = true;
            var _options = null;
            const _fileErrors = [];
            const _uploadError = {
                status: translate('status'),
                statusText: translate('status-text'),
                data: {
                    errors: _fileErrors,
                },
            };

            $scope.canUpload = function () {
                return _canUpload && blade.hasUpdatePermission();
            }

            function initialize() {
                if ($scope.uploader || !blade.hasUpdatePermission()) {
                    blade.isLoading = false;
                    return;
                }

                // create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    method: 'POST',
                    autoUpload: true,
                    removeAfterUpload: true
                });

                uploader.onAfterAddingAll = function (items) {
                    clearErrors();

                    angular.forEach(items, function (item) {
                        const result = validateFile(item.file);
                        if (!result.succeeded) {
                            addFileError(item.file.name, result.errorCode, result.errorParameter);
                        }
                    });

                    if (_fileErrors.length > 0) {
                        uploader.clearQueue();
                    }
                };

                uploader.onSuccessItem = function (item, response, status, headers) {
                    angular.forEach(response, function (result) {
                        if (result.succeeded === false) {
                            addFileError(item.file.name, result.errorCode, result.errorParameter, result.errorMessage);
                        }
                        else {
                            result.mimeType = result.contentType;
                            blade.currentEntities.push(result);
                        }
                    });
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    addFileError(item.file.name, 'UNKNOWN_ERROR', status, response.message);
                };

                quotes.getAttachmentOptions(function (options) {
                    if (!options.scope) {
                        _canUpload = false;
                        bladeNavigationService.setError(translate('invalid-configuration'), blade);
                    }
                    else {
                        options.allowedExtensions = options.allowedExtensions.map(x => x.toLowerCase());
                        uploader.url = `/api/files/${options.scope}`;
                        _options = options;
                    }

                    blade.isLoading = false;
                });
            }

            $scope.removeAction = function (asset) {
                const idx = blade.currentEntities.indexOf(asset);
                if (idx >= 0) {
                    blade.currentEntities.splice(idx, 1);
                }
            };

            $scope.copyUrl = function (data) {
                var link = document.createElement("a");
                link.href = data.url;
                window.prompt('Copy to clipboard: Ctrl+C, Enter', link.href);
            };

            function validateFile(file) {
                const result = {
                    succeeded: true,
                };

                const existingFileNames = blade.currentEntities.map(x => x.name.toLowerCase());

                if (existingFileNames.includes(file.name.toLowerCase())) {
                    result.succeeded = false;
                    result.errorCode = 'duplicate-name';
                }
                else if (_options) {
                    if (file.size > _options.maxFileSize) {
                        result.succeeded = false;
                        result.errorCode = 'INVALID_SIZE';
                        result.errorParameter = _options.maxFileSize;
                    }
                    else {
                        if (_options.allowedExtensions.length) {
                            const fileExtension = /(\.[^.]+)?$/.exec(file.name)?.[1]?.toLowerCase();
                            if (!_options.allowedExtensions.includes(fileExtension)) {
                                result.succeeded = false;
                                result.errorCode = 'INVALID_EXTENSION';
                                result.errorParameter = _options.allowedExtensions;
                            }
                        }
                    }
                }

                return result;
            }

            function clearErrors() {
                _fileErrors.length = 0;
                bladeNavigationService.clearError(blade);
            }

            function addFileError(fileName, code, parameter, message) {
                if (code === 'INVALID_EXTENSION' && Array.isArray(parameter)) {
                    parameter = parameter.join(', ');
                }

                var errorMessage = translateError(code, { parameter: parameter, message: message }) || message || parameter;
                const error = translate('template', { fileName: fileName, errorMessage: errorMessage });
                _fileErrors.push(error);
                bladeNavigationService.setError(_uploadError, blade);
            }

            function translateError(key, parameters) {
                const result = translate(key, parameters);
                return result !== key ? result : null;
            }

            function translate(key, parameters) {
                return $translate.instant(`file-upload-error.${key}`, parameters);
            }

            initialize();
        }]);
