app.controller('contentFieldTypeOptionsNumberController', ['$scope', function ($scope) {

    // set default options
    if (!$scope.contentField.options.controlType) {
        $scope.contentField.options.controlType = 'textbox';
    }
    if (!$scope.contentField.options.step) {
        $scope.contentField.options.step = 1;
    }
    if (!$scope.contentField.options.minValue) {
        $scope.contentField.options.minValue = 0;
    }
    if (!$scope.contentField.options.maxValue) {
        $scope.contentField.options.maxValue = 100;
    }

}]);