using Flagger.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flagger
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create()
        {
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());

            client.Timeout = TimeSpan.FromSeconds(3);

            return client;
        }
    }
}
