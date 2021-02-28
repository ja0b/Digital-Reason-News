using ReasonDigitalNews.Core.Builders;
using ReasonDigitalNews.Core.Constants;
using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using ReasonDigitalNews.Core.Models.ViewModels.HijackViewModels;
using ReasonDigitalNews.Core.Models.ViewModels.SuggestionViewModels;
using Umbraco.Core.Mapping;

namespace ReasonDigitalNews.Core.Mapping.Maps
{
    public class ApplicationMapDefinitions : IMapDefinition
    {
        private readonly IBlockListBuilder _blockListBuilder;
        private readonly IContentResponseBuilder _contentResponseBuilder;

        public ApplicationMapDefinitions(IBlockListBuilder blockListBuilder, IContentResponseBuilder contentResponseBuilder)
        {
            _blockListBuilder = blockListBuilder;
            _contentResponseBuilder = contentResponseBuilder;
        }

        public void DefineMaps(UmbracoMapper mapper)
        {
            mapper.Define<HomePage, BaseContentViewModel>((source, context) => new BaseContentViewModel
            {
                Title = source.PageTitle.HasValue() ? source.PageTitle : source.Name,
                Description = source.PageDescription
            });

            mapper.Define<HomePage, BaseContentSuggestionViewModel>((source, context) => new BaseContentSuggestionViewModel
            {
                Title = source.PageTitle.HasValue() ? source.PageTitle : source.Name,
                Description = source.PageDescription,
                //Added this to display the reminder and children nodes data but this is not required in my suggestion
                PageInfo = _contentResponseBuilder.GetPageInfoData(source, ApplicationConstants.ReminderForSuggestion)
            });

            mapper.Define<NewsListingPage, BaseContentViewModel>((source, context) => new BaseContentViewModel
            {
                Title = source.PageTitle.HasValue() ? source.PageTitle : source.Name,
                Description = source.PageDescription
            });

            mapper.Define<NewsListingPage, BaseContentSuggestionViewModel>((source, context) => new BaseContentSuggestionViewModel
            {
                Title = source.PageTitle.HasValue() ? source.PageTitle : source.Name,
                Description = source.PageDescription,
                //Added this to display the reminder and children nodes data but this is not required in my suggestion
                PageInfo = _contentResponseBuilder.GetPageInfoData(source, ApplicationConstants.ReminderForSuggestion)
            });

            mapper.Define<NewsPage, NewsPageViewModel>((source, context) => new NewsPageViewModel
            {
                Name = source.Name,
                PublishedDate = source.PublishedDate,
                Content = _blockListBuilder.BuildBlockListValues(source.Blocks)
            });

            mapper.Define<NewsPage, NewsPageSuggestionViewModel>((source, context) => new NewsPageSuggestionViewModel
            {
                Name = source.Name,
                PublishedDate = source.PublishedDate,
                Content = _blockListBuilder.BuildBlockListValues(source.Blocks),
                //Added this to display the reminder and children nodes data but this is not required in my suggestion
                PageInfo = _contentResponseBuilder.GetPageInfoData(source, ApplicationConstants.ReminderForSuggestion)
            });
        }
    }
}