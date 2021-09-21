app.controller('contentFieldTypeOptionsBooleanController', ['$scope', function ($scope) {

    // set default options
    if (!$scope.contentField.options.controlType) {
        $scope.contentField.options.controlType = "switch";
    }
    if (!$scope.contentField.options.customLabels) {
        $scope.contentField.options.customLabels = false;
    }

}]);