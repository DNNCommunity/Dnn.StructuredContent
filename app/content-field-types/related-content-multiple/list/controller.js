app.controller('contentFieldTypeRelatedContentMultipleListController', ['$scope', 'toastr', '$uibModal', function ($scope, toastr, $uibModal) {

    $scope.relationship;
    $scope.contentType;
    $scope.contentItem;
    $scope.relatedContentItems;

    $scope.form;
    $scope.submitted;

    //$scope.selected = [];
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
                contentType: $scope.relatedContentType()
            }
        });

        modalInstance.result.then(
            function (item) {
                console.log('item', item);
                $scope.relatedContentItems.push(item);
                toastr.success("'" + item.name + "' was added to '" + $scope.relatedContentType().plural + "'.", "Success");
            },
            function () {
            }
        );
    };
    $scope.deleteRelatedContentItem = function (index) {

        var contentItem = $scope.relatedContentItems[index];

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeRelatedContentMultipleDeleteController',
            size: 'lg dnn-structured-content',
            resolve: {
                contentItem: contentItem,
                contentType: $scope.relatedContentType()
            }
        });

        modalInstance.result.then(
            function () {
                $scope.relatedContentItems.splice(index, 1);
                toastr.success("The item '" + contentItem.name + "' was removed from " + $scope.relatedContentType().plural + ".", "Success");
            },
            function () {
            }
        );
    };

    $scope.primaryContentType = function () {
        if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
            return $scope.relationship.bContentType;
        }
        else {
            return $scope.relationship.aContentType;
        }
    };
    $scope.relatedContentType = function () {
        if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
            return $scope.relationship.aContentType;
        }
        else {
            return $scope.relationship.bContentType;
        }
    };

    $scope.minLimit = function () {
        if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
            return $scope.relationship.aMinLimit;
        }
        else {
            return $scope.relationship.bMinLimit;
        }
    };
    $scope.maxLimit = function () {
        if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
            return $scope.relationship.aMaxLimit;
        }
        else {
            return $scope.relationship.bMaxLimit;
        }
    };
    $scope.helpText = function () {
        if ($scope.relationship.key === 'o2m') {
            if ($scope.contentType.id === $scope.relationship.aContentTypeId) {
                return $scope.relationship.bHelpText;
            }

            if ($scope.contentType.id === relationship.aContentTypeId) {
                return $scope.relationship.aHelpText;
            }
        }
        if ($scope.relationship.key === 'm2m') {
            if ($scope.contentType.id === $scope.relationship.aContentTypeId) {
                return $scope.relationship.bHelpText;
            }

            if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
                return $scope.relationship.aHelpText;
            }
        }
    };

    //$scope.relatedContentItems = function () {
    //    if ($scope.relationship.key === 'o2m') {
    //        if ($scope.contentType.id === $scope.relationship.aContentTypeId) {
    //            // the edited item is on the one side of a o2m, so the related items are in on the b (many) side
    //            return $scope.contentItem[$scope.relationship.bContentType.plural.toLowerCase()];
    //        }
    //    }

    //    //if (relationship.key === 'm2m') {
    //    //    if ($scope.contentType.id === relationship.aContentTypeId) {
    //    //        relationship.relatedContentItems = $scope.contentItem[relationship.bContentType.plural.toLowerCase()];
    //    //    }

    //    //    if ($scope.contentType.id === relationship.bContentTypeId) {
    //    //        relationship.relatedContentItems = $scope.contentItem[relationship.aContentType.plural.toLowerCase()];
    //    //    }
    //    //}
    //};

    if ($scope.relationship.key === 'o2m') {
        if ($scope.contentType.id === $scope.relationship.aContentTypeId) {
            $scope.relatedContentItems = $scope.contentItem[$scope.relationship.bContentType.plural.toLowerCase()];
        }
    }
    if ($scope.relationship.key === 'm2m') {
        if ($scope.contentType.id === $scope.relationship.aContentTypeId) {
            $scope.relatedContentItems = $scope.contentItem[$scope.relationship.bContentType.plural.toLowerCase()];
        }

        if ($scope.contentType.id === $scope.relationship.bContentTypeId) {
            $scope.relatedContentItems = $scope.contentItem[$scope.relationship.aContentType.plural.toLowerCase()];
        }
    }

}]);

