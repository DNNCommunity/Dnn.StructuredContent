// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    public static partial class RelationshipDtoExtensions
    {
        public static RelationshipDto ToDto(this StructuredContent_Relationship item)
        {
            var dto = new RelationshipDto
            {
                Id = item.Id,
                Key = item.Key,

                AContentTypeId = item.AContentTypeId,
                BContentTypeId = item.BContentTypeId,

                ARequired = item.ARequired,
                AMaxLimit = item.AMaxLimit,
                AMinLimit = item.AMinLimit,
                AHelpText = item.AHelpText,

                BRequired = item.BRequired,
                BMaxLimit = item.BMaxLimit,
                BMinLimit = item.BMinLimit,
                BHelpText = item.BHelpText,

                ALayoutRow = item.ALayoutRow,
                ALayoutColumn = item.ALayoutColumn,
                BLayoutRow = item.BLayoutRow,
                BLayoutColumn = item.BLayoutColumn,

                AContentType = item.StructuredContent_ContentType?.ToDto(),
                BContentType = item.StructuredContent_ContentType1?.ToDto(),
            };

            return dto;
        }

        public static StructuredContent_Relationship ToItem(this RelationshipDto dto, StructuredContent_Relationship item)
        {
            if (item == null)
            {
                item = new StructuredContent_Relationship();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.Key = dto.Key;

            item.AContentTypeId = dto.AContentTypeId;
            item.BContentTypeId = dto.BContentTypeId;

            item.ARequired = dto.ARequired;
            item.AMaxLimit = dto.AMaxLimit;
            item.AMinLimit = dto.AMinLimit;
            item.AHelpText = dto.AHelpText;

            item.BRequired = dto.BRequired;
            item.BMaxLimit = dto.BMaxLimit;
            item.BMinLimit = dto.BMinLimit;
            item.BHelpText = dto.BHelpText;

            item.ALayoutRow = dto.ALayoutRow;
            item.ALayoutColumn = dto.ALayoutColumn;
            item.BLayoutRow = dto.BLayoutRow;
            item.BLayoutColumn = dto.BLayoutColumn;

            return item;
        }
    }
}
