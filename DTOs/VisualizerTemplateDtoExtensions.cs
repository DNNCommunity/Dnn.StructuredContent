// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    public static partial class VisualizerTemplateDtoExtensions
    {
        public static VisualizerTemplateDto ToDto(this StructuredContent_VisualizerTemplate item)
        {
            var dto = new VisualizerTemplateDto
            {
                Id = item.Id,
                ContentTypeId = item.ContentTypeId,
                Name = item.Name,
                Description = item.Description,
                Script = item.Script,
                Css = item.Css,
                Template = item.Template,
                Language = item.Language,
                ContentSize = item.ContentSize,
            };

            return dto;
        }

        public static StructuredContent_VisualizerTemplate ToItem(this VisualizerTemplateDto dto, StructuredContent_VisualizerTemplate item)
        {
            if (item == null)
            {
                item = new StructuredContent_VisualizerTemplate();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.ContentTypeId = dto.ContentTypeId;
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Script = dto.Script;
            item.Css = dto.Css;
            item.Template = dto.Template;
            item.Language = dto.Language;
            item.ContentSize = dto.ContentSize;

            return item;
        }
    }
}
