app.directive('contentFieldTypeStatic', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/template.html',
        controller: 'contentFieldTypeStaticController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
