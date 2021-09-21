// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.StructuredContent.Controllers.Serializers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http.Formatting;
    using System.Web.Http.Controllers;

    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Uses camelCase json serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonCamelCaseSerializerAttribute : Attribute, IControllerConfiguration
    {
        /// <inheritdoc/>
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            Contract.Requires(controllerSettings != null);

            var formatter = controllerSettings.Formatters.JsonFormatter;

            controllerSettings.Formatters.Remove(formatter);

            formatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                },
            };

            controllerSettings.Formatters.Insert(0, formatter);
        }
    }
}
