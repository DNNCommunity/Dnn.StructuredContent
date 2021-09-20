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
    using Newtonsoft.Json;
    using StructuredContent.DAL;

    // [SupportedModules("StructuredContent")]
    public class RevisionController : DnnApiController
    {
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int? content_type_id = null, int? item_id = null, bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dc.StructuredContent_Revisions.OrderByDescending(i => i.revision_date).AsQueryable();

                // content_type_id, item_id
                if (content_type_id.HasValue && item_id.HasValue)
                {
                    query = query.Where(i => i.content_type_id == content_type_id.GetValueOrDefault() && i.item_id == item_id.GetValueOrDefault());
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
                    var list = query.Select(i => new { i.id, i.revision_date, i.activity_type });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    List<RevisionDTO> dtos = new List<RevisionDTO>();

                    foreach (StructuredContent_Revision item in query)
                    {
                        RevisionDTO dto = item.ToDto();
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
                StructuredContent_Revision item = this.dc.StructuredContent_Revisions.Where(i => i.id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(RevisionDTO dto)
        {
            try
            {
                StructuredContent_Revision item = dto.ToItem(null);

                this.dc.StructuredContent_Revisions.InsertOnSubmit(item);

                this.dc.SubmitChanges();

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
        public HttpResponseMessage Put(RevisionDTO dto)
        {
            try
            {
                StructuredContent_Revision item = this.dc.StructuredContent_Revisions.Where(i => i.id == dto.id).SingleOrDefault();
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
                StructuredContent_Revision item = this.dc.StructuredContent_Revisions.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dc.StructuredContent_Revisions.DeleteOnSubmit(item);
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
