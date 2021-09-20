// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    public class VisualizerTemplateDto
    {
        public int Id { get; set; }

        public int ContentTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Script { get; set; }

        public string Css { get; set; }

        public string Template { get; set; }

        public string Language { get; set; }

        public string ContentSize { get; set; }
    }
}
