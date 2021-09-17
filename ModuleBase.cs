using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;

namespace StructuredContent
{
    public class ModuleBase : PortalModuleBase
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);

            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("https://use.fontawesome.com/releases/v5.7.2/css/all.css"), 1);
            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/bootstrap/css/bootstrap.css"), 2);

            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-toastr/angular-toastr.min.css"), 1);
            
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular.min.js"), 2);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-messages.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-animate.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-sanitize.min.js"), 4);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-cookies.min.js"), 4);

            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-toastr/angular-toastr.tpls.min.js"), 5);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ui.bootstrap/ui-bootstrap-tpls-2.5.0.min.js"), 6);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-drap-and-drop-lists/angular-drap-and-drop-lists.min.js"), 5);

            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/app.js"), 20);

            // content-field-type-options
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox-list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/checkbox-list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/drop-down/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/drop-down/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/multi-select/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/multi-select/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/radio-button-list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/radio-button-list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/rich-text-editor/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/rich-text-editor/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/switch/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/switch/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/textarea/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/textarea/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/textbox/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/textbox/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/controller.js"), 21);


            // content-field-types
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox-list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/checkbox-list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/drop-down/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/drop-down/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/multi-select/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/multi-select/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/radio-button-list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/radio-button-list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/rich-text-editor/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/rich-text-editor/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/switch/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/switch/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textarea/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textarea/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textbox/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/textbox/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/controller.js"), 21);


            // content-types
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/add/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/delete/controller.js"), 21);


            // content-fields
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/delete/controller.js"), 21);

            // content-item
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/delete/controller.js"), 21);

            // relationship
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/delete/controller.js"), 21);

            // revision
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/detail/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/list/controller.js"), 21);

            // visualizer-template
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/delete/controller.js"), 21);

            // visualizers
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/view/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/view/directive.js"), 21);


        }
    }
}