using ReasonDigitalNews.Core.Extensions;
using System;
using System.Linq;
using ReasonDigitalNews.Core.Constants;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace ReasonDigitalNews.Core.ContentFinders
{
    public class ApplicationContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedRequest request)
        {
            if (request == null) { return false; }

            var path = request.Uri.GetAbsolutePathDecoded().Replace($"/{ApplicationConstants.ApiRoutePrefix}/", "/").Replace($"/{ApplicationConstants.ApiRoutePrefix}", "/");
            var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var publishedContent = request.UmbracoContext.Content.GetByRoute(path);

            if (publishedContent.HasValue())
            {
                request.PublishedContent = publishedContent;
            }
            else if (parts.Length == 2)
            {
                var nodeIdValue = parts.LastOrDefault();
                int nodeId = int.TryParse(nodeIdValue, out nodeId) ? nodeId : 0;

                request.PublishedContent = request.UmbracoContext.Content.GetById(nodeId);
            }

            return request.PublishedContent != null;
        }
    }
}