// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;
    using System.Collections.Generic;

    public class ContentItemDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public int content_type_id { get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_modified { get; set; }

        public List<ContentFieldDTO> content_fields { get; set; }
    }

    // there is no ToItem or ToDto methods for ContentItems
}
