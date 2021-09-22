app.controller('contentTypeEditController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentFieldService', 'relationshipService', 'contentFieldTypeService', 'contentType', function ($scope, $q, $uibModal, $uibModalInstance, toastr, contentTypeService, contentFieldService, relationshipService, contentFieldTypeService, contentType) {

    $scope.loading = false;
    $scope.validate = false;

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;
    $scope.contentFieldTypes = [];

    $scope.contentType = contentType;

    $scope.contentFields = [];
    $scope.relationships = [];

    $scope.selected = null;
    $scope.canvas = {
        rows: []
    };

    getContentFieldTypes = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentFieldTypeService.search("", true).then(
            function (response) {
                $scope.contentFieldTypes = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldTypes failed', response);
                toastr.error("There was a problem loading the content-field-types", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    getContentFieldType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentFieldTypeService.get($scope.contentField.contentFieldTypeId).then(
            function (response) {
                $scope.contentFieldType = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldTypes failed', response);
                toastr.error("There was a problem loading the Content Field Types", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;

                $q.all([getContentFields(), getRelationships()]).then(function () {
                    buildCanvas();
                    $scope.loading = false;
                    deferred.resolve();
                });
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading the Content Type", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.saveContentType = function () {
        $scope.submitted = true;
        $scope.loading = true;

        if ($scope.formContentType.$valid) {

            contentTypeService.save($scope.contentType).then(
                function (response) {
                    $scope.contentType.id = response.data.id;

                    saveItems();

                    $scope.loading = false;
                    $scope.submitted = false;

                    $uibModalInstance.close($scope.contentType);
                },
                function (response) {
                    console.log('save ContentType failed', response);
                    toastr.error("There was a problem saving the Content Type", "Error");
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

    $scope.editItem = function (item) {

        switch (item._type) {
            case "content_field":
                editContentField(item);
                break;

            case "relationship":
                editRelationship(item);
                break;
        }
    };
    $scope.copyItem = function (item) {
        alert("Not implemented yet");
    };
    $scope.deleteItem = function (item, column) {
        switch (item._type) {
            case "content_field":
                deleteContentField(item, column);
                break;

            case "relationship":
                deleteRelationship(item, column);
                break;
        }
    };

    getContentFields = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentFieldService.search($scope.contentType.urlSlug, true).then(
            function (response) {
                $scope.contentFields = response.data;

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFields failed', response);
                toastr.error("There was a problem loading the Content Fields for the this Content Type", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    editContentField = function (contentField) {

        var clone = $.extend({}, contentField);

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldEditController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentField: function () {
                    return clone;
                }
            }
        });

        modalInstance.result.then(
            function (content_field) {
                $scope.canvas.rows[contentField.layoutRow].columns[contentField.layoutColumn].data = contentField;
                toastr.success("The Content Field '" + contentField.name + "' was saved.", "Success");
            },
            function () { }
        );
    };
    deleteContentField = function (contentField, column) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/delete/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                contentField: function () {
                    return contentField;
                },
                contentType: function () {
                    return $scope.contentType;
                }
            }
        });

        modalInstance.result.then(
            function () {
                toastr.success("The Content Field '" + contentField.name + "' was deleted.", "Success");
                column.data = null;
                cleanUpEmptyRowsColumns();
                saveItems();
            },
            function () {
                getContentFields();
            }
        );
    };

    getRelationships = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        relationshipService.search($scope.contentType.id).then(
            function (response) {
                $scope.relationships = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getRelationships failed', response);
                toastr.error("There was a problem loading the relationships for the this Content Type", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    editRelationship = function (relationship) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/template.html?c=' + new Date().getTime(),
            controller: 'relationshipEditController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                relationship: function () {
                    return relationship;
                },
                contentType: function () {
                    return $scope.contentType;
                }
            }
        });

        modalInstance.result.then(
            function (retRelationship) {
                relationship = retRelationship;
                toastr.success("The relationship between " + relationship.aContentType.name + " and " + relationship.bContentType.name + " was saved.", "Success");
            },
            function () { }
        );
    };
    deleteRelationship = function (relationship, column) {
        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/delete/template.html?c=' + new Date().getTime(),
            controller: 'relationshipDeleteController',
            size: 'lg dnn-structured-content',
            backdrop: 'static',
            resolve: {
                relationship: function () {
                    return relationship;
                }
            }
        });

        modalInstance.result.then(
            function () {
                toastr.success("The relationship was deleted.", "Success");
                column.data = null;
                cleanUpEmptyRowsColumns();
                saveItems();
            },
            function () { }
        );
    };

    saveItems = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        var promises = [];

        for (var indexRow = 0; indexRow < $scope.canvas.rows.length; indexRow++) {
            var row = $scope.canvas.rows[indexRow];

            for (var indexColumn = 0; indexColumn < row.columns.length; indexColumn++) {
                var column = row.columns[indexColumn];

                promises.push(saveItem(column.data, indexRow, indexColumn));
            }
        }
        $q.all(promises).then(
            function () {
                $scope.loading = false;
                deferred.resolve();
            },
            function () {
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    saveItem = function (item, row, column) {
        var deferred = $q.defer();
        $scope.loading = true;

        if (item._type === "content_field") {

            item.layoutRow = row;
            item.layoutColumn = column;

            item.options = angular.toJson(item.options);

            contentFieldService.save($scope.contentType.urlSlug, item).then(
                function (response) {
                    item = response.data;
                    deferred.resolve();
                },
                function (response) {
                    if (response.status === 409) {// duplicate column name
                        toastr.error("Duplicate Name or Column Name", "Error");
                    }
                    else {
                        console.log('save ContentField failed', response);
                        toastr.error("There was a problem saving the Content Field", "Error");
                    }
                    deferred.reject();
                }
            );
            item.options = angular.fromJson(item.options);
        }

        if (item._type === "relationship") {

            if ($scope.contentType.id === item.aContentTypeId) {
                item.aLayoutRow = row;
                item.aLayoutColumn = column;
            }

            if ($scope.contentType.id === item.bContentTypeId) {
                item.bLayoutRow = row;
                item.bLayoutColumn = column;
            }

            relationshipService.save(item).then(
                function (response) {
                    item = response.data;
                    deferred.resolve();
                },
                function (response) {
                    if (response.status === 409) {// duplicate column name
                        toastr.error("Duplicate Name or Column Name", "Error");
                    }
                    else {
                        console.log('save relationship failed', response);
                        toastr.error("There was a problem saving the relationship", "Error");
                    }
                    deferred.reject();
                }
            );
        }

        $scope.loading = false;
        return deferred.promise;
    };

    $scope.getUrlSlug = function () {

        var slug;

        slug = $scope.contentType.urlSlug;

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

        if (!$scope.contentType.id) {
            $scope.contentType.tableName = $scope.contentType.urlSlug;
        }
    };

    $scope.rowDrop = function (event, index, type, external, dropEffect, callback, item) {
        console.log('type=' + type, 'item=' + item);
        event.stopPropagation();

        var newRow = { class: "row", columns: [] };
        var newColumn = { class: "col", data: {} };

        if (type === "contentfieldtype") {

            if (item.type === "content_field") {
                var contentField = {
                    contentFieldTypeId: item.id,
                    contentTypeId: $scope.contentType.id,
                    _type: "content_field"
                };

                var modalInstanceContentField = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/template.html?c=' + new Date().getTime(),
                    controller: 'contentFieldEditController',
                    size: 'xl dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        contentField: function () {
                            return contentField;
                        }
                    }
                });

                modalInstanceContentField.result.then(
                    function (content_field) {
                        toastr.success("The Content Field '" + contentField.name + "' was created.", "Success");

                        newColumn.data = contentField;
                        newRow.columns.push(newColumn);

                        $scope.canvas.rows.splice(index, 0, newRow);

                        saveItems();

                        return true;
                    },
                    function () {
                        return false;
                    }
                );
            }

            if (item.type === "relationship") {
                var relationship = {
                    ContentTypeId: $scope.contentType.id,
                    _type: "relationship"
                };

                var modalInstanceRelationship = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/template.html?c=' + new Date().getTime(),
                    controller: 'relationshipEditController',
                    size: 'lg dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        relationship: function () {
                            return relationship;
                        },
                        contentType: function () {
                            return $scope.contentType;
                        }
                    }
                });

                modalInstanceRelationship.result.then(
                    function (relationship) {
                        toastr.success("The relationship was created.", "Success");

                        newColumn.data = relationship;
                        newRow.columns.push(newColumn);

                        $scope.canvas.rows.splice(index, 0, newRow);

                        saveItems();

                        return true;
                    },
                    function () {
                        return false;
                    }
                );
            }
        }

        if (type === "column") {
            newRow.columns.push(item);
            $scope.canvas.rows.splice(index, 0, newRow);
            return true;
        }

    };
    $scope.columnDrop = function (event, index, type, dropEffect, item, row) {
        console.log('type=' + type, 'item=' + item);
        event.stopPropagation();

        var newColumn = { class: "col", data: {} };

        if (type === "contentfieldtype") {

            if (item.type === "content_field") {
                var contentField = {
                    contentFieldTypeId: item.id,
                    contentTypeId: $scope.contentType.id,
                    _type: "content_field"
                };

                var modalInstanceContentField = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/template.html?c=' + new Date().getTime(),
                    controller: 'contentFieldEditController',
                    size: 'lg dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        contentField: function () {
                            return contentField;
                        }
                    }
                });

                modalInstanceContentField.result.then(
                    function (contentField) {
                        toastr.success("The Content Field '" + contentField.Name + "' was created.", "Success");

                        newColumn.data = contentField;

                        row.columns.splice(index, 0, newColumn);

                        saveItems();

                        return true;
                    },
                    function () {
                        return false;
                    }
                );
            }

            if (item.type === "relationship") {
                var relationship = {
                    contentTypeId: $scope.contentType.id,
                    _type: "relationship"
                };

                var modalInstanceRelationship = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/template.html?c=' + new Date().getTime(),
                    controller: 'relationshipEditController',
                    size: 'lg dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        relationship: function () {
                            return relationship;
                        },
                        contentType: function () {
                            return $scope.contentType;
                        }
                    }
                });

                modalInstanceRelationship.result.then(
                    function (relationship) {
                        toastr.success("The relationship was created.", "Success");

                        newColumn.data = relationship;

                        row.columns.splice(index, 0, newColumn);

                        saveItems();

                        return true;
                    },
                    function () {
                        return false;
                    }
                );
            }
        }

        if (type === "column") {
            return item;
        }

    };
    $scope.columnMoved = function (row, $index) {
        row.columns.splice($index, 1);
        cleanUpEmptyRowsColumns();
        saveItems();
    };

    buildCanvas = function () {

        $scope.canvas = {
            rows: []
        };

        var notPlacedFields = []; // this can happen if the user closed the dialog without saving.
        var notPlacedRelationships = []; // this can happen if the user closed the dialog without saving.

        var maxRow = 0;
        var maxColumn = 0;

        // iterate over content fields to find max row and column
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

        // iterate over relationships to find max row and column
        $scope.relationships.forEach(function (item) {
            if (item.aLayoutRow > maxRow) {
                maxRow = item.aLayoutRow;
            }
            if (item.bLayoutRow > maxRow) {
                maxRow = item.bLayoutRow;
            }
            if (item.aLayoutColumn > maxColumn) {
                maxColumn = item.aLayoutColumn;
            }
            if (item.bLayoutColumn > maxColumn) {
                maxColumn = item.bLayoutColumn;
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
                    class: 'col'
                };
                newRow.columns.push(newColumn);
            }
        }

        // insert the ContentFields
        $scope.contentFields.forEach(function (item) {
            if (item.isSystem === false) { // skip the system fields
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

            if ($scope.contentType.id === item.aContentTypeId) {
                if ($scope.canvas.rows[item.aLayoutRow] && $scope.canvas.rows[item.aLayoutRow].columns[item.aLayoutColumn]) {
                    $scope.canvas.rows[item.aLayoutRow].columns[item.aLayoutColumn].data = item;
                }
                else {
                    notPlacedRelationships.push(item);
                }
            }

            if ($scope.contentType.id === item.bContentTypeId) {
                if ($scope.canvas.rows[item.bLayoutRow] && $scope.canvas.rows[item.bLayoutRow].columns[item.bLayoutColumn]) {
                    $scope.canvas.rows[item.bLayoutRow].columns[item.bLayoutColumn].data = item;
                }
                else {
                    notPlacedRelationships.push(item);
                }
            }
        });

        // clean up the empty rows and columns
        cleanUpEmptyRowsColumns();

        // place any non placed items at end (shouldn't happen)
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
        promises.push(getContentFieldTypes());
        if ($scope.contentType.id) {
            promises.push(getContentType());
        }
        return $q.all(promises);
    };
    init();

}]);

