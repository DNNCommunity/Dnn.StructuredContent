namespace StructuredContent.DAL
{
    public class VisualizerTemplateDTO
    {
        public int id { get; set; }
        public int content_type_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string script { get; set; }
        public string css { get; set; }
        public string template { get; set; }
        public string language { get; set; }
        public string content_size { get; set; }
    }

    public static partial class Extensions
    {
        public static VisualizerTemplateDTO ToDto(this StructuredContent_VisualizerTemplate item)
        {
            VisualizerTemplateDTO dto = new VisualizerTemplateDTO
            {
                id = item.id,
                content_type_id = item.content_type_id,
                name = item.name,
                description = item.description,
                script = item.script,
                css = item.css,
                template = item.template,
                language = item.language,
                content_size = item.content_size
            };

            return dto;
        }

        public static StructuredContent_VisualizerTemplate ToItem(this VisualizerTemplateDTO dto, StructuredContent_VisualizerTemplate item)
        {
            if (item == null)
            {
                item = new StructuredContent_VisualizerTemplate();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.content_type_id = dto.content_type_id;
            item.name = dto.name;
            item.description = dto.description;
            item.script = dto.script;
            item.css = dto.css;
            item.template = dto.template;
            item.language = dto.language;
            item.content_size = dto.content_size;

            return item;
        }
    }
}