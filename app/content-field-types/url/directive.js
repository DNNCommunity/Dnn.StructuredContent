app.directive('contentFieldTypeUrl', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/template.html',
        controller: 'contentFieldTypeUrlController',
        scope: {
            form: '=',
            contentField: '=',
            contentItem: '=',
            submitted: '='
        }
    };
});
