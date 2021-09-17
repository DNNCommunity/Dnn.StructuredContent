app.controller('visualizerEditController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentItemService', 'visualizerTemplateService', 'visualizerService', 'visualizer', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, contentItemService, visualizerTemplateService, visualizerService, visualizer) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;

    $scope.contentTypes = [];
    $scope.contentType = null;
    $scope.content_type_id = null;
    $scope.visualizer_templates = [];
    $scope.visualizer_template = null;
    $scope.visualizer = visualizer;

    $scope.getContentTypes = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentTypeService.search().then(
            function (response) {
                $scope.contentTypes = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentTypes failed', response);
                toastr.error("Error", "There was a problem loading Content Types");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentTypeService.get($scope.visualizer.content_type_id).then(
            function (response) {
                $scope.contentType = response.data;

                $scope.getContentItems();

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("Error", "There was a problem loading Content Type");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getVisualizerTemplates = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        visualizerTemplateService.search('', $scope.visualizer.content_type_id).then(
            function (response) {
                $scope.visualizer_templates = response.data;

                if ($scope.visualizer.visualizer_template_id) {
                    $scope.selectVisualizerTemplate($scope.visualizer.visualizer_template_id);
                }

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getVisualizerTemplates failed', response);
                toastr.error("Error", "There was a problem loading the visualzier Temaplates");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getContentItems = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentItemService.search($scope.contentType.name).then(
            function (response) {
                $scope.contentItems = response.data;
                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentItems failed', response);
                toastr.error("Error", "There was a problem loading Content Items");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.content_type_changed = function () {
        $scope.visualizer_template = null;
        $scope.visualizer.visualizer_template_id = null;
        $scope.visualizer_templates = null;

        if ($scope.visualizer.content_type_id) {
            $scope.getContentType();
            $scope.getVisualizerTemplates();
        }
    };

    $scope.selectVisualizerTemplate = function (id) {
        $scope.visualizer.visualizer_template_id = id;

        $scope.visualizer_templates.forEach(function (visualizer_template) {
            if (visualizer_template.id === id) {
                $scope.visualizer_template = visualizer_template;
                if (visualizer_template.content_size === 'multiple') {
                    $scope.visualizer.item_id = null;
                }
            }
        });
    };

    $scope.saveVisualizer = function () {
        $scope.submitted = true;

        if ($scope.formVisualizer.$valid) {

            visualizerService.save($scope.visualizer).then(
                function (response) {
                    $scope.visualizer.id = response.data.id;

                    $scope.submitted = false;

                    $uibModalInstance.close($scope.visualizer);
                },
                function (response) {
                    console.log('save Visualizer failed', response);
                    toastr.error("Error", "There was a problem saving the visualizer");
                    $scope.submitted = false;
                }
            );
        }
        else {
            $scope.loading = false;
            $('#formVisualizer').find('.ng-invalid:visible:first').focus();
        }
    };

    $scope.init = function () {
        var promises = [];
        promises.push($scope.getContentTypes());

        return $q.all(promises);
    };
    $scope.init().then(
        function () {
            if ($scope.visualizer.content_type_id) {
                $scope.getContentType();
                $scope.getVisualizerTemplates();
            }
        }
    );
}]);


