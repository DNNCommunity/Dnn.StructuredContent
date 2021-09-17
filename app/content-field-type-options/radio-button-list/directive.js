app.directive('contentFieldTypeOptionsRadioButtonList', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/radio-button-list/template.html',
        controller: 'contentFieldTypeOptionsRadioButtonListController',
        scope: {
            contentField: '='
        }
    };
});
