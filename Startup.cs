// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.StructuredContent
{

    using DotNetNuke.DependencyInjection;
    using global::StructuredContent.DAL;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Fires up upon DNN Startup.
    /// </summary>
    public class Startup : IDnnStartup
    {
        /// <inheritdoc/>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISQLHelper, SQLHelper>();
        }
    }
}
