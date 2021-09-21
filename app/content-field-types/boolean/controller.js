app.controller('contentFieldTypeBooleanController', ['$scope', function ($scope) {

    $scope.trueLabel;
    $scope.falseLabel;

    // convert true/false into string values for the radiobutton control
    if ($scope.contentField.options.controlType === 'radiobuttons') {
        if ($scope.contentField.value === true) {
            $scope.contentField.value = "true";
        }
        if ($scope.contentField.value === false) {
            $scope.contentField.value = "false";
        }
    }

    $scope.trueLabel = function () {
        if ($scope.contentField.options.customLabels === true) {
            return $scope.contentField.options.labelTrue;
        }
        else {
            return "True";
        }
    };
    $scope.falseLabel = function () {
        if ($scope.contentField.options.customLabels === true) {
            return $scope.contentField.options.labelFalse;
        }
        else {
            return "False";
        }
    };

}]);

