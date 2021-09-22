app.controller('contentFieldTypeChoiceController', ['$scope', function ($scope) {

    $scope.selected = {};

    $scope.anySelected = function (object) {
        return object && Object.keys(object).some(function (key) {
            return object[key];
        });
    };

    $scope.checkBoxClick = function (value) {

        var arr = $scope.values;
        if (!Array.isArray(arr)) {
            arr = [];
        }

        if (arr.indexOf(value) === -1) {
            arr.push(value);
        } else {
            arr.splice(arr.indexOf(value), 1);
        }

        $scope.values = arr;
        $scope.contentItem[$scope.contentField.columnName] = $scope.values.join("|");
    };

    $scope.$watch('contentItem', function () {
        if ($scope.contentItem) {
            if ($scope.contentItem[$scope.contentField.columnName]) {
                $scope.values = $scope.contentItem[$scope.contentField.columnName].split("|");
                $scope.values.forEach(function (value) {
                    $scope.selected[value] = true;
                });
            }
        }
    });

}]);

