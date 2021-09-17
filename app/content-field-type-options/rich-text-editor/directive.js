app.directive('contentFieldTypeOptionsRichTextEditor', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/rich-text-editor/template.html',
        controller: 'contentFieldTypeOptionsRichTextEditorController',
        scope: {
            contentField: '='
        }
    };
});
