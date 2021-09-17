app.directive('contentFieldTypeTextarea', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textarea/template.html',
        controller: 'contentFieldTypeTextareaController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
