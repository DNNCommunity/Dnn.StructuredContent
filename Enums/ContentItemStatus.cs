// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.Enums
{
    using System;

    /// <summary>
    /// An enum representing various workflow states.
    /// </summary>
    [Serializable]
    public enum ContentItemStatus
    {
        /// <summary>
        /// Draft status.
        /// </summary>
        Draft,

        /// <summary>
        /// Staged status.
        /// </summary>
        Staged,

        /// <summary>
        /// Published status.
        /// </summary>
        Published,

        /// <summary>
        /// Archived status.
        /// </summary>
        Archived,
    }
}
