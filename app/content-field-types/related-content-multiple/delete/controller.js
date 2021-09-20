app.controller('contentFieldTypeRelatedContentMultipleDeleteController', ['$scope', '$q', 'toastr', '$uibModalInstance', 'contentItemService', 'item', 'contentType', function ($scope, $q, toastr, $uibModalInstance, contentItemService, item, contentType) {

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