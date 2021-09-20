app.controller('contentFieldTypeOptionsTextController', ['$scope', function ($scope) {    
    // set default options
    if (!$scope.contentField.options.control_type) {
        $scope.contentField.options.control_type = 'textbox';
    }

}]);