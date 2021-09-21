// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System.Collections.Generic;

    /// <summary>
    /// Helper class for SQL interactions.
    /// </summary>
    public interface ISQLHelper
    {
        /// <summary>
        /// Adds a column to a Structured Content table.
        /// </summary>
        /// <param name="contentField"><see cref="StructuredContent_ContentField"/>.</param>
        void AddColumn(StructuredContent_ContentField contentField);

        /// <summary>
        /// Creates a custom table for Structured Content.
        /// </summary>
        /// <param name="contentType">The content type to create the table for, <see cref="StructuredContent_ContentType"/>.</param>
        void CreateContentTable(StructuredContent_ContentType contentType);

        /// <summary>
        /// Creates the tables needed for a many-to-many relationship.
        /// </summary>
        /// <param name="aContentType">The first content type for the relation.</param>
        /// <param name="bContentType">The second content type for the relation.</param>
        void CreateManyToManyRelationship(StructuredContent_ContentType aContentType, StructuredContent_ContentType bContentType);

        /// <summary>
        /// Creates a one-to-many relationship in the database.
        /// </summary>
        /// <param name="oneContentType">The content type for the 'one' side.</param>
        /// <param name="manyContentType">The content type for the 'many' side.</param>
        void CreateOneToManyRelationship(StructuredContent_ContentType oneContentType, StructuredContent_ContentType manyContentType);

        /// <summary>
        /// Deletes a column for a given content item.
        /// </summary>
        /// <param name="contentField">The content field to delete.</param>
        void DeleteColumn(StructuredContent_ContentField contentField);

        /// <summary>
        /// Deletes a row in the content table.
        /// </summary>
        /// <param name="contentType">The structured content type.</param>
        /// <param name="id">The id of the item to delete.</param>
        void DeleteContentItem(StructuredContent_ContentType contentType, int id);

        /// <summary>
        /// Deletes a whole content type table.
        /// </summary>
        /// <param name="contentType">The content type for which to delete the table for.</param>
        void DeleteContentTable(StructuredContent_ContentType contentType);

        /// <summary>
        /// Removes a many-to-many relationship in the database.
        /// </summary>
        /// <param name="aContentType">One of the content types.</param>
        /// <param name="bContentType">The other content type.</param>
        void DeleteManyToManyRelationship(StructuredContent_ContentType aContentType, StructuredContent_ContentType bContentType);

        /// <summary>
        /// Deletes a single many-to-many relationship row.
        /// </summary>
        /// <param name="relationship">The relationship reference.</param>
        /// <param name="primaryContentType">The primary content type.</param>
        /// <param name="primaryContentItemId">The id of the primary content type.</param>
        void DeleteManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primaryContentType, int primaryContentItemId);

        /// <summary>
        /// Deletes the many-to-many junction table as well as the metadata about it.
        /// </summary>
        /// <param name="oneContentType">One of the content types.</param>
        /// <param name="manyContentType">The other content type in the relation.</param>
        void DeleteOneToManyRelationship(StructuredContent_ContentType oneContentType, StructuredContent_ContentType manyContentType);

        /// <summary>
        /// Deletes a one-to-many relationship.
        /// </summary>
        /// <param name="primaryContentType">The content type on the primary side.</param>
        /// <param name="relatedContentType">The related content type (the many side).</param>
        /// <param name="primaryContentItemId">The ID of the content item on the 'one' side of the relationship.</param>
        void DeleteOneToManyRelationship(StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId);

        /// <summary>
        /// Executes an SQL query that has no return.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        void ExecuteNonQuery(string query);

        /// <summary>
        /// Executes an SQL query that returns a single value.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <returns>An object containing the returned value.</returns>
        object ExecuteScalar(string query);

        /// <summary>
        /// Gets the items related to another item.
        /// </summary>
        /// <param name="relationship">The ralationship definition metadata.</param>
        /// <param name="contentType">The content type of the primary side of the raltion.</param>
        /// <param name="id">The Id of the item to get the related items for.</param>
        /// <returns>An enumeration of a dictionary where the key is the field name and the value is the field value.</returns>
        IEnumerable<IDictionary<string, object>> GetRelatedItems(StructuredContent_Relationship relationship, StructuredContent_ContentType contentType, int id);

        /// <summary>
        /// Inserts a new content item in its table.
        /// </summary>
        /// <param name="contentType">The type of the item.</param>
        /// <param name="contentIitem">The item itself.</param>
        /// <returns>The Id of the inserted row.</returns>
        int InsertContentItem(StructuredContent_ContentType contentType, dynamic contentItem);

        /// <summary>
        /// Creates a many-to-many relationship between two items.
        /// </summary>
        /// <param name="relationship">The relationship definition.</param>
        /// <param name="primaryContentType">The content type on the primary side.</param>
        /// <param name="relatedContentType">The content type on the ralted side.</param>
        /// <param name="primaryContentItemId">The id of the item that should be related on the primary side.</param>
        /// <param name="relatedContentItemId">The id of the item that should be related on the ralated side.</param>
        void SaveManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId, int relatedContentItemId);

        /// <summary>
        /// Creates a one-to-many relationship between two items.
        /// </summary>
        /// <param name="primaryContentType">The content type on the primary side.</param>
        /// <param name="relatedContentType">The content type on the related side.</param>
        /// <param name="primaryContentItemId">The id of the item that should be related on the primary side.</param>
        /// <param name="relatedContentItemId">The id of the item that should be related on the related side.</param>
        void SaveOneToManyRelationship(StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId, int relatedContentItemId);

        /// <summary>
        /// Gets a single item by its id.
        /// </summary>
        /// <param name="contentType">The type of the content item.</param>
        /// <param name="id">The id of the item.</param>
        /// <returns>A dictionary representation of the database row where the key is the field name and the value is the field value.</returns>
        IDictionary<string, object> SelectDynamicItem(StructuredContent_ContentType contentType, int id);

        /// <summary>
        /// Gets a list of items using an sql clause.
        /// </summary>
        /// <param name="contentType">The content type of the item.</param>
        /// <param name="whereClause">The sql Where clause to use.</param>
        /// <returns>An enumeration of dictionnaries where their key is the field name and their values are the field value.</returns>
        IEnumerable<IDictionary<string, object>> SelectDynamicList(StructuredContent_ContentType contentType, string whereClause);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="contentType">The type of the item to update.</param>
        /// <param name="contentItem">The actual item information to update.</param>
        void UpdateContentItem(StructuredContent_ContentType contentType, dynamic contentItem);
    }
}
