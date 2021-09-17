app.controller('contentFieldDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'contentFieldService', 'contentField', 'content_type_url_slug', function ($scope, $uibModalInstance, toastr, contentFieldService, contentField, content_type_url_slug) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentField = contentField;

    $scope.delete = function () {

        contentFieldService.remove(content_type_url_slug, $scope.contentField.id).then(
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

