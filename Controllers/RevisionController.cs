using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;
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
    public class RevisionController : DnnApiController
    {
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(Nullable<int> content_type_id = null, Nullable<int> item_id = null, Nullable<bool> verbose = null, Nullable<int> skip = null, Nullable<int> take = null)
        {
            try
            {
                var query = dc.StructuredContent_Revisions.OrderByDescending(i => i.revision_date).AsQueryable();

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
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    List<RevisionDTO> dtos = new List<RevisionDTO>();

                    foreach (StructuredContent_Revision item in query)
                    {
                        RevisionDTO dto = item.ToDto();
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
                StructuredContent_Revision item = dc.StructuredContent_Revisions.Where(i => i.id == id).SingleOrDefault();
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
        public HttpResponseMessage Post(RevisionDTO dto)
        {
            try
            {
                StructuredContent_Revision item = dto.ToItem(null);

                dc.StructuredContent_Revisions.InsertOnSubmit(item);

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
        public HttpResponseMessage Put(RevisionDTO dto)
        {
            try
            {
                StructuredContent_Revision item = dc.StructuredContent_Revisions.Where(i => i.id == dto.id).SingleOrDefault();
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
                StructuredContent_Revision item = dc.StructuredContent_Revisions.Where(i => i.id == id).SingleOrDefault();
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dc.StructuredContent_Revisions.DeleteOnSubmit(item);
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