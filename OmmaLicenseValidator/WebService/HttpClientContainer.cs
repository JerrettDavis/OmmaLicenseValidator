using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace OmmaLicenseValidator.WebService
{
    public class HttpClientContainer 
    {
        private readonly CookieContainer _cookieContainer;
        public HttpClient Client { get; }

        public HttpClientContainer()
        {
            _cookieContainer = new CookieContainer();
            var clientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            };
            
            Client = new HttpClient(clientHandler);
        }

        public IEnumerable<Cookie> GetCookies(string url)
        {
            var uri = new Uri(url);

            return _cookieContainer.GetCookies(uri).Cast<Cookie>();

        }
    }
}
