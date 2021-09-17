using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructuredContent.DAL;

namespace StructuredContent
{
    //[SupportedModules("StructuredContent")]
    public class ContentItemController : DnnApiController
    {
        protected DataContext dc = new DataContext();
        SQLHelper sqlHelper = new SQLHelper();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string contentType, string name = "", Nullable<bool> verbose = null, Nullable<int> skip = null, Nullable<int> take = null)
        {
            try
            {
                StructuredContent_ContentType content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                IEnumerable<dynamic> query = sqlHelper.SelectDynamicList(content_type, string.Empty);

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

                return Request.CreateResponse(HttpStatusCode.OK, list);
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
                StructuredContent_ContentType content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                IDictionary<string, object> item = sqlHelper.SelectDynamicItem(content_type, id);

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // this will add related object collections to the requested content item
                // [TODO] - add verbose flag to supress this
                var relationships = content_type.StructuredContent_Relationships;
                foreach (var relationship in relationships)
                {
                    IEnumerable<IDictionary<string, object>> items = sqlHelper.GetRelatedItems(relationship, content_type, id);

                    if (relationship.key == "o2m" && relationship.a_content_type_id == content_type.id)
                    {
                        item[relationship.StructuredContent_ContentType1.plural.ToLower()] = items;
                    }

                    if (relationship.key == "m2m" && relationship.a_content_type_id == content_type.id)
                    {
                        item[relationship.StructuredContent_ContentType1.plural.ToLower()] = items;
                    }

                    if (relationship.key == "m2m" && relationship.b_content_type_id == content_type.id)
                    {
                        item[relationship.StructuredContent_ContentType.plural.ToLower()] = items;
                    }
                }

                var relationships1 = content_type.StructuredContent_Relationships1;
                foreach (var relationship1 in relationships1)
                {
                    IEnumerable<IDictionary<string, object>> items = sqlHelper.GetRelatedItems(relationship1, content_type, id);

                    if (relationship1.key == "o2m" && relationship1.a_content_type_id == content_type.id)
                    {
                        item[relationship1.StructuredContent_ContentType1.plural.ToLower()] = items;
                    }

                    if (relationship1.key == "m2m" && relationship1.a_content_type_id == content_type.id)
                    {
                        item[relationship1.StructuredContent_ContentType1.plural.ToLower()] = items;
                    }

                    if (relationship1.key == "m2m" && relationship1.b_content_type_id == content_type.id)
                    {
                        item[relationship1.StructuredContent_ContentType.plural.ToLower()] = items;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // insert
        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(string contentType, [FromBody] JToken item_data)
        {
            try
            {
                StructuredContent_ContentType content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dynamic content_item = JsonConvert.DeserializeObject<dynamic>(item_data.ToString());

                int id = sqlHelper.InsertContentItem(content_type, content_item);

                return Request.CreateResponse(HttpStatusCode.OK, id); // send back the inserted record id
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // update
        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(string contentType, [FromBody] JToken item_data)
        {
            try
            {
                StructuredContent_ContentType primary_content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (primary_content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dynamic primary_content_item = JsonConvert.DeserializeObject<dynamic>(item_data.ToString());
                IDictionary<String, Object> old_content_item = sqlHelper.SelectDynamicItem(primary_content_type, (int)primary_content_item.id);

                // record delta
                Nullable<int> user_id = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo().UserID;
                var revision = new StructuredContent_Revision
                {
                    revision_date = DateTime.Now,
                    user_id = user_id,
                    activity_type = "UPDATE",
                    content_type_id = primary_content_type.id,
                    item_id = primary_content_item.id
                };
                var delta = new ExpandoObject() as IDictionary<string, object>;
                bool record_changed = false;
                foreach (var content_field in primary_content_type.StructuredContent_ContentFields)
                {
                    if (content_field.is_system == false)
                    {
                        if (
                            (old_content_item[content_field.column_name] is DBNull && primary_content_item[content_field.column_name] != null)
                            ||
                            (primary_content_item[content_field.column_name] != old_content_item[content_field.column_name])
                            )
                        {
                            record_changed = true;
                            delta[content_field.column_name] = primary_content_item[content_field.column_name];
                        }
                    }
                }
                if (record_changed)
                {
                    dc.StructuredContent_Revisions.InsertOnSubmit(revision);
                }
                revision.delta = JsonConvert.SerializeObject(delta);
                revision.data = JsonConvert.SerializeObject(old_content_item);

                sqlHelper.UpdateContentItem(primary_content_type, primary_content_item);

                // iterate over the relationships for the content item and update the relationships
                foreach (StructuredContent_Relationship relationship in primary_content_type.StructuredContent_Relationships)
                {
                    if (relationship.key == "m2m")
                    {
                        // delete any existing cross references for the primary_content_type
                        sqlHelper.DeleteManyToManyRelationship(relationship, primary_content_type, (int)primary_content_item.id);

                        // add back any related_content_type records present in the data model                        
                        StructuredContent_ContentType related_content_type = null;
                        if (relationship.a_content_type_id == primary_content_type.id)
                        {
                            related_content_type = relationship.StructuredContent_ContentType;
                        }
                        if (relationship.a_content_type_id == primary_content_type.id)
                        {
                            related_content_type = relationship.StructuredContent_ContentType1;
                        }

                        string related_content_type_name = related_content_type.plural.ToLower();
                        if (primary_content_item[related_content_type_name] != null)
                        {
                            foreach (var related_content_item in primary_content_item[related_content_type_name])
                            {
                                sqlHelper.SaveManyToManyRelationship(relationship, primary_content_type, related_content_type, (int)primary_content_item.id, (int)related_content_item.id);
                            }
                        }
                    }

                    if (relationship.key == "o2m")
                    {
                        StructuredContent_ContentType related_content_type = relationship.StructuredContent_ContentType1;

                        // clear foreign keys
                        sqlHelper.DeleteOneToManyRelationship(primary_content_type, related_content_type, (int)primary_content_item.id);

                        // set foreign keys
                        string related_content_type_name = related_content_type.plural.ToLower();
                        foreach (var related_content_item in primary_content_item[related_content_type_name])
                        {
                            sqlHelper.SaveOneToManyRelationship(primary_content_type, related_content_type, (int)primary_content_item.id, (int)related_content_item.id);
                        }
                    }
                }

                dc.SubmitChanges();
                int id = primary_content_item.id;

                return Request.CreateResponse(HttpStatusCode.OK, id); // send back the updated record id
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

                StructuredContent_ContentType content_type = dc.StructuredContent_ContentTypes.Where(i => i.name == contentType).SingleOrDefault();

                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                sqlHelper.DeleteContentItem(content_type, id);

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
