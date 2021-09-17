app.controller('contentFieldEditController', ['$scope', '$q', '$timeout', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentFieldService', 'contentFieldTypeService', 'content_field', function ($scope, $q, $timeout, $uibModalInstance, toastr, contentTypeService, contentFieldService, contentFieldTypeService, content_field) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.loading = true;
    $scope.submitted = false;
    $scope.validate = false;

    $scope.contentField = content_field;

    $scope.content_field_type = {
        id: content_field.content_field_type_id
    };

    $scope.contentType = {
        id: content_field.content_type_id
    };

    $scope.getContentFieldType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentFieldTypeService.get($scope.content_field_type.id).then(
            function (response) {
                $scope.content_field_type = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldType failed', response);
                toastr.error("Error", "There was a problem loading the content field type");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("Error", "There was a problem loading the Content type");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.saveContentField = function () {
        $scope.loading = true;
        $scope.submitted = true;
        if ($scope.formContentField.$valid) {

            $scope.contentField.options = angular.toJson($scope.contentField.options);

            contentFieldService.save($scope.contentType.url_slug, $scope.contentField).then(
                function (response) {
                    $scope.contentField.id = response.data.id;
                    $scope.submitted = false;
                    $scope.loading = false;
                    $scope.contentField.options = angular.fromJson($scope.contentField.options);
                    $uibModalInstance.close($scope.contentField);
                },
                function (response) {
                    $scope.contentField.options = angular.fromJson($scope.contentField.options);
                    if (response.status === 409) { // duplicate column name
                        toastr.error("Duplicate Name or Column Name", "Error");
                    }
                    else {
                        toastr.error("There was a problem saving the Content Field", "Error");
                    }

                    $scope.loading = false;
                }
            );
        }
        else {
            // set the focus on first invalid field
            // simulate click to on tab
            toastr.error("Please complete this field.", "Error");
            $scope.loading = false;

            var field = $('#formContentField').find('.ng-invalid:first');
            var parent = field.parents('.tab-pane').first();
            var id = parent.attr('id');
            var button = $('[data-target="#' + id + '"]');
            button.click();

            $timeout(function () { // give time for tab to fade in
                field.focus();
            }, 300);
        }
    };

    $scope.nameChanged = function () {
        if (!$scope.contentField.id) {
            if ($scope.contentField.name) {
                $scope.contentField.column_name = safeColumnName($scope.contentField.name);
            }
        }
    };
    $scope.columnNameChanged = function () {
        if ($scope.contentField.column_name) {
            $scope.contentField.column_name = safeColumnName($scope.contentField.column_name);
        }
    };

    init = function () {
        var promises = [];

        if ($scope.contentType.id) {
            promises.push($scope.getContentType());
        }
        if ($scope.content_field_type.id) {
            promises.push($scope.getContentFieldType());
        }
        return $q.all(promises);
    };
    init().then(
        function () {
            if (!$scope.contentField.id) {
                $scope.contentField.name = "New " + $scope.content_field_type.name;
                $scope.nameChanged();
                $scope.contentField.data_type = $scope.content_field_type.default_data_type;
                $scope.contentField.data_length = $scope.content_field_type.default_data_length;
                $scope.contentField.content_field_type = $scope.content_field_type;                
                $scope.contentField.options = $scope.content_field_type.default_options;
                $scope.contentField.allow_null = true;
            }
        }
    );
}]);

