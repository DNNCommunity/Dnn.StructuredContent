app.controller('contentFieldTypeRelatedContentSingleController', ['$scope', '$q', 'toastr', 'contentTypeService', 'contentItemService', function ($scope, $q, toastr, contentTypeService, contentItemService) {

    $scope.relationship;
    $scope.primaryContentType;
    $scope.relatedContentType;
    $scope.relatedContentItems;
    $scope.form;
    $scope.submitted;

    $scope.controlName = 'relation_' + $scope.relationship.id;

    getRelatedContentType = function () {
        var deferred = $q.defer();

        // the AContentType is the 'one' side of the o2m relationship
        // need the UrlSlug of the content type
        var relatedContentTypeId;
        related_ContentTypeId = $scope.relationship.aContentTypeId;

        contentTypeService.get(relatedContentTypeId).then(
            function (response) {
                $scope.relatedContentType = response.data;

                contentItemService.search($scope.relatedContentType.urlSlug).then(
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
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content Type", "Error");
                deferred.reject;
            }
        );

        return deferred.promise;
    };

    $scope.isRequired = function () {
        if ($scope.relatedContentType && $scope.relationship) {
            if ($scope.relatedContentType.id === $scope.relationship.bContentTypeId && $scope.relationship.bRequired)
                return true;

            if ($scope.relatedContentType.id === $scope.relationship.aContentTypeId && $scope.relationship.aRequired)
                return true;
        }
        return false;
    };

    $scope.$watch('relationship', function () {
        getRelatedContentType();
    });
}]);


