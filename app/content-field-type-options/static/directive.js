app.directive('contentFieldTypeOptionsStatic', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/template.html',
        controller: 'contentFieldTypeOptionsStaticController',
        scope: {
            contentField: '='
        }
    };
});
