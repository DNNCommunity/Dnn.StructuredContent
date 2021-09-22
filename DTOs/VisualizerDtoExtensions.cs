// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// Extends the VisualizerDto class to provide converters to/from the StructuredContent_Visualizer database object.
    /// </summary>
    public static partial class VisualizerDtoExtensions
    {
        /// <summary>
        /// Converts a StructuredContent_Visualizer database object into a VisualizerDto.
        /// </summary>
        /// <param name="item">The StructuredContent_Visualizer object.</param>
        /// <returns>A VisualizerDto object.</returns>
        public static VisualizerDto ToDto(this StructuredContent_Visualizer item)
        {
            var dto = new VisualizerDto
            {
                Id = item.Id,
                ModuleId = item.ModuleId,
                ContentTypeId = item.ContentTypeId,
                VisualizerTemplateId = item.VisualizerTemplateId,
                ItemId = item.ItemId,
                ItemFilter = item.ItemFilter,
            };

            return dto;
        }

        /// <summary>
        /// Converts a VisualizerDto into a StructuredContent_Visualizer database object.
        /// </summary>
        /// <param name="dto">The VisualizerDto.</param>
        /// <param name="item">The StructuredContent_Visualizer.</param>
        /// <returns>A StructuredContent_Visualizer.</returns>
        public static StructuredContent_Visualizer ToItem(this VisualizerDto dto, StructuredContent_Visualizer item)
        {
            if (item == null)
            {
                item = new StructuredContent_Visualizer();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.ModuleId = dto.ModuleId;
            item.ContentTypeId = dto.ContentTypeId;
            item.VisualizerTemplateId = dto.VisualizerTemplateId;
            item.ItemId = dto.ItemId;
            item.ItemFilter = dto.ItemFilter;

            return item;
        }
    }
}
