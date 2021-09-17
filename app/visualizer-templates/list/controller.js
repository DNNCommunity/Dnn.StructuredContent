app.controller('visualizerTemplateListController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentTypeService', 'visualizerTemplateService', 'content_type_id', function ($scope, $q, $uibModal, $uibModalInstance, toastr, contentTypeService, visualizerTemplateService, content_type_id) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.visualizer_templates = [];
    $scope.contentType = {
        id: content_type_id
    };

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
                toastr.error("Error", "There was a problem loading the Content Type");
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
                toastr.error("Error", "There was a problem loading the visualzier Temaplates");
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
                content_type_id: function () {
                    return $scope.contentType.id;
                },
                content_url_slug: function () {
                    return $scope.contentType.url_slug;
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
                content_type_id: function () {
                    return $scope.contentType.id;
                },
                content_url_slug: function () {
                    return $scope.contentType.url_slug;
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
    $scope.deleteVisualizerTemplate = function (visualizer_template) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/delete/template.html?c=' + new Date().getTime(),
            controller: 'visualizerTemplateDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                visualizer_template: function () {
                    return visualizer_template;
                }
            }
        });

        modalInstance.result.then(
            function (visualizer_template) {
                getVisualizerTemplates();
                toastr.success("The visualizer template '" + visualizer_template.name + "' was deleted.", "Success");
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

