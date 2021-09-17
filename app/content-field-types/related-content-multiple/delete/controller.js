app.controller('contentFieldTypeRelatedContentMultipleDeleteController', ['$scope', '$q', 'toastr', '$uibModalInstance', 'contentItemService', 'item', 'content_type', function ($scope, $q, toastr, $uibModalInstance, contentItemService, item, content_type) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.item = item;
    $scope.content_type = content_type;

    $scope.form;
    $scope.submitted;

}]);