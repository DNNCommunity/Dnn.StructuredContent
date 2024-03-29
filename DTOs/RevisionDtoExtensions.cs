﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using Newtonsoft.Json;

    /// <summary>
    /// Extends the RevisionDto class to provide converters to/from the StructuredContent_Revision database object.
    /// </summary>
    public static partial class RevisionDtoExtensions
    {
        /// <summary>
        /// Converts a StructuredContent_Revision database object into a RevisionDto.
        /// </summary>
        /// <param name="item">The StructuredContent_Revision object.</param>
        /// <returns>A RevisionDto object.</returns>
        public static RevisionDto ToDto(this StructuredContent_Revision item)
        {
            var dto = new RevisionDto
            {
                Id = item.Id,
                RevisionDate = item.RevisionDate,
                UserId = item.UserId,
                ActivityType = item.ActivityType,
                ContentTypeId = item.ContentTypeId,
                ItemId = item.ItemId,
                Delta = JsonConvert.DeserializeObject<dynamic>(item.Delta),
                Data = JsonConvert.DeserializeObject<dynamic>(item.Data),
            };

            return dto;
        }

        /// <summary>
        /// Converts a RevisionDto into a StructuredContent_Revision database object.
        /// </summary>
        /// <param name="dto">The RevisionDto.</param>
        /// <param name="item">The StructuredContent_Revision.</param>
        /// <returns>A StructuredContent_Revision.</returns>
        public static StructuredContent_Revision ToItem(this RevisionDto dto, StructuredContent_Revision item)
        {
            if (item == null)
            {
                item = new StructuredContent_Revision();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.RevisionDate = dto.RevisionDate;
            item.UserId = dto.UserId;
            item.ActivityType = dto.ActivityType;
            item.ContentTypeId = dto.ContentTypeId;
            item.ItemId = dto.ItemId;
            item.Delta = JsonConvert.SerializeObject(dto.Delta);
            item.Data = JsonConvert.SerializeObject(dto.Data);

            return item;
        }
    }
}
