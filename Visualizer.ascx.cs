// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Client.ClientResourceManagement;

    /// <summary>
    /// The Visualizer page module.
    /// </summary>
    public partial class Visualizer : ModuleBase
    {
        /// <summary>
        /// Hooks into the Page_Load event.
        /// </summary>
        /// <param name="sender">The page.</param>
        /// <param name="e">The event.</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                ClientResourceManager.RegisterScript(this.Page, this.ResolveUrl("/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ace/ui-ace.js"), 11);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}
