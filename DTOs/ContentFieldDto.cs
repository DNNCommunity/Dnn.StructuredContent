// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    /// <summary>
    /// A DTO representing a ContentField.
    /// </summary>
    public class ContentFieldDto
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
        /// Gets or sets a value indicating whether the ContentField is a system level field.
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets Ordinal.
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets ColumnName.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets DataType.
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Gets or sets DataLength.
        /// </summary>
        public string DataLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether field allows null values.
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// Gets or sets DefaultValue.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets HelpText.
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Gets or sets LayoutRow.
        /// </summary>
        public int? LayoutRow { get; set; }

        /// <summary>
        /// Gets or sets LayoutColumn.
        /// </summary>
        public int? LayoutColumn { get; set; }

        /// <summary>
        /// Gets or sets ContentFieldTypeId.
        /// </summary>
        public int? ContentFieldTypeId { get; set; }

        /// <summary>
        /// Gets or sets Options.
        /// </summary>
        public object Options { get; set; }

        /// <summary>
        /// Gets or sets related ContentFieldTypeDto.
        /// </summary>
        public ContentFieldTypeDto ContentFieldType { get; set; }
    }
}
