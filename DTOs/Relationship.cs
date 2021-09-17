using System;

namespace StructuredContent.DAL
{
    public class RelationshipDTO
    {
        public int id { get; set; }
        public string key { get; set; }

        public int a_content_type_id { get; set; }
        public int b_content_type_id { get; set; }

        public Nullable<bool> a_required { get; set; }
        public Nullable<int> a_min_limit { get; set; }
        public Nullable<int> a_max_limit { get; set; }
        public string a_help_text { get; set; }

        public Nullable<bool> b_required { get; set; }
        public Nullable<int> b_min_limit { get; set; }
        public Nullable<int> b_max_limit { get; set; }
        public string b_help_text { get; set; }

        public Nullable<int> a_layout_row { get; set; }
        public Nullable<int> a_layout_column { get; set; }

        public Nullable<int> b_layout_row { get; set; }
        public Nullable<int> b_layout_column { get; set; }

        public string relationship_name
        {
            get
            {
                switch (key)
                {
                    case "o2m":
                        return "One To Many";

                    case "m2o":
                        return "Many To One";

                    case "m2m":
                        return "Many To Many";

                    default:
                        return string.Empty;
                }
            }
        }

        public ContentTypeDTO a_content_type { get; set; }
        public ContentTypeDTO b_content_type { get; set; }
    }

    public static partial class Extensions
    {
        public static RelationshipDTO ToDto(this StructuredContent_Relationship item)
        {
            RelationshipDTO dto = new RelationshipDTO
            {
                id = item.id,
                key = item.key,

                a_content_type_id = item.a_content_type_id,
                b_content_type_id = item.b_content_type_id,

                a_required = item.a_required,
                a_max_limit = item.a_max_limit,
                a_min_limit = item.a_min_limit,
                a_help_text = item.a_help_text,

                b_required = item.b_required,
                b_max_limit = item.b_max_limit,
                b_min_limit = item.b_min_limit,
                b_help_text = item.b_help_text,

                a_layout_row = item.a_layout_row,
                a_layout_column = item.a_layout_column,
                b_layout_row = item.b_layout_row,
                b_layout_column = item.b_layout_column,

                a_content_type = item.StructuredContent_ContentType != null ? item.StructuredContent_ContentType.ToDto() : null,
                b_content_type = item.StructuredContent_ContentType1 != null ? item.StructuredContent_ContentType1.ToDto() : null,
            };

            return dto;
        }

        public static StructuredContent_Relationship ToItem(this RelationshipDTO dto, StructuredContent_Relationship item)
        {
            if (item == null)
            {
                item = new StructuredContent_Relationship();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.key = dto.key;

            item.a_content_type_id = dto.a_content_type_id;
            item.b_content_type_id = dto.b_content_type_id;

            item.a_required = dto.a_required;
            item.a_max_limit = dto.a_max_limit;
            item.a_min_limit = dto.a_min_limit;
            item.a_help_text = dto.a_help_text;

            item.b_required = dto.b_required;
            item.b_max_limit = dto.b_max_limit;
            item.b_min_limit = dto.b_min_limit;
            item.b_help_text = dto.b_help_text;

            item.a_layout_row = dto.a_layout_row;
            item.a_layout_column = dto.a_layout_column;
            item.b_layout_row = dto.b_layout_row;
            item.b_layout_column = dto.b_layout_column;

            return item;
        }
    }
}