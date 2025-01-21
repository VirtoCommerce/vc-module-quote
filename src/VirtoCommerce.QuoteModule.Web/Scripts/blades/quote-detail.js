angular.module('virtoCommerce.quoteModule')
    .controller('virtoCommerce.quoteModule.quoteDetailController', ['$scope',
        'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.metaFormsService',
        'virtoCommerce.quoteModule.quotes', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.memberTypesResolverService',
        function ($scope, bladeNavigationService, settings, dialogService, metaFormsService, quotes, members, memberTypesResolverService) {

                const QuoteProposalSentStatus = 'Proposal sent';

                var blade = $scope.blade;

                var onHoldCommand = {
                    updateName: function () {
                        this.name = (blade.currentEntity && blade.currentEntity.isLocked) ? 'quotes.commands.release-hold' : 'quotes.commands.place-on-hold';
                    },
                    // name: this.updateName(),
                    icon: 'fa fa-lock', // icon: 'fa fa-hand-paper-o',
                    executeMethod: function () {
                        var dialog = {
                            id: "confirmDialog",
                            title: "quotes.dialogs.hold-confirmation.title",
                            message: (blade.currentEntity.isLocked ? 'quotes.dialogs.hold-confirmation.message-release' : 'quotes.dialogs.hold-confirmation.message-place'),
                            callback: function (ok) {
                                if (ok) {
                                    blade.currentEntity.isLocked = !blade.currentEntity.isLocked;
                                    saveChanges();
                                }
                            }
                        };
                        dialogService.showConfirmationDialog(dialog);
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: blade.updatePermission
                };

                blade.updatePermission = 'quote:update';
                blade.metaFields = blade.metaFields ? blade.metaFields : metaFormsService.getMetaFields('quoteDetails');

                blade.refresh = function (parentRefresh) {
                    quotes.get({ id: blade.currentEntityId }, function (data) {
                        initializeBlade(data);
                        if (parentRefresh) {
                            blade.parentBlade.refresh();
                        }
                    });
                }

                function initializeBlade(data) {
                    blade.title = data.number;

                    blade.currentEntity = angular.copy(data);
                    blade.origEntity = data;
                    blade.isLoading = false;

                    initShipmentMethod();
                    onHoldCommand.updateName();
                }

                function isDirty() {
                    return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
                }

                function canSave() {
                    return isDirty() && $scope.formScope && $scope.formScope.$valid && !blade.isLocked();
                }

                function saveChanges() {
                    blade.isLoading = true;

                    quotes.update({}, blade.currentEntity, function () {
                        blade.refresh(true);
                    });
                }

                blade.openItemsBlade = function () {
                    var newBlade = {
                        id: 'quoteItems',
                        title: 'quotes.blades.quote-items.title',
                        titleValues: { title: blade.title },
                        subtitle: 'quotes.blades.quote-items.subtitle',
                        recalculateFn: blade.recalculate,
                        shippingMethods: blade.shippingMethods,
                        currentEntity: blade.currentEntity,
                        isLocked: blade.isLocked,
                        controller: 'virtoCommerce.quoteModule.quoteItemsController',
                        template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-items.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                blade.recalculate = function () {
                    quotes.recalculate({}, blade.currentEntity, function (data) {
                        blade.currentEntity.totals = data.totals;
                        bladeNavigationService.setError(null, blade);
                    });
                }

                function deleteEntry() {
                    var dialog = {
                        id: "confirmDelete",
                        title: "quotes.dialogs.quote-delete.title",
                        message: "quotes.dialogs.quote-delete.message",
                        callback: function (remove) {
                            if (remove) {
                                blade.isLoading = true;

                                quotes.remove({
                                    ids: blade.currentEntityId
                                }, function () {
                                    $scope.bladeClose();
                                    blade.parentBlade.refresh();
                                });
                            }
                        }
                    }
                    dialogService.showConfirmationDialog(dialog);
                }

                $scope.setForm = function (form) {
                    $scope.formScope = form;
                }

                blade.onClose = function (closeCallback) {
                    bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback,
                        "quotes.dialogs.quote-save.title", "quotes.dialogs.quote-save.message");
                };

                blade.isLocked = function () {
                    return blade.currentEntity && blade.currentEntity.isLocked;
                };

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.save",
                        icon: 'fas fa-save',
                        executeMethod: saveChanges,
                        canExecuteMethod: canSave,
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.reset",
                        icon: 'fa fa-undo',
                        executeMethod: function () {
                            angular.copy(blade.origEntity, blade.currentEntity);
                            onHoldCommand.updateName();
                        },
                        canExecuteMethod: isDirty,
                        permission: blade.updatePermission
                    },
                    {
                        name: "quotes.commands.submit-proposal", icon: 'fa fa-check-square-o',
                        executeMethod: function () {
                            if (blade.currentEntity.items.length === 0 ||
                                blade.currentEntity.totals.grandTotalInclTax === 0) {
                                var warningDialog = {
                                    id: "submitProposalWithWarning",
                                    title: "quotes.dialogs.proposal-submit-with-warning.title",
                                    message: "quotes.dialogs.proposal-submit-with-warning.message",
                                    callback: function (ok) {
                                        if (ok) {
                                            blade.currentEntity.status = QuoteProposalSentStatus;
                                            saveChanges();
                                        }
                                    }
                                };
                                dialogService.showWarningDialog(warningDialog);
                            }
                            else {
                                var confirmationDialog = {
                                    id: "submitProposal",
                                    title: "quotes.dialogs.proposal-submit.title",
                                    message: "quotes.dialogs.proposal-submit.message",
                                    callback: function (ok) {
                                        if (ok) {
                                            blade.currentEntity.status = QuoteProposalSentStatus;
                                            saveChanges();
                                        }
                                    }
                                };
                                dialogService.showConfirmationDialog(confirmationDialog);
                            }
                        },
                        canExecuteMethod: function () {
                            return blade.origEntity && blade.origEntity.status !== QuoteProposalSentStatus;
                        },
                        permission: blade.updatePermission
                    },
                    onHoldCommand,
                    {
                        name: "quotes.commands.cancel-document", icon: 'fa fa-remove',
                        executeMethod: function () {
                            var dialog = {
                                id: "confirmCancelOperation",
                                callback: function (reason) {
                                    if (reason) {
                                        blade.currentEntity.cancelReason = reason;
                                        blade.currentEntity.isCancelled = true;
                                        blade.currentEntity.status = 'Canceled';
                                        saveChanges();
                                    }
                                }
                            };
                            dialogService.showDialog(dialog,
                                'Modules/$(VirtoCommerce.Quote)/Scripts/dialogs/cancelQuote-dialog.tpl.html', 'virtoCommerce.quoteModule.confirmCancelDialogController');
                        },
                        canExecuteMethod: function () {
                            return blade.currentEntity && !blade.currentEntity.isCancelled;
                        },
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.delete", icon: 'fas fa-trash-alt',
                        executeMethod: deleteEntry,
                        canExecuteMethod: function () { return true; },
                        permission: 'quote:delete'
                    }
                ];

                $scope.openDictionarySettingManagement = function () {
                    var newBlade = {
                        id: 'settingDetailChild',
                        isApiSave: true,
                        currentEntityId: 'Quotes.Status',
                        parentRefresh: function (data) { $scope.quoteStatuses = data; },
                        controller: 'platformWebApp.settingDictionaryController',
                        template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                // datepicker
                $scope.datepickers = {}
                $scope.today = new Date();
                $scope.open = function ($event, which) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.datepickers[which] = true;
                };

                blade.refresh(false);

                $scope.quoteStatuses = settings.getValues({ id: 'Quotes.Status' });
                blade.shippingMethods = quotes.getShippingMethods({ id: blade.currentEntityId }, initShipmentMethod);

                blade.fetchEmployees = function (criteria) {
                    criteria.memberType = 'Employee';
                    criteria.deepSearch = true;
                    criteria.sort = 'name';

                    return members.search(criteria);
                };

                function showMemberDetailBlade(member) {
                    var foundTemplate = memberTypesResolverService.resolve(member.memberType);
                    if (foundTemplate) {
                        var newBlade = angular.copy(foundTemplate.detailBlade);
                        newBlade.currentEntity = member;
                        bladeNavigationService.showBlade(newBlade, blade);
                    } else {
                        dialogService.showNotificationDialog({
                            id: "error",
                            title: "quote.dialogs.unknown-member-type.title",
                            message: "quote.dialogs.unknown-member-type.message",
                            messageValues: { memberType: member.memberType }
                        });
                    }
                }

                blade.openCustomerDetails = function () {
                    if (blade.currentEntity.customerId) {
                        members.getByUserId({ userId: blade.currentEntity.customerId }, function (member) {
                            if (member && member.id) {
                                showMemberDetailBlade(member);
                            }
                        });
                    }
                };

                blade.openOrganizationDetails = function () {
                    if (blade.currentEntity.organizationId) {
                        members.get({ id: blade.currentEntity.organizationId }, function (member) {
                            if (member && member.id) {
                                showMemberDetailBlade(member);
                            }
                        });
                    }
                };

                function initShipmentMethod() {
                    if (blade.currentEntity && blade.currentEntity.shipmentMethod && blade.shippingMethods.$resolved) {
                        blade.currentEntity.shipmentMethod = _.findWhere(blade.shippingMethods, {
                            shipmentMethodCode: blade.currentEntity.shipmentMethod.shipmentMethodCode,
                            optionName: blade.currentEntity.shipmentMethod.optionName,
                            currency: blade.currentEntity.currency
                        }) || blade.currentEntity.shipmentMethod;

                        blade.origEntity.shipmentMethod = blade.currentEntity.shipmentMethod;
                    }
                }
            }])

    .controller('virtoCommerce.quoteModule.confirmCancelDialogController', ['$scope', '$modalInstance', function ($scope, $modalInstance) {

        $scope.cancelReason = undefined;
        $scope.yes = function () {
            $modalInstance.close($scope.cancelReason);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);
