// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    [Serializable]
    public partial class StructuredContent_Relationship
    {
        private const string TablePrefix = "StructuredContent_ContentType_";

        // derive the table name for the relationship
        public string table_name
        {
            get
            {
                string a_table_name = TablePrefix + this.StructuredContent_ContentType.table_name;
                string b_table_name = TablePrefix + this.StructuredContent_ContentType1.table_name;
                string junction_table_name = a_table_name + "X" + b_table_name;

                return junction_table_name;
            }
        }
    }
}
