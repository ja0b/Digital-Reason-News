using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using ReasonDigitalNews.Core.Constants;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace ReasonDigitalNews.Core.UrlProviders
{
    public class ApplicationUrlProvider : IUrlProvider
    {
        public UrlInfo GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            if (!content.HasValue())
            {
                return null;
            }

            var urls = GetOtherUrls(umbracoContext, content.Id, current);
            return urls.FirstOrDefault();
        }

        public IEnumerable<UrlInfo> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            var node = umbracoContext.Content.GetById(id);
            var cultureFromDomains = node.GetCultureFromDomains(current);

            switch (node.ContentType.Alias)
            {
                case HomePage.ModelTypeAlias:
                    {
                        var originalUrl = new UrlInfo("/", true, cultureFromDomains);
                        var newUrl = new UrlInfo($"/{ApplicationConstants.ApiRoutePrefix}", true, cultureFromDomains);
                        var urls = new List<UrlInfo> { originalUrl, newUrl};
                        return urls;
                    }

                case NewsListingPage.ModelTypeAlias:
                    {
                        var newsListingPage = node as NewsListingPage;
                        var newsPageNameUrl = newsListingPage.UrlSegment;

                        var originalUrl = new UrlInfo($"/{newsPageNameUrl}", true, cultureFromDomains);
                        var newUrl = new UrlInfo($"/{ApplicationConstants.ApiRoutePrefix}/{newsPageNameUrl}", true, cultureFromDomains);

                        var urls = new List<UrlInfo> { originalUrl, newUrl };

                        return urls;
                    }

                case NewsPage.ModelTypeAlias:
                    {
                        var newsPage = node as NewsPage;
                        var newsListingPageUrl = newsPage.Parent.Url();
                        var newsPageNameUrl = newsPage.UrlSegment;

                        var originalUrl = new UrlInfo($"{newsListingPageUrl}/{newsPageNameUrl}", true, cultureFromDomains);
                        var newUrl = new UrlInfo($"/{ApplicationConstants.ApiRoutePrefix}{newsListingPageUrl}/{newsPage.Id}", true, cultureFromDomains);

                        var urls = new List<UrlInfo> { originalUrl, newUrl };

                        return urls;
                    }
            }

            return Enumerable.Empty<UrlInfo>();
        }
    }
}