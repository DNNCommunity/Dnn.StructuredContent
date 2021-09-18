app.directive('contentFieldTypeOptionsChoice', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/choice/template.html',
        controller: 'contentFieldTypeOptionsChoiceController',
        scope: {
            contentField: '='
        }
    };
});
