app.controller('contentFieldTypeRelatedContentMultipleDeleteController', ['$scope', '$uibModalInstance', 'contentItem', 'contentType', function ($scope, $uibModalInstance, contentItem, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.contentItem = contentItem;
    $scope.contentType = contentType;

    $scope.form;
    $scope.submitted;

}]);