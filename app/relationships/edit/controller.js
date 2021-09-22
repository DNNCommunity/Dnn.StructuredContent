app.controller('relationshipEditController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'relationshipService', 'relationship', 'contentType', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, relationshipService, relationship, contentType) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.submitted = false;

    $scope.contentTypes = [];
    $scope.contentType = contentType;
    $scope.relationship = relationship;

    $scope.aContentType = {};
    $scope.bContentType = {};

    $scope.getContentTypes = function () {
        var deferred = $q.defer();

        contentTypeService.search('', true).then(
            function (response) {
                $scope.contentTypes = response.data;

                $scope.contentTypeChange();
                deferred.resolve();
            },
            function (response) {
                console.log('getContentTypes failed', response);
                toastr.error("There was a problem loading the Content Types", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getContentType = function () {
        var deferred = $q.defer();

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content type", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.saveRelationship = function () {
        $scope.submitted = true;
        if ($scope.formRelationship.$valid) {

            relationshipService.save($scope.relationship).then(
                function (response) {                    
                    $scope.relationship.id = response.data.id;
                    $scope.submitted = false;
                    $scope.relationship.aContentType_name = $scope.aContentType.name;
                    $scope.relationship.bContentType_name = $scope.bContentType.name;
                    $uibModalInstance.close($scope.relationship);
                },
                function (response) {

                    if (response.status === 409) {// duplciate column name
                        toastr.error("Duplicate Name or Column Name", "Error");
                        $scope.submitted = false;
                    }
                    else {
                        console.log('save relationship failed', response);
                        toastr.error("There was a problem saving the relationship", "Error");
                        $scope.submitted = false;
                    }
                }
            );
        }
    };

    $scope.contentTypeChange = function () {        
        for (var x = 0; x < $scope.contentTypes.length; x++) {
            var contentType = $scope.contentTypes[x];
            if (contentType.id === $scope.relationship.aContentTypeId) {
                $scope.aContentType = contentType;
            }
            if (contentType.id === $scope.relationship.bContentTypeId) {
                $scope.bContentType = contentType;
            }
        }
    };
    
    $scope.init = function () {
        var promises = [];
        promises.push($scope.getContentTypes());
        if ($scope.contentType.id) {
            promises.push($scope.getContentType());
        }
        return $q.all(promises);
    };
    $scope.init().then(
        function () {
            // set defaults for new relationship
            if (!$scope.relationship.id) {
                $scope.relationship.key = 'o2m';
                $scope.relationship.aContentTypeId = $scope.contentType.id;
                $scope.relationship.bContentTypeId = $scope.contentType.id;
                $scope.relationship.aMinLimit = null;
                $scope.relationship.aMaxLimit = null;
                $scope.relationship.bMinLimit = null;
                $scope.relationship.bMaxLimit = null;

                $scope.contentTypeChange();
            }
        },
        function () { }
    );
}]);

