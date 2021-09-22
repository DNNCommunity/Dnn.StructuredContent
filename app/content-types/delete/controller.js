app.controller('contentTypeDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentType', function ($scope, $uibModalInstance, toastr, contentTypeService, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;

    $scope.delete = function () {

        contentTypeService.remove(contentType.id).then(
            function (response) {                
                $uibModalInstance.close($scope.contentType.name);
            },
            function (response) {
                console.log('deleteContentTypes failed', response);
                toastr.error("There was a problem deleteing the content type", "Error");
            }
        );
    };

}]);

