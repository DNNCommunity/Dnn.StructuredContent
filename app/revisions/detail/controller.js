app.controller('revisionDetailController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentFieldService', 'revisionService', 'id', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, contentFieldService, revisionService, id) {

    $scope.loading = true;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = {};
    $scope.revision = {
        id: id
    };
    $scope.contentFields = [];

    $scope.getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                $q.all([$scope.getContentFields()]).then(
                    function () {
                        $scope.loading = false;
                        deferred.resolve();
                    },
                    function () {
                        $scope.loading = false;
                        deferred.resolve();
                    }
                );
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content Type", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.getContentFields = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentFieldService.search(contentType.urlSlug, true).then(
            function (response) {
                $scope.contentFields = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                toastr.error("There was a problem loading the Content Fields", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getRevision = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        revisionService.get($scope.revision.id).then(
            function (response) {
                $scope.revision = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getRevision failed', response);
                toastr.error("There was a problem loading the revision", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };


    $scope.init = function () {
        var promises = [];

        if ($scope.contentType.id) {
            promises.push($scope.getContentType());
        }

        if ($scope.revision.id) {
            promises.push($scope.getRevision());
        }

        return $q.all(promises);
    };

    $scope.init().then(
        function () { }
    );

}]);

