using ReasonDigitalNews.Core.ContentFinders;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace ReasonDigitalNews.Core.Composers
{
    public class ContentFindersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.SetContentLastChanceFinder<NotFoundContentFinder>();

            composition.ContentFinders()
                .InsertBefore<ContentFinderByUrl, ApplicationContentFinder>();
        }
    }
}