// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    /// <summary>
    /// Extends the StructuredContent_Relationship database object.
    /// </summary>
    [Serializable]
    public partial class StructuredContent_Relationship
    {
        private const string TablePrefix = "StructuredContent_ContentType_";

        /// <summary>
        /// Gets the name of the junction table for a many to many relationship.
        /// </summary>
        public string TableName
        {
            get
            {
                var aTableName = TablePrefix + this.StructuredContent_ContentType.TableName;
                var bTableName = TablePrefix + this.StructuredContent_ContentType1.TableName;
                var junctionTableName = aTableName + "X" + bTableName;

                return junctionTableName;
            }
        }
    }
}
