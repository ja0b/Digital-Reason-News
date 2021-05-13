using ReasonDigitalNews.Core.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace ReasonDigitalNews.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UmbracoEventsComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<UmbracoEventsComponent>();
        }
    }
}