using ReasonDigitalNews.Core.UrlProviders;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace ReasonDigitalNews.Core.Composers
{
    public class UrlProvidersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.UrlProviders().Insert<ApplicationUrlProvider>();
        }
    }
}