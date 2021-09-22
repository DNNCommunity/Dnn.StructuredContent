// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// A DTO representing a ContentType.
    /// </summary>
    public class ContentTypeDto
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
        /// Gets or sets Singular.
        /// </summary>
        public string Singular { get; set; }

        /// <summary>
        /// Gets or sets Plural.
        /// </summary>
        public string Plural { get; set; }

        /// <summary>
        /// Gets or sets UrlSlug.
        /// </summary>
        public string UrlSlug { get; set; }

        /// <summary>
        /// Gets or sets TableName.
        /// </summary>
        public string TableName { get; set; }
    }
}
