// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.Enums
{
    using System;

    /// <summary>
    /// Enum representing the various types of database datatypes.
    /// </summary>
    [Serializable]
    public enum DataTypes
    {
        /// <summary>
        /// The nvarchar data type.
        /// </summary>
        Nvarchar,

        /// <summary>
        /// The numeric data type.
        /// </summary>
        Numeric,

        /// <summary>
        /// The datetime data type.
        /// </summary>
        Datetime,

        /// <summary>
        /// The bit data type.
        /// </summary>
        Bit,

        /// <summary>
        /// The integer data type.
        /// </summary>
        Integer,
    }
}
