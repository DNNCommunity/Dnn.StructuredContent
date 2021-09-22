app.controller('contentFieldTypeRelatedContentMultipleAddController', ['$scope', '$q', 'toastr', '$uibModalInstance', 'contentItemService', 'contentType', function ($scope, $q, toastr, $uibModalInstance, contentItemService, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;
    $scope.items;

    $scope.form;
    $scope.submitted;

    $scope.search = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentItemService.search($scope.contentType.urlSlug, $scope.searchName).then(
            function (response) {
                $scope.items = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getItems failed', response);
                toastr.error("There was a problem searching the content items", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.clear = function () {
        $scope.searchName = '';
        $scope.items = null;
    };

    $scope.select = function (item) {
        $uibModalInstance.close(item);
    };

}]);