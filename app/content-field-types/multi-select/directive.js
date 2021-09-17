app.directive('contentFieldTypeMultiSelect', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/multi-select/template.html',
        controller: 'contentFieldTypeMultiSelectController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
