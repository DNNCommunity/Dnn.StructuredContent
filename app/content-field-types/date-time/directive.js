app.directive('contentFieldTypeDateTime', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/template.html',
        controller: 'contentFieldTypeDateTimeController',
        scope: {
            form: '=',
            contentField: '=',
            contentItem: '=',
            submitted: '='
        }
    };
});
