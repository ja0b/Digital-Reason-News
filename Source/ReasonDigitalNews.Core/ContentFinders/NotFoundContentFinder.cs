using Umbraco.Web.Routing;

namespace ReasonDigitalNews.Core.ContentFinders
{
    public class NotFoundContentFinder : IContentLastChanceFinder
    {
        public bool TryFindContent(PublishedRequest contentRequest)
        {
            if (!contentRequest.Is404)
            {
                return contentRequest.PublishedContent != null;
            }

            return contentRequest.PublishedContent != null;
        }
    }
}