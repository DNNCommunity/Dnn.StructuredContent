app.controller('contentFieldTypeNumberController', ['$scope', function ($scope) {

    //this builds the list of choices for the dropdown control
    $scope.contentField.choices = [];

    $scope.options = function () {
        var arr = []
        for (let i = $scope.contentField.options.minValue; i <= $scope.contentField.options.maxValue; i = i + $scope.contentField.options.step) {
            arr.push(i);
        }
        return arr;
    };
}]);

