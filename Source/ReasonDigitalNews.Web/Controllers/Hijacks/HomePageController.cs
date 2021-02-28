using Newtonsoft.Json;
using ReasonDigitalNews.Core.Builders;
using ReasonDigitalNews.Core.Constants;
using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using ReasonDigitalNews.Core.Models.ViewModels.HijackViewModels;
using ReasonDigitalNews.Core.Models.ViewModels.SuggestionViewModels;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core.Mapping;
using Umbraco.Web.Mvc;

namespace ReasonDigitalNews.Web.Controllers.Hijacks
{
    public class HomePageController : RenderMvcController
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly UmbracoMapper _umbracoMapper;
        private readonly IContentResponseBuilder _contentResponseBuilder;

        public HomePageController(JsonSerializerSettings jsonSerializerSettings, UmbracoMapper umbracoMapper, IContentResponseBuilder contentResponseBuilder)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _umbracoMapper = umbracoMapper;
            _contentResponseBuilder = contentResponseBuilder;
        }

        public virtual ActionResult Index(HomePage model)
        {
            var contentResponseViewModel = string.Empty.IsApiRequest()
                ? (object)_umbracoMapper.Map<BaseContentSuggestionViewModel>(model)
                : _contentResponseBuilder.BuildContentResponse(_umbracoMapper.Map<BaseContentViewModel>(model), model);

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(contentResponseViewModel, _jsonSerializerSettings),
                ContentEncoding = Encoding.UTF8,
                ContentType = ApplicationConstants.ContentTypeApplicationJson
            };
        }
    }
}