using ReasonDigitalNews.Core.Models.Shared;
using System;
using System.Collections.Generic;

namespace ReasonDigitalNews.Core.Models.ViewModels.SuggestionViewModels
{
    public class NewsPageSuggestionViewModel : PageInfoModel
    {
        public string Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public IEnumerable<BlockElement> Content { get; set; }
    }
}