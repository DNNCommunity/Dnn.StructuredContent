app.directive('contentFieldTypeRelatedContentMultipleAdd', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/template.html',
        controller: 'contentFieldTypeRelatedContentMultipleAddController',
        scope: {            
            relationship: '=',            
            contentType: '='
        }
    };
});
