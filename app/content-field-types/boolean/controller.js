app.controller('contentFieldTypeBooleanController', ['$scope', function ($scope) {

    $scope.true_label;
    $scope.false_label;

    // convert true/false into string values for the radiobutton control
    if ($scope.contentField.options.control_type === 'radiobuttons') {
        if ($scope.contentField.value === true) {
            $scope.contentField.value = "true";
        }
        if ($scope.contentField.value === false) {
            $scope.contentField.value = "false";
        }
    }

    $scope.true_label = function () {
        if ($scope.contentField.options.custom_labels === true) {
            return $scope.contentField.options.label_true;
        }
        else {
            return "True";
        }
    };
    $scope.false_label = function () {
        if ($scope.contentField.options.custom_labels === true) {
            return $scope.contentField.options.label_false;
        }
        else {
            return "False";
        }
    };

}]);

