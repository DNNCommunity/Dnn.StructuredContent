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

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using StructuredContent.DAL;

    // [SupportedModules("StructuredContent")]
    public class ContentFieldTypeController : DnnApiController
    {
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string name = "", bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dc.StructuredContent_ContentFieldTypes.OrderBy(i => i.ordinal).AsQueryable();

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
                    List<ContentFieldTypeDTO> dtos = new List<ContentFieldTypeDTO>();

                    foreach (StructuredContent_ContentFieldType item in query)
                    {
                        ContentFieldTypeDTO dto = item.ToDto();
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
                StructuredContent_ContentFieldType item = this.dc.StructuredContent_ContentFieldTypes.Where(i => i.id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(ContentFieldTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentFieldType item = dto.ToItem(null);

                this.dc.StructuredContent_ContentFieldTypes.InsertOnSubmit(item);

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
        public HttpResponseMessage Put(ContentFieldTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentFieldType item = this.dc.StructuredContent_ContentFieldTypes.Where(i => i.id == dto.id).SingleOrDefault();
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
                StructuredContent_ContentFieldType item = this.dc.StructuredContent_ContentFieldTypes.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dc.StructuredContent_ContentFieldTypes.DeleteOnSubmit(item);
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
