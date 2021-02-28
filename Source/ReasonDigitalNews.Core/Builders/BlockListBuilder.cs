using ReasonDigitalNews.Core.Extensions;
using ReasonDigitalNews.Core.Models.CmsModels;
using ReasonDigitalNews.Core.Models.Shared;
using System.Collections.Generic;
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
                }
            }

            return blockListValues;
        }
    }
}