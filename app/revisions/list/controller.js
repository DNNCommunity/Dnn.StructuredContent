app.controller('revisionListController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'revisionService', 'contentType', 'contentItem', function ($scope, $q, $uibModal, $uibModalInstance, toastr, revisionService, contentType, contentItem) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.loading = true;
    $scope.revisions = [];

    $scope.contentType = contentType;
    $scope.contentItem = contentItem;

    $scope.getRevisions = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        revisionService.search($scope.contentType.id, $scope.contentItem.id).then(
            function (response) {
                $scope.revisions = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getRevisions failed', response);
                toastr.error("There was a problem loading Revisions", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.viewRevision = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/view/template.html?c=' + new Date().getTime(),
            controller: 'revisionViewController',
            size: 'xl dnn-structured-content',
            backdrop: 'static',
            resolve: {
                id: function () {
                    return id;
                }
            }
        });

        modalInstance.result.then(
            function () {
            },
            function () { }
        );

    };

    init = function () {
        var promises = [];
        promises.push($scope.getRevisions());
        return $q.all(promises);
    };
    init();
}]);

