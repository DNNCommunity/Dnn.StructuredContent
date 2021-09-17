app.directive('contentFieldTypeNumber', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/template.html',
        controller: 'contentFieldTypeNumberController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
