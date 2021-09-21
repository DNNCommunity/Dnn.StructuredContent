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
    /// Web API to manage content revisions (for audit logs).
    /// </summary>
    [JsonCamelCaseSerializer]
    public class RevisionController : DnnApiController
    {
        private readonly DataContext dataContext = new DataContext();

        /// <summary>
        /// Gets a list of content revisions.
        /// </summary>
        /// <param name="contentTypeId">The ID of the content type to get revisions for.</param>
        /// <param name="itemId">The ID of the item to get (optionsal).</param>
        /// <param name="verbose">Whether to return verbose results.</param>
        /// <param name="skip">The number of items to skip (for paging).</param>
        /// <param name="take">The number of items to take (for paging).</param>
        /// <returns>A list of <see cref="RevisionDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int? contentTypeId = null, int? itemId = null, bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dataContext.StructuredContent_Revisions.OrderByDescending(i => i.RevisionDate).AsQueryable();

                // ContentTypeId, ItemId
                if (contentTypeId.HasValue && itemId.HasValue)
                {
                    query = query.Where(i => i.ContentTypeId == contentTypeId.GetValueOrDefault() && i.ItemId == itemId.GetValueOrDefault());
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
                    var list = query.Select(i => new { i.Id, i.RevisionDate, i.ActivityType });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    var dtos = new List<RevisionDto>();

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

        /// <summary>
        /// Gets a single revision.
        /// </summary>
        /// <param name="id">The ID of the revision to get.</param>
        /// <returns><see cref="RevisionDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Revisions.Where(i => i.Id == id).SingleOrDefault();
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
        /// Creates a new revision.
        /// </summary>
        /// <param name="dto"><see cref="RevisionDto"/>.</param>
        /// <returns>The created <see cref="RevisionDto"/>.</returns>
        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(RevisionDto dto)
        {
            try
            {
                var item = dto.ToItem(null);

                this.dataContext.StructuredContent_Revisions.InsertOnSubmit(item);

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
        /// Updates an existing revision.
        /// </summary>
        /// <param name="dto"><see cref="RevisionDto"/>.</param>
        /// <returns>The updated <see cref="RevisionDto"/>.</returns>
        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(RevisionDto dto)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Revisions.Where(i => i.Id == dto.Id).SingleOrDefault();
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

        /// <summary>
        /// Deletes an existing revision.
        /// </summary>
        /// <param name="id">The id of the revision to delete.</param>
        /// <returns>OK or InternalServerError.</returns>
        [HttpDelete]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Revisions.Where(i => i.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dataContext.StructuredContent_Revisions.DeleteOnSubmit(item);
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
