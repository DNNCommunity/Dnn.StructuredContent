app.directive('contentFieldTypeOptionsBoolean', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/boolean/template.html',
        controller: 'contentFieldTypeOptionsBooleanController',
        scope: {
            contentField: '='
        }
    };
});
