app.controller('contentFieldTypeOptionsTextController', ['$scope', function ($scope) {
    
    // set default Options
    if (!$scope.contentField.options.controlType) {
        $scope.contentField.options.controlType = 'textbox';
    }

}]);