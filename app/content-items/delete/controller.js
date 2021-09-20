app.controller('contentItemDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'contentItemService', 'contentItem', 'contentType', function ($scope, $uibModalInstance, toastr, contentItemService, contentItem, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentItem = contentItem;
    $scope.contentType = contentType;

    $scope.delete = function () {

        contentItemService.remove(contentType.name, contentItem.id).then(
            function () {
                $uibModalInstance.close($scope.contentItem.name);
            },
            function (response) {
                console.log('deleteContentItem failed', response);
                toastr.error("There was a problem deleteing the content item", "Error");
            }
        );
    };

}]);

