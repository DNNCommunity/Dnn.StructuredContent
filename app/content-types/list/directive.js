app.directive('contentTypeList', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/template.html',
        controller: 'contentTypeListController',
        scope: {
            contentField: '='
        }
    };
});
