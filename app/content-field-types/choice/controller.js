app.controller('contentFieldTypeChoiceController', ['$scope', function ($scope) {

     $scope.selected = {};

    $scope.anySelected = function (object) {
        return object && Object.keys(object).some(function (key) {
            return object[key];
        });
    };

    $scope.checkBoxClick = function (value) {

        var arr = $scope.contentField.values;
        if (!Array.isArray(arr)) {
            arr = [];
        }

        if (arr.indexOf(value) === -1) {
            arr.push(value);
        } else {
            arr.splice(arr.indexOf(value), 1);
        }

        $scope.contentField.values = arr;
        $scope.contentField.value = $scope.contentField.values.join("|");
    };

    $scope.$watch('contentField', function () {        
        if ($scope.contentField.value) {
            $scope.contentField.values = $scope.contentField.value.split("|");
            for (var x = 0; x < $scope.contentField.values.length; x++) {
                var value = $scope.contentField.values[x];
                $scope.selected[value] = true;
            }
        }
    });

}]);

