app.directive('contentFieldTypeCheckbox', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox/template.html',
        controller: 'contentFieldTypeCheckboxController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
