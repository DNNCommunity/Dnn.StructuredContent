// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;

    using DotNetNuke.Common.Utilities;
    using StructuredContent.DAL;

    public static partial class ContentFieldTypeDtoExtensions
    {
        public static ContentFieldTypeDto ToDto(this StructuredContent_ContentFieldType item)
        {
            var dto = new ContentFieldTypeDto
            {
                Id = item.Id,
                Key = item.Key,
                Type = item.Type,
                Name = item.Name,
                Ordinal = item.Ordinal,
                DefaultDataType = item.DefaultDataType,
                DefaultDataLength = item.DefaultDataLength,
                DefaultOptions = item.DefaultOptions != null ? item.DefaultOptions.FromJson<object>() : new object(),
                Icon = item.Icon,
            };

            return dto;
        }

        public static StructuredContent_ContentFieldType ToItem(this ContentFieldTypeDto dto, StructuredContent_ContentFieldType item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentFieldType();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.Key = dto.Key;
            item.Type = dto.Type;
            item.Name = dto.Name;
            item.Ordinal = dto.Ordinal;
            item.DefaultDataType = dto.DefaultDataType;
            item.DefaultDataLength = dto.DefaultDataLength;
            item.Icon = dto.Icon;

            if (dto.DefaultOptions == null)
            {
                item.DefaultOptions = null;
            }
            else
            {
                item.DefaultOptions = dto.DefaultOptions.ToString();
            }

            return item;
        }
    }
}
