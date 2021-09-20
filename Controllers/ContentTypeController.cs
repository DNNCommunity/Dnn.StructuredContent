// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using StructuredContent.DAL;

    public class ContentTypeController : DnnApiController
    {
        private ISQLHelper sqlHelper;
        DataContext dc = new DataContext();

        public ContentTypeController(ISQLHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string name = "", bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dc.StructuredContent_ContentTypes.OrderBy(i => i.name).AsQueryable();

                // name
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(i => i.name.ToLower().Contains(name.ToLower()));
                }

                // skip
                if (skip.HasValue)
                {
                    query = query.Skip(skip.GetValueOrDefault());
                }

                // take
                if (take.HasValue)
                {
                    query = query.Take(take.GetValueOrDefault());
                }

                // verbose
                if (verbose.GetValueOrDefault() == false)
                {
                    var list = query.Select(i => new { i.id, i.name });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    List<ContentTypeDTO> dtos = new List<ContentTypeDTO>();

                    foreach (StructuredContent_ContentType item in query)
                    {
                        ContentTypeDTO dto = item.ToDto();
                        dtos.Add(dto);
                    }

                    return this.Request.CreateResponse(HttpStatusCode.OK, dtos);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                StructuredContent_ContentType item = this.dc.StructuredContent_ContentTypes.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(ContentTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentType content_type = dto.ToItem(null);

                this.dc.StructuredContent_ContentTypes.InsertOnSubmit(content_type);

                this.sqlHelper.CreateContentTable(content_type);

                // record the field definitions for the system fields
                StructuredContent_ContentField idField = new StructuredContent_ContentField()
                {
                    name = "ID",
                    is_system = true,
                    ordinal = 0,
                    column_name = "id",
                    data_type = (int)Enums.DataTypes.integer,
                    allow_null = false,
                };
                content_type.StructuredContent_ContentFields.Add(idField);

                StructuredContent_ContentField nameField = new StructuredContent_ContentField()
                {
                    name = "Name",
                    is_system = true,
                    ordinal = 0,
                    column_name = "name",
                    data_type = (int)Enums.DataTypes.nvarchar,
                    data_length = "250",
                    allow_null = false,
                    options = "{'required':true, 'control_type':'textbox'}",
                };
                content_type.StructuredContent_ContentFields.Add(nameField);

                StructuredContent_ContentField statusField = new StructuredContent_ContentField()
                {
                    name = "Status",
                    is_system = true,
                    ordinal = 0,
                    column_name = "status",
                    data_type = (int)Enums.DataTypes.nvarchar,
                    data_length = "250",
                    allow_null = false,
                    options = "{'required':true}",
                };
                content_type.StructuredContent_ContentFields.Add(statusField);

                StructuredContent_ContentField dateCreatedField = new StructuredContent_ContentField()
                {
                    name = "Date Created",
                    is_system = true,
                    ordinal = 0,
                    column_name = "date_created",
                    data_type = (int)Enums.DataTypes.datetime,
                    allow_null = false,
                };
                content_type.StructuredContent_ContentFields.Add(dateCreatedField);

                StructuredContent_ContentField dateUpdatedField = new StructuredContent_ContentField()
                {
                    name = "Date Modified",
                    is_system = true,
                    ordinal = 0,
                    column_name = "date_modified",
                    data_type = (int)Enums.DataTypes.datetime,
                    allow_null = false,
                };
                content_type.StructuredContent_ContentFields.Add(dateUpdatedField);

                StructuredContent_ContentField datePublishedField = new StructuredContent_ContentField()
                {
                    name = "Date Published",
                    is_system = true,
                    ordinal = 0,
                    column_name = "date_published",
                    data_type = (int)Enums.DataTypes.datetime,
                    allow_null = false,
                };
                content_type.StructuredContent_ContentFields.Add(datePublishedField);

                this.dc.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, content_type.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(ContentTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentType item = this.dc.StructuredContent_ContentTypes.Where(i => i.id == dto.id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item = dto.ToItem(item);
                this.dc.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                StructuredContent_ContentType content_type = this.dc.StructuredContent_ContentTypes.Where(i => i.id == id).SingleOrDefault();
                if (content_type == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // delete all the relationships first
                var relationships = this.dc.StructuredContent_Relationships.Where(i => i.a_content_type_id == content_type.id || i.b_content_type_id == content_type.id).Distinct().ToList();
                foreach (var relationship in relationships)
                {
                    switch (relationship.key)
                    {
                        case "o2m":
                            this.sqlHelper.DeleteOneToManyRelationship(relationship.StructuredContent_ContentType, relationship.StructuredContent_ContentType1);
                            break;

                        case "m2m":
                            this.sqlHelper.DeleteManyToManyRelationship(relationship.StructuredContent_ContentType, relationship.StructuredContent_ContentType1);
                            break;
                    }
                }

                this.dc.StructuredContent_Relationships.DeleteAllOnSubmit(relationships);

                // delete the table
                this.sqlHelper.DeleteContentTable(content_type);

                this.dc.StructuredContent_ContentTypes.DeleteOnSubmit(content_type);
                this.dc.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
