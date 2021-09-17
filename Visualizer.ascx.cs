using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;

namespace StructuredContent
{
    public partial class Visualizer : ModuleBase
    {
        protected new void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ace/ui-ace.js"), 11);

               
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

    }
}

