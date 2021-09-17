app.directive('contentFieldTypeEmail', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/template.html',
        controller: 'contentFieldTypeEmailController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
