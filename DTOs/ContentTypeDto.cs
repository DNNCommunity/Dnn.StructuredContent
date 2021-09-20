// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    public class ContentTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Singular { get; set; }

        public string Plural { get; set; }

        public string UrlSlug { get; set; }

        public string TableName { get; set; }
    }
}
