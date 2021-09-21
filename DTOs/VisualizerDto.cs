// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    public class VisualizerDto
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int ContentTypeId { get; set; }

        public int? VisualizerTemplateId { get; set; }

        public int? ItemId { get; set; }

        public string ItemFilter { get; set; }

        public string Content { get; set; }
    }
}
