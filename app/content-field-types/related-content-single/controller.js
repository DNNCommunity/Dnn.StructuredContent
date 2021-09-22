app.controller('contentFieldTypeRelatedContentSingleController', ['$scope', '$q', 'toastr', 'contentItemService', function ($scope, $q, toastr, contentItemService) {

    $scope.relationship;

    $scope.relatedContentItems;

    $scope.controlName = 'relation_' + $scope.relationship.id;

    getRelatedItems = function () {
        var deferred = $q.defer();

        contentItemService.search($scope.relationship.aContentType.urlSlug).then(
            function (response) {
                $scope.relatedContentItems = response.data;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentItems failed', response);
                toastr.error("There was a problem loading the Content Items");
                deferred.reject;
            }
        );

        return deferred.promise;
    };

    getRelatedItems();

}]);


