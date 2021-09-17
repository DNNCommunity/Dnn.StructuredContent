app.directive('contentFieldTypeOptionsTextbox', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/textbox/template.html',
        controller: 'contentFieldTypeOptionsTextboxController',
        scope: {
            contentField: '='
        },
        replace: true
    };
});
