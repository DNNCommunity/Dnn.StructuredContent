app.controller('contentFieldTypeOptionsBooleanController', ['$scope', function ($scope) {

    // set default options
    if (!$scope.contentField.options.control_type) {
        $scope.contentField.options.control_type = "switch";
    }
    if (!$scope.contentField.options.label_set) {
        $scope.contentField.options.label_set = "true_false";
    }
    
}]);