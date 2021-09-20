app.controller('contentItemListController', ['$parse', '$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentItemService', 'contentTypeService', 'content_type_id', function ($parse, $scope, $q, $uibModal, $uibModalInstance, toastr, contentItemService, contentTypeService, content_type_id) {

    $scope.loading = true;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.content_items = [];
    $scope.contentType = {
        id: content_type_id
    };

    getContentType = function () {
        $scope.loading = true;
        var deferred = $q.defer();

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType= response.data;

                getContentItems();
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("Error", "There was a problem loading the Content Type");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.editContentType = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentTypeEditController',
            size: 'full dnn-structured-content',
            backdrop: 'static',
            resolve: {
                id: function () {
                    return $scope.contentType.id;
                }
            }
        });

        modalInstance.result.then(
            function () {
                getContentType();
            },
            function () {
            }
        );
    };

    getContentItems = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentItemService.search($scope.contentType.name).then(
            function (response) {
                $scope.content_items = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentItems failed', response);
                toastr.error("Error", "There was a problem loading Content Items");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.addContentItem = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentItemEditController',
            size: 'xl dnn-structured-content',
            backdrop: 'static',
            resolve: {
                id: function () {
                    return null;
                },
                contentType: function () {
                    return $scope.contentType;
                }
            }
        });

        modalInstance.result.then(
            function (content_item) {
                toastr.success("'" + content_item.name + "' was saved.", "Success");
                getContentItems();
            },
            function () {
            }
        );

    };
    $scope.editContentItem = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentItemEditController',
            size: 'xl dnn-structured-content',
            backdrop: 'static',
            resolve: {
                id: function () {
                    return id;
                },
                contentType: function () {
                    return $scope.contentType;
                }
            }
        });

        modalInstance.result.then(
            function (content_item) {
                toastr.success("'" + content_item.name + "' was saved.", "Success");
                getContentItems();
            },
            function () {
            }
        );

    };
    $scope.deleteContentItem = function (contentItem) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentItemDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentItem: function () {
                    return contentItem;
                },
                contentType: function () {
                    return $scope.contentType;
                }
            }
        });

        modalInstance.result.then(
            function (content_item_name) {
                getContentItems();
                toastr.success("'" + content_item_name + "' was deleted.", "Success");
            },
            function () {
                getContentItems();
            }
        );
    };

    $scope.import = function () {
        alert("Coming soon!");
    };
    $scope.export = function () {
        alert("Coming soon!");
    };

    $scope.viewRevisions = function (item) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/list/template.html?c=' + new Date().getTime(),
            controller: 'revisionListController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                content_type: function () {
                    return $scope.contentType;
                },
                content_item: function () {
                    return item;
                }
            }
        });

        modalInstance.result.then(
            function () { },
            function () {
            }
        );
    };

    $scope.translationContentItem = function () {
        alert("Coming soon!");
    };

    init = function () {
        var promises = [];
        promises.push(getContentType());
        return $q.all(promises);
    };
    init();
}]);

