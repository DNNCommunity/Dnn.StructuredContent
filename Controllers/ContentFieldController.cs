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

    /// <summary>
    /// Manages Content Fields.
    /// </summary>
    public class ContentFieldController : DnnApiController
    {
        private readonly ISQLHelper sqlHelper;
        private readonly DataContext dataContext = new DataContext();

        public ContentFieldController(ISQLHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string contentTypeUrlSlug, bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dataContext.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower()).OrderBy(i => i.Ordinal).AsQueryable();

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
                    var list = query.Select(i => new { i.Id, i.Name, i.IsSystem, ContentFieldTypeName = i.StructuredContent_ContentFieldType.Name }).ToList();
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    var list = query.Select(i => i.ToDto()).ToList();
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
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
        public HttpResponseMessage Get(string contentTypeUrlSlug, int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower() && i.Id == id).SingleOrDefault();

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
        public HttpResponseMessage Post(string contentTypeUrlSlug, ContentFieldDto dto)
        {
            try
            {
                var item = dto.ToItem(null);

                item.IsSystem = false;

                // set the Ordinal
                var count = this.dataContext.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower()).Count();
                item.Ordinal = count++;

                if (dto.Options != null)
                { // use config settings passed in, if present
                    item.Options = dto.Options.ToString();
                }

                this.dataContext.StructuredContent_ContentFields.InsertOnSubmit(item);

                // check for duplicate column name
                var duplicate_check = this.dataContext.StructuredContent_ContentFields.Where(i => i.ContentTypeId == item.ContentTypeId && (i.Name == item.Name || i.ColumnName == item.ColumnName)).Any();
                if (duplicate_check)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Conflict);
                }

                var contentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug == contentTypeUrlSlug).SingleOrDefault();

                if (contentType == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item.StructuredContent_ContentType = contentType;

                this.sqlHelper.AddColumn(item);

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
        public HttpResponseMessage Put(string contentTypeUrlSlug, ContentFieldDto dto)
        {
            try
            {
                var item = this.dataContext.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower() && i.Id == dto.Id).SingleOrDefault();

                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item = dto.ToItem(item);

                // check for duplicate column name
                var duplicate_check = this.dataContext.StructuredContent_ContentFields.Where(i => i.Id != item.Id && i.ContentTypeId == item.ContentTypeId && (i.Name == item.Name || i.ColumnName == item.ColumnName)).Any();
                if (duplicate_check)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Conflict);
                }

                this.dataContext.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto()); // send back the updated record
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
        public HttpResponseMessage Delete(string contentTypeUrlSlug, int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower() && i.Id == id).SingleOrDefault();

                this.sqlHelper.DeleteColumn(item);

                this.dataContext.StructuredContent_ContentFields.DeleteOnSubmit(item);

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
