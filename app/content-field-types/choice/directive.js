app.directive('contentFieldTypeChoice', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/template.html',
        controller: 'contentFieldTypeChoiceController',
        scope: {
            form: '=',
            contentField: '=',
            contentItem: '=',
            submitted: '='
        }
    };
});
