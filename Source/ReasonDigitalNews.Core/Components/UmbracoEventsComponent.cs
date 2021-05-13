using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace ReasonDigitalNews.Core.Components
{
    public class UmbracoEventsComponent : IComponent
    {
        public void Initialize()
        {
            ContentService.Published += ContentService_Published;
            ContentService.Unpublishing += ContentService_UnPublishing;
            ContentService.Deleted += ContentService_Deleted;
            ContentService.Moved += ContentService_Moved;
        }

        public void Terminate()
        {
        }

        private static void ContentService_Published(IContentService sender, ContentPublishedEventArgs e)
        {
            _ = MakePublishedPost();
        }

        private static void ContentService_UnPublishing(IContentService sender, PublishEventArgs<IContent> e)
        {
        }

        private static void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
        }

        private static void ContentService_Moved(IContentService sender, MoveEventArgs<IContent> e)
        {
        }

        private static async Task MakePublishedPost()
        {
            //to post where and in what format ??
            // do I need to create then viewmodel for the pages I have available, massage this data and served clean in this model in json?
            //how will the FE make a navigation for example ?

            var responseString = await "http://www.example.com/recepticle.aspx"
                .PostUrlEncodedAsync(new { thing1 = "hello", thing2 = "world" })
                .ReceiveString();

            var person = await "https://api.com"
                .AppendPathSegment("person")
                .SetQueryParams(new {a = 1, b = 2})
                .WithOAuthBearerToken("my_oauth_token")
                .PostJsonAsync(new
                {
                    first_name = "Claire",
                    last_name = "Underwood"
                });
            //.ReceiveJson<Person>();
        }
        
    }
}