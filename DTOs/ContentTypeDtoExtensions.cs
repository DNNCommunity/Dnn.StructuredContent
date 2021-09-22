// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// Extends the ContentTypeDto class to provide converters to/from the StructuredContent_ContentType database object.
    /// </summary>
    public static partial class ContentTypeDtoExtensions
    {
        /// <summary>
        /// Converts a StructuredContent_ContentType database object into a DTO.
        /// </summary>
        /// <param name="item">The incoming StructuredContent_ContentType database object.</param>
        /// <returns>ContentTypeDto.</returns>
        public static ContentTypeDto ToDto(this StructuredContent_ContentType item)
        {
            var dto = new ContentTypeDto
            {
                Id = item.Id,
                Name = item.Name,
                Singular = item.Singular,
                Plural = item.Plural,
                UrlSlug = item.UrlSlug,
                TableName = item.TableName,
            };

            return dto;
        }

        /// <summary>
        /// Converts a ContentTypeDto into a StructuredContent_ContentType database object.
        /// </summary>
        /// <param name="dto">The ContentTypeDto.</param>
        /// <param name="item">The StructuredContent_ContentType.</param>
        /// <returns>StructuredContent_ContentType.</returns>
        public static StructuredContent_ContentType ToItem(this ContentTypeDto dto, StructuredContent_ContentType item)
        {
            if (item == null)
            {
                item = new StructuredContent_ContentType();
            }

            if (dto == null)
            {
                return item;
            }

            item.Id = dto.Id;
            item.Name = dto.Name;
            item.Singular = dto.Singular;
            item.Plural = dto.Plural;
            item.UrlSlug = dto.UrlSlug;
            item.TableName = dto.TableName;

            return item;
        }
    }
}
