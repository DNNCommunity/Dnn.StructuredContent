using DotLiquid;
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
using System.Dynamic;

namespace StructuredContent
{
    //[SupportedModules("StructuredContent")]
    public class VisualizerController : DnnApiController
    {
        DataContext dc = new DataContext();
        SQLHelper sqlHelper = new SQLHelper();

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(Nullable<int> skip = null, Nullable<int> take = null)
        {
            try
            {
                var query = dc.StructuredContent_Visualizers.AsQueryable();


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

                List<VisualizerDTO> dtos = new List<VisualizerDTO>();

                foreach (StructuredContent_Visualizer item in query)
                {
                    VisualizerDTO dto = item.ToDto();
                    dtos.Add(dto);
                }

                return Request.CreateResponse(HttpStatusCode.OK, dtos);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int module_id)
        {
            try
            {
                StructuredContent_Visualizer visualizer = dc.StructuredContent_Visualizers.Where(i => i.module_id == module_id).SingleOrDefault();
                if (visualizer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                VisualizerDTO dto = visualizer.ToDto();

                StructuredContent_VisualizerTemplate visualizer_template = visualizer.StructuredContent_VisualizerTemplate;
                if (visualizer_template != null)
                {
                    var content_type = visualizer_template.StructuredContent_ContentType;

                    switch (visualizer_template.content_size)
                    {
                        case "single":
                            IDictionary<string, object> content_item = sqlHelper.SelectDynamicItem(content_type, visualizer.item_id.GetValueOrDefault());
                            if (content_item == null)
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound);
                            }

                            switch (visualizer_template.language)
                            {
                                case "liquid":
                                    Template template = Template.Parse(visualizer_template.template);
                                    dto.content = template.Render(Hash.FromDictionary(content_item));

                                    break;

                                case "razor":
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case "multiple":

                            IDictionary<string, object> content = new Dictionary<string, object>();

                            List<IDictionary<string, object>> items = sqlHelper.SelectDynamicList(content_type, string.Empty).ToList();
                            content.Add("items", items);

                            switch (visualizer_template.language)
                            {
                                case "liquid":
                                    Template template = Template.Parse(visualizer_template.template);
                                    dto.content = template.Render(Hash.FromDictionary(content));

                                    break;

                                case "razor":
                                    break;

                                default:
                                    break;
                            }

                            break;

                        default:
                            break;
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, dto);
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
        public HttpResponseMessage Post(VisualizerDTO dto)
        {
            try
            {
                StructuredContent_Visualizer content_type = dto.ToItem(null);

                dc.StructuredContent_Visualizers.InsertOnSubmit(content_type);

                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, content_type.ToDto());
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
        public HttpResponseMessage Put(VisualizerDTO dto)
        {
            try
            {
                StructuredContent_Visualizer item = dc.StructuredContent_Visualizers.Where(i => i.id == dto.id).SingleOrDefault();
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
                StructuredContent_Visualizer content_type = dc.StructuredContent_Visualizers.Where(i => i.id == id).SingleOrDefault();
                if (content_type == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dc.StructuredContent_Visualizers.DeleteOnSubmit(content_type);
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