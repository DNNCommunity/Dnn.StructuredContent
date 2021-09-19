using System;

namespace StructuredContent.DAL
{
    [Serializable]
    public partial class StructuredContent_Relationship
    {
        private const string table_prefix = "StructuredContent_ContentType_";

        // derive the table name for the relationship
        public string table_name
        {
            get
            {
                string a_table_name = table_prefix + this.StructuredContent_ContentType.table_name;
                string b_table_name = table_prefix + this.StructuredContent_ContentType1.table_name;
                string junction_table_name = a_table_name + "X" + b_table_name;

                return junction_table_name;
            }
        }
    }
}