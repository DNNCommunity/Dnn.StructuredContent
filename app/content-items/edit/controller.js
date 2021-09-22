app.controller('contentItemEditController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentItemService', 'contentFieldService', 'relationshipService', 'contentType', 'id', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, contentItemService, contentFieldService, relationshipService, contentType, id) {

    $scope.loading = true;
    $scope.submitted = false;

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.contentType = contentType;
    $scope.contentItem = {
        id: id,
        contentTypeId: contentType.id,
        status: "Draft"
    };

    $scope.contentFields = [];
    $scope.relationships = [];

    getContentItem = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentItemService.get(contentType.urlSlug, $scope.contentItem.id).then(
            function (response) {
                console.log(response.data);
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

    getContentFields = function () {
        // get a list of contentFields for this contentType so that we can populate the contentItem' data into the edit control for the contentField
        var deferred = $q.defer();
        $scope.loading = true;
        contentFieldService.search(contentType.urlSlug, true).then(
            function (response) {
                console.log('contentFields', response.data);
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
        // get a list of relationships for this contentType so that we can populate the contentItem' data into the edit control for the contentField
        var deferred = $q.defer();
        $scope.loading = true;
        relationshipService.search($scope.contentType.id).then(
            function (response) {
                console.log('relationships', response.data);
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

    prepItemFromLoad = function () {
        // move values from the contentItem object into the contentField editors 

        $scope.contentFields.forEach(function (contentField) {
            contentField.value = $scope.contentItem[contentField.columnName];
        });

        // for each relationship, determine the primary and related content types
        $scope.relationships.forEach(function (relationship) {

            if (relationship.key === 'o2m') {
                if ($scope.contentType.id === relationship.bContentTypeId) {
                    // the current item is on the many side of a o2m
                    relationship.aValue = $scope.contentItem[relationship.aContentType.singular.toLowerCase() + "_id"];
                }

                if ($scope.contentType.id === relationship.aContentTypeId) {
                    // the current item is on the one side of a o2m
                    relationship.relatedContentItems = $scope.contentItem[relationship.bContentType.plural.toLowerCase()];
                }
            }

            if (relationship.key === 'm2m') {                
                if ($scope.contentType.id === relationship.aContentTypeId) {
                    relationship.relatedContentItems = $scope.contentItem[relationship.bContentType.plural.toLowerCase()];
                }

                if ($scope.contentType.id === relationship.bContentTypeId) {
                    relationship.relatedContentItems = $scope.contentItem[relationship.aContentType.plural.toLowerCase()];
                }
            }
        });
    };

    //prepItemForSave = function () {
    //    // move data from content_field editors into content_item
    //    $scope.contentFields.forEach(function (contentField) {
    //        if (!contentField.isSystem) {
    //            $scope.contentItem[contentField.columnName] = contentField.value;
    //        }
    //    });

    //    $scope.relationships.forEach(function (relationship) {
    //        if (relationship.key === 'o2m') {
    //            $scope.contentItem[relationship.aContentType.singular.toLowerCase() + "_id"] = relationship.aValue;
    //        }
    //    });
    //};

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
        promises.push(getContentFields());
        promises.push(getRelationships());

        if ($scope.contentItem.id) {
            promises.push(getContentItem());
        }

        return $q.all(promises);
    };

    init().then(
        function () {
            //prepItemFromLoad();
            buildCanvas();
        }
    );

}]);

