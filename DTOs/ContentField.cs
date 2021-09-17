using DotNetNuke.Common.Utilities;
using StructuredContent.DAL;
using System;

namespace StructuredContent
{
    public class ContentFieldDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public int content_type_id { get; set; }
        public bool is_system { get; set; }
        public int ordinal { get; set; }
        public string column_name { get; set; }
        public int data_type { get; set; }
        public string data_length { get; set; }
        public bool allow_null { get; set; }
        public string default_value { get; set; }
        public string help_text { get; set; }

        public Nullable<int> layout_row { get; set; }
        public Nullable<int> layout_column { get; set; }

        public Nullable<int> content_field_type_id { get; set; }
        public object options { get; set; }

        public ContentFieldTypeDTO content_field_type { get; set; }


        public object value { get; set; }
    }

    public static partial class Extensions
    {
        public static ContentFieldDTO ToDto(this StructuredContent_ContentField item)
        {
            ContentFieldDTO dto = new ContentFieldDTO
            {
                id = item.id,
                name = item.name,
                content_type_id = item.content_type_id,
                is_system = item.is_system,
                ordinal = item.ordinal,
                column_name = item.column_name,

                data_type = item.data_type,
                data_length = item.data_length,
                allow_null = item.allow_null,
                default_value = item.default_value,

                help_text = item.help_text,
                content_field_type_id = item.content_field_type_id,

                layout_row = item.layout_row,
                layout_column = item.layout_column,

                options = item.options != null ? item.options.FromJson<object>() : new object(),
                content_field_type = item.StructuredContent_ContentFieldType != null ? item.StructuredContent_ContentFieldType.ToDto() : null
            };

            return dto;
        }

        public static StructuredContent_ContentField ToItem(this ContentFieldDTO dto, StructuredContent_ContentField item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentField();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.name = dto.name;
            item.content_type_id = dto.content_type_id;
            item.is_system = dto.is_system;
            item.ordinal = dto.ordinal;
            item.column_name = dto.column_name;
            item.data_type = dto.data_type;
            item.data_length = dto.data_length;
            item.allow_null = dto.allow_null;
            item.default_value = dto.default_value;

            item.help_text = dto.help_text;
            item.content_field_type_id = dto.content_field_type_id;

            item.layout_row = dto.layout_row;
            item.layout_column = dto.layout_column;

            if (dto.options == null)
            {
                item.options = null;
            }
            else
            {
                item.options = dto.options.ToString();
            }

            return item;
        }
    }
}