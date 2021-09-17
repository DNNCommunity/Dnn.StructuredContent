app.controller('contentFieldTypeRelatedContentSingleController', ['$scope', '$q', 'toastr', 'contentTypeService', 'contentItemService', function ($scope, $q, toastr, contentTypeService, contentItemService) {

    $scope.relationship;
    $scope.primaryContentType;
    $scope.relatedContentType;
    $scope.relatedContentItems;
    $scope.form;
    $scope.submitted;

    $scope.control_name = 'relation_' + $scope.relationship.id;

    getRelatedContentType = function () {
        var deferred = $q.defer();

        // the a_content_type is the 'one' side of the o2m relationship
        // need the url_slug of the content type
        var related_content_type_id;
        related_content_type_id = $scope.relationship.a_content_type_id;

        contentTypeService.get(related_content_type_id).then(
            function (response) {
                $scope.relatedContentType = response.data;

                contentItemService.search($scope.relatedContentType.url_slug).then(
                    function (response) {
                        $scope.relatedContentItems = response.data;
                        deferred.resolve();
                    },
                    function (response) {
                        console.log('getContentItems failed', response);
                        toastr.error("Error", "There was a problem loading the Content Items");
                        deferred.reject;
                    }
                );
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("Error", "There was a problem loading the Content Type");
                deferred.reject;
            }
        );

        return deferred.promise;
    };

    $scope.isRequired = function () {
        if ($scope.relatedContentType && $scope.relationship) {
            if ($scope.relatedContentType.id === $scope.relationship.b_content_type_id && $scope.relationship.b_required)
                return true;

            if ($scope.relatedContentType.id === $scope.relationship.a_content_type_id && $scope.relationship.a_required)
                return true;
        }
        return false;
    };

    $scope.$watch('relationship', function () {
        getRelatedContentType();
    });
}]);


