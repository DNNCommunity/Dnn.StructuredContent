// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    public class RevisionDto
    {
        public int Id { get; set; }

        public DateTime RevisionDate { get; set; }

        public int? UserId { get; set; }

        public string ActivityType { get; set; }

        public int ContentTypeId { get; set; }

        public int ItemId { get; set; }

        public object Delta { get; set; }

        public object Data { get; set; }
    }
}
