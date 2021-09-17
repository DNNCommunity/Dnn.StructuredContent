using System;

namespace StructuredContent.DAL
{
    [Serializable]
    public partial class StructuredContent_ContentField
    {
        //public string content_field_type_name
        //{
        //    get
        //    {
        //        if (this.content_field_type_id.HasValue)
        //        {
        //            return this.StructuredContent_ContentFieldType.name;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}

        //public string content_field_type_key
        //{
        //    get
        //    {
        //        if (this.content_field_type_id.HasValue)
        //        {
        //            return this.StructuredContent_ContentFieldType.key;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}

        // converts the enum name to SQL Server data type name
        public string data_type_name
        {
            get
            {
                if (this.data_type == (int)Enums.DataTypes.integer)
                {
                    return "int";
                }
                else
                {
                    return Enum.GetName(typeof(Enums.DataTypes), (Enums.DataTypes)this.data_type);
                }

            }
        }
    }
}