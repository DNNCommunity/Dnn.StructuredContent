app.controller('visualizerController', ['$scope', '$q', '$uibModal', '$sce', '$window', 'toastr', 'visualizerService', function ($scope, $q, $uibModal, $sce, $window, toastr, visualizerService) {

    $scope.moduleId = ModuleId;

    $scope.visualizer = {
        moduleId: ModuleId
    };

    $scope.trustedHtml;

    getVisualizer = function () {
        $scope.loading = true;

        visualizerService.get($scope.visualizer.moduleId).then(
            function (response) {
                $scope.visualizer = response.data;

                $scope.trustedHtml = $sce.trustAsHtml($scope.visualizer.content);

                $scope.loading = false;
            },
            function (response) {
                if (response.status === 404) {
                    $scope.loading = false;
                }
                else {
                    console.log('getVisualizer failed', response);
                    toastr.error("There was a problem loading the visualizer", "Error");
                    $scope.loading = false;
                }
            }
        );
    };
    $scope.editVisualizer = function () {
        var clone = $.extend({}, $scope.visualizer);

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/edit/template.html?c=' + new Date().getTime(),
            controller: 'visualizerEditController',
            size: 'xl dnn-structured-content',
            backdrop: 'static',
            resolve: {
                visualizer: function () {
                    return clone;
                }
            }
        });

        modalInstance.result.then(
            function () {
                getVisualizer();
            },
            function () { }
        );

    };
    $scope.manageContent = function () {

        var id = $scope.visualizer.contentTypeId;

        var modalInstance;
        if (id) {
            modalInstance = $uibModal.open({
                templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/list/template.html?c=' + new Date().getTime(),
                controller: 'contentItemListController',
                size: 'xl dnn-structured-content',
                backdrop: 'static',
                resolve: {
                    contentTypeId: function () {
                        return id;
                    }
                }
            });

        }
        else {
            modalInstance = $uibModal.open({
                templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/add/template.html?c=' + new Date().getTime(),
                controller: 'contentTypeAddController',
                size: 'lg dnn-structured-content',
                resolve: {}
            });
        }

        modalInstance.result.then(
            function () {
                $window.location.reload();
            },
            function () {
                $window.location.reload();
            }
        );

    };

    init = function () {
        var promises = [];
        promises.push(getVisualizer());
        return $q.all(promises);
    };
    init();
}]);

