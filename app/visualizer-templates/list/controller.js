app.controller('visualizerTemplateListController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentTypeService', 'visualizerTemplateService', 'contentType', function ($scope, $q, $uibModal, $uibModalInstance, toastr, contentTypeService, visualizerTemplateService, contentType) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.visualizer_templates = [];
    $scope.contentType = contentType;

    getContentType = function () {
        $scope.loading = true;
        var deferred = $q.defer();

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                getVisualizerTemplates();
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

    getVisualizerTemplates = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        visualizerTemplateService.search('', $scope.contentType.id).then(
            function (response) {
                $scope.visualizer_templates = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getVisualizerTemplates failed', response);
                toastr.error("There was a problem loading the visualzier Temaplates", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.addVisualizerTemplate = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/edit/template.html?c=' + new Date().getTime(),
            controller: 'visualizerTemplateEditController',
            size: 'fullscreen dnn-structured-content',
            backdrop: 'static',
            resolve: {
                id: function () {
                    return null;
                },
                ContentTypeId: function () {
                    return $scope.contentType.id;
                },
                content_UrlSlug: function () {
                    return $scope.contentType.UrlSlug;
                }
            }
        });

        modalInstance.result.then(
            function (visualizer_template) {
                getVisualizerTemplates();
                toastr.success("The visualizer template '" + visualizer_template.name + "' was created.", "Success");
            },
            function () {
                getVisualizerTemplates();
                console.log('edit visualizer template cancelled');
            }
        );

    };
    $scope.editVisualizerTemplate = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/edit/template.html?c=' + new Date().getTime(),
            controller: 'visualizerTemplateEditController',
            size: 'fullscreen dnn-structured-content',
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
            function (visualizer_template) {
                getVisualizerTemplates();
                toastr.success("The visualizer template '" + visualizer_template.name + "' was saved.", "Success");
            },
            function () {
                getVisualizerTemplates();
            }
        );

    };
    $scope.deleteVisualizerTemplate = function (visualizerTemplate) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/delete/template.html?c=' + new Date().getTime(),
            controller: 'visualizerTemplateDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                visualizerTemplate: function () {
                    return visualizerTemplate;
                }
            }
        });

        modalInstance.result.then(
            function (visualizerTemplate) {
                getVisualizerTemplates();
                toastr.success("The visualizer template '" + visualizerTemplate.name + "' was deleted.", "Success");
            },
            function () {
                getVisualizerTemplates();
                console.log('delete visualizer template cancelled');
            }
        );
    };

    init = function () {
        var promises = [];
        promises.push(getContentType());
        return $q.all(promises);
    };
    init();

}]);

