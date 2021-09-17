app.directive('contentFieldTypeOptionsSwitch', function () {
    return {
        templateUrl: '/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/switch/template.html',
        controller: 'contentFieldTypeOptionsSwitchController',
        scope: {
            contentField: '='
        }
    };
});
