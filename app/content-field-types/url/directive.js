app.directive('contentFieldTypeUrl', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/template.html',
        controller: 'contentFieldTypeUrlController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
