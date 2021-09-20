app.controller('contentItemEditController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentItemService', 'contentFieldService', 'relationshipService', 'id', 'contentType', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, contentItemService, contentFieldService, relationshipService, id, contentType) {

    $scope.loading = true;
    $scope.submitted = false;

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;

    $scope.contentItem = {
        id: id,
        contentTypeId: contentTypeId,
        status: "Draft"
    };
    $scope.contentFields = [];
    $scope.relationships = [];

    getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                $q.all([getContentFields(), getRelationships()]).then(
                    function () {
                        buildCanvas();
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
                $scope.loading = false;
                toastr.error("There was a problem loading the Content Type", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    getContentFields = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentFieldService.search(contentType.urlSlug, true).then(
            function (response) {
                $scope.contentFields = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFields failed', response);
                toastr.error("There was a problem loading the Content Fields", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    getRelationships = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        relationshipService.search($scope.contentType.id, $scope.contentType.id).then(
            function (response) {
                $scope.relationships = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function () {
                console.log('getRelationships failed', response);
                toastr.error("There was a problem loading the relationships", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.saveDraft = function () {
        var deferred = $q.defer();
        $scope.saving = true;

        if ($scope.formContentItem.name.$valid) {

            $scope.contentItem.status = "Draft";
            $scope.contentItem.datePublished = null;

            prepItemForSave();

            contentItemService.save(contentType.urlSlug, $scope.contentItem).then(
                function (response) {
                    $scope.contentItem.id = response.data;
                    deferred.resolve();
                    $uibModalInstance.close($scope.contentItem);
                },
                function (response) {
                    console.log('save ContentItemDraft failed', response);
                    toastr.error("There was a problem saving the content item", "Error");
                    $scope.saving = false;
                    $scope.loading = false;
                    deferred.reject();
                }
            );
        }
        else {
            var field = $('#formContentItem').find('.ng-invalid:first');
            field.focus();
            toastr.error("Please complete this field.", "Error");
            $scope.saving = false;
            $scope.loading = false;
            deferred.reject();
        }

        return deferred.promise;
    };
    $scope.savePublish = function () {
        var deferred = $q.defer();
        $scope.saving = true;
        $scope.loading = true;
        $scope.submitted = true;

        if ($scope.formContentItem.$valid) {

            $scope.contentItem.status = "Published";

            prepItemForSave();

            contentItemService.save(contentType.urlSlug, $scope.contentItem).then(
                function (response) {
                    $scope.contentItem.id = response.data; // in the case of an insert - need the id
                    deferred.resolve();
                    $uibModalInstance.close($scope.contentItem);
                },
                function (response) {
                    console.log('save ContentItemPublish failed', response);
                    toastr.error("There was a problem saving the content item", "Error");
                    $scope.saving = false;
                    $scope.loading = false;
                    deferred.reject();
                }
            );
        }
        else {
            // set the focus on first invalid field            
            var field = $('#formContentItem').find('.ng-invalid:first');
            field.focus();
            toastr.error("Please complete this field.", "Error");
            $scope.saving = false;
            $scope.loading = false;
            deferred.reject();
        }
        return deferred.promise;
    };

    saveRelationship = function (relationship) {
        var deferred = $q.defer();
        $scope.saving = true;
        $scope.loading = true;

        return deferred.promise;
    };

    getContentItem = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentItemService.get(contentType.urlSlug, $scope.contentItem.id).then(
            function (response) {
                $scope.contentItem = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentItem failed', response);
                toastr.error("There was a problem loading the Content Item", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    prepItemFromLoad = function () {

        // move the values from the content_item object to the content_field collection for editing
        $scope.contentFields.forEach(function (contentField) {
            contentField.value = $scope.contentItem[contentField.columnName];
        });

        // for each relationship, determine the primary and related content types
        $scope.relationships.forEach(function (relationship) {

            if (relationship.key === 'o2m') {
                if ($scope.contentType.id === relationship.bContentTypeId) {
                    relationship.aValue = $scope.contentItem[relationship.aColumnName];
                }

                if ($scope.contentType.id === relationship.aContentTypeId) {
                    relationship.primaryContenType = relationship.aContentType;
                    relationship.relatedContentType = relationship.bContentType;
                    relationship.relatedItems = $scope.contentItem[relationship.bContentType.plural.toLowerCase()];
                    relationship.minLimit = relationship.bMinLimit;
                    relationship.maxLimit = relationship.bMaxLimit;
                    relationship.helpText = relationship.bHelpText;
                }
            }

            if (relationship.key === 'm2m') {
                if ($scope.contentType.id === relationship.aContentTypeId) {
                    relationship.primaryContentType = relationship.aContentType;
                    relationship.relatedContentType = relationship.bContentType;
                    relationship.relatedItems = $scope.contentItem[relationship.bContentType.plural.toLowerCase()];
                    relationship.minLimit = relationship.bMinLimit;
                    relationship.maxLimit = relationship.bMaxLimit;
                    relationship.helpText = relationship.bHelpText;
                }

                if ($scope.contentType.id === relationship.bContentTypeId) {
                    relationship.primaryContentType = relationship.bContentType;
                    relationship.relatedContentType = relationship.aContentType;
                    relationship.relatedItems = $scope.contentItem[relationship.aContentType.plural.toLowerCase()];
                    relationship.minLimit = relationship.aMinLimit;
                    relationship.maxLimit = relationship.aMaxLimit;
                    relationship.helpText = relationship.aHelpText;
                }
            }
        });
    };

    // move data from content_field editors into content_item
    prepItemForSave = function () {
        for (var indexField = 0; indexField < $scope.contentFields.length; indexField++) {
            var contentField = $scope.contentFields[indexField];

            if (!contentField.isSystem) {
                $scope.contentItem[contentField.columnName] = contentField.value;
            }
        }
    };

    buildCanvas = function () {

        $scope.canvas = {
            rows: []
        };

        var notPlacedFields = []; // this can happen if the user closed the dialog without saving.
        var notPlacedRelationships = []; // this can happen if the user closed the dialog without saving.

        var maxRow = 0;
        var maxColumn = 0;

        // content fields
        $scope.contentFields.forEach(function (item) {
            if (item.isSystem === false) {
                if (item.layoutRow > maxRow) {
                    maxRow = item.layoutRow;
                }
                if (item.layoutColumn > maxColumn) {
                    maxColumn = item.layoutColumn;
                }
            }
        });

        // relationships
        $scope.relationships.forEach(function (item) {
            if (item.layoutRow > maxRow) {
                maxRow = item.layoutRow;
            }
            if (item.layoutColumn > maxColumn) {
                maxColumn = item.layoutColumn;
            }
        });

        // create the rows and columns
        for (var indexMaxRow = 0; indexMaxRow <= maxRow; indexMaxRow++) {

            var newRow = {
                class: 'row',
                columns: []
            };
            $scope.canvas.rows.push(newRow);

            for (var indexMaxColumn = 0; indexMaxColumn <= maxColumn; indexMaxColumn++) {
                var newColumn = {
                    class: 'col',
                    data: null
                };
                newRow.columns.push(newColumn);
            }
        }


        // insert the ContentFields
        $scope.contentFields.forEach(function (item) {
            if (item.isSystem === false) {
                item._type = 'content_field';
                if ($scope.canvas.rows[item.layoutRow] && $scope.canvas.rows[item.layoutRow].columns[item.layoutColumn]) {
                    $scope.canvas.rows[item.layoutRow].columns[item.layoutColumn].data = item;
                }
                else {
                    notPlacedFields.push(item);
                }
            }
        });

        // insert the relationships 
        $scope.relationships.forEach(function (item) {
            item._type = 'relationship';
            if ($scope.canvas.rows[item.layoutRow] && $scope.canvas.rows[item.layoutRow].columns[item.layoutColumn]) {
                $scope.canvas.rows[item.layoutRow].columns[item.layoutColumn].data = item;
            }
            else {
                notPlacedRelationships.push(item);
            }
        });

        // clean up the emptys
        cleanUpEmptyRowsColumns()

        // place any non placed items
        notPlacedFields.forEach(function (item) {
            var newRow = {
                class: 'row',
                columns: [
                    {
                        class: 'col',
                        data: item
                    }
                ]
            };
            $scope.canvas.rows.push(newRow);
        });
        notPlacedRelationships.forEach(function (item) {
            var newRow = {
                class: 'row',
                columns: [
                    {
                        class: 'col',
                        data: item
                    }
                ]
            };
            $scope.canvas.rows.push(newRow);
        });
    };
    cleanUpEmptyRowsColumns = function () {

        // cleanup empty rows
        for (var indexRow = $scope.canvas.rows.length - 1; indexRow >= 0; indexRow--) {
            var row = $scope.canvas.rows[indexRow];

            // cleanup empty columns
            for (var indexColumn = row.columns.length - 1; indexColumn >= 0; indexColumn--) {
                var column = row.columns[indexColumn];
                if (!column.data) {
                    row.columns.splice(indexColumn, 1);
                }
            }

            if (row.columns.length === 0) {
                $scope.canvas.rows.splice(indexRow, 1);
            }
        }
    };

    init = function () {
        var promises = [];

        if ($scope.contentType.id) {
            promises.push(getContentType());
        }

        if ($scope.contentItem.id) {
            promises.push(getContentItem());
        }

        return $q.all(promises);
    };

    init().then(
        function () {
            prepItemFromLoad();
        }
    );

}]);

