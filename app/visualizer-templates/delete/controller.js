app.controller('visualizerTemplateDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'visualizerTemplateService', 'visualizer_template', function ($scope, $uibModalInstance, toastr, visualizerTemplateService, visualizerTemplate) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

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

