using ReasonDigitalNews.Core.Mapping.Maps;
using Umbraco.Core.Composing;
using Umbraco.Core.Mapping;

namespace ReasonDigitalNews.Core.Composers
{
    public class ApplicationMapDefinitionsComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<ApplicationMapDefinitions>();
        }
    }
}