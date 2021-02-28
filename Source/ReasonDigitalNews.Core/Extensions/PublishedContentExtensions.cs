using Umbraco.Core.Models.PublishedContent;

namespace ReasonDigitalNews.Core.Extensions
{
    public static class PublishedContentExtensions
    {
        public static bool HasValue(this IPublishedContent publishedContent)
            => publishedContent != null;
    }
}