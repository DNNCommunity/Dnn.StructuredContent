app.directive('contentFieldTypeOptionsDateTime', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/template.html',
        controller: 'contentFieldTypeOptionsDateTimeController',
        scope: {
            contentField: '='
        }
    };
});
