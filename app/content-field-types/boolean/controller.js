app.controller('contentFieldTypeBooleanController', ['$scope', function ($scope) {

    $scope.label_sets =
    {
        "true_false": ["True", "False"],
        "yes_no": ["Yes", "No"],
        "open_closed": ["Open", "Close"],
        "active_inactive": ["Active", "Inactive"]
    }

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
        if ($scope.contentField.options.label_set) {
            if ($scope.contentField.options.label_set !== "other") {
                return $scope.label_sets[$scope.contentField.options.label_set][0];
            }
            else {
                return $scope.contentField.options.label_true;
            }
        }
    };
    $scope.false_label = function () {
        if ($scope.contentField.options.label_set) {
            if ($scope.contentField.options.label_set !== "other") {
                return $scope.label_sets[$scope.contentField.options.label_set][1];
            }
            else {
                return $scope.contentField.options.label_false;
            }
        }
    };

}]);

