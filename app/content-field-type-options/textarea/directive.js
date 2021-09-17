app.directive('contentFieldTypeOptionsTextarea', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app//content-field-type-options/textarea/template.html',
        controller: 'contentFieldTypeOptionsTextareaController',
        scope: {
            contentField: '='
        }
    };
});
