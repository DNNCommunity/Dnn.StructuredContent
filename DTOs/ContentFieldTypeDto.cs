// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    /// <summary>
    /// A DTO representing a ContentFieldType.
    /// </summary>
    public class ContentFieldTypeDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Ordinal.
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets DefaultDataType.
        /// </summary>
        public int DefaultDataType { get; set; }

        /// <summary>
        /// Gets or sets DefaultDataLength.
        /// </summary>
        public string DefaultDataLength { get; set; }

        /// <summary>
        /// Gets or sets DefaultOptions.
        /// </summary>
        public object DefaultOptions { get; set; }

        /// <summary>
        /// Gets or sets Icon.
        /// </summary>
        public string Icon { get; set; }
    }
}
