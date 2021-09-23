app.controller('contentFieldTypeTextController', ['$scope', function ($scope) {

    $scope.getMaskAttributes = function () {
        if ($scope.contentField.options.inputMask) {
            return "mask='" + $scope.contentField.options.inputMask + "' mask-restrict='reject' mask-validate='true'";
        }
        else {
            return "nothing";
        }
    };

    $scope.getMaskRestrict = function () {
        if ($scope.contentField.options.inputMask) {
            return "restrict";
        }
        else {
            return "accept";
        }
    };

    $scope.getMaskValidate= function () {
        if ($scope.contentField.options.inputMask) {
            return "true";
        }
        else {
            return "false";
        }
    };

}]);

