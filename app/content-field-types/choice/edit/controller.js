app.controller('contentFieldTypeChoiceEditController', ['$scope', '$uibModalInstance', 'choice', function ($scope, $uibModalInstance, choice) {

    $scope.choice = choice;

    $scope.save = function () {
        $uibModalInstance.close($scope.choice);
    };

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.textKeyUp = function () {
        $scope.choice.value = $scope.choice.text;
    };

}]);

