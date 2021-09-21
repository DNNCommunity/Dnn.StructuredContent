app.controller('contentFieldTypeRelatedContentMultipleDeleteController', ['$scope', '$uibModalInstance', 'item', 'content_type', function ($scope,  $uibModalInstance, item, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.item = item;
    $scope.contentType = contentType;

    $scope.form;
    $scope.submitted;

}]);