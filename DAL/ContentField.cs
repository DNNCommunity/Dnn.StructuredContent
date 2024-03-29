﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    /// <summary>
    /// Extends the StructuredContent_ContentField database object.
    /// </summary>
    [Serializable]
    public partial class StructuredContent_ContentField
    {
        // converts the enum name to SQL Server data type name

        /// <summary>
        /// Gets the DataType Name from the DataType Enum.
        /// </summary>
        public string DataTypeName
        {
            get
            {
                if (this.DataType == (int)Enums.DataTypes.Integer)
                {
                    return "int";
                }
                else
                {
                    return Enum.GetName(typeof(Enums.DataTypes), (Enums.DataTypes)this.DataType);
                }
            }
        }
    }
}
