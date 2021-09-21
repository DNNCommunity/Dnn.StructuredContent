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
                sb.Append("[id] [int] IDENTITY(1,1) NOT NULL, ");
                sb.Append("[name] [nvarchar](256) NOT NULL, ");
                sb.Append("[status] [varchar](10) NOT NULL, ");
                sb.Append("[DateCreated] [datetime] NOT NULL, ");
                sb.Append("[DateModified] [datetime] NOT NULL, ");
                sb.Append("[date_published] [datetime] NULL, ");
                sb.Append("CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED ([id] ASC)");
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
                // create the foreign key  on the child table

                // set up relationship
                var sb = new StringBuilder();
                sb.Append("ALTER TABLE [dbo].[" + TablePrefix + manyContentType.TableName + "]");
                sb.Append(" ADD CONSTRAINT [FK_" + TablePrefix + oneContentType.TableName + "_" + TablePrefix + manyContentType.TableName + "]");
                sb.Append(" FOREIGN KEY (" + oneContentType.Singular + "_id" + ")");
                sb.Append(" REFERENCES [dbo].[" + TablePrefix + oneContentType.TableName + "] (id)");
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
        public void DeleteOneToManyRelationship(StructuredContent_ContentType one_content_type, StructuredContent_ContentType many_content_type)
        {
            try
            {
                var foreign_key_ColumnName = one_content_type.Singular.ToLower() + "_id";

                // drop relationship
                var sbOneToMany = new StringBuilder();
                sbOneToMany.Append("ALTER TABLE [dbo].[" + TablePrefix + many_content_type.TableName + "]");
                sbOneToMany.Append(" DROP CONSTRAINT [FK_" + TablePrefix + one_content_type.TableName + "_" + TablePrefix + many_content_type.TableName + "]");

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

                var a_foreign_key_ColumnName = aContentType.Singular.ToLower() + "_id";
                var b_foreign_key_ColumnName = bContentType.Singular.ToLower() + "_id";

                // TSQL for CREATE TABLE
                var sbCreateJunctionTable = new StringBuilder();
                sbCreateJunctionTable.Append("CREATE TABLE [dbo].[" + junctionTableName + "] ");
                sbCreateJunctionTable.Append("(");
                sbCreateJunctionTable.Append("[id] [int] IDENTITY(1,1) NOT NULL, ");
                sbCreateJunctionTable.Append("[" + a_foreign_key_ColumnName + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("[" + b_foreign_key_ColumnName + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("CONSTRAINT [PK_" + junctionTableName + "] PRIMARY KEY CLUSTERED ([id] ASC)");
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

                var aForeignKeyColumnName = aContentType.Singular.ToLower() + "_id";
                var bForeignKeyColumnName = bContentType.Singular.ToLower() + "_id";

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
        public IEnumerable<IDictionary<string, object>> SelectDynamicList(StructuredContent_ContentType content_type, string where_clause)
        {
            var tableName = TablePrefix + content_type.TableName;

            // TSQL to get item
            var sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM [" + tableName + "] ");

            if (!string.IsNullOrEmpty(where_clause))
            {
                sb.Append("WHERE " + where_clause);
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
        public IDictionary<string, object> SelectDynamicItem(StructuredContent_ContentType content_type, int id)
        {
            try
            {
                // related content types
                var related_content_types = new List<StructuredContent_ContentType>();

                var tableName = TablePrefix + content_type.TableName;

                // TSQL to get item
                var sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append(" FROM [" + tableName + "]");
                sb.Append(" WHERE [id]=" + id.ToString());

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
        public int InsertContentItem(StructuredContent_ContentType content_type, dynamic content_item)
        {
            try
            {
                var tableName = TablePrefix + content_type.TableName;

                // TSQL FOR INSERT
                var sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO [dbo].[" + tableName + "]");
                sbInsert.Append(" ([name], [status], [DateCreated], [DateModified], [date_published]");

                foreach (var content_field in content_type.StructuredContent_ContentFields)
                {
                    if (content_field.IsSystem == false)
                    {
                        sbInsert.Append(", [" + content_field.ColumnName + "]");
                    }
                }

                sbInsert.Append(")");

                sbInsert.Append(" VALUES ");

                sbInsert.Append("(");
                sbInsert.Append("'" + content_item.name + "'");
                sbInsert.Append(", '" + content_item.status + "'");
                sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");
                sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");

                if (content_item.status == "Published")
                {
                    sbInsert.Append(", '" + DateTime.Now.ToShortDateString() + "'");
                }
                else
                {
                    sbInsert.Append(", null");
                }

                foreach (var content_field in content_type.StructuredContent_ContentFields)
                {
                    if (content_field.IsSystem == false)
                    {
                        sbInsert.Append(", ");

                        object value = content_item[content_field.ColumnName];

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
        public void UpdateContentItem(StructuredContent_ContentType contentType, dynamic contentIitem)
        {
            try
            {
                var tableName = TablePrefix + contentType.TableName;

                // T-SQL TO UPDATE CONTENT ITEM
                var sbUpdateContentItem = new StringBuilder();
                sbUpdateContentItem.Append("UPDATE [dbo].[" + tableName + "]");
                sbUpdateContentItem.Append(" SET");

                sbUpdateContentItem.Append(" [name]='" + contentIitem.name + "'");
                sbUpdateContentItem.Append(", [status]='" + contentIitem.status + "'");
                sbUpdateContentItem.Append(", [DateModified]='" + DateTime.Now.ToShortDateString() + "'");

                if (contentIitem.status == "Published")
                {
                    sbUpdateContentItem.Append(", [date_published]='" + DateTime.Now.ToShortDateString() + "'");
                }
                else
                {
                    sbUpdateContentItem.Append(", [date_published]=null");
                }

                // iterate over the content fields for the content item to build the T-SQL
                foreach (var contentField in contentType.StructuredContent_ContentFields)
                {
                    if (contentField.IsSystem == false)
                    {
                        sbUpdateContentItem.Append(", [" + contentField.ColumnName + "]");
                        sbUpdateContentItem.Append("=");

                        var fieldValue = contentIitem[contentField.ColumnName];

                        if (fieldValue == default(dynamic))
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
                }

                sbUpdateContentItem.Append(" WHERE [id]=" + contentIitem.id.ToString());
                this.ExecuteNonQuery(sbUpdateContentItem.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public void DeleteContentItem(StructuredContent_ContentType content_type, int id)
        {
            try
            {
                var tableName = TablePrefix + content_type.TableName;

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
        public void DeleteManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, int primary_content_ItemId)
        {
            var tableName = relationship.TableName;
            var primary_foreign_key_ColumnName = primary_content_type.Singular.ToLower() + "_id";

            // delete all the related cross reference table records
            var sbDelete = new StringBuilder();
            sbDelete.Append("DELETE FROM [" + tableName + "]");
            sbDelete.Append(" WHERE [" + primary_foreign_key_ColumnName + "] = " + primary_content_ItemId.ToString());
            this.ExecuteNonQuery(sbDelete.ToString());
        }

        /// <inheritdoc/>
        public void SaveManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_ItemId, int related_content_ItemId)
        {
            var tableName = relationship.TableName;
            var primaryForeignKeyColumnName = primary_content_type.Singular.ToLower() + "_id";
            var relatedForeignKeyColumnName = related_content_type.Singular.ToLower() + "_id";

            // insert the related cross reference table record
            var sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO [" + tableName + "]");
            sbInsert.Append(" (" + primaryForeignKeyColumnName + ", " + relatedForeignKeyColumnName + ") ");
            sbInsert.Append(" VALUES (" + primary_content_ItemId.ToString() + ", " + related_content_ItemId.ToString() + ")");
            this.ExecuteNonQuery(sbInsert.ToString());
        }

        /// <inheritdoc/>
        public void DeleteOneToManyRelationship(StructuredContent_ContentType primaryContentType, StructuredContent_ContentType relatedContentType, int primaryContentItemId)
        {
            var relatedContentTableName = TablePrefix + relatedContentType.TableName;
            var foreignKeyColumnName = primaryContentType.Singular.ToLower() + "_id";

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
            var foreign_key_ColumnName = primaryContentType.Singular.ToLower() + "_id";

            // delete all the related cross reference table records
            var sbMany = new StringBuilder();
            sbMany.Append("UPDATE [" + related_content_TableName + "]");
            sbMany.Append(" SET [" + foreign_key_ColumnName + "] = " + primaryContentItemId.ToString());
            sbMany.Append(" WHERE [id] = " + relatedContentItemId.ToString());
            this.ExecuteNonQuery(sbMany.ToString());
        }

        /// <inheritdoc/>
        public IEnumerable<IDictionary<string, object>> GetRelatedItems(StructuredContent_Relationship relationship, StructuredContent_ContentType content_type, int id)
        {
            var source_TableName = string.Empty;

            var a_foreign_key_ColumnName = relationship.StructuredContent_ContentType.Singular.ToLower() + "_id";
            var b_foreign_key_ColumnName = relationship.StructuredContent_ContentType1.Singular.ToLower() + "_id";

            var a_TableName = TablePrefix + relationship.StructuredContent_ContentType.TableName;
            var b_TableName = TablePrefix + relationship.StructuredContent_ContentType1.TableName;
            var junction_TableName = a_TableName + "X" + b_TableName;

            var sbWhere = new StringBuilder();

            if (relationship.Key == "o2m" && relationship.AContentTypeId == content_type.Id)
            {
                source_TableName = b_TableName;
                sbWhere.Append(a_foreign_key_ColumnName + "=" + id);
            }

            if (relationship.Key == "m2m" && relationship.AContentTypeId == content_type.Id)
            {
                source_TableName = TablePrefix + relationship.StructuredContent_ContentType1.TableName;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + b_foreign_key_ColumnName + "]");
                sbWhere.Append(" FROM [" + junction_TableName + "]");
                sbWhere.Append(" WHERE [" + a_foreign_key_ColumnName + "] = " + id.ToString());
                sbWhere.Append(" )");
            }

            if (relationship.Key == "m2m" && relationship.BContentTypeId == content_type.Id)
            {
                source_TableName = TablePrefix + relationship.StructuredContent_ContentType.TableName;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + a_foreign_key_ColumnName + "]");
                sbWhere.Append(" FROM [" + junction_TableName + "]");
                sbWhere.Append(" WHERE [" + b_foreign_key_ColumnName + "] = " + id.ToString());
                sbWhere.Append(" )");
            }

            // TSQL to get item
            var sb = new StringBuilder();
            sb.Append("SELECT *");
            sb.Append(" FROM [" + source_TableName + "]");
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
