// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{

    using DotNetNuke.Common.Utilities;
    using StructuredContent.DAL;

    public static partial class ContentFieldDtoExtensions
    {
        public static ContentFieldDto ToDto(this StructuredContent_ContentField item)
        {
            var dto = new ContentFieldDto
            {
                Id = item.Id,
                Name = item.Name,
                ContentTypeId = item.ContentTypeId,
                IsSystem = item.IsSystem,
                Ordinal = item.Ordinal,
                ColumnName = item.ColumnName,

                DataType = item.DataType,
                DataLength = item.DataLength,
                AllowNull = item.AllowNull,
                DefaultValue = item.DefaultValue,

                HelpText = item.HelpText,
                ContentFieldTypeId = item.ContentFieldTypeId,

                LayoutRow = item.LayoutRow,
                LayoutColumn = item.LayoutColumn,

                Options = item.Options != null ? item.Options.FromJson<object>() : new object(),
                ContentFieldType = item.StructuredContent_ContentFieldType?.ToDto(),
            };

            return dto;
        }

        public static StructuredContent_ContentField ToItem(this ContentFieldDto dto, StructuredContent_ContentField item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentField();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.Name = dto.Name;
            item.ContentTypeId = dto.ContentTypeId;
            item.IsSystem = dto.IsSystem;
            item.Ordinal = dto.Ordinal;
            item.ColumnName = dto.ColumnName;
            item.DataType = dto.DataType;
            item.DataLength = dto.DataLength;
            item.AllowNull = dto.AllowNull;
            item.DefaultValue = dto.DefaultValue;

            item.HelpText = dto.HelpText;
            item.ContentFieldTypeId = dto.ContentFieldTypeId;

            item.LayoutRow = dto.LayoutRow;
            item.LayoutColumn = dto.LayoutColumn;

            if (dto.Options == null)
            {
                item.Options = null;
            }
            else
            {
                item.Options = dto.Options.ToString();
            }

            return item;
        }
    }
}
