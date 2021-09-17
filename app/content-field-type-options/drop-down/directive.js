app.directive('contentFieldTypeOptionsDropDown', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/drop-down/template.html',
        controller: 'contentFieldTypeOptionsDropDownController',
        scope: {
            contentField: '='
        }
    };
});
