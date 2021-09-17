namespace StructuredContent.DAL
{
    public class ContentTypeDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string singular { get; set; }
        public string plural { get; set; }
        public string url_slug { get; set; }
        public string table_name { get; set; }
    }

    public static partial class Extensions
    {
        public static ContentTypeDTO ToDto(this StructuredContent_ContentType item)
        {
            ContentTypeDTO dto = new ContentTypeDTO
            {
                id = item.id,
                name = item.name,
                singular = item.singular,
                plural = item.plural,
                url_slug = item.url_slug,
                table_name = item.table_name
            };

            return dto;
        }

        public static StructuredContent_ContentType ToItem(this ContentTypeDTO dto, StructuredContent_ContentType item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentType();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.name = dto.name;
            item.singular = dto.singular;
            item.plural = dto.plural;
            item.url_slug = dto.url_slug;
            item.table_name = dto.table_name;

            return item;
        }
    }
}