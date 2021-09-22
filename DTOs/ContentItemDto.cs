// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A DTO representing a ContentItem.
    /// </summary>
    public class ContentItemDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets ContentTypeId.
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets DateCreated.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets DateModified.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets ContentFields.
        /// </summary>
        public List<ContentFieldDto> ContentFields { get; set; }
    }
}
