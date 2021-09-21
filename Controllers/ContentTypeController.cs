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
    using Dnn.StructuredContent.Controllers.Serializers;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using StructuredContent.DAL;

    [JsonCamelCaseSerializer]
    public class ContentTypeController : DnnApiController
    {
        private readonly ISQLHelper sqlHelper;
        private readonly DataContext dataContext = new DataContext();

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
                var query = this.dataContext.StructuredContent_ContentTypes.OrderBy(i => i.Name).AsQueryable();

                // name
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(name.ToLower()));
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
                    var list = query.Select(i => new { i.Id, i.Name });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    var dtos = new List<ContentTypeDto>();

                    foreach (var item in query)
                    {
                        var dto = item.ToDto();
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
                var item = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(ContentTypeDto dto)
        {
            try
            {
                var item = dto.ToItem(null);

                this.dataContext.StructuredContent_ContentTypes.InsertOnSubmit(item);

                this.sqlHelper.CreateContentTable(item);

                // record the field definitions for the system fields
                var idField = new StructuredContent_ContentField()
                {
                    Name = "ID",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "id",
                    DataType = (int)Enums.DataTypes.Integer,
                    AllowNull = false,
                };
                item.StructuredContent_ContentFields.Add(idField);

                var nameField = new StructuredContent_ContentField()
                {
                    Name = "Name",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "name",
                    DataType = (int)Enums.DataTypes.Nvarchar,
                    DataLength = "250",
                    AllowNull = false,
                    Options = "{'required':true, 'control_type':'textbox'}",
                };
                item.StructuredContent_ContentFields.Add(nameField);

                var statusField = new StructuredContent_ContentField()
                {
                    Name = "Status",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "status",
                    DataType = (int)Enums.DataTypes.Nvarchar,
                    DataLength = "250",
                    AllowNull = false,
                    Options = "{'required':true}",
                };
                item.StructuredContent_ContentFields.Add(statusField);

                var dateCreatedField = new StructuredContent_ContentField()
                {
                    Name = "Date Created",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "DateCreated",
                    DataType = (int)Enums.DataTypes.Datetime,
                    AllowNull = false,
                };
                item.StructuredContent_ContentFields.Add(dateCreatedField);

                var dateUpdatedField = new StructuredContent_ContentField()
                {
                    Name = "Date Modified",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "DateModified",
                    DataType = (int)Enums.DataTypes.Datetime,
                    AllowNull = false,
                };
                item.StructuredContent_ContentFields.Add(dateUpdatedField);

                var datePublishedField = new StructuredContent_ContentField()
                {
                    Name = "Date Published",
                    IsSystem = true,
                    Ordinal = 0,
                    ColumnName = "DatePublished",
                    DataType = (int)Enums.DataTypes.Datetime,
                    AllowNull = false,
                };
                item.StructuredContent_ContentFields.Add(datePublishedField);

                this.dataContext.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
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
        public HttpResponseMessage Put(ContentTypeDto dto)
        {
            try
            {
                var item = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == dto.Id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item = dto.ToItem(item);
                this.dataContext.SubmitChanges();

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
                var item = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // delete all the relationships first
                var relationships = this.dataContext.StructuredContent_Relationships.Where(i => i.AContentTypeId == item.Id || i.BContentTypeId == item.Id).Distinct().ToList();
                foreach (var relationship in relationships)
                {
                    switch (relationship.Key)
                    {
                        case "o2m":
                            this.sqlHelper.DeleteOneToManyRelationship(relationship.StructuredContent_ContentType, relationship.StructuredContent_ContentType1);
                            break;

                        case "m2m":
                            this.sqlHelper.DeleteManyToManyRelationship(relationship.StructuredContent_ContentType, relationship.StructuredContent_ContentType1);
                            break;
                    }
                }

                this.dataContext.StructuredContent_Relationships.DeleteAllOnSubmit(relationships);

                // delete the table
                this.sqlHelper.DeleteContentTable(item);

                this.dataContext.StructuredContent_ContentTypes.DeleteOnSubmit(item);
                this.dataContext.SubmitChanges();

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
