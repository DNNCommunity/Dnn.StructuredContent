using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructuredContent.DAL;

namespace StructuredContent
{
    //[SupportedModules("StructuredContent")]
    public class relationshipController : DnnApiController
    {
        SQLHelper sqlHelper = new SQLHelper();
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(Nullable<int> content_type_id = null)
        {
            try
            {
                var query = dc.StructuredContent_Relationships.AsQueryable();

                // content_type_id
                if (content_type_id.HasValue)
                {
                    query = query.Where(i => i.a_content_type_id == content_type_id.GetValueOrDefault() || i.b_content_type_id == content_type_id.GetValueOrDefault());
                }

                List<RelationshipDTO> dtos = new List<RelationshipDTO>();

                foreach (StructuredContent_Relationship item in query)
                {
                    RelationshipDTO dto = item.ToDto();
                    dtos.Add(dto);
                }

                return Request.CreateResponse(HttpStatusCode.OK, dtos);

            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                StructuredContent_Relationship item = dc.StructuredContent_Relationships.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(RelationshipDTO dto)
        { // this handles saving the relationship between content types - the abstract definition
            try
            {
                StructuredContent_ContentType a_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == dto.a_content_type_id).SingleOrDefault();
                StructuredContent_ContentType b_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == dto.b_content_type_id).SingleOrDefault();

                StructuredContent_Relationship o2m_relationship = new StructuredContent_Relationship();
                StructuredContent_Relationship m2o_relationship = new StructuredContent_Relationship();

                switch (dto.key)
                {
                    case "o2m": // a=one , b=many

                        // check for duplicate column name
                        string o2m_foreign_key_column_name = a_content_type.singular.ToLower() + "_id";
                        bool o2m_duplicate_check = dc.StructuredContent_ContentFields.Where(i => i.content_type_id == dto.b_content_type_id && (i.name == o2m_foreign_key_column_name || i.column_name == o2m_foreign_key_column_name)).Any();
                        if (o2m_duplicate_check)
                        {
                            return Request.CreateResponse(HttpStatusCode.Conflict);
                        }

                        // record foreign key field of the one table                        
                        StructuredContent_ContentField content_field = new StructuredContent_ContentField
                        {
                            StructuredContent_ContentType = b_content_type,
                            name = o2m_foreign_key_column_name,
                            column_name = o2m_foreign_key_column_name,
                            ordinal = 0,
                            data_type = (int)Enums.DataTypes.integer,
                            allow_null = true,
                            is_system = true
                        };
                        dc.StructuredContent_ContentFields.InsertOnSubmit(content_field);

                        //record relationship == 1->many
                        o2m_relationship = new StructuredContent_Relationship
                        {
                            key = "o2m",
                            a_content_type_id = dto.a_content_type_id,
                            b_content_type_id = dto.b_content_type_id,

                            a_required = dto.a_required,
                            a_max_limit = dto.a_max_limit,
                            a_min_limit = dto.a_min_limit,
                            a_help_text = dto.a_help_text,

                            b_required = dto.b_required,
                            b_max_limit = dto.b_max_limit,
                            b_min_limit = dto.b_min_limit,
                            b_help_text = dto.b_help_text,

                            a_layout_column = dto.a_layout_column,
                            a_layout_row = dto.a_layout_row,
                            b_layout_column = dto.b_layout_column,
                            b_layout_row = dto.b_layout_row

                        };
                        dc.StructuredContent_Relationships.InsertOnSubmit(o2m_relationship);

                        sqlHelper.AddColumn(content_field);
                        sqlHelper.CreateOneToManyRelationship(a_content_type, b_content_type);

                        dc.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, o2m_relationship.ToDto());

                    case "m2m": // many-to-many, need junction table

                        // set up the relationship in the db
                        sqlHelper.CreateManyToManyRelationship(a_content_type, b_content_type);

                        // record relationship == a->b
                        StructuredContent_Relationship m2m_relationship = new StructuredContent_Relationship
                        {
                            key = "m2m",
                            a_content_type_id = dto.a_content_type_id,
                            b_content_type_id = dto.b_content_type_id,

                            a_required = dto.a_required,
                            a_max_limit = dto.a_max_limit,
                            a_min_limit = dto.a_min_limit,
                            a_help_text = dto.a_help_text,

                            b_required = dto.b_required,
                            b_max_limit = dto.b_max_limit,
                            b_min_limit = dto.b_min_limit,
                            b_help_text = dto.b_help_text,

                            a_layout_column = dto.a_layout_column,
                            a_layout_row = dto.a_layout_row,
                            b_layout_column = dto.b_layout_column,
                            b_layout_row = dto.b_layout_row
                        };
                        dc.StructuredContent_Relationships.InsertOnSubmit(m2m_relationship);
                        dc.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, m2m_relationship.ToDto());

                    default:
                        return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(RelationshipDTO dto, int content_item_id, int content_type_id)
        { // this handles saving the relationships between content items - actual items
            try
            {
                StructuredContent_ContentType a_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == dto.a_content_type_id).SingleOrDefault();
                StructuredContent_ContentType b_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == dto.b_content_type_id).SingleOrDefault();

                //switch (dto.key)
                //{
                //    case "o2m": // a=one , b=many

                //        sqlHelper.SaveOneToManyRelationship(a_content_type, b_content_type, dto, content_item_id, content_type_id);

                //        break;

                //    case "m2m": // many-to-many, need junction table

                //        sqlHelper.SaveManyToManyRelationship(a_content_type, b_content_type, dto, content_item_id, content_type_id);

                //        break;
                //}
                dc.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(RelationshipDTO dto)
        {
            try
            {
                // the only things that can be updated are the min/max limits on the relaitonship
                StructuredContent_Relationship relationship = dc.StructuredContent_Relationships.Where(i => i.id == dto.id).SingleOrDefault();
                if (relationship == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                relationship.a_required = dto.a_required;
                relationship.a_max_limit = dto.a_max_limit;
                relationship.a_min_limit = dto.a_min_limit;
                relationship.a_help_text = dto.a_help_text;

                relationship.b_required = dto.b_required;
                relationship.b_max_limit = dto.b_max_limit;
                relationship.b_min_limit = dto.b_min_limit;
                relationship.b_help_text = dto.b_help_text;

                relationship.a_layout_row = dto.a_layout_row;
                relationship.a_layout_column = dto.a_layout_column;
                relationship.b_layout_row = dto.b_layout_row;
                relationship.b_layout_column = dto.b_layout_column;

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, relationship.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                StructuredContent_Relationship relationship = dc.StructuredContent_Relationships.Where(i => i.id == id).SingleOrDefault();
                if (relationship == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                dc.StructuredContent_Relationships.DeleteOnSubmit(relationship);

                StructuredContent_ContentType a_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == relationship.a_content_type_id).SingleOrDefault();
                StructuredContent_ContentType b_content_type = dc.StructuredContent_ContentTypes.Where(i => i.id == relationship.b_content_type_id).SingleOrDefault();

                // do db work
                switch (relationship.key)
                {
                    case "o2m":
                        // delete the relationship
                        sqlHelper.DeleteOneToManyRelationship(relationship.StructuredContent_ContentType, relationship.StructuredContent_ContentType1);

                        // delete foreign key field meta of the child table
                        string o2m_foreign_key_column_name = relationship.StructuredContent_ContentType.singular.ToLower() + "_id";
                        StructuredContent_ContentField o2m_foreign_key_content_field = dc.StructuredContent_ContentFields.Where(i => i.content_type_id == relationship.b_content_type_id && i.column_name == o2m_foreign_key_column_name).FirstOrDefault();
                        if (o2m_foreign_key_content_field != null)
                        {
                            dc.StructuredContent_ContentFields.DeleteOnSubmit(o2m_foreign_key_content_field);
                        }

                        // delete the foreign key column
                        sqlHelper.DeleteColumn(o2m_foreign_key_content_field);

                        break;

                    case "m2m":
                        sqlHelper.DeleteManyToManyRelationship(a_content_type, b_content_type);

                        break;
                }

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}