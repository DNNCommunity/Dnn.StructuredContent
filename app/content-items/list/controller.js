app.controller('contentItemListController', ['$parse', '$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentItemService', 'contentTypeService', 'ContentTypeId', function ($parse, $scope, $q, $uibModal, $uibModalInstance, toastr, contentItemService, contentTypeService, ContentTypeId) {

    $scope.loading = true;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.content_items = [];
    $scope.content_type = {
        id: ContentTypeId
    };

    getContentType = function () {
        $scope.loading = true;
        var deferred = $q.defer();

        contentTypeService.get($scope.content_type.id).then(
            function (response) {
                $scope.content_type = response.data;

                getContentItems();
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content Type", "Error");
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
        contentItemService.search($scope.content_type.UrlSlug).then(
            function (response) {
                $scope.content_items = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentItems failed', response);
                toastr.error("There was a problem loading Content Items", "Error");
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
                ContentTypeId: function () {
                    return $scope.content_type.id;
                },
                content_UrlSlug: function () {
                    return $scope.content_type.UrlSlug;
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
                content_type_UrlSlug: function () {
                    return $scope.content_type.UrlSlug;
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
                    return $scope.content_type;
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

