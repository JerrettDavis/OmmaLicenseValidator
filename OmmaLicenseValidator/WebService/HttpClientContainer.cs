using System.Net;
using System.Net.Http;

namespace OmmaLicenseValidator.WebService
{
    public class HttpClientContainer 
    {
        public HttpClient Client { get; }

        public HttpClientContainer()
        {
            var cookieContainer = new CookieContainer();
            var clientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            
            Client = new HttpClient(clientHandler);
        }
    }
}
