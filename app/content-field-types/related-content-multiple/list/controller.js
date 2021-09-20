app.controller('contentFieldTypeRelatedContentMultipleListController', ['$scope', 'toastr', '$uibModal', function ($scope, toastr, $uibModal) {

    $scope.relationship;
    $scope.relatedItems = $scope.relationship.relatedItems;

    $scope.min;
    $scope.max;
    $scope.helpText;

    $scope.form;
    $scope.submitted;

    $scope.selected = [];
    $scope.controlName = 'relation_' + $scope.relationship.id;

    $scope.checkValidity = function () {
        var count = 0;
        if ($scope.related_items) {
            count = $scope.relatedItems.length;
        }

        $scope.form[$scope.controlName].$setValidity('min', count >= $scope.relationship.minLimit);
        $scope.form[$scope.controlName].$setValidity('max', count <= $scope.relationship.maxLimit);
    }

    $scope.getClass = function () {
        if ($scope.submitted) {
            if ($scope.form[$scope.controlName].$valid)
                return 'is-valid';

            if ($scope.form[$scope.controlName].$invalid)
                return 'is-invalid';
        }
    };

    $scope.addRelatedContentItem = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeRelatedContentMultipleAddController',
            size: 'md dnn-structured-content',
            resolve: {
                contentType: $scope.relationship.relatedContentType
            }
        });

        modalInstance.result.then(
            function (item) {
                $scope.relatedItems.push(item);
                toastr.success("The item '" + item.name + "' was added to the related " + $scope.relationship.relatedContentType.name + ".", "Success");
            },
            function () {
            }
        );
    };
    $scope.deleteRelatedContentItem = function (index) {

        var item = $scope.relatedItems[index];

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeRelatedContentMultipleDeleteController',
            size: 'lg dnn-structured-content',
            resolve: {
                item: item,
                content_type: $scope.relationship.relatedContentType
            }
        });

        modalInstance.result.then(
            function () {
                $scope.relatedItems.splice(index, 1);
                toastr.success("The item '" + item.name + "' was removed from " + $scope.relationship.relatedContentType.Name + ".", "Success");
            },
            function () {
            }
        );
    };
}]);

