app.directive('contentFieldTypeOptionsMultiSelect', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/multi-select/template.html',
        controller: 'contentFieldTypeOptionsMultiSelectController',
        scope: {
            contentField: '='
        }
    };
});
