app.directive('contentFieldTypeOptionsCheckbox', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox/template.html',
        controller: 'contentFieldTypeOptionsCheckboxController',
        scope: {
            contentField: '='
        }
    };
});
