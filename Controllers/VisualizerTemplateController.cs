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
    using DotLiquid;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using StructuredContent.DAL;

    [JsonCamelCaseSerializer]
    public class VisualizerTemplateController : DnnApiController
    {
        private readonly DataContext dataContext = new DataContext();
        private readonly ISQLHelper sqlHelper;

        public VisualizerTemplateController(ISQLHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string name = "", int? contentTypeId = null, bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dataContext.StructuredContent_VisualizerTemplates.OrderBy(i => i.Name).AsQueryable();

                // name
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(name.ToLower()));
                }

                // ContentTypeId
                if (contentTypeId.HasValue)
                {
                    query = query.Where(i => i.ContentTypeId == contentTypeId.GetValueOrDefault());
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
                    var list = query.Select(i => new { i.Id, i.Name, i.Description, i.ContentSize });
                    return this.Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    var dtos = new List<VisualizerTemplateDto>();

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
                var item = this.dataContext.StructuredContent_VisualizerTemplates.Where(i => i.Id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(VisualizerTemplateDto dto)
        {
            try
            {
                var item = dto.ToItem(null);

                this.dataContext.StructuredContent_VisualizerTemplates.InsertOnSubmit(item);

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
        public HttpResponseMessage Put(VisualizerTemplateDto dto)
        {
            try
            {
                var item = this.dataContext.StructuredContent_VisualizerTemplates.Where(i => i.Id == dto.Id).SingleOrDefault();
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
                var item = this.dataContext.StructuredContent_VisualizerTemplates.Where(i => i.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dataContext.StructuredContent_VisualizerTemplates.DeleteOnSubmit(item);
                this.dataContext.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is VisualizerTemplateController controller &&
                   EqualityComparer<ISQLHelper>.Default.Equals(this.sqlHelper, controller.sqlHelper);
        }

        public override int GetHashCode()
        {
            return -566121293 + EqualityComparer<ISQLHelper>.Default.GetHashCode(this.sqlHelper);
        }
    }
}
