app.directive('contentFieldTypeText', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/text/template.html',
        controller: 'contentFieldTypeTextController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
