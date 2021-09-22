// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// A DTO representing a Visualizer.
    /// </summary>
    public class VisualizerDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets ModuleId.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets ContentTypeId.
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets VisualizerTemplateId.
        /// </summary>
        public int? VisualizerTemplateId { get; set; }

        /// <summary>
        /// Gets or sets ItemId.
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets ItemFilter.
        /// </summary>
        public string ItemFilter { get; set; }

        /// <summary>
        /// Gets or sets Contnet.
        /// </summary>
        public string Content { get; set; }
    }
}
