// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;

    using DotNetNuke.Common.Utilities;
    using StructuredContent.DAL;

    public class ContentFieldTypeDTO
    {
        public int id { get; set; }

        public string key { get; set; }

        public string type { get; set; }

        public string name { get; set; }

        public int ordinal { get; set; }

        public int default_data_type { get; set; }

        public string default_data_length { get; set; }

        public object default_options { get; set; }

        public string icon { get; set; }
    }

    public static partial class Extensions
    {
        public static ContentFieldTypeDTO ToDto(this StructuredContent_ContentFieldType item)
        {
            ContentFieldTypeDTO dto = new ContentFieldTypeDTO
            {
                id = item.id,
                key = item.key,
                type = item.type,
                name = item.name,
                ordinal = item.ordinal,
                default_data_type = item.default_data_type,
                default_data_length = item.default_data_length,
                default_options = item.default_options != null ? item.default_options.FromJson<object>() : new object(),
                icon = item.icon,
            };

            return dto;
        }

        public static StructuredContent_ContentFieldType ToItem(this ContentFieldTypeDTO dto, StructuredContent_ContentFieldType item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentFieldType();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.key = dto.key;
            item.type = dto.type;
            item.name = dto.name;
            item.ordinal = dto.ordinal;
            item.default_data_type = dto.default_data_type;
            item.default_data_length = dto.default_data_length;
            item.icon = dto.icon;

            if (dto.default_options == null)
            {
                item.default_options = null;
            }
            else
            {
                item.default_options = dto.default_options.ToString();
            }

            return item;
        }
    }
}
