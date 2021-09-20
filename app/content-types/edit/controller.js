app.controller('contentTypeEditController', ['$scope', '$q', '$uibModal', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentFieldService', 'relationshipService', 'contentFieldTypeService', 'id', function ($scope, $q, $uibModal, $uibModalInstance, toastr, contentTypeService, contentFieldService, relationshipService, contentFieldTypeService, id) {

    $scope.loading = false;
    $scope.validate = false;

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;
    $scope.content_field_types = [];

    $scope.contentType = {
        id: id
    };
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
                $scope.content_field_types = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldTypes failed', response);
                toastr.error("Error", "There was a problem loading the content-field-types");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    getContentFieldType = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        contentFieldTypeService.get($scope.contentField.content_field_type_id).then(
            function (response) {
                $scope.content_field_type = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFieldTypes failed', response);
                toastr.error("Error", "There was a problem loading the Content Field Types");
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
                    prepItemFromLoad();
                    buildCanvas();
                    $scope.loading = false;
                    deferred.resolve();
                });
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("Error", "There was a problem loading the Content Type");
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

                    $uibModalInstance.close($scope.contentType.name);
                },
                function (response) {
                    console.log('save ContentType failed', response);
                    toastr.error("Error", "There was a problem saving the Content Type");
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

    prepItemFromLoad = function () {

        // for each relationship, determine the primary and related content types
        $scope.relationships.forEach(function (relationship) {

            if (relationship.key === 'o2m') {
                if ($scope.contentType.id === relationship.b_content_type_id) {
                    relationship.help_text = relationship.a_help_text;
                }

                if ($scope.contentType.id === relationship.a_content_type_id) {
                    relationship.primary_content_type = relationship.a_content_type;
                    relationship.related_content_type = relationship.b_content_type;
                    relationship.min_limit = relationship.b_min_limit;
                    relationship.max_limit = relationship.b_max_limit;
                    relationship.help_text = relationship.b_help_text;
                }
            }

            if (relationship.key === 'm2m') {
                if ($scope.contentType.id === relationship.a_content_type_id) {
                    relationship.primary_content_type = relationship.a_content_type;
                    relationship.related_content_type = relationship.b_content_type;
                    relationship.min_limit = relationship.b_min_limit;
                    relationship.max_limit = relationship.b_max_limit;
                    relationship.help_text = relationship.b_help_text;
                }

                if ($scope.contentType.id === relationship.b_content_type_id) {
                    relationship.primary_content_type = relationship.b_content_type;
                    relationship.related_content_type = relationship.a_content_type;
                    relationship.min_limit = relationship.a_min_limit;
                    relationship.max_limit = relationship.a_max_limit;
                    relationship.help_text = relationship.a_help_text;
                }
            }
        });
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
    $scope.copyItem = function (item) { };
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

        contentFieldService.search($scope.contentType.name, true).then(
            function (response) {
                $scope.contentFields = response.data;

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentFields failed', response);
                toastr.error("Error", "There was a problem loading the Content Fields for the this Content Type");
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
                content_field: function () {
                    return clone;
                }
            }
        });

        modalInstance.result.then(
            function (content_field) {
                $scope.canvas.rows[content_field.layout_row].columns[content_field.layout_column].data = content_field;
                toastr.success("The Content Field '" + content_field.name + "' was saved.", "Success");
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
                toastr.error("Error", "There was a problem loading the relationships for the this Content Type");
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
            function (ret_relationship) {
                relationship = ret_relationship;
                toastr.success("The relationship between " + relationship.aContentType_name + " and " + relationship.bContentType_name + " was saved.", "Success");
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

            item.layout_row = row;
            item.layout_column = column;

            item.options = angular.toJson(item.options);

            contentFieldService.save($scope.contentType.name, item).then(
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

            if ($scope.contentType.id === item.a_content_type_id) {
                item.a_layout_row = row;
                item.a_layout_column = column;
            }

            if ($scope.contentType.id === item.b_content_type_id) {
                item.b_layout_row = row;
                item.b_layout_column = column;
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

        slug = $scope.contentType.url_slug;

        if (!slug) {
            slug = $scope.contentType.name;
        }

        if (slug) {
            slug = slugify(slug);
        }

        return slug;
    };
    $scope.urlSlugify = function () {
        if ($scope.contentType.url_slug) {
            $scope.contentType.url_slug = slugify($scope.contentType.url_slug);
        }
    };
    $scope.nameAutoFormat = function () {
        var name = $scope.contentType.name;
        if (!name) {
            name = "item";
        }

        $scope.contentType.plural = pluralize.plural(name);
        $scope.contentType.singular = pluralize.singular(name);

        $scope.contentType.url_slug = $scope.contentType.plural;
        $scope.urlSlugify();

        if (!$scope.contentType.id) {
            $scope.contentType.table_name = $scope.contentType.url_slug;
        }
    };

    //$scope.showSystemChange = function () {
    //    if ($scope.show_system) {
    //        $scope.content_field_filter.is_system = undefined;
    //    }
    //    else {
    //        $scope.content_field_filter.is_system = false;
    //    }
    //};

    //$scope.relatedContentTypeName = function (relationship) {
    //    if (relationship.a_content_type_id === $scope.contentType.id) {
    //        return relationship.b_content_type_name;
    //    }
    //    if (relationship.b_content_type_id === $scope.contentType.id) {
    //        return relationship.a_content_type_name;
    //    }
    //};

    //$scope.relationship_filter = function (relationship) {
    //    if (!$scope.relationship_search) {
    //        return true;
    //    }

    //    var relatedContentTypeName = $scope.relatedContentTypeName(relationship);

    //    if (relatedContentTypeName.toLowerCase().includes($scope.relationship_search)) {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //};

    $scope.rowDrop = function (event, index, type, external, dropEffect, callback, item) {
        event.stopPropagation();

        var newRow = { class: "row", columns: [] };
        var newColumn = { class: "col", data: {} };

        if (type === "content_field_type") {

            if (item.type === "content_field") {
                var content_field = {
                    content_field_type_id: item.id,
                    content_type_id: $scope.contentType.id,
                    _type: "content_field"
                };

                var modalInstanceContentField = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/template.html?c=' + new Date().getTime(),
                    controller: 'contentFieldEditController',
                    size: 'xl dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        content_field: function () {
                            return content_field;
                        }
                    }
                });

                modalInstanceContentField.result.then(
                    function (content_field) {
                        toastr.success("The Content Field '" + content_field.name + "' was created.", "Success");

                        newColumn.data = content_field;
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
                    content_type_id: $scope.contentType.id,
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
        event.stopPropagation();

        var newColumn = { class: "col", content_field: {} };

        if (type === "content_field_type") {

            if (item.type === "content_field") {
                var content_field = {
                    content_field_type_id: item.id,
                    content_type_id: $scope.contentType.id,
                    _type: "content_field"
                };

                var modalInstanceContentField = $uibModal.open({
                    templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/template.html?c=' + new Date().getTime(),
                    controller: 'contentFieldEditController',
                    size: 'lg dnn-structured-content',
                    backdrop: 'static',
                    resolve: {
                        content_field: function () {
                            return content_field;
                        }
                    }
                });

                modalInstanceContentField.result.then(
                    function (content_field) {
                        toastr.success("The Content Field '" + content_field.name + "' was created.", "Success");

                        newColumn.data = content_field;

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
                    content_type_id: $scope.contentType.id,
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
            if (item.is_system === false) {
                if (item.layout_row > maxRow) {
                    maxRow = item.layout_row;
                }
                if (item.layout_column > maxColumn) {
                    maxColumn = item.layout_column;
                }
            }
        });

        // iterate over relationships to find max row and column
        $scope.relationships.forEach(function (item) {
            if (item.a_layout_row > maxRow) {
                maxRow = item.a_layout_row;
            }
            if (item.b_layout_row > maxRow) {
                maxRow = item.b_layout_row;
            }
            if (item.a_layout_column > maxColumn) {
                maxColumn = item.a_layout_column;
            }
            if (item.b_layout_column > maxColumn) {
                maxColumn = item.b_layout_column;
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

        // insert the content_fields
        $scope.contentFields.forEach(function (item) {
            if (item.is_system === false) { // skip the system fields
                item._type = 'content_field';
                if ($scope.canvas.rows[item.layout_row] && $scope.canvas.rows[item.layout_row].columns[item.layout_column]) {
                    $scope.canvas.rows[item.layout_row].columns[item.layout_column].data = item;
                }
                else {
                    notPlacedFields.push(item);
                }
            }
        });

        // insert the relationships 
        $scope.relationships.forEach(function (item) {
            item._type = 'relationship';

            if ($scope.contentType.id === item.a_content_type_id) {
                if ($scope.canvas.rows[item.a_layout_row] && $scope.canvas.rows[item.a_layout_row].columns[item.a_layout_column]) {
                    $scope.canvas.rows[item.a_layout_row].columns[item.a_layout_column].data = item;
                }
                else {
                    notPlacedRelationships.push(item);
                }
            }

            if ($scope.contentType.id === item.b_content_type_id) {
                if ($scope.canvas.rows[item.b_layout_row] && $scope.canvas.rows[item.b_layout_row].columns[item.b_layout_column]) {
                    $scope.canvas.rows[item.b_layout_row].columns[item.b_layout_column].data = item;
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

