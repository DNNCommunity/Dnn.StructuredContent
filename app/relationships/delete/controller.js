app.controller('relationshipDeleteController', ['$scope', '$uibModalInstance', 'toastr', 'relationshipService', 'relationship', function ($scope, $uibModalInstance, toastr, relationshipService, relationship) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.relationship = relationship;

    $scope.delete = function () {

        relationshipService.remove($scope.relationship.id).then(
            function (response) {
                $uibModalInstance.close();
            },
            function (response) {
                console.log('delete relationship failed', response);
                toastr.error("Error", "There was a problem deleteing the relationship");
            }
        );
    };
}]);

