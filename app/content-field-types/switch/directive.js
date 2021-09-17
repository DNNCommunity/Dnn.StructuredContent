app.directive('contentFieldTypeSwitch', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/switch/template.html',
        controller: 'contentFieldTypeSwitchController',
        scope: {
            form: '=',
            contentField: '=',
            submitted: '='
        }
    };
});
