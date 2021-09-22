app.controller('contentTypeListController', ['$scope', '$q', '$uibModal', 'toastr', 'contentTypeService', function ($scope, $q, $uibModal, toastr, contentTypeService) {

    $scope.loading = true;
    $scope.contentTypes = [];

    getContentTypes = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentTypeService.search('', true).then(
            function (response) {
                $scope.contentTypes = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentTypes failed', response);
                toastr.error("There was a problem loading Content Types", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.addContentType = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/add/template.html?c=' + new Date().getTime(),
            controller: 'contentTypeAddController',
            size: 'lg dnn-structured-content',
            resolve: {}
        });

        modalInstance.result.then(
            function (contentType) {
                getContentTypes();
                toastr.success("The Content Type '" + contentType.name + "' was created.", "Success");

                $scope.editContentType(contentType);
            },
            function () {
                getContentTypes();
            }
        );

    };
    $scope.editContentType = function (contentType) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentTypeEditController',
            size: 'full dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentType: contentType
            }
        });

        modalInstance.result.then(
            function (contentType) {
                getContentTypes();
                toastr.success("The Content Type '" + contentType.name + "' was saved.", "Success");
            },
            function () {
                getContentTypes();
            }
        );

    };
    $scope.copyContentType = function (contentType) {
        alert('not implemented yet');
    };
    $scope.deleteContentType = function (contentType) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentTypeDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentType: function () {
                    return contentType;
                }
            }
        });

        modalInstance.result.then(
            function (contentType) {
                getContentTypes();
                toastr.success("The Content Type '" + contentType.name + "' was deleted.", "Success");
            },
            function () {
                getContentTypes();
            }
        );
    };

    $scope.viewContentItems = function (contentType) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/list/template.html?c=' + new Date().getTime(),
            controller: 'contentItemListController',
            size: 'xl dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentType: contentType
            }
        });

        modalInstance.result.then(
            function () { },
            function () { }
        );

    };
    $scope.viewVisualizers = function (contentType) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/list/template.html?c=' + new Date().getTime(),
            controller: 'visualizerTemplateListController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentType: function () {
                    return contentType;
                }
            }
        });

        modalInstance.result.then(
            function () { },
            function () { }
        );

    };

    init = function () {
        var promises = [];
        promises.push(getContentTypes());
        return $q.all(promises);
    };
    init();
}]);

