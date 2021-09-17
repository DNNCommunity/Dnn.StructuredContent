app.directive('contentFieldTypeRadioButtonList', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/radio-button-list/template.html',
        controller: 'contentFieldTypeRadioButtonListController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
