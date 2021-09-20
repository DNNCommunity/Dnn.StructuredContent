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
        /// <param name="content_field"><see cref="StructuredContent_ContentField"/>.</param>
        void AddColumn(StructuredContent_ContentField content_field);

        /// <summary>
        /// Creates a custom table for Structured Content.
        /// </summary>
        /// <param name="content_type">The content type to create the table for, <see cref="StructuredContent_ContentType"/>.</param>
        void CreateContentTable(StructuredContent_ContentType content_type);

        /// <summary>
        /// Creates the tables needed for a many-to-many relationship.
        /// </summary>
        /// <param name="a_content_type">The first content type for the relation.</param>
        /// <param name="b_content_type">The second content type for the relation.</param>
        void CreateManyToManyRelationship(StructuredContent_ContentType a_content_type, StructuredContent_ContentType b_content_type);

        /// <summary>
        /// Creates a one-to-many relationship in the database.
        /// </summary>
        /// <param name="one_content_type">The content type for the 'one' side.</param>
        /// <param name="many_content_type">The content type for the 'many' side.</param>
        void CreateOneToManyRelationship(StructuredContent_ContentType one_content_type, StructuredContent_ContentType many_content_type);

        /// <summary>
        /// Deletes a column for a given content item.
        /// </summary>
        /// <param name="content_field">The content field to delete.</param>
        void DeleteColumn(StructuredContent_ContentField content_field);

        /// <summary>
        /// Deletes a row in the content table.
        /// </summary>
        /// <param name="content_type">The structured content type.</param>
        /// <param name="id">The id of the item to delete.</param>
        void DeleteContentItem(StructuredContent_ContentType content_type, int id);

        /// <summary>
        /// Deletes a whole content type table.
        /// </summary>
        /// <param name="content_type">The content type for which to delete the table for.</param>
        void DeleteContentTable(StructuredContent_ContentType content_type);

        /// <summary>
        /// Removes a many-to-many relationship in the database.
        /// </summary>
        /// <param name="a_content_type">One of the content types.</param>
        /// <param name="b_content_type">The other content type.</param>
        void DeleteManyToManyRelationship(StructuredContent_ContentType a_content_type, StructuredContent_ContentType b_content_type);

        /// <summary>
        /// Deletes a single many-to-many relationship row.
        /// </summary>
        /// <param name="relationship">The relationship reference.</param>
        /// <param name="primary_content_type">The primary content type.</param>
        /// <param name="primary_content_item_id">The id of the primary content type.</param>
        void DeleteManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, int primary_content_item_id);

        /// <summary>
        /// Deletes the many-to-many junction table as well as the metadata about it.
        /// </summary>
        /// <param name="one_content_type">One of the content types.</param>
        /// <param name="many_content_type">The other content type in the relation.</param>
        void DeleteOneToManyRelationship(StructuredContent_ContentType one_content_type, StructuredContent_ContentType many_content_type);

        /// <summary>
        /// Deletes a one-to-many relationship.
        /// </summary>
        /// <param name="primary_content_type">The content type on the primary side.</param>
        /// <param name="related_content_type">The related content type (the many side).</param>
        /// <param name="primary_content_item_id">The ID of the content item on the 'one' side of the relationship.</param>
        void DeleteOneToManyRelationship(StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id);

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
        /// <param name="content_type">The content type of the primary side of the raltion.</param>
        /// <param name="id">The Id of the item to get the related items for.</param>
        /// <returns>An enumeration of a dictionary where the key is the field name and the value is the field value.</returns>
        IEnumerable<IDictionary<string, object>> GetRelatedItems(StructuredContent_Relationship relationship, StructuredContent_ContentType content_type, int id);

        /// <summary>
        /// Inserts a new content item in its table.
        /// </summary>
        /// <param name="content_type">The type of the item.</param>
        /// <param name="content_item">The item itself.</param>
        /// <returns>The Id of the inserted row.</returns>
        int InsertContentItem(StructuredContent_ContentType content_type, dynamic content_item);

        /// <summary>
        /// Creates a many-to-many relationship between two items.
        /// </summary>
        /// <param name="relationship">The relationship definition.</param>
        /// <param name="primary_content_type">The content type on the primary side.</param>
        /// <param name="related_content_type">The content type on the ralted side.</param>
        /// <param name="primary_content_item_id">The id of the item that should be related on the primary side.</param>
        /// <param name="related_content_item_id">The id of the item that should be related on the ralated side.</param>
        void SaveManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id, int related_content_item_id);

        /// <summary>
        /// Creates a one-to-many relationship between two items.
        /// </summary>
        /// <param name="primary_content_type">The content type on the primary side.</param>
        /// <param name="related_content_type">The content type on the related side.</param>
        /// <param name="primary_content_item_id">The id of the item that should be related on the primary side.</param>
        /// <param name="related_content_item_id">The id of the item that should be related on the related side.</param>
        void SaveOneToManyRelationship(StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id, int related_content_item_id);

        /// <summary>
        /// Gets a single item by its id.
        /// </summary>
        /// <param name="content_type">The type of the content item.</param>
        /// <param name="id">The id of the item.</param>
        /// <returns>A dictionary representation of the database row where the key is the field name and the value is the field value.</returns>
        IDictionary<string, object> SelectDynamicItem(StructuredContent_ContentType content_type, int id);

        /// <summary>
        /// Gets a list of items using an sql clause.
        /// </summary>
        /// <param name="content_type">The content type of the item.</param>
        /// <param name="where_clause">The sql Where clause to use.</param>
        /// <returns>An enumeration of dictionnaries where their key is the field name and their values are the field value.</returns>
        IEnumerable<IDictionary<string, object>> SelectDynamicList(StructuredContent_ContentType content_type, string where_clause);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="content_type">The type of the item to update.</param>
        /// <param name="content_item">The actual item information to update.</param>
        void UpdateContentItem(StructuredContent_ContentType content_type, dynamic content_item);
    }
}
