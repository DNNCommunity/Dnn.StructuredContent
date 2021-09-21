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

    /// <summary>
    /// Web API to manage Visualizers.
    /// </summary>
    [JsonCamelCaseSerializer]
    public class VisualizerController : DnnApiController
    {
        private readonly DataContext dataContext = new DataContext();
        private readonly ISQLHelper sqlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizerController"/> class.
        /// </summary>
        /// <param name="sqlHelper">The sql helper to use.</param>
        public VisualizerController(ISQLHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        /// <summary>
        /// Gets a list of Visualizers.
        /// </summary>
        /// <param name="skip">How many items to skip (for paging).</param>
        /// <param name="take">How many items to take (for paging).</param>
        /// <returns>A list of <see cref="VisualizerDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int? skip = null, int? take = null)
        {
            try
            {
                var query = this.dataContext.StructuredContent_Visualizers.AsQueryable();

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

                var dtos = new List<VisualizerDto>();

                foreach (var item in query)
                {
                    var dto = item.ToDto();
                    dtos.Add(dto);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, dtos);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Gets a single Visualizer.
        /// </summary>
        /// <param name="moduleId">The ID of the module for which to get the template for.</param>
        /// <returns>A single <see cref="VisualizerDto"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int moduleId)
        {
            try
            {
                var visualizer = this.dataContext.StructuredContent_Visualizers.Where(i => i.ModuleId == moduleId).SingleOrDefault();
                if (visualizer == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                var dto = visualizer.ToDto();

                var visualizerTemplate = visualizer.StructuredContent_VisualizerTemplate;
                if (visualizerTemplate != null)
                {
                    var content_type = visualizerTemplate.StructuredContent_ContentType;

                    switch (visualizerTemplate.ContentSize)
                    {
                        case "single":
                            var contentItem = this.sqlHelper.SelectDynamicItem(content_type, visualizer.ItemId.GetValueOrDefault());
                            if (contentItem == null)
                            {
                                return this.Request.CreateResponse(HttpStatusCode.NotFound);
                            }

                            switch (visualizerTemplate.Language)
                            {
                                case "liquid":
                                    var template = Template.Parse(visualizerTemplate.Template);
                                    dto.Content = template.Render(Hash.FromDictionary(contentItem));

                                    break;

                                case "razor":
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case "multiple":

                            IDictionary<string, object> content = new Dictionary<string, object>();

                            var items = this.sqlHelper.SelectDynamicList(content_type, string.Empty).ToList();
                            content.Add("items", items);

                            switch (visualizerTemplate.Language)
                            {
                                case "liquid":
                                    var template = Template.Parse(visualizerTemplate.Template);
                                    dto.Content = template.Render(Hash.FromDictionary(content));

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

                return this.Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Creates a visualizer.
        /// </summary>
        /// <param name="dto"><see cref="VisualizerDto"/>.</param>
        /// <returns>The created <see cref="VisualizerDto"/>.</returns>
        [HttpPost]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Post(VisualizerDto dto)
        {
            try
            {
                var item = dto.ToItem(null);

                this.dataContext.StructuredContent_Visualizers.InsertOnSubmit(item);

                this.dataContext.SubmitChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, item.ToDto());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Updates an existing visualizer.
        /// </summary>
        /// <param name="dto"><see cref="VisualizerDto"/>.</param>
        /// <returns>The updated <see cref="VisualizerDto"/>.</returns>
        [HttpPut]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Put(VisualizerDto dto)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Visualizers.Where(i => i.Id == dto.Id).SingleOrDefault();
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

        /// <summary>
        /// Deletes an existing visualizer.
        /// </summary>
        /// <param name="id">The ID of the visualizer to delete.</param>
        /// <returns>OK or InternalServerError.</returns>
        [HttpDelete]
        [AllowAnonymous]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var item = this.dataContext.StructuredContent_Visualizers.Where(i => i.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                this.dataContext.StructuredContent_Visualizers.DeleteOnSubmit(item);
                this.dataContext.SubmitChanges();

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
