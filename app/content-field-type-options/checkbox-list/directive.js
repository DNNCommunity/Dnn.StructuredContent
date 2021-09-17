app.directive('contentFieldTypeOptionsCheckboxList', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox-list/template.html',
        controller: 'contentFieldTypeOptionsCheckboxListController',
        scope: {
            contentField: '='
        }
    };
});
