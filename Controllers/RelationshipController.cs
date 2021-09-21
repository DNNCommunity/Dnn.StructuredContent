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

    /// <summary>
    /// Web API to manage relationships.
    /// </summary>
    [JsonCamelCaseSerializer]
    public class RelationshipController : DnnApiController
    {
        private readonly ISQLHelper sqlHelper;
        private readonly DataContext dataContext = new DataContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipController"/> class.
        /// </summary>
        /// <param name="sqlHelper">The sql helper to use.</param>
        public RelationshipController(ISQLHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        /// <summary>
        /// Gets relationsips.
        /// </summary>
        /// <param name="contentTypeId">The ID of the content type to get relationships for.</param>
        /// <returns>A list of <see cref="RelationshipDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int? contentTypeId = null)
        {
            try
            {
                var query = this.dataContext.StructuredContent_Relationships.AsQueryable();

                // ContentTypeId
                if (contentTypeId.HasValue)
                {
                    query = query.Where(i => i.AContentTypeId == contentTypeId.GetValueOrDefault() || i.BContentTypeId == contentTypeId.GetValueOrDefault());
                }

                var dtos = new List<RelationshipDto>();

                foreach (var item in query)
                {
                    var dto = item.ToDto();
                    dtos.Add(dto);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, dtos);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Gets a single relationship.
        /// </summary>
        /// <param name="id">The ID of the relashionship to get.</param>
        /// <returns><see cref="RelationshipDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Relationships.Where(i => i.Id == id).SingleOrDefault();
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

        /// <summary>
        /// Creates a new relationship.
        /// </summary>
        /// <param name="dto"><see cref="RelationshipDto"/>.</param>
        /// <returns>The created <see cref="RelationshipDto"/>.</returns>
        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(RelationshipDto dto)
        { // this handles saving the relationship between content types - the abstract definition
            try
            {
                var aContentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == dto.AContentTypeId).SingleOrDefault();
                var bContentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == dto.BContentTypeId).SingleOrDefault();

                var o2m_relationship = new StructuredContent_Relationship();
                var m2o_relationship = new StructuredContent_Relationship();

                switch (dto.Key)
                {
                    case "o2m": // a=one , b=many

                        // check for duplicate column name
                        var o2m_foreign_key_ColumnName = aContentType.Singular.ToLower() + "_id";
                        var o2m_duplicate_check = this.dataContext.StructuredContent_ContentFields.Where(i => i.ContentTypeId == dto.BContentTypeId && (i.Name == o2m_foreign_key_ColumnName || i.ColumnName == o2m_foreign_key_ColumnName)).Any();
                        if (o2m_duplicate_check)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.Conflict);
                        }

                        // record foreign key field of the one table
                        var content_field = new StructuredContent_ContentField
                        {
                            StructuredContent_ContentType = bContentType,
                            Name = o2m_foreign_key_ColumnName,
                            ColumnName = o2m_foreign_key_ColumnName,
                            Ordinal = 0,
                            DataType = (int)Enums.DataTypes.Integer,
                            AllowNull = true,
                            IsSystem = true,
                        };
                        this.dataContext.StructuredContent_ContentFields.InsertOnSubmit(content_field);

                        // record relationship == 1->many
                        o2m_relationship = new StructuredContent_Relationship
                        {
                            Key = "o2m",
                            AContentTypeId = dto.AContentTypeId,
                            BContentTypeId = dto.BContentTypeId,

                            ARequired = dto.ARequired,
                            AMaxLimit = dto.AMaxLimit,
                            AMinLimit = dto.AMinLimit,
                            AHelpText = dto.AHelpText,

                            BRequired = dto.BRequired,
                            BMaxLimit = dto.BMaxLimit,
                            BMinLimit = dto.BMinLimit,
                            BHelpText = dto.BHelpText,

                            ALayoutColumn = dto.ALayoutColumn,
                            ALayoutRow = dto.ALayoutRow,
                            BLayoutColumn = dto.BLayoutColumn,
                            BLayoutRow = dto.BLayoutRow,
                        };
                        this.dataContext.StructuredContent_Relationships.InsertOnSubmit(o2m_relationship);

                        this.sqlHelper.AddColumn(content_field);
                        this.sqlHelper.CreateOneToManyRelationship(aContentType, bContentType);

                        this.dataContext.SubmitChanges();

                        return this.Request.CreateResponse(HttpStatusCode.OK, o2m_relationship.ToDto());

                    case "m2m": // many-to-many, need junction table

                        // set up the relationship in the db
                        this.sqlHelper.CreateManyToManyRelationship(aContentType, bContentType);

                        // record relationship == a->b
                        var m2m_relationship = new StructuredContent_Relationship
                        {
                            Key = "m2m",
                            AContentTypeId = dto.AContentTypeId,
                            BContentTypeId = dto.BContentTypeId,

                            ARequired = dto.ARequired,
                            AMaxLimit = dto.AMaxLimit,
                            AMinLimit = dto.AMinLimit,
                            AHelpText = dto.AHelpText,

                            BRequired = dto.BRequired,
                            BMaxLimit = dto.BMaxLimit,
                            BMinLimit = dto.BMinLimit,
                            BHelpText = dto.BHelpText,

                            ALayoutColumn = dto.ALayoutColumn,
                            ALayoutRow = dto.ALayoutRow,
                            BLayoutColumn = dto.BLayoutColumn,
                            BLayoutRow = dto.BLayoutRow,
                        };
                        this.dataContext.StructuredContent_Relationships.InsertOnSubmit(m2m_relationship);
                        this.dataContext.SubmitChanges();

                        return this.Request.CreateResponse(HttpStatusCode.OK, m2m_relationship.ToDto());

                    default:
                        return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Updates an existing relationship.
        /// </summary>
        /// <param name="dto"><see cref="RelationshipDto"/>.</param>
        /// <returns>The updated <see cref="RelationshipDto"/>.</returns>
        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(RelationshipDto dto)
        {
            try
            {
                // the only things that can be updated are the min/max limits on the relaitonship
                var item = this.dataContext.StructuredContent_Relationships.Where(i => i.Id == dto.Id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item.ARequired = dto.ARequired;
                item.AMaxLimit = dto.AMaxLimit;
                item.AMinLimit = dto.AMinLimit;
                item.AHelpText = dto.AHelpText;

                item.BRequired = dto.BRequired;
                item.BMaxLimit = dto.BMaxLimit;
                item.BMinLimit = dto.BMinLimit;
                item.BHelpText = dto.BHelpText;

                item.ALayoutRow = dto.ALayoutRow;
                item.ALayoutColumn = dto.ALayoutColumn;
                item.BLayoutRow = dto.BLayoutRow;
                item.BLayoutColumn = dto.BLayoutColumn;

                this.dataContext.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Deletes an existing relationship.
        /// </summary>
        /// <param name="id">The ID of the relationship to delete.</param>
        /// <returns>OK or InternalServerError.</returns>
        [HttpDelete]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Relationships.Where(i => i.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dataContext.StructuredContent_Relationships.DeleteOnSubmit(item);

                var aContentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == item.AContentTypeId).SingleOrDefault();
                var bContentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.Id == item.BContentTypeId).SingleOrDefault();

                // do db work
                switch (item.Key)
                {
                    case "o2m":
                        // delete the relationship
                        this.sqlHelper.DeleteOneToManyRelationship(item.StructuredContent_ContentType, item.StructuredContent_ContentType1);

                        // delete foreign key field meta of the child table
                        var o2m_foreign_key_ColumnName = item.StructuredContent_ContentType.Singular.ToLower() + "_id";
                        var o2m_foreign_key_content_field = this.dataContext.StructuredContent_ContentFields.Where(i => i.ContentTypeId == item.BContentTypeId && i.ColumnName == o2m_foreign_key_ColumnName).FirstOrDefault();
                        if (o2m_foreign_key_content_field != null)
                        {
                            this.dataContext.StructuredContent_ContentFields.DeleteOnSubmit(o2m_foreign_key_content_field);
                        }

                        // delete the foreign key column
                        this.sqlHelper.DeleteColumn(o2m_foreign_key_content_field);

                        break;

                    case "m2m":
                        this.sqlHelper.DeleteManyToManyRelationship(aContentType, bContentType);

                        break;
                }

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
