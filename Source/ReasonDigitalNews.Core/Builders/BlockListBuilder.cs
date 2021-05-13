using Newtonsoft.Json;
using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using ReasonDigitalNews.Core.Models.Shared;
using ReasonDigitalNews.Core.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.Blocks;
using Umbraco.Web;

namespace ReasonDigitalNews.Core.Builders
{
    public interface IBlockListBuilder
    {
        IEnumerable<BlockElement> BuildBlockListValues(BlockListModel blocks);
    }

    public class BlockListBuilder : IBlockListBuilder
    {
        public IEnumerable<BlockElement> BuildBlockListValues(BlockListModel blocks)
        {
            var blockListValues = new List<BlockElement>();

            if (!blocks.HasAny())
            {
                return blockListValues;
            }

            foreach (var block in blocks)
            {
                var blockElement = new BlockElement
                {
                    BlockType = block.Content.ContentType.Alias
                };

                switch (block.Content.ContentType.Alias)
                {
                    case TextBlock.ModelTypeAlias:
                        {
                            var textBlock = new TextBlock(block.Content);

                            blockElement.BlockValue = textBlock.Text.ToString();
                            blockListValues.Add(blockElement);

                            break;
                        }

                    case ImageBlock.ModelTypeAlias:
                        {
                            var imageBlock = new ImageBlock(block.Content);

                            blockElement.BlockValue = imageBlock.Image.Url();
                            blockListValues.Add(blockElement);

                            break;
                        }

                    case CarouselBlock.ModelTypeAlias:
                        {
                            var carouselBlock = new CarouselBlock(block.Content);
                            var carouselItemsBlock = carouselBlock.CarouselItems.Select(x => x.Content)
                                .OfType<CarouselItemBlock>();

                            var carouselItems = carouselItemsBlock.Select(carouselItemBlock =>
                                    new CarouselItemBlockViewModel
                                    {
                                        Image = carouselItemBlock.Image.Url()
                                    })
                                .ToList();

                            blockElement.BlockValue = JsonConvert.SerializeObject(carouselItems);
                            blockListValues.Add(blockElement);

                            break;
                        }
                }
            }

            return blockListValues;
        }
    }
}