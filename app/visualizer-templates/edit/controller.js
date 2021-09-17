app.controller('visualizerTemplateEditController', ['$scope', '$q', '$timeout', '$uibModalInstance', 'toastr', 'contentTypeService', 'visualizerTemplateService', 'id', 'content_type_id', function ($scope, $q, $timeout, $uibModalInstance, toastr, contentTypeService, visualizerTemplateService, id, content_type_id) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;

    $scope.ace_editor;

    $scope.contentType = {
        id: content_type_id
    };
    $scope.visualizer_template = {
        id: id,
        content_type_id: content_type_id,
        language: 'razor',
        content_size: 'single'
    };
    $scope.modes = ['Liquid', 'Razor'];
    $scope.mode = $scope.modes[0];

    $scope.content_field_types = [];
    $scope.contentTypes = [];
    $scope.contentFields = [];
    $scope.relationships = [];

    getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentTypeService.get($scope.contentType.id).then(
            function (response) {
                $scope.contentType = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentTypes failed', response);
                toastr.error("Error", "There was a problem loading the content types");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    getVisualizerTemplate = function () {
        var deferred = $q.defer();
        $scope.loading = true;

        visualizerTemplateService.get($scope.visualizer_template.id).then(
            function (response) {
                $scope.visualizer_template = response.data;
            },
            function (response) {
                console.log('get Visualizer Template failed', response);
                toastr.error("Error", "There was a problem loading the Visualizer Template");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.saveVisualizerTemplate = function () {
        var deferred = $q.defer();
        $scope.saving = true;

        if ($scope.formVisualizerTemplate.$valid) {
            visualizerTemplateService.save($scope.visualizer_template).then(
                function (response) {
                    $scope.visualizer_template.id = response.data.id; // in the case of an insert - need the id
                    $scope.saving = false;
                    $scope.loading = false;
                    deferred.resolve();
                    $uibModalInstance.close($scope.visualizer_template);
                },
                function (response) {
                    console.log('save Visualizer Template failed', response);
                    toastr.error("Error", "There was a problem saving the Visualizer Template");
                    $scope.saving = false;
                    $scope.loading = false;
                    deferred.reject();
                }
            );
        }
        else {
            toastr.error("Please complete this field.", "Error");
            $scope.loading = false;

            var field = $('#formVisualizerTemplate').find('.ng-invalid:first');
            var parent = field.parents('.tab-pane').first();
            var id = parent.attr('id');
            var button = $('[data-target="#' + id + '"]');
            button.click();

            $timeout(function () { // give time for tab to fade in
                field.focus();
            }, 300);
        };
        return deferred.promise;
    };

    var aceLoaded = function (_editor) {
        $scope.ace_editor = _editor;
    };

    $scope.templateEditorOptions = {
        mode: $scope.mode.toLowerCase(),
        height: '400px',
        onLoad: function (_ace) {
            $scope.modeChanged = function () {
                _ace.getSession().setMode('ace/mode/' + $scope.mode.toLowerCase());
            };
        }
    };
    $scope.scriptEditorOptions = {
        mode: 'javascript'
    };
    $scope.cssEditorOptions = {
        mode: 'css'
    };

    init = function () {
        var promises = [];
        promises.push(getContentType());
        if ($scope.visualizer_template.id) {
            promises.push(getVisualizerTemplate());
        }
        return $q.all(promises);
    };
    init();
}]);


