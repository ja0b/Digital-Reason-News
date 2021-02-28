using ReasonDigitalNews.Core.Constants;
using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using ReasonDigitalNews.Core.Models.Shared;
using ReasonDigitalNews.Core.Models.ViewModels;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace ReasonDigitalNews.Core.Builders
{
    public interface IContentResponseBuilder
    {
        ContentResponseViewModel BuildContentResponse(object data, IPublishedContent node);

        PageInfoData GetPageInfoData(IPublishedContent source, string reminderText);
    }

    public class ContentResponseBuilder : IContentResponseBuilder
    {
        public ContentResponseViewModel BuildContentResponse(object data, IPublishedContent node)
        {
            var contentResponseViewModel = new ContentResponseViewModel
            {
                Data = data,
                PageInfo = GetPageInfoData(node, ApplicationConstants.ReminderForHijack)
            };

            return contentResponseViewModel;
        }

        public PageInfoData GetPageInfoData(IPublishedContent source, string reminderText)
        {
            var pageInfoData = new PageInfoData();

            if (source is IPublishedContent node)
            {
                pageInfoData.PageType = node.ContentType.Alias;
                pageInfoData.ChildrenNodes = GetChildrenNodes(node);
                pageInfoData.Reminder = reminderText;
            }

            return pageInfoData;
        }

        private static IEnumerable<ChildNode> GetChildrenNodes(IPublishedContent source)
        {
            var pageInfoData = new List<ChildNode>();
            var childrenNodes = source.Children;

            if (!childrenNodes.HasAny())
            {
                return pageInfoData;
            }

            foreach (var publishedContent in childrenNodes)
            {
                var isApiRequest = string.Empty.IsApiRequest();

                var url = isApiRequest
                    ? $"/{ApplicationConstants.ApiRoutePrefix}{publishedContent.Url()}"
                    : publishedContent.Url();

                if (publishedContent.ContentType.Alias.Equals(NewsPage.ModelTypeAlias) && isApiRequest)
                {
                    url = url.Replace($"{publishedContent.UrlSegment}", $"{publishedContent.Id}");
                }

                var urlWithDomain = $"{string.Empty.GetUrlWithDomain()}{url}";

                var childNode = new ChildNode
                { Name = publishedContent.Name, Url = url, UrlWithDomain = urlWithDomain };

                pageInfoData.Add(childNode);
            }

            return pageInfoData;
        }
    }
}