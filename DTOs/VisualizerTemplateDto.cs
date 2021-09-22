// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// A DTO representing a VisualizerTemplate.
    /// </summary>
    public class VisualizerTemplateDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets ContentTypeId.
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Script.
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets Css.
        /// </summary>
        public string Css { get; set; }

        /// <summary>
        /// Gets or sets Template.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets ContentSize.
        /// </summary>
        public string ContentSize { get; set; }
    }
}
