app.controller('revisionListController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'revisionService', 'content_type', 'content_item', function ($scope, $q, $uibModal, $uibModalInstance, toastr, revisionService, content_type, content_item) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.loading = true;
    $scope.revisions = [];

    $scope.content_type = content_type;
    $scope.content_item = content_item;

    $scope.getRevisions = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        revisionService.search($scope.content_type.id, $scope.content_item.id).then(
            function (response) {
                $scope.revisions = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getRevisions failed', response);
                toastr.error("Error", "There was a problem loading Revisions");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.viewRevision = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/view/template.html?c=' + new Date().getTime(),
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

