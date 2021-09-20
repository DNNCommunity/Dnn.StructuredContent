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

    using DotLiquid;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using StructuredContent.DAL;

    // [SupportedModules("StructuredContent")]
    public class VisualizerTemplateController : DnnApiController
    {
        DataContext dc = new DataContext();
        SQLHelper sqlHelper = new SQLHelper();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string name = "", int? content_type_id = null, bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dc.StructuredContent_VisualizerTemplates.OrderBy(i => i.name).AsQueryable();

                // name
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(i => i.name.ToLower().Contains(name.ToLower()));
                }

                // content_type_id
                if (content_type_id.HasValue)
                {
                    query = query.Where(i => i.content_type_id == content_type_id.GetValueOrDefault());
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
                    var list = query.Select(i => new { i.id, i.name, i.description, i.content_size });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    List<VisualizerTemplateDTO> dtos = new List<VisualizerTemplateDTO>();

                    foreach (StructuredContent_VisualizerTemplate item in query)
                    {
                        VisualizerTemplateDTO dto = item.ToDto();
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
                StructuredContent_VisualizerTemplate item = this.dc.StructuredContent_VisualizerTemplates.Where(i => i.id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(VisualizerTemplateDTO dto)
        {
            try
            {
                StructuredContent_VisualizerTemplate content_type = dto.ToItem(null);

                this.dc.StructuredContent_VisualizerTemplates.InsertOnSubmit(content_type);

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
        public HttpResponseMessage Put(VisualizerTemplateDTO dto)
        {
            try
            {
                StructuredContent_VisualizerTemplate item = this.dc.StructuredContent_VisualizerTemplates.Where(i => i.id == dto.id).SingleOrDefault();
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
                StructuredContent_VisualizerTemplate content_type = this.dc.StructuredContent_VisualizerTemplates.Where(i => i.id == id).SingleOrDefault();
                if (content_type == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dc.StructuredContent_VisualizerTemplates.DeleteOnSubmit(content_type);
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
