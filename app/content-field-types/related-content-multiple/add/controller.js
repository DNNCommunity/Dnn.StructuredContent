app.controller('contentFieldTypeRelatedContentMultipleAddController', ['$scope', '$q', 'toastr', '$uibModalInstance', 'contentItemService', 'content_type', function ($scope, $q, toastr, $uibModalInstance, contentItemService, content_type) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.content_type = content_type;
    $scope.items;

    $scope.form;
    $scope.submitted;

    $scope.search = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentItemService.search($scope.content_type.url_slug, $scope.search_name).then(
            function (response) {
                $scope.items = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getItems failed', response);
                toastr.error("Error", "There was a problem searching the content items");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.clear = function () {
        $scope.search_name = '';
        $scope.items = null;
    };

    $scope.select = function (item) {
        $uibModalInstance.close(item);
    };

}]);