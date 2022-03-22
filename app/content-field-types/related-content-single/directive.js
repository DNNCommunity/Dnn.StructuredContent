app.directive('contentFieldTypeRelatedContentSingle', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/template.html',
        controller: 'contentFieldTypeRelatedContentSingleController',
        scope: {
            form: '=',
            relationship: '=',
            contentType: "=",
            contentItem:"=",
            submitted: '='
        }
    };
});
