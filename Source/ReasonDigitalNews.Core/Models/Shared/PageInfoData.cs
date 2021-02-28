using System.Collections.Generic;

namespace ReasonDigitalNews.Core.Models.Shared
{
    public class PageInfoData
    {
        public string PageType { get; set; }
        public string Reminder { get; set; }
        public IEnumerable<ChildNode> ChildrenNodes { get; set; }
    }
}