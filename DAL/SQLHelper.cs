// TODO:  Need to add SQL inject security

using DotNetNuke.Web.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructuredContent.DAL;

namespace StructuredContent
{

    public class SQLHelper
    {
        private const string table_prefix = "zz_";

        private string connectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ToString();

        public void ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
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
        public object ExecuteScalar(string query)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        object ret = sqlCommand.ExecuteScalar();
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateContentTable(StructuredContent_ContentType content_type)
        {
            try
            {
                string table_name = table_prefix + content_type.table_name;

                // TSQL for CREATE TABLE
                StringBuilder sb = new StringBuilder();
                sb.Append("CREATE TABLE [dbo].[" + table_name + "] ");
                sb.Append("(");
                sb.Append("[id] [int] IDENTITY(1,1) NOT NULL, ");
                sb.Append("[name] [nvarchar](256) NOT NULL, ");
                sb.Append("[status] [varchar](10) NOT NULL, ");
                sb.Append("[date_created] [datetime] NOT NULL, ");
                sb.Append("[date_modified] [datetime] NOT NULL, ");
                sb.Append("[date_published] [datetime] NULL, ");
                sb.Append("CONSTRAINT [PK_" + table_name + "] PRIMARY KEY CLUSTERED ([id] ASC)");
                sb.Append(")");

                ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContentTable(StructuredContent_ContentType content_type)
        {
            try
            {
                string table_name = table_prefix + content_type.table_name;

                // TSQL for DELETE TABLE
                StringBuilder sb = new StringBuilder();
                sb.Append("DROP TABLE [dbo].[" + table_name + "] ");

                ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddColumn(StructuredContent_ContentField content_field)
        {
            try
            {
                string table_name = table_prefix + content_field.StructuredContent_ContentType.table_name;
                string column_name = content_field.column_name;
                string data_type = content_field.data_type_name;
                string data_length = content_field.data_length;
                bool allow_null = content_field.allow_null;
                string default_value = content_field.default_value;

                // TSQL for ADD COLUMN 
                StringBuilder sbAddColumn = new StringBuilder();
                sbAddColumn.Append("ALTER TABLE [dbo].[" + table_name + "]");
                sbAddColumn.Append(" ADD [" + column_name + "] [" + data_type.ToLower() + "]" + (!string.IsNullOrEmpty(data_length) ? "(" + data_length + ")" : "") + (allow_null ? "" : " NOT") + " NULL");

                if (!string.IsNullOrEmpty(default_value))
                {
                    string constraint_name = "DF_" + table_name + "_" + column_name;

                    sbAddColumn.Append(" CONSTRAINT [" + constraint_name + "] DEFAULT ");
                    if (data_type == "nvarchar")
                    {
                        sbAddColumn.Append(" N'" + default_value + "'");
                    }
                    else
                    {
                        sbAddColumn.Append(" " + default_value);
                    }
                }

                ExecuteNonQuery(sbAddColumn.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteColumn(StructuredContent_ContentField content_field)
        {
            try
            {
                string table_name = table_prefix + content_field.StructuredContent_ContentType.table_name;
                string column_name = content_field.column_name;


                // DROP default_value constraints?
                if (!string.IsNullOrEmpty(content_field.default_value))
                {
                    string constraint_name = "DF_" + table_name + "_" + column_name;
                    StringBuilder sbDropConstraint = new StringBuilder();
                    sbDropConstraint.Append("ALTER TABLE [dbo].[" + table_name + "]");
                    sbDropConstraint.Append(" DROP CONSTRAINT [" + constraint_name + "]");

                    ExecuteNonQuery(sbDropConstraint.ToString());
                }

                // TSQL for DROP COLUMN 
                StringBuilder sbDropColumn = new StringBuilder();
                sbDropColumn.Append("ALTER TABLE [dbo].[" + table_name + "] ");
                sbDropColumn.Append("DROP COLUMN [" + column_name + "]");

                ExecuteNonQuery(sbDropColumn.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateOneToManyRelationship(StructuredContent_ContentType one_content_type, StructuredContent_ContentType many_content_type)
        {
            try
            {
                // both tables need to exist
                // create the foreign key  on the child table

                // set up relationship
                StringBuilder sb = new StringBuilder();
                sb.Append("ALTER TABLE [dbo].[" + table_prefix + many_content_type.table_name + "]");
                sb.Append(" ADD CONSTRAINT [FK_" + table_prefix + one_content_type.table_name + "_" + table_prefix + many_content_type.table_name + "]");
                sb.Append(" FOREIGN KEY (" + one_content_type.singular + "_id" + ")");
                sb.Append(" REFERENCES [dbo].[" + table_prefix + one_content_type.table_name + "] (id)");
                sb.Append(" ON UPDATE NO ACTION");
                sb.Append(" ON DELETE NO ACTION");

                ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteOneToManyRelationship(StructuredContent_ContentType one_content_type, StructuredContent_ContentType many_content_type)
        {
            try
            {
                string foreign_key_column_name = one_content_type.singular.ToLower() + "_id";

                // drop relationship
                StringBuilder sbOneToMany = new StringBuilder();
                sbOneToMany.Append("ALTER TABLE [dbo].[" + table_prefix + many_content_type.table_name + "]");
                sbOneToMany.Append(" DROP CONSTRAINT [FK_" + table_prefix + one_content_type.table_name + "_" + table_prefix + many_content_type.table_name + "]");

                ExecuteNonQuery(sbOneToMany.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateManyToManyRelationship(StructuredContent_ContentType a_content_type, StructuredContent_ContentType b_content_type)
        {
            try
            {
                // create the junction table

                string a_table_name = table_prefix + a_content_type.table_name;
                string b_table_name = table_prefix + b_content_type.table_name;
                string junction_table_name = a_table_name + "X" + b_table_name;

                string a_foreign_key_column_name = a_content_type.singular.ToLower() + "_id";
                string b_foreign_key_column_name = b_content_type.singular.ToLower() + "_id";


                // TSQL for CREATE TABLE
                StringBuilder sbCreateJunctionTable = new StringBuilder();
                sbCreateJunctionTable.Append("CREATE TABLE [dbo].[" + junction_table_name + "] ");
                sbCreateJunctionTable.Append("(");
                sbCreateJunctionTable.Append("[id] [int] IDENTITY(1,1) NOT NULL, ");
                sbCreateJunctionTable.Append("[" + a_foreign_key_column_name + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("[" + b_foreign_key_column_name + "] [int] NOT NULL, ");
                sbCreateJunctionTable.Append("CONSTRAINT [PK_" + junction_table_name + "] PRIMARY KEY CLUSTERED ([id] ASC)");
                sbCreateJunctionTable.Append(")");

                ExecuteNonQuery(sbCreateJunctionTable.ToString());

                // TSQL to set up relationships
                StringBuilder sbARelationship = new StringBuilder();
                sbARelationship.Append("ALTER TABLE [dbo].[" + junction_table_name + "]");
                sbARelationship.Append(" ADD CONSTRAINT [FK_" + junction_table_name + "_" + a_table_name + "]");
                sbARelationship.Append(" FOREIGN KEY (" + a_foreign_key_column_name + ")");
                sbARelationship.Append(" REFERENCES [dbo].[" + a_table_name + "] (id)");
                sbARelationship.Append(" ON UPDATE CASCADE");
                sbARelationship.Append(" ON DELETE CASCADE");

                ExecuteNonQuery(sbARelationship.ToString());

                StringBuilder sbBRelationship = new StringBuilder();
                sbBRelationship.Append("ALTER TABLE [dbo].[" + junction_table_name + "]");
                sbBRelationship.Append(" ADD CONSTRAINT [FK_" + junction_table_name + "_" + b_table_name + "]");
                sbBRelationship.Append(" FOREIGN KEY (" + b_foreign_key_column_name + ")");
                sbBRelationship.Append(" REFERENCES [dbo].[" + b_table_name + "] (id)");
                sbBRelationship.Append(" ON UPDATE CASCADE");
                sbBRelationship.Append(" ON DELETE CASCADE");

                ExecuteNonQuery(sbBRelationship.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteManyToManyRelationship(StructuredContent_ContentType a_content_type, StructuredContent_ContentType b_content_type)
        {
            try
            {
                // delete the junction table

                string a_table_name = table_prefix + a_content_type.table_name;
                string b_table_name = table_prefix + b_content_type.table_name;
                string junction_table_name = a_table_name + "X" + b_table_name;

                string a_foreign_key_column_name = a_content_type.singular.ToLower() + "_id";
                string b_foreign_key_column_name = b_content_type.singular.ToLower() + "_id";

                // TSQL to remove relationships
                StringBuilder sbARelationship = new StringBuilder();
                sbARelationship.Append("ALTER TABLE [dbo].[" + junction_table_name + "]");
                sbARelationship.Append(" DROP CONSTRAINT [FK_" + junction_table_name + "_" + a_table_name + "]");

                ExecuteNonQuery(sbARelationship.ToString());

                StringBuilder sbBRelationship = new StringBuilder();
                sbBRelationship.Append("ALTER TABLE [dbo].[" + junction_table_name + "]");
                sbBRelationship.Append(" DROP CONSTRAINT [FK_" + junction_table_name + "_" + b_table_name + "]");

                ExecuteNonQuery(sbBRelationship.ToString());

                // TSQL for CREATE TABLE
                StringBuilder sbCreateJunctionTable = new StringBuilder();
                sbCreateJunctionTable.Append("DROP TABLE [dbo].[" + junction_table_name + "] ");

                ExecuteNonQuery(sbCreateJunctionTable.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<IDictionary<string, object>> SelectDynamicList(StructuredContent_ContentType content_type, string where_clause)
        {
            var table_name = table_prefix + content_type.table_name;

            // TSQL to get item
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM [" + table_name + "] ");

            if (!string.IsNullOrEmpty(where_clause))
            {
                sb.Append("WHERE " + where_clause);
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        foreach (IDataRecord record in reader as IEnumerable)
                        {
                            var item = new Dictionary<string, object>();
                            foreach (var name in names)
                                item.Add(name, record[name]);

                            yield return item;
                        }
                    }
                }
            }
        }

        public IDictionary<string, object> SelectDynamicItem(StructuredContent_ContentType content_type, int id)
        {
            try
            {
                // related content types
                List<StructuredContent_ContentType> related_content_types = new List<StructuredContent_ContentType>();


                var table_name = table_prefix + content_type.table_name;

                // TSQL to get item
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append(" FROM [" + table_name + "]");
                sb.Append(" WHERE [id]=" + id.ToString());

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                    {
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            if (dt.Rows.Count > 0)
                            {
                                DataRow dataRow = dt.Rows[0];

                                var item = new Dictionary<string, object>();
                                foreach (var name in names)
                                    item.Add(name, dataRow[name]);

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


        public int InsertContentItem(StructuredContent_ContentType content_type, dynamic content_item)
        {
            try
            {
                string table_name = table_prefix + content_type.table_name;

                // TSQL FOR INSERT

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO [dbo].[" + table_name + "]");
                sbInsert.Append(" ([name], [status], [date_created], [date_modified], [date_published]");

                foreach (StructuredContent_ContentField content_field in content_type.StructuredContent_ContentFields)
                {
                    if (content_field.is_system == false)
                    {
                        sbInsert.Append(", [" + content_field.column_name + "]");
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


                foreach (StructuredContent_ContentField content_field in content_type.StructuredContent_ContentFields)
                {
                    if (content_field.is_system == false)
                    {
                        sbInsert.Append(", ");

                        object value = content_item[content_field.column_name];

                        if (value == null)
                        {
                            sbInsert.Append("null");
                        }
                        else
                        {
                            switch (content_field.data_type)
                            {
                                case (int)Enums.DataTypes.bit:
                                    if (bool.Parse(value.ToString()))
                                    {
                                        sbInsert.Append("1");
                                    }
                                    else
                                    {
                                        sbInsert.Append("0");
                                    }

                                    break;

                                case (int)Enums.DataTypes.nvarchar:
                                case (int)Enums.DataTypes.datetime:
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

                ExecuteNonQuery(sbInsert.ToString());


                StringBuilder sbID = new StringBuilder();
                sbID.Append("SELECT MAX([id])");
                sbID.Append(" FROM [" + table_name + "]");

                int id = (int)ExecuteScalar(sbID.ToString());

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateContentItem(StructuredContent_ContentType content_type, dynamic content_item)
        {
            try
            {
                string table_name = table_prefix + content_type.table_name;

                // T-SQL TO UPDATE CONTENT ITEM
                StringBuilder sbUpdateContentItem = new StringBuilder();
                sbUpdateContentItem.Append("UPDATE [dbo].[" + table_name + "]");
                sbUpdateContentItem.Append(" SET");

                sbUpdateContentItem.Append(" [name]='" + content_item.name + "'");
                sbUpdateContentItem.Append(", [status]='" + content_item.status + "'");
                sbUpdateContentItem.Append(", [date_modified]='" + DateTime.Now.ToShortDateString() + "'");

                if (content_item.status == "Published")
                {
                    sbUpdateContentItem.Append(", [date_published]='" + DateTime.Now.ToShortDateString() + "'");
                }
                else
                {
                    sbUpdateContentItem.Append(", [date_published]=null");
                }

                // iterate over the content fields for the content item to build the T-SQL
                foreach (StructuredContent_ContentField content_field in content_type.StructuredContent_ContentFields)
                {
                    if (content_field.is_system == false)
                    {
                        sbUpdateContentItem.Append(", [" + content_field.column_name + "]");

                        sbUpdateContentItem.Append("=");

                        object value = content_item[content_field.column_name];

                        if (value == null)
                        {
                            sbUpdateContentItem.Append("null");
                        }
                        else
                        {
                            switch (content_field.data_type)
                            {
                                case (int)Enums.DataTypes.bit:
                                    if (bool.Parse(value.ToString()))
                                    {
                                        sbUpdateContentItem.Append("1");
                                    }
                                    else
                                    {
                                        sbUpdateContentItem.Append("0");
                                    }

                                    break;

                                case (int)Enums.DataTypes.nvarchar:
                                case (int)Enums.DataTypes.datetime:
                                    sbUpdateContentItem.Append("'" + value + "'");
                                    break;

                                default:
                                    sbUpdateContentItem.Append(value);
                                    break;
                            }
                        }
                    }
                }
                sbUpdateContentItem.Append(" WHERE [id]=" + content_item.id.ToString());
                ExecuteNonQuery(sbUpdateContentItem.ToString());



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContentItem(StructuredContent_ContentType content_type, int id)
        {
            try
            {
                string table_name = table_prefix + content_type.table_name;

                // TSQL FOR DELETE

                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM [dbo].[" + table_name + "]");
                sb.Append(" WHERE [id]=" + id.ToString());
                ExecuteNonQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, int primary_content_item_id)
        {
            string table_name = relationship.table_name;
            string primary_foreign_key_column_name = primary_content_type.singular.ToLower() + "_id";

            // delete all the related cross reference table records
            StringBuilder sbDelete = new StringBuilder();
            sbDelete.Append("DELETE FROM [" + table_name + "]");
            sbDelete.Append(" WHERE [" + primary_foreign_key_column_name + "] = " + primary_content_item_id.ToString());
            ExecuteNonQuery(sbDelete.ToString());
        }

        public void SaveManyToManyRelationship(StructuredContent_Relationship relationship, StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id, int related_content_item_id)
        {
            string table_name = relationship.table_name;
            string primary_foreign_key_column_name = primary_content_type.singular.ToLower() + "_id";
            string related_foreign_key_column_name = related_content_type.singular.ToLower() + "_id";

            // insert the related cross reference table record
            StringBuilder sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO [" + table_name + "]");
            sbInsert.Append(" (" + primary_foreign_key_column_name + ", " + related_foreign_key_column_name + ") ");
            sbInsert.Append(" VALUES (" + primary_content_item_id.ToString() + ", " + related_content_item_id.ToString() + ")");
            ExecuteNonQuery(sbInsert.ToString());
        }

        public void DeleteOneToManyRelationship(StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id)
        {
            string related_content_table_name = table_prefix + related_content_type.table_name;
            string foreign_key_column_name = primary_content_type.singular.ToLower() + "_id";

            // delete all the related cross reference table records            
            StringBuilder sbMany = new StringBuilder();
            sbMany.Append("UPDATE [" + related_content_table_name + "]");
            sbMany.Append(" SET [" + foreign_key_column_name + "] = NULL");
            sbMany.Append(" WHERE [" + foreign_key_column_name + "] = " + primary_content_item_id.ToString());
            ExecuteNonQuery(sbMany.ToString());
        }

        public void SaveOneToManyRelationship(StructuredContent_ContentType primary_content_type, StructuredContent_ContentType related_content_type, int primary_content_item_id, int related_content_item_id)
        {
            string related_content_table_name = table_prefix + related_content_type.table_name;
            string foreign_key_column_name = primary_content_type.singular.ToLower() + "_id";

            // delete all the related cross reference table records            
            StringBuilder sbMany = new StringBuilder();
            sbMany.Append("UPDATE [" + related_content_table_name + "]");
            sbMany.Append(" SET [" + foreign_key_column_name + "] = " + primary_content_item_id.ToString());
            sbMany.Append(" WHERE [id] = " + related_content_item_id.ToString());
            ExecuteNonQuery(sbMany.ToString());
        }

        // this returns all the related objects for a given object
        public IEnumerable<IDictionary<string, object>> GetRelatedItems(StructuredContent_Relationship relationship, StructuredContent_ContentType content_type, int id)
        {
            string source_table_name = string.Empty;

            string a_foreign_key_column_name = relationship.StructuredContent_ContentType.singular.ToLower() + "_id";
            string b_foreign_key_column_name = relationship.StructuredContent_ContentType1.singular.ToLower() + "_id";

            string a_table_name = table_prefix + relationship.StructuredContent_ContentType.table_name;
            string b_table_name = table_prefix + relationship.StructuredContent_ContentType1.table_name;
            string junction_table_name = a_table_name + "X" + b_table_name;

            StringBuilder sbWhere = new StringBuilder();


            if (relationship.key == "o2m" && relationship.a_content_type_id == content_type.id)
            {
                source_table_name = b_table_name;
                sbWhere.Append(a_foreign_key_column_name + "=" + id);
            }

            if (relationship.key == "m2m" && relationship.a_content_type_id == content_type.id)
            {
                source_table_name = table_prefix + relationship.StructuredContent_ContentType1.table_name;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + b_foreign_key_column_name + "]");
                sbWhere.Append(" FROM [" + junction_table_name + "]");
                sbWhere.Append(" WHERE [" + a_foreign_key_column_name + "] = " + id.ToString());
                sbWhere.Append(" )");
            }

            if (relationship.key == "m2m" && relationship.b_content_type_id == content_type.id)
            {
                source_table_name = table_prefix + relationship.StructuredContent_ContentType.table_name;
                sbWhere.Append("[id] IN");
                sbWhere.Append(" (");
                sbWhere.Append(" SELECT [" + a_foreign_key_column_name + "]");
                sbWhere.Append(" FROM [" + junction_table_name + "]");
                sbWhere.Append(" WHERE [" + b_foreign_key_column_name + "] = " + id.ToString());
                sbWhere.Append(" )");
            }


            // TSQL to get item
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *");
            sb.Append(" FROM [" + source_table_name + "]");
            if (!string.IsNullOrEmpty(sbWhere.ToString()))
            {
                sb.Append(" WHERE " + sbWhere.ToString());
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sb.ToString(), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        foreach (IDataRecord record in reader as IEnumerable)
                        {
                            var item = new Dictionary<string, object>();
                            foreach (var name in names)
                                item.Add(name, record[name]);

                            yield return item;
                        }
                    }
                }
            }

        }
    }
}

