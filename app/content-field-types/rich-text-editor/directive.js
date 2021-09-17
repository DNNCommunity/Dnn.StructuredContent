app.directive('contentFieldTypeRichTextEditor', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/rich-text-editor/template.html',
        controller: 'contentFieldTypeRichTextEditorController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
