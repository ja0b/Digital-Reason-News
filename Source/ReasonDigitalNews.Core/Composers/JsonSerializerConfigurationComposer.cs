using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReasonDigitalNews.Core.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace ReasonDigitalNews.Core.Composers
{
    public class JsonSerializerConfigurationComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            RegisterJsonSettings(composition);

            composition.Components().Append<JsonSerializerConfigurationComponent>();
        }

        private static void RegisterJsonSettings(Composition composition)
        {
            composition.Register(_ => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            }, Lifetime.Singleton);
        }
    }
}