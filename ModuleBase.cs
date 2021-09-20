// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework.JavaScriptLibraries;
    using DotNetNuke.Web.Client.ClientResourceManagement;

    public class ModuleBase : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);

            ClientResourceManager.RegisterStyleSheet(this.Page, this.ResolveUrl("https://use.fontawesome.com/releases/v5.7.2/css/all.css"), 1);
            ClientResourceManager.RegisterStyleSheet(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/bootstrap/css/bootstrap.css"), 2);

            ClientResourceManager.RegisterStyleSheet(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-toastr/angular-toastr.min.css"), 1);

            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular.min.js"), 2);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-messages.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-animate.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-sanitize.min.js"), 4);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular-cookies.min.js"), 4);

            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-toastr/angular-toastr.tpls.min.js"), 5);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ui.bootstrap/ui-bootstrap-tpls-2.5.0.min.js"), 6);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-drap-and-drop-lists/angular-drap-and-drop-lists.min.js"), 5);

            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/app.js"), 20);

            // content-field-type-options
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/boolean/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/boolean/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/choice/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/choice/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/text/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/text/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/controller.js"), 21);

            // content-field-types
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/boolean/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/boolean/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/text/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/text/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/controller.js"), 21);

            // content-types
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/directive.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/add/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/delete/controller.js"), 21);

            // content-fields
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/delete/controller.js"), 21);

            // content-item
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/delete/controller.js"), 21);

            // relationship
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/delete/controller.js"), 21);

            // revision
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/detail/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/list/controller.js"), 21);

            // visualizer-template
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/list/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/delete/controller.js"), 21);

            // visualizers
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/service.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/edit/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/view/controller.js"), 21);
            ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizers/view/directive.js"), 21);
        }
    }
}
