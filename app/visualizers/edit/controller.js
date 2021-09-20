app.controller('visualizerEditController', ['$scope', '$q', '$uibModalInstance', 'toastr', 'contentTypeService', 'contentItemService', 'visualizerTemplateService', 'visualizerService', 'visualizer', function ($scope, $q, $uibModalInstance, toastr, contentTypeService, contentItemService, visualizerTemplateService, visualizerService, visualizer) {

    $scope.loading = false;
    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submitted = false;

    $scope.contentTypes = [];
    $scope.contentType = null;
    $scope.ContentTypeId = null;
    $scope.visualizerTemplates = [];
    $scope.visualizerTemplate = null;
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
                toastr.error("There was a problem loading Content Types", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };
    $scope.getContentType = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        contentTypeService.get($scope.visualizer.contentTypeId).then(
            function (response) {
                $scope.contentType = response.data;

                $scope.getContentItems();

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getContentType failed', response);
                toastr.error("There was a problem loading Content Type", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getVisualizerTemplates = function () {
        var deferred = $q.defer();
        $scope.loading = true;
        visualizerTemplateService.search('', $scope.visualizer.contentTypeId).then(
            function (response) {
                $scope.visualizerTemplates = response.data;

                if ($scope.visualizer.visualizerTemplateId) {
                    $scope.selectVisualizerTemplate($scope.visualizer.visualizerTemplateId);
                }

                $scope.loading = false;
                deferred.resolve();
            },
            function (response) {
                console.log('getVisualizerTemplates failed', response);
                toastr.error("There was a problem loading the visualzier Temaplates", "Error");
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
                toastr.error("There was a problem loading Content Items", "Error");
                $scope.loading = false;
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.contentTypeChanged = function () {
        $scope.visualizerTemplate = null;
        $scope.visualizer.visualizerTemplateId = null;
        $scope.visualizerTemplates = null;

        if ($scope.visualizer.contentTypeId) {
            $scope.getContentType();
            $scope.getVisualizerTemplates();
        }
    };

    $scope.selectVisualizerTemplate = function (id) {
        $scope.visualizer.VisualizerTemplateId = id;

        $scope.visualizerTemplates.forEach(function (visualizerTemplate) {
            if (visualizerTemplate.id === id) {
                $scope.visualizerTemplate = visualizerTemplate;
                if (visualizerTemplate.contentSize === 'multiple') {
                    $scope.visualizer.itemId = null;
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
                    toastr.error("There was a problem saving the visualizer", "Error");
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
            if ($scope.visualizer.contentTypeId) {
                $scope.getContentType();
                $scope.getVisualizerTemplates();
            }
        }
    );
}]);


