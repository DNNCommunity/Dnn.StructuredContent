app.directive('contentFieldTypeDropDown', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/drop-down/template.html',
        controller: 'contentFieldTypeDropDownController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
