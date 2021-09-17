app.controller('contentItemDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'contentItemService', 'contentItem', 'content_type_url_slug', function ($scope, $uibModalInstance, toastr, contentItemService, contentItem, content_type_url_slug) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentItem = contentItem;

    $scope.delete = function () {

        contentItemService.remove(content_type_url_slug, contentItem.id).then(
            function () {                
                $uibModalInstance.close($scope.contentItem.name);
            },
            function (response) {
                console.log('deleteContentItem failed', response);
                toastr.error("Error", "There was a problem deleteing the content item");
            }
        );
    };

}]);

