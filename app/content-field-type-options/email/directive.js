app.directive('contentFieldTypeOptionsEmail', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/template.html',
        controller: 'contentFieldTypeOptionsEmailController',
        scope: {
            contentField: '='
        }
    };
});
