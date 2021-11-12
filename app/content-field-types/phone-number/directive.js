app.directive('contentFieldTypePhoneNumber', function () {
    return {
        templateUrl: siteRoot + 'DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/template.html',
        controller: 'contentFieldTypePhoneNumberController',
        scope: {
            form: '=',
            contentField: '=',
            contentItem: '=',
            submitted: '='
        }
    };
});
