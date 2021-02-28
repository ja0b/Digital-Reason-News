using ReasonDigitalNews.Core.Models.Shared;

namespace ReasonDigitalNews.Core.Models.ViewModels.SuggestionViewModels
{
    public class BaseContentSuggestionViewModel : PageInfoModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}