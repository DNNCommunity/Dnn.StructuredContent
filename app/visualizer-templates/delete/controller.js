app.controller('visualizerTemplateDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'visualizerTemplateService', 'contentType', 'visualizerTemplate', function ($scope, $uibModalInstance, toastr, visualizerTemplateService, contentType, visualizerTemplate) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;
    $scope.visualizerTemplate = visualizerTemplate;

    $scope.delete = function () {

        visualizerTemplateService.remove(visualizerTemplate.id).then(
            function () {
                $uibModalInstance.close($scope.visualizerTemplate);
            },
            function (response) {
                console.log('deleteVisualizerTemplate failed', response);
                toastr.error("There was a problem deleteing the visualizer template", "Error");
            }
        );
    };

}]);

