app.controller('contentFieldTypeOptionsChoiceController', ['$scope', '$uibModal', function ($scope, $uibModal) {

    $scope.addChoice = function () {
        modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/template.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeChoiceController',
            size: 'md dnn-structured-content',
            backdrop: 'static',
            resolve: {
                choice: function () {
                    return null;
                }
            }
        });

        modalInstance.result.then(
            function (retChoice) {
                if ($scope.contentField.options === undefined || $scope.contentField.options.choices === undefined) {
                    $scope.contentField.options = {
                        choices: []
                    };
                }
                var newChoice = {
                    text: retChoice.text,
                    value: retChoice.value
                };
                $scope.contentField.options.choices.push(newChoice);
            },
            function () { }
        );
    };

    $scope.editChoice = function (choice) {
        var clone = $.extend({}, choice);

        var modalInstance = $uibModal.open({
            templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/controller.html?c=' + new Date().getTime(),
            controller: 'contentFieldTypeChoiceController',
            size: 'md dnn-structured-content',
            backdrop: 'static',
            resolve: {
                choice: function () {
                    return clone;
                }
            }
        });

        modalInstance.result.then(
            function (retChoice) {
                choice.text = retChoice.text;
                choice.value = retChoice.value;
            },
            function () { }
        );
    };

    $scope.deleteChoice = function (choice) {
        for (var x = 0; x < $scope.contentField.options.choices.length; x++) {
            var listChoice = $scope.contentField.options.choices[x];
            if (choice === listChoice) {
                $scope.contentField.options.choices.splice(x, 1);
                break;
            }
        }
    };

    // set default Options
    if (!$scope.contentField.options.controlType) {
        $scope.contentField.options.controlType = 'dropdown-list';
    }

    if (!$scope.contentField.options.layout) {
        $scope.contentField.options.layout= 'layout-one-column';
    }

}]);