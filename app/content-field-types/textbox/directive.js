app.directive('contentFieldTypeTextbox', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textbox/template.html',
        controller: 'contentFieldTypeTextboxController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
