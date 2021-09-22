app.controller('contentItemListController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentItemService', 'contentType', function ($scope, $q, $uibModal, $uibModalInstance, toastr, contentItemService, contentType) {

    $scope.loading = true;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;
    $scope.contentItems = [];

    $scope.editContentType = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentTypeEditController',
            size: 'full dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentType: $scope.contentType
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
        contentItemService.search($scope.contentType.urlSlug).then(
            function (response) {
                $scope.contentItems = response.data;
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
                contentType: $scope.contentType,
                id: null                
            }
        });

        modalInstance.result.then(
            function (contentItem) {
                toastr.success("'" + contentItem.name + "' was saved.", "Success");
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
                id: id,
                contentType: $scope.contentType
            }
        });

        modalInstance.result.then(
            function (contentItem) {
                toastr.success("'" + contentItem.name + "' was saved.", "Success");
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
                contentItem: contentItem,
                contentType: $scope.contentType.urlSlug
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
                contentType: $scope.contentType,
                contentItem: item
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
        promises.push(getContentItems());
        return $q.all(promises);
    };
    init();
}]);

