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
        public string TableName
        {
            get
            {
                string a_TableName = TablePrefix + this.StructuredContent_ContentType.TableName;
                string b_TableName = TablePrefix + this.StructuredContent_ContentType1.TableName;
                string junction_TableName = a_TableName + "X" + b_TableName;

                return junction_TableName;
            }
        }
    }
}
