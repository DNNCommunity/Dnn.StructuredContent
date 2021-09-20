// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    public class VisualizerDTO
    {
        public int id { get; set; }

        public int module_id { get; set; }

        public int content_type_id { get; set; }

        public int? visualizer_template_id { get; set; }

        public int? item_id { get; set; }

        public string item_filter { get; set; }

        public string content { get; set; }
    }

    public static partial class Extensions
    {
        public static VisualizerDTO ToDto(this StructuredContent_Visualizer item)
        {
            VisualizerDTO dto = new VisualizerDTO
            {
                id = item.id,
                module_id = item.module_id,
                content_type_id = item.content_type_id,
                visualizer_template_id = item.visualizer_template_id,
                item_id = item.item_id,
                item_filter = item.item_filter,
            };

            return dto;
        }

        public static StructuredContent_Visualizer ToItem(this VisualizerDTO dto, StructuredContent_Visualizer item)
        {
            if (item == null)
            {
                item = new StructuredContent_Visualizer();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.module_id = dto.module_id;
            item.content_type_id = dto.content_type_id;
            item.visualizer_template_id = dto.visualizer_template_id;
            item.item_id = dto.item_id;
            item.item_filter = dto.item_filter;

            return item;
        }
    }
}
