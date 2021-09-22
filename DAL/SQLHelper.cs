// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A suite of SQL helper methods to access the data.
    /// </summary>
    // TODO:  Need to add SQL inject security
    internal class SQLHelper : ISQLHelper
    {
        private const string TablePrefix = "StructuredContent_ContentType_";

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ToString();

        /// <inheritdoc/>
        public void ExecuteNonQuery(string query)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(this.connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public object ExecuteScalar(string query)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(this.connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        var ret = sqlCommand.ExecuteScalar();
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void CreateContentTable(StructuredContent_ContentType contentType)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // TSQL for CREATE TABLE
                var sb = new StringBuilder();
                sb.Append("CREATE TABLE [dbo].[" + tableName + "] ");
                sb.Append("(");
                sb.Append("[Id] [int] IDENTITY(1,1) NOT NULL, ");
                sb.Append("[Name] [nvarchar](256) NOT NULL, ");
                sb.Append("[Status] [varchar](10) NOT NULL, ");
                sb.Append("[DateCreated] [datetime] NOT NULL, ");
                sb.Append("[DateModified] [datetime] NOT NULL, ");
                sb.Append("[DatePublished] [datetime] NULL, ");
                sb.Append("CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED ([Id] ASC)");
                sb.Append(")");

                this.ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteContentTable(StructuredContent_ContentType contentType)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // TSQL for DELETE TABLE
                var sb = new StringBuilder();
                sb.Append("DROP TABLE [dbo].[" + tableName + "] ");

                this.ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void AddColumn(StructuredContent_ContentField contentField)
        {
            try
            {
                var tableName = TablePrefix + contentField.StructuredContent_ContentType.TableName;
                var columnName = contentField.ColumnName;
                var dataType = contentField.DataType_name;
                var dataLength = contentField.DataLength;
                var allowNull = contentField.AllowNull;
                var defaultValue = contentField.DefaultValue;

                // TSQL for ADD COLUMN
                var sbAddColumn = new StringBuilder();
                sbAddColumn.Append("ALTER TABLE [dbo].[" + tableName + "]");
                sbAddColumn.Append(" ADD [" + columnName + "] [" + dataType.ToLower() + "]" + (!string.IsNullOrEmpty(dataLength) ? "(" + dataLength + ")" : string.Empty) + (allowNull ? string.Empty : " NOT") + " NULL");

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    var constraint_name = "DF_" + tableName + "_" + columnName;

                    sbAddColumn.Append(" CONSTRAINT [" + constraint_name + "] DEFAULT ");
                    if (dataType == "nvarchar")
                    {
                        sbAddColumn.Append(" N'" + defaultValue + "'");
                    }
                    else
                    {
                        sbAddColumn.Append(" " + defaultValue);
                    }
                }

                this.ExecuteNonQuery(sbAddColumn.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteColumn(StructuredContent_ContentField contentField)
        {
            try
            {
                var tableName = TablePrefix + contentField.StructuredContent_ContentType.TableName;
                var columnName = contentField.ColumnName;

                // DROP DefaultValue constraints?
                if (!string.IsNullOrEmpty(contentField.DefaultValue))
                {
                    var constraint_name = "DF_" + tableName + "_" + columnName;
                    var sbDropConstraint = new StringBuilder();
                    sbDropConstraint.Append("ALTER TABLE [dbo].[" + tableName + "]");
                    sbDropConstraint.Append(" DROP CONSTRAINT [" + constraint_name + "]");

                    this.ExecuteNonQuery(sbDropConstraint.ToString());
                }

                // TSQL for DROP COLUMN
                var sbDropColumn = new StringBuilder();
                sbDropColumn.Append("ALTER TABLE [dbo].[" + tableName + "] ");
                sbDropColumn.Append("DROP COLUMN [" + columnName + "]");

                this.ExecuteNonQuery(sbDropColumn.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void CreateOneToManyRelationship(StructuredContent_ContentType oneContentType, StructuredContent_ContentType manyContentType)
        {
            try
            {
                // both tables need to exist
                // create the constraint on the child table

                var sb = new StringBuilder();
                sb.Append("ALTER TABLE [dbo].[" + TablePrefix + manyContentType.TableName + "]");
                sb.Append(" ADD CONSTRAINT [FK_" + TablePrefix + oneContentType.TableName + "_" + TablePrefix + manyContentType.TableName + "]");
                sb.Append(" FOREIGN KEY (" + oneContentType.Singular + "Id" + ")");
                sb.Append(" REFERENCES [dbo].[" + TablePrefix + oneContentType.TableName + "] (Id)");
                sb.Append(" ON UPDATE NO ACTION");
                sb.Append(" ON DELETE NO ACTION");

                this.ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteOneToManyRelationship(StructuredContent_ContentType oneContentTtype, StructuredContent_ContentType manyContentType)
        {
            try
            {
                var foreignKeyColumnName = oneContentTtype.Singular + "Id";

                // drop relationship
                var sbOneToMany = new StringBuilder();
                sbOneToMany.Append("ALTER TABLE [dbo].[" + TablePrefix + manyContentType.TableName + "]");
                sbOneToMany.Append(" DROP CONSTRAINT [FK_" + TablePrefix + oneContentTtype.TableName + "_" + TablePrefix + manyContentType.TableName + "]");

                this.ExecuteNonQuery(sbOneToMany.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void CreateManyToManyRelationship(StructuredContent_ContentType aContentType, StructuredContent_ContentType bContentType)
        {
            try
            {
                // create the junction table
                var aTableName = TablePrefix + aContentType.TableName;
                var bTableName = TablePrefix + bContentType.TableName;
                var junctionTableName = aTableName + "X" + bTableName;

                var a_foreign_key_ColumnName = aContentType.Singular + "Id";
                var b_foreign_key_ColumnName = bContentType.Singular + "Id";

                // TSQL for CREATE TABLE
                var sbCreateJunctionTable = new StringBuilder();
                sbCreateJunctionTable.Append("CREATE TABLE [dbo].[" + junctionTableName + "] ");
                sbCreateJunctionTable.Append("(");
                sbCreateJunctionTable.Append("[id] [int] IDENTITY(1,1) NOT NULL, ");
                sbCreateJunctionTable.Append("[" + a_foreign_key_ColumnName + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("[" + b_foreign_key_ColumnName + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("CONSTRAINT [PK_" + junctionTableName + "] PRIMARY KEY CLUSTERED ([Id] ASC)");
                sbCreateJunctionTable.Append(")");

                this.ExecuteNonQuery(sbCreateJunctionTable.ToString());

                // TSQL to set up relationships
                var sbARelationship = new StringBuilder();
                sbARelationship.Append("ALTER TABLE [dbo].[" + junctionTableName + "]");
                sbARelationship.Append(" ADD CONSTRAINT [FK_" + junctionTableName + "_" + aTableName + "]");
                sbARelationship.Append(" FOREIGN KEY (" + a_foreign_key_ColumnName + ")");
                sbARelationship.Append(" REFERENCES [dbo].[" + aTableName + "] (id)");
                sbARelationship.Append(" ON UPDATE CASCADE");
                sbARelationship.Append(" ON DELETE CASCADE");

                this.ExecuteNonQuery(sbARelationship.ToString());

                var sbBRelationship = new StringBuilder();
                sbBRelationship.Append("ALTER TABLE [dbo].[" + junctionTableName + "]");
                sbBRelationship.Append(" ADD CONSTRAINT [FK_" + junctionTableName + "_" + bTableName + "]");
                sbBRelationship.Append(" FOREIGN KEY (" + b_foreign_key_ColumnName + ")");
                sbBRelationship.Append(" REFERENCES [dbo].[" + bTableName + "] (id)");
                sbBRelationship.Append(" ON UPDATE CASCADE");
                sbBRelationship.Append(" ON DELETE CASCADE");

                this.ExecuteNonQuery(sbBRelationship.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteManyToManyRelationship(StructuredContent_ContentType aContentType, StructuredContent_ContentType bContentType)
        {
            try
            {
                // delete the junction table
                var aTableName = TablePrefix + aContentType.TableName;
                var bTableName = TablePrefix + bContentType.TableName;
                var junctionTableName = aTableName + "X" + bTableName;

                var aForeignKeyColumnName = aContentType.Singular + "Id";
                var bForeignKeyColumnName = bContentType.Singular + "Id";

                // TSQL to remove relationships
                var sbARelationship = new StringBuilder();
                sbARelationship.Append("ALTER TABLE [dbo].[" + junctionTableName + "]");
                sbARelationship.Append(" DROP CONSTRAINT [FK_" + junctionTableName + "_" + aTableName + "]");

                this.ExecuteNonQuery(sbARelationship.ToString());

                var sbBRelationship = new StringBuilder();
                sbBRelationship.Append("ALTER TABLE [dbo].[" + junctionTableName + "]");
                sbBRelationship.Append(" DROP CONSTRAINT [FK_" + junctionTableName + "_" + bTableName + "]");

                this.ExecuteNonQuery(sbBRelationship.ToString());

                // TSQL for CREATE TABLE
                var sbCreateJunctionTable = new StringBuilder();
                sbCreateJunctionTable.Append("DROP TABLE [dbo].[" + junctionTableName + "] ");

                this.ExecuteNonQuery(sbCreateJunctionTable.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IDictionary<string, object>> SelectDynamicList(StructuredContent_ContentType contentType, string whereClause)
        {
            var tableName = TablePrefix + contentType.TableName;

            // TSQL to get item
            var sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM [" + tableName + "] ");

            if (!string.IsNullOrEmpty(whereClause))
            {
                sb.Append("WHERE " + whereClause);
            }

            using (var sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        foreach (IDataRecord record in reader as IEnumerable)
                        {
                            var item = new Dictionary<string, object>();
                            foreach (var name in names)
                            {
                                item.Add(name, record[name]);
                            }

                            yield return item;
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, object> SelectDynamicItem(StructuredContent_ContentType contentType, int id)
        {
            try
            {
                // related content types
                var related_content_types = new List<StructuredContent_ContentType>();

                var tableName = TablePrefix + contentType.TableName;

                // TSQL to get item
                var sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append(" FROM [" + tableName + "]");
                sb.Append(" WHERE [Id]=" + id.ToString());

                using (var sqlConnection = new SqlConnection(this.connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                    {
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                            var dt = new DataTable();
                            dt.Load(reader);

                            if (dt.Rows.Count > 0)
                            {
                                var dataRow = dt.Rows[0];

                                var item = new Dictionary<string, object>();
                                foreach (var name in names)
                                {
                                    item.Add(name, dataRow[name]);
                                }

                                return item;
                            }
                            else
                            {
                                return default(Dictionary<string, object>);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public int InsertContentItem(StructuredContent_ContentType contentType, dynamic contentItem)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // TSQL FOR INSERT
                var sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO [dbo].[" + tableName + "]");
                sbInsert.Append(" ([Name], [Status], [DateCreated], [DateModified], [DatePublished]");

                foreach (var content_field in contentType.StructuredContent_ContentFields)
                {
                    if (content_field.IsSystem == false)
                    {
                        sbInsert.Append(", [" + content_field.ColumnName + "]");
                    }
                }

                sbInsert.Append(")");

                sbInsert.Append(" VALUES ");

                sbInsert.Append("(");
                sbInsert.Append("'" + contentItem.name + "'");
                sbInsert.Append(", '" + contentItem.status + "'");
                sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");
                sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");

                if (contentItem.status == "Published")
                {
                    sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");
                }
                else
                {
                    sbInsert.Append(", null");
                }

                foreach (var content_field in contentType.StructuredContent_ContentFields)
                {
                    if (content_field.IsSystem == false)
                    {
                        sbInsert.Append(", ");

                        object value = contentItem[content_field.ColumnName];

                        if (value == null)
                        {
                            sbInsert.Append("null");
                        }
                        else
                        {
                            switch (content_field.DataType)
                            {
                                case (int)Enums.DataTypes.Bit:
                                    if (bool.Parse(value.ToString()))
                                    {
                                        sbInsert.Append("1");
                                    }
                                    else
                                    {
                                        sbInsert.Append("0");
                                    }

                                    break;

                                case (int)Enums.DataTypes.Nvarchar:
                                case (int)Enums.DataTypes.Datetime:
                                    sbInsert.Append("'" + value + "'");
                                    break;

                                default:
                                    sbInsert.Append(value);
                                    break;
                            }
                        }
                    }
                }

                sbInsert.Append(")");

                this.ExecuteNonQuery(sbInsert.ToString());

                var sbID = new StringBuilder();
                sbID.Append("SELECT MAX([id])");
                sbID.Append(" FROM [" + tableName + "]");

                var id = (int)this.ExecuteScalar(sbID.ToString());

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void UpdateContentItem(StructuredContent_ContentType contentType, dynamic contentItem)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // T-SQL TO UPDATE CONTENT ITEM
                var sbUpdateContentItem = new StringBuilder();
                sbUpdateContentItem.Append("UPDATE [dbo].[" + tableName + "]");
                sbUpdateContentItem.Append(" SET");

                sbUpdateContentItem.Append(" [Name]='" + contentItem.name + "'");
                sbUpdateContentItem.Append(", [Status]='" + contentItem.status + "'");
                sbUpdateContentItem.Append(", [DateModified]='" + DateTime.Now.ToShortDateString() + "'");

                if (contentItem.status == "Published")
                {
                    sbUpdateContentItem.Append(", [DatePublished]='" + DateTime.Now.ToShortDateString() + "'");
                }
                else
                {
                    sbUpdateContentItem.Append(", [DatePublished]=null");
                }

                // iterate over the content fields for the content item to build the T-SQL
                foreach (var contentField in contentType.StructuredContent_ContentFields.Where(i => i.ContentFieldTypeId.HasValue))
                {
                    sbUpdateContentItem.Append(", [" + contentField.ColumnName + "]");
                    sbUpdateContentItem.Append("=");

                    dynamic fieldValue = contentItem[char.ToLowerInvariant(contentField.ColumnName[0]) + contentField.ColumnName.Substring(1)];

                    if (fieldValue.Value == null || string.IsNullOrEmpty(fieldValue.Value.ToString()))
                    {
                        sbUpdateContentItem.Append("null");
                    }
                    else
                    {
                        switch (contentField.DataType)
                        {
                            case (int)Enums.DataTypes.Bit:
                                if (bool.Parse(fieldValue.ToString()))
                                {
                                    sbUpdateContentItem.Append("1");
                                }
                                else
                                {
                                    sbUpdateContentItem.Append("0");
                                }

                                break;

                            case (int)Enums.DataTypes.Nvarchar:
                            case (int)Enums.DataTypes.Datetime:
                                sbUpdateContentItem.Append("'" + fieldValue + "'");
                                break;

                            default:
                                sbUpdateContentItem.Append(fieldValue);
                                break;
                        }
                    }
                }

                sbUpdateContentItem.Append(" WHERE [id]=" + contentItem.id.ToString());
                this.ExecuteNonQuery(sbUpdateContentItem.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteContentItem(StructuredContent_ContentType contentType, int id)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // TSQL FOR DELETE
                var sb = new StringBuilder();
                sb.Append("DELETE FROM [dbo].[" + tableName + "]");
                sb.Append(" WHERE [id]=" + id.ToString());
                this.ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primaryContentType, int primaryContentItemId)
        {
            var tableName = relationship.TableName;
            var primaryForeignKeyColumnName = primaryContentType.Singular + "Id";

            // delete all the related cross reference table records
            var sbDelete = new StringBuilder();
            sbDelete.Append("DELETE FROM [" + tableName + "]");
            sbDelete.Append(" WHERE [" + primaryForeignKeyColumnName + "] = " + primaryContentItemId.ToString());
            this.ExecuteNonQuery(sbDelete.ToString());
        }

        /// <inheritdoc/>
        public void SaveManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId, int relatedContentItemId)
        {
            var tableName = relationship.TableName;
            var primaryForeignKeyColumnName = primaryContentType.Singular + "Id";
            var relatedForeignKeyColumnName = relatedContentType.Singular + "Id";

            // insert the related cross reference table record
            var sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO [" + tableName + "]");
            sbInsert.Append(" (" + primaryForeignKeyColumnName + ", " + relatedForeignKeyColumnName + ") ");
            sbInsert.Append(" VALUES (" + primaryContentItemId.ToString() + ", " + relatedContentItemId.ToString() + ")");
            this.ExecuteNonQuery(sbInsert.ToString());
        }

        /// <inheritdoc/>
        public void DeleteOneToManyRelationship(StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId)
        {
            var relatedContentTableName = TablePrefix + relatedContentType.TableName;
            var foreignKeyColumnName = primaryContentType.Singular + "Id";

            // delete all the related cross reference table records
            var sbMany = new StringBuilder();
            sbMany.Append("UPDATE [" + relatedContentTableName + "]");
            sbMany.Append(" SET [" + foreignKeyColumnName + "] = NULL");
            sbMany.Append(" WHERE [" + foreignKeyColumnName + "] = " + primaryContentItemId.ToString());
            this.ExecuteNonQuery(sbMany.ToString());
        }

        /// <inheritdoc/>
        public void SaveOneToManyRelationship(StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId, int relatedContentItemId)
        {
            var related_content_TableName = TablePrefix + relatedContentType.TableName;
            var foreign_key_ColumnName = primaryContentType.Singular + "Id";

            // delete all the related cross reference table records
            var sbMany = new StringBuilder();
            sbMany.Append("UPDATE [" + related_content_TableName + "]");
            sbMany.Append(" SET [" + foreign_key_ColumnName + "] = " + primaryContentItemId.ToString());
            sbMany.Append(" WHERE [id] = " + relatedContentItemId.ToString());
            this.ExecuteNonQuery(sbMany.ToString());
        }

        /// <inheritdoc/>
        public IEnumerable<IDictionary<string, object>> GetRelatedItems(StructuredContent_Relationship relationship, StructuredContent_ContentType contentType, int id)
        {
            var sourceTableName = string.Empty;

            var aForeignKeyColumnName = relationship.StructuredContent_ContentType.Singular + "Id";
            var bForeignKeyColumnName = relationship.StructuredContent_ContentType1.Singular + "Id";

            var aTableName = TablePrefix + relationship.StructuredContent_ContentType.TableName;
            var bTableName = TablePrefix + relationship.StructuredContent_ContentType1.TableName;
            var junctionTableName = aTableName + "X" + bTableName;

            var sbWhere = new StringBuilder();

            if (relationship.Key == "o2m" && relationship.AContentTypeId == contentType.Id)
            {
                sourceTableName = bTableName;
                sbWhere.Append(aForeignKeyColumnName + "=" + id);
            }

            if (relationship.Key == "m2m" && relationship.AContentTypeId == contentType.Id)
            {
                sourceTableName = TablePrefix + relationship.StructuredContent_ContentType1.TableName;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + bForeignKeyColumnName + "]");
                sbWhere.Append(" FROM [" + junctionTableName + "]");
                sbWhere.Append(" WHERE [" + aForeignKeyColumnName + "] = " + id.ToString());
                sbWhere.Append(" )");
            }

            if (relationship.Key == "m2m" && relationship.BContentTypeId == contentType.Id)
            {
                sourceTableName = TablePrefix + relationship.StructuredContent_ContentType.TableName;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + aForeignKeyColumnName + "]");
                sbWhere.Append(" FROM [" + junctionTableName + "]");
                sbWhere.Append(" WHERE [" + bForeignKeyColumnName + "] = " + id.ToString());
                sbWhere.Append(" )");
            }

            // TSQL to get item
            var sb = new StringBuilder();
            sb.Append("SELECT *");
            sb.Append(" FROM [" + sourceTableName + "]");
            if (!string.IsNullOrEmpty(sbWhere.ToString()))
            {
                sb.Append(" WHERE " + sbWhere.ToString());
            }

            using (var sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        foreach (IDataRecord record in reader as IEnumerable)
                        {
                            var item = new Dictionary<string, object>();
                            foreach (var name in names)
                            {
                                item.Add(name, record[name]);
                            }

                            yield return item;
                        }
                    }
                }
            }
        }
    }
}
