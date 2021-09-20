app.controller('contentFieldDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'contentFieldService', 'contentField', 'contentType', function ($scope, $uibModalInstance, toastr, contentFieldService, contentField, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentField = contentField;

    $scope.delete = function () {

        contentFieldService.remove(contentType.name, $scope.contentField.id).then(
            function (response) {
                $uibModalInstance.close($scope.contentField.name);
            },
            function (response) {
                console.log('deleteContentField failed', response);
                toastr.error("Error", "There was a problem deleteing the content field");
            }
        );
    };
}]);

