app.directive('contentFieldTypeOptionsText', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/text/template.html',
        controller: 'contentFieldTypeOptionsTextController',
        scope: {
            contentField: '='
        }
    };
});
