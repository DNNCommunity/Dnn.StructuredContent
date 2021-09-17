app.controller('contentFieldTypeMultiSelectController', ['$scope', function ($scope) {

    $scope.multiselectChange = function () {
        console.log($scope.contentField);
        if ($scope.contentField.values) {
            $scope.contentField.value = $scope.contentField.values.join("|");
        }
    };

    if ($scope.contentField.value) {
        $scope.contentField.values = $scope.contentField.value.split("|");
    }    
}]);

