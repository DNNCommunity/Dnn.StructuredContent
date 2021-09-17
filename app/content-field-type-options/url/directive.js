app.directive('contentFieldTypeOptionsUrl', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/template.html',
        controller: 'contentFieldTypeOptionsUrlController',
        scope: {
            contentField: '='
        }
    };
});
