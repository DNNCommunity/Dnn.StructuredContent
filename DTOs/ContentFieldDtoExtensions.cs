// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using DotNetNuke.Common.Utilities;
    using StructuredContent.DAL;

    /// <summary>
    /// Extends the ContentFieldDTO class to provide converters to/from the StructuredContent_ContentField database object.
    /// </summary>
    public static partial class ContentFieldDtoExtensions
    {
        /// <summary>
        /// Converts a StructuredContent_ContentField database object into a ContentFieldDto.
        /// </summary>
        /// <param name="item">The StructuredContent_ContentField object.</param>
        /// <returns>A ContentFieldDto object.</returns>
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

        /// <summary>
        /// Converts a ContentFieldDto into a StructuredContent_ContentField database object.
        /// </summary>
        /// <param name="dto">The ContentFieldDto.</param>
        /// <param name="item">The StructuredContent_ContentField.</param>
        /// <returns>A StructuredContent_ContentField.</returns>
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
