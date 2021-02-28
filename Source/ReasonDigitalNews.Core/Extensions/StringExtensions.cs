using System.Web;
using ReasonDigitalNews.Core.Constants;

namespace ReasonDigitalNews.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static string GetUrlWithDomain(this string input)
        {
            var httpRequest = HttpContext.Current.Request;
            var domainUrl = $"{httpRequest.Url.Scheme}://{httpRequest.Url.Host}";

            return domainUrl;
        }

        public static bool IsApiRequest(this string input)
        {
            var httpRequest = HttpContext.Current.Request;
            var isApiUrl = httpRequest.Url.AbsolutePath.Contains($"/{ApplicationConstants.ApiRoutePrefix}/");
            
            return isApiUrl;
        }
    }
}