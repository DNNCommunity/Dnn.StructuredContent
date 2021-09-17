app.directive('contentFieldTypeOptionsNumber', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/template.html',
        controller: 'contentFieldTypeOptionsNumberController',
        scope: {
            contentField: '='
        }
    };
});
