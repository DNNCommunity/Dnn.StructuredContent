using DotNetNuke.Common.Utilities;
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
    public class ContentFieldTypeController : DnnApiController
    {
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string name = "", Nullable<bool> verbose = null, Nullable<int> skip = null, Nullable<int> take = null)
        {
            try
            {
                var query = dc.StructuredContent_ContentFieldTypes.OrderBy(i => i.ordinal).AsQueryable();

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
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    List<ContentFieldTypeDTO> dtos = new List<ContentFieldTypeDTO>();

                    foreach (StructuredContent_ContentFieldType item in query)
                    {
                        ContentFieldTypeDTO dto = item.ToDto();
                        dtos.Add(dto);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, dtos);
                }
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
                StructuredContent_ContentFieldType item = dc.StructuredContent_ContentFieldTypes.Where(i => i.id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(ContentFieldTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentFieldType item = dto.ToItem(null);

                dc.StructuredContent_ContentFieldTypes.InsertOnSubmit(item);

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
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
        public HttpResponseMessage Put(ContentFieldTypeDTO dto)
        {
            try
            {
                StructuredContent_ContentFieldType item = dc.StructuredContent_ContentFieldTypes.Where(i => i.id == dto.id).SingleOrDefault();
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                item = dto.ToItem(item);
                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
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
                StructuredContent_ContentFieldType item = dc.StructuredContent_ContentFieldTypes.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dc.StructuredContent_ContentFieldTypes.DeleteOnSubmit(item);
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