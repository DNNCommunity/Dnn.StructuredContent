app.controller('visualizerTemplateDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'visualizerTemplateService', 'visualizer_template', function ($scope, $uibModalInstance, toastr, visualizerTemplateService, visualizer_template) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.visualizer_template = visualizer_template;

    $scope.delete = function () {

        visualizerTemplateService.remove(visualizer_template.id).then(
            function () {
                $uibModalInstance.close($scope.visualizer_template);
            },
            function (response) {
                console.log('deleteVisualizerTemplate failed', response);
                toastr.error("Error", "There was a problem deleteing the visualizer template");
            }
        );
    };

}]);

