// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    /// <summary>
    /// A DTO representing a Revision.
    /// </summary>
    public class RevisionDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets RevisionDate.
        /// </summary>
        public DateTime RevisionDate { get; set; }

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets ActivityType.
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Gets or sets ContentTypeId.
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets ItemId.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets Delta.
        /// </summary>
        public object Delta { get; set; }

        /// <summary>
        /// Gets or sets Data.
        /// </summary>
        public object Data { get; set; }
    }
}
