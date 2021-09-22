// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System.Web.Http;

    using DotNetNuke.Web.Api;

    /// <summary>
    /// Registers the routes used by this module.
    /// </summary>
    public class RouteMapper : IServiceRouteMapper
    {
        /// <inheritdoc/>
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            // Content Type
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "ContentType",
                routeName: "ContentType",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "ContentType",
                },
                namespaces: new[] { "StructuredContent" });

            // Content Field
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "ContentField",
                routeName: "ContentField",
                url: "{contentTypeUrlSlug}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    contentType = RouteParameter.Optional,
                    controller = "ContentField",
                },
                namespaces: new[] { "StructuredContent" });

            // Content Field Types
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "ContentFieldType",
                routeName: "ContentFieldType",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "ContentFieldType",
                },
                namespaces: new[] { "StructuredContent" });

            // Revisions
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "Revision",
                routeName: "Revision",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "Revision",
                },
                namespaces: new[] { "StructuredContent" });

            // Relationships
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "Relationship",
                routeName: "Relationship",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "Relationship",
                },
                namespaces: new[] { "StructuredContent" });

            // Content Item
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "content",
                routeName: "content",
                url: "{contentTypeUrlSlug}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "ContentItem",
                },
                namespaces: new[] { "StructuredContent" });

            // Visualizer Template
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "VisualizerTemplate",
                routeName: "VisualizerTemplate",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "VisualizerTemplate",
                },
                namespaces: new[] { "StructuredContent" });

            // Visualizer
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "Visualizer",
                routeName: "Visualize2",
                url: "{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    controller = "Visualizer",
                },
                namespaces: new[] { "StructuredContent" });

            // Default
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "StructuredContent",
                routeName: "default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                },
                namespaces: new[] { "StructuredContent" });
        }
    }
}
