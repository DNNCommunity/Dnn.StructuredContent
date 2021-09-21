// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;
    using System.Collections.Generic;

    public class ContentItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ContentTypeId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public List<ContentFieldDto> ContentFields { get; set; }
    }
}
