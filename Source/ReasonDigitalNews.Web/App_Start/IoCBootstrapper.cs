using ReasonDigitalNews.Core.Builders;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace ReasonDigitalNews.Web
{
    public class DependencyComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            IoCBootstrapper.IoCSetup(composition);
        }
    }

    public class IoCBootstrapper
    {
        public static void IoCSetup(Composition composition)
        {
            RegisterBuilders(composition);
        }

        private static void RegisterBuilders(IRegister composition)
        {
            composition.Register<IBlockListBuilder, BlockListBuilder>();
            composition.Register<IContentResponseBuilder, ContentResponseBuilder>();
        }
    }
}