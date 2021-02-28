using System;
using System.Collections.Generic;
using ReasonDigitalNews.Core.Models.Shared;

namespace ReasonDigitalNews.Core.Models.ViewModels.HijackViewModels
{
    public class NewsPageViewModel
    {
        public string Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public IEnumerable<BlockElement> Content { get; set; }
    }
}