using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Web.Http;
using Umbraco.Core.Composing;

namespace ReasonDigitalNews.Core.Components
{
    public class JsonSerializerConfigurationComponent : IComponent
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonSerializerConfigurationComponent(JsonSerializerSettings jsonSerializerSettings)
            => _jsonSerializerSettings = jsonSerializerSettings;

        public void Initialize()
        {
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings = _jsonSerializerSettings,
                UseDataContractJsonSerializer = false
            });
        }

        public void Terminate()
        {
        }
    }
}