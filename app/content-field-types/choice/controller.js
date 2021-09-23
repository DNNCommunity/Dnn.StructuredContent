app.controller('contentFieldTypeChoiceController', ['$scope', function ($scope) {

    $scope.otherValue;
    $scope.otherSelected;

    $scope.radioOtherValueChanged = function () {
        console.log($scope.otherValue);
        console.log($scope.contentItem[$scope.contentField.columnName]);
        if ($scope.contentItem) {
            $scope.contentItem[$scope.contentField.columnName] = $scope.otherValue;
        }
    };

    $scope.anySelected = function (object) {
        return object && Object.keys(object).some(function (key) {
            return object[key];
        });
    };

    $scope.checkBoxChange = function (value) {
        var arr = [];
        $scope.contentField.options.choices.forEach(function (choice) {            
            if (choice.selected) {
                arr.push(choice.value);
            }
        });        
        if ($scope.otherSelected) {
            arr.push($scope.otherValue);
        }
        $scope.contentItem[$scope.contentField.columnName] = arr.join("|");
    };
    $scope.otherValueChanged = function () {
        $scope.checkBoxChange();
    };


    // for radiobuttonlist, if the contentItem value is not in the list of choices, but allowOther is true, then create the value as a selectable option
    if ($scope.contentItem) {

        var itemValue = $scope.contentItem[$scope.contentField.columnName]; // could be multichoice joined by "|"
        $scope.itemValues = [];
        if (itemValue) {
            $scope.itemValues = itemValue.split("|");
        }

        // determine the "other value"
        if ($scope.contentField.options.allowOther) {
            $scope.itemValues.forEach(function (value) {
                var found = false;
                $scope.contentField.options.choices.forEach(function (choice) {
                    if (choice.value === value) {
                        found = true;
                    }
                });
                if (found === false) {
                    $scope.otherValue = value;
                    $scope.otherSelected = true;
                }
            });
        }

        $scope.contentField.options.choices.forEach(function (choice) {
            choice.selected = $scope.itemValues.indexOf(choice.value) >= 0
        });
    }

}]);

