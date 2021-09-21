app.controller('contentFieldTypeOptionsTextController', ['$scope', function ($scope) {
    console.log($scope.contentField);
    // set default Options
    if (!$scope.contentField.options.controlType) {
        $scope.contentField.options.controlType = 'textbox';
    }

}]);