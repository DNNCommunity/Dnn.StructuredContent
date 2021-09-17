app.controller('contentFieldTypeRelatedContentMultipleListController', ['$scope', '$q', 'toastr', '$uibModal', 'contentTypeService', 'contentItemService', function ($scope, $q, toastr, $uibModal, contentTypeService, contentItemService) {

    $scope.relationship;
    $scope.related_items = $scope.relationship.related_items;

    $scope.min;
    $scope.max;
    $scope.help_text;

    $scope.form;
    $scope.submitted;

    $scope.selected = [];
    $scope.control_name = 'relation_' + $scope.relationship.id;

    $scope.checkValidity = function () {
        var count = 0;
        if ($scope.related_items) {
            count = $scope.related_items.length;
        }

        $scope.form[$scope.control_name].$setValidity('min', count >= $scope.relationship.min_limit);
        $scope.form[$scope.control_name].$setValidity('max', count <= $scope.relationship.max_limit);
    }

    $scope.getClass = function () {
        if ($scope.submitted) {
            if ($scope.form[$scope.control_name].$valid)
                return 'is-valid';

            if ($scope.form[$scope.control_name].$invalid)
                return 'is-invalid';
        }
    };

    $scope.addRelatedContentItem = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeRelatedContentMultipleAddController',
            size: 'md dnn-structured-content',
            resolve: {
                content_type: $scope.relationship.related_content_type
            }
        });

        modalInstance.result.then(
            function (item) {
                $scope.related_items.push(item);
                toastr.success("The item '" + item.name + "' was added to the related " + $scope.relationship.related_content_type.name + ".", "Success");
            },
            function () {
            }
        );
    };
    $scope.deleteRelatedContentItem = function (index) {

        var item = $scope.related_items[index];

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeRelatedContentMultipleDeleteController',
            size: 'lg dnn-structured-content',
            resolve: {
                item: item,
                content_type: $scope.relationship.related_content_type
            }
        });

        modalInstance.result.then(
            function () {
                $scope.related_items.splice(index, 1);
                toastr.success("The item '" + item.name + "' was removed from " + $scope.relationship.related_content_type_name + ".", "Success");
            },
            function () {
            }
        );
    };
}]);

