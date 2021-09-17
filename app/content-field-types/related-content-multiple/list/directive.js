app.directive('contentFieldTypeRelatedContentMultipleList', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/template.html',
        controller: 'contentFieldTypeRelatedContentMultipleListController',
        scope: {
            form: '=',
            relationship: '=',
            submitted: '='
        }
    };
});
