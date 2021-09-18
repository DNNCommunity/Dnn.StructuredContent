app.directive('contentFieldTypeBoolean', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/boolean/template.html',
        controller: 'contentFieldTypeBooleanController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
