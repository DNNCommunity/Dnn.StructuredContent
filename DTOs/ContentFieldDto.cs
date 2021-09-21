// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    public class ContentFieldDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ContentTypeId { get; set; }

        public bool IsSystem { get; set; }

        public int Ordinal { get; set; }

        public string ColumnName { get; set; }

        public int DataType { get; set; }

        public string DataLength { get; set; }

        public bool AllowNull { get; set; }

        public string DefaultValue { get; set; }

        public string HelpText { get; set; }

        public int? LayoutRow { get; set; }

        public int? LayoutColumn { get; set; }

        public int? ContentFieldTypeId { get; set; }

        public object Options { get; set; }

        public ContentFieldTypeDto ContentFieldType { get; set; }

        public object Value { get; set; }
    }
}
