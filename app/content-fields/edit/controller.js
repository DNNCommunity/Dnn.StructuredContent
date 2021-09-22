app.controller('contentFieldEditController', ['$scope', '$q', '$timeout', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentFieldService', 'contentFieldTypeService', 'contentField', function ($scope, $q, $timeout, $uibModalInstance, toastr, contentTypeService, contentFieldService, contentFieldTypeService, contentField) {

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.loading = true;
    $scope.submitted = false;
    $scope.validate = false;

    $scope.contentField = contentField;

    $scope.contentFieldType = {
        id: contentField.contentFieldTypeId
    };

    $scope.contentType = {
        id: contentField.contentTypeId
    };

    $scope.getContentFieldType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentFieldTypeService.get($scope.contentFieldType.id).then(
            function (response) {
                console.log('contentFieldType', response.data);
                $scope.contentFieldType = response.data;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldType failed', response);
                toastr.error("There was a problem loading the content field type", "Error");        
                deferred.reject();
            }
        );
        $scope.loading = false;
        return deferred.promise;
    };

    $scope.getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                console.log('contentType', response.data);
                $scope.contentType = response.data;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content type", "Error");                
                deferred.reject();
            }
        );
        $scope.loading = false;
        return deferred.promise;
    };

    $scope.saveContentField = function () {
        $scope.loading = true;
        $scope.submitted = true;
        if ($scope.formContentField.$valid) {

            $scope.contentField.options = angular.toJson($scope.contentField.options);

            console.log($scope.contentType.urlSlug, $scope.contentField);
            contentFieldService.save($scope.contentType.urlSlug, $scope.contentField).then(
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
                $scope.contentField.columnName = safeColumnName($scope.contentField.name);
            }
        }
    };
    $scope.columnNameChanged = function () {
        if ($scope.contentField.columnName) {
            $scope.contentField.columnName = safeColumnName($scope.contentField.columnName);
        }
    };

    init = function () {
        var promises = [];

        if ($scope.contentType.id) {
            promises.push($scope.getContentType());
        }
        if ($scope.contentFieldType.id) {
            promises.push($scope.getContentFieldType());
        }
        return $q.all(promises);
    };
    init().then(
        function () {
            if (!$scope.contentField.id) {
                $scope.contentField.name = "New " + $scope.contentFieldType.name;
                $scope.nameChanged();
                $scope.contentField.dataType = $scope.contentFieldType.defaultDataType;
                $scope.contentField.dataLength = $scope.contentFieldType.defaultDataLength;
                $scope.contentField.contentFieldType = $scope.contentFieldType;                
                $scope.contentField.options = angular.copy($scope.contentFieldType.defaultOptions);
                $scope.contentField.allowNull = true;
            }
        }
    );
}]);

