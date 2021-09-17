using Newtonsoft.Json;
using System;

namespace StructuredContent.DAL
{
    public class RevisionDTO
    {
        public int id { get; set; }
        public DateTime revision_date { get; set; }
        public Nullable<int> user_id { get; set; }
        public string activity_type { get; set; }
        public int content_type_id { get; set; }
        public int item_id { get; set; }
        public object delta { get; set; }
        public object data { get; set; }
    }

    public static partial class Extensions
    {
        public static RevisionDTO ToDto(this StructuredContent_Revision item)
        {
            RevisionDTO dto = new RevisionDTO
            {
                id = item.id,
                revision_date = item.revision_date,
                user_id = item.user_id,
                activity_type = item.activity_type,
                content_type_id = item.content_type_id,
                item_id = item.item_id,
                delta = JsonConvert.DeserializeObject<dynamic>(item.delta),
                data = JsonConvert.DeserializeObject<dynamic>(item.data)
            };

            return dto;
        }

        public static StructuredContent_Revision ToItem(this RevisionDTO dto, StructuredContent_Revision item)
        {
            if (item == null)
            {
                item = new StructuredContent_Revision();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.revision_date = dto.revision_date;
            item.user_id = dto.user_id;
            item.activity_type = dto.activity_type;
            item.content_type_id = dto.content_type_id;
            item.item_id = dto.item_id;
            item.delta = JsonConvert.SerializeObject(dto.delta);
            item.data = JsonConvert.SerializeObject(dto.data);

            return item;
        }
    }
}