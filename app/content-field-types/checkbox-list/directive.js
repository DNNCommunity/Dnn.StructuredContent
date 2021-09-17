app.directive('contentFieldTypeCheckboxList', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox-list/template.html',
        controller: 'contentFieldTypeCheckboxListController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
