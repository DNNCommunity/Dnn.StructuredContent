// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Api;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using StructuredContent.DAL;

    // [SupportedModules("StructuredContent")]
    public class ContentItemController : DnnApiController
    {
        private readonly DataContext dataContext;
        private readonly ISQLHelper sqlHelper;

        public ContentItemController(ISQLHelper sqlHelper)
        {
            this.dataContext = new DataContext();
            this.sqlHelper = sqlHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string contentTypeUrlSlug, string name = "", bool? verbose = null, int? skip = null, int? take = null)
        {
            try
            {
                var contentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug.ToLower() == contentTypeUrlSlug.ToLower()).SingleOrDefault();

                if (contentType == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                IEnumerable<dynamic> query = this.sqlHelper.SelectDynamicList(contentType, string.Empty);

                var list = query.ToList();

                // name
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(i => i["name"].ToLower().Contains(name.ToLower())).ToList();
                }

                // skip
                if (skip.HasValue)
                {
                    list = list.Skip(skip.GetValueOrDefault()).ToList();
                }

                // take
                if (take.HasValue)
                {
                    list = list.Take(take.GetValueOrDefault()).ToList();
                }

                // verbose
                if (verbose.HasValue)
                {
                    // do something
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, list);
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
                var contentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug == contentTypeUrlSlug).SingleOrDefault();

                if (contentType == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                var item = this.sqlHelper.SelectDynamicItem(contentType, id);

                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // this will add related object collections to the requested content item
                // [TODO] - add verbose flag to supress this
                var relationships = contentType.StructuredContent_Relationships;
                foreach (var relationship in relationships)
                {
                    var items = this.sqlHelper.GetRelatedItems(relationship, contentType, id);

                    if (relationship.Key == "o2m" && relationship.AContentTypeId == contentType.Id)
                    {
                        item[relationship.StructuredContent_ContentType1.Plural.ToLower()] = items;
                    }

                    if (relationship.Key == "m2m" && relationship.AContentTypeId == contentType.Id)
                    {
                        item[relationship.StructuredContent_ContentType1.Plural.ToLower()] = items;
                    }

                    if (relationship.Key == "m2m" && relationship.BContentTypeId == contentType.Id)
                    {
                        item[relationship.StructuredContent_ContentType.Plural.ToLower()] = items;
                    }
                }

                var relationships1 = contentType.StructuredContent_Relationships1;
                foreach (var relationship1 in relationships1)
                {
                    var items = this.sqlHelper.GetRelatedItems(relationship1, contentType, id);

                    if (relationship1.Key == "o2m" && relationship1.AContentTypeId == contentType.Id)
                    {
                        item[relationship1.StructuredContent_ContentType1.Plural.ToLower()] = items;
                    }

                    if (relationship1.Key == "m2m" && relationship1.AContentTypeId == contentType.Id)
                    {
                        item[relationship1.StructuredContent_ContentType1.Plural.ToLower()] = items;
                    }

                    if (relationship1.Key == "m2m" && relationship1.BContentTypeId == contentType.Id)
                    {
                        item[relationship1.StructuredContent_ContentType.Plural.ToLower()] = items;
                    }
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // insert
        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(string contentTypeUrlSlug, [FromBody] JToken itemData)
        {
            try
            {
                var contentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug == contentTypeUrlSlug).SingleOrDefault();

                if (contentType == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dynamic item = JsonConvert.DeserializeObject<dynamic>(itemData.ToString());

                int id = this.sqlHelper.InsertContentItem(contentType, item);

                return this.Request.CreateResponse(HttpStatusCode.OK, id); // send back the inserted record id
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // update
        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(string contentTypeUrlSlug, [FromBody] JToken itemData)
        {
            try
            {
                var primaryContentType = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug == contentTypeUrlSlug).SingleOrDefault();

                if (primaryContentType == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dynamic primaryContentItem = JsonConvert.DeserializeObject<dynamic>(itemData.ToString());
                var oldContentItem = this.sqlHelper.SelectDynamicItem(primaryContentType, (int)primaryContentItem.id);

                // record Delta
                int? userId = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo().UserID;
                var revision = new StructuredContent_Revision
                {
                    RevisionDate = DateTime.Now,
                    UserId = userId,
                    ActivityType = "UPDATE",
                    ContentTypeId = primaryContentType.Id,
                    ItemId = primaryContentItem.id,
                };
                var delta = new ExpandoObject() as IDictionary<string, object>;
                var recordChanged = false;
                foreach (var contentField in primaryContentType.StructuredContent_ContentFields)
                {
                    if (contentField.IsSystem == false)
                    {
                        if (
                            (oldContentItem[contentField.ColumnName] is DBNull && primaryContentItem[contentField.ColumnName] != null)
                            ||
                            (primaryContentItem[contentField.ColumnName] != oldContentItem[contentField.ColumnName]))
                        {
                            recordChanged = true;
                            delta[contentField.ColumnName] = primaryContentItem[contentField.ColumnName];
                        }
                    }
                }

                if (recordChanged)
                {
                    this.dataContext.StructuredContent_Revisions.InsertOnSubmit(revision);
                }

                revision.Delta = JsonConvert.SerializeObject(delta);
                revision.Data = JsonConvert.SerializeObject(oldContentItem);

                this.sqlHelper.UpdateContentItem(primaryContentType, primaryContentItem);

                // iterate over the relationships for the content item and update the relationships
                foreach (var relationship in primaryContentType.StructuredContent_Relationships)
                {
                    if (relationship.Key == "m2m")
                    {
                        // delete any existing cross references for the primary_content_type
                        this.sqlHelper.DeleteManyToManyRelationship(relationship, primaryContentType, (int)primaryContentItem.id);

                        // add back any related_content_type records present in the data model
                        StructuredContent_ContentType relatedContentType = null;
                        if (relationship.AContentTypeId == primaryContentType.Id)
                        {
                            relatedContentType = relationship.StructuredContent_ContentType;
                        }

                        if (relationship.AContentTypeId == primaryContentType.Id)
                        {
                            relatedContentType = relationship.StructuredContent_ContentType1;
                        }

                        var relatedContentTypeName = relatedContentType.Plural.ToLower();
                        if (primaryContentItem[relatedContentTypeName] != null)
                        {
                            foreach (var relatedContentItem in primaryContentItem[relatedContentTypeName])
                            {
                                this.sqlHelper.SaveManyToManyRelationship(relationship, primaryContentType, relatedContentType, (int)primaryContentItem.id, (int)relatedContentItem.id);
                            }
                        }
                    }

                    if (relationship.Key == "o2m")
                    {
                        var relatedContentType = relationship.StructuredContent_ContentType1;

                        // clear foreign keys
                        this.sqlHelper.DeleteOneToManyRelationship(primaryContentType, relatedContentType, (int)primaryContentItem.id);

                        // set foreign keys
                        var relatedContentTypeName = relatedContentType.Plural.ToLower();
                        foreach (var relatedContentItem in primaryContentItem[relatedContentTypeName])
                        {
                            this.sqlHelper.SaveOneToManyRelationship(primaryContentType, relatedContentType, (int)primaryContentItem.id, (int)relatedContentItem.id);
                        }
                    }
                }

                this.dataContext.SubmitChanges();
                int id = primaryContentItem.id;

                return this.Request.CreateResponse(HttpStatusCode.OK, id); // send back the updated record id
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
                var content_type = this.dataContext.StructuredContent_ContentTypes.Where(i => i.UrlSlug == contentTypeUrlSlug).SingleOrDefault();

                if (content_type == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.sqlHelper.DeleteContentItem(content_type, id);

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
