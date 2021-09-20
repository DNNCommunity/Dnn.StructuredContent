app.controller('contentTypeAddController', ['$scope', '$uibModalInstance', 'toastr', 'contentTypeService', function ($scope, $uibModalInstance, toastr, contentTypeService) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;

    $scope.contentType = {};

    $scope.saveContentType = function () {
        $scope.submitted = true;
        $scope.loading = true;

        if ($scope.formContentType.$valid) {

            contentTypeService.save($scope.contentType).then(
                function (response) {
                    $scope.contentType = response.data;

                    $scope.loading = false;
                    $scope.submitted = false;

                    $uibModalInstance.close($scope.contentType);
                },
                function (response) {
                    console.log('saveContentType failed', response);
                    toastr.error(response.data.Message, "Error");
                    $scope.submitted = false;
                    $scope.loading = false;
                }
            );
        }
        else {
            $scope.loading = false;
            $('#formContentType').find('.ng-invalid:visible:first').focus();
        }
    };

    $scope.getUrlSlug = function () {
        var slug = $scope.contentType.urlSlug;

        if (!slug) {
            slug = $scope.contentType.name;
        }

        if (slug) {
            slug = slugify(slug);
        }

        return slug;
    };
    $scope.urlSlugify = function () {
        if ($scope.contentType.urlSlug) {
            $scope.contentType.urlSlug = slugify($scope.contentType.urlSlug);
        }
    };
    $scope.nameAutoFormat = function () {
        var name = $scope.contentType.name;
        if (!name) {
            name = "item";
        }

        $scope.contentType.plural = pluralize.plural(name);
        $scope.contentType.singular = pluralize.singular(name);

        $scope.contentType.urlSlug = $scope.contentType.plural;
        $scope.urlSlugify();

        $scope.contentType.tableName = $scope.contentType.urlSlug;
    };

}]);

