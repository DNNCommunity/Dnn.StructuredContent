﻿using DotNetNuke.Common.Utilities;
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
    public class ContentFieldController : DnnApiController
    {
        SQLHelper sqlHelper = new SQLHelper();
        DataContext dc = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string contentType, Nullable<bool> verbose = null, Nullable<int> skip = null, Nullable<int> take = null)
        {
            try
            {
                var query = dc.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.name.ToLower() == contentType.ToLower()).OrderBy(i => i.ordinal).AsQueryable();

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
                    var list = query.Select(i => new { i.id, i.name, i.is_system, content_field_type_name = i.StructuredContent_ContentFieldType.name }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    var list = query;

                    List<ContentFieldDTO> dtos = new List<ContentFieldDTO>();
                    foreach (StructuredContent_ContentField item in query)
                    {
                        ContentFieldDTO dto = item.ToDto();
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
        public HttpResponseMessage Get(string contentType, int id)
        {
            try
            {
                StructuredContent_ContentField content_field = dc.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.name.ToLower() == contentType.ToLower() && i.id == id).SingleOrDefault();

                if (content_field == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, content_field.ToDto());
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
        public HttpResponseMessage Post(string contentType, ContentFieldDTO dto)
        {
            try
            {
                StructuredContent_ContentField content_field = dto.ToItem(null);

                content_field.is_system = false;

                // set the ordinal
                int count = dc.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.name.ToLower() == contentType.ToLower()).Count();
                content_field.ordinal = count++;

                if (dto.options != null)
                { // use config settings passed in, if present
                    content_field.options = dto.options.ToString();
                }

                dc.StructuredContent_ContentFields.InsertOnSubmit(content_field);

                // check for duplicate column name
                bool duplicate_check = dc.StructuredContent_ContentFields.Where(i => i.content_type_id == content_field.content_type_id && (i.name == content_field.name || i.column_name == content_field.column_name)).Any();
                if (duplicate_check)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                }

                StructuredContent_ContentType content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                content_field.StructuredContent_ContentType = content_type;

                sqlHelper.AddColumn(content_field);

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, content_field.ToDto());
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
        public HttpResponseMessage Put(string contentType, ContentFieldDTO dto)
        {
            try
            {
                StructuredContent_ContentField content_field = dc.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.name.ToLower() == contentType.ToLower() && i.id == dto.id).SingleOrDefault();

                if (content_field == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                content_field = dto.ToItem(content_field);

                // check for duplicate column name
                bool duplicate_check = dc.StructuredContent_ContentFields.Where(i => i.id != content_field.id && i.content_type_id == content_field.content_type_id && (i.name == content_field.name || i.column_name == content_field.column_name)).Any();
                if (duplicate_check)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                }

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, content_field.ToDto()); // send back the updated record
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
        public HttpResponseMessage Delete(string contentType, int id)
        {
            try
            {
                StructuredContent_ContentField content_field = dc.StructuredContent_ContentFields.Where(i => i.StructuredContent_ContentType.name.ToLower() == contentType.ToLower() && i.id == id).SingleOrDefault();

                sqlHelper.DeleteColumn(content_field);

                dc.StructuredContent_ContentFields.DeleteOnSubmit(content_field);

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