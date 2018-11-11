using HtmlAgilityPack;
using OmmaLicenseValidator.Common;
using OmmaLicenseValidator.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OmmaLicenseValidator.WebService
{
    /// <summary>
    /// The service worker does the heavy lifting. It makes the web requests and
    /// parses out the responses. It may be prudent to separate out the parsing,
    /// but it's not overly complex, so really there's not much need.
    /// </summary>
    public class ServiceWorker
    {
        #region Constants
        private const string OmmaVerifyUrl = "https://ommaverify.ok.gov/";
        private const string ValueName = "value";
        private const string LicenseFieldName = "content";
        private const string TokenFieldName = "__RequestVerificationToken";
        private const string Selector = "//input[@name='__RequestVerificationToken']";

        private const string CountyPath = "//div[contains(@class, 'info-1')]//div";
        private const string DatePath = "//div[contains(@class, 'info-2')]//div";
        private const string ApprovedPath = "//div[contains(@class, 'info-3')]//div";

        private const string ApprovedString = "Approved";
        #endregion

        private readonly HttpClientContainer _httpClient;

        public ServiceWorker(HttpClientContainer httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Returns the details about the entered license.
        /// </summary>
        /// <param name="license">The license number of the id to check.</param>
        /// <returns>The license's details if valid.</returns>
        public async Task<ValidatorResponse> GetLicenseValidationResponse(string license)
        {
            var result = await _httpClient.Client.GetAsync(OmmaVerifyUrl);
            var verificationToken = await GetRequestVerificationToken(result);
            var postResult = await PostValidationResponse(license, verificationToken);
            var response = await GetValidationReponse(postResult);

            return response;
        }

        private static async Task<string> GetRequestVerificationToken(HttpResponseMessage result)
        {
            var doc = await GetHtmlDocument(result);
            var node = doc.DocumentNode.SelectSingleNode(Selector);

            return node.GetAttributeValue(ValueName, "");
        }

        private async Task<HttpResponseMessage> PostValidationResponse(string license, string verificationToken)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>(LicenseFieldName, license),
                new KeyValuePair<string, string>(TokenFieldName, verificationToken)
            });

            return await _httpClient.Client.PostAsync(OmmaVerifyUrl, content);
        }

        private static async Task<ValidatorResponse> GetValidationReponse(HttpResponseMessage result)
        {
            var doc = await GetHtmlDocument(result);
            var county = doc.DocumentNode?.SelectSingleNode(CountyPath)?.LastChild?.InnerText?.CleanString();
            var date = doc.DocumentNode?.SelectSingleNode(DatePath)?.LastChild?.InnerText?.CleanString();
            var approved = doc.DocumentNode?.SelectSingleNode(ApprovedPath)?.LastChild?.InnerText?.CleanString();

            return new ValidatorResponse
            {
                CountyIssued = county,
                ExpirationDate = date.ToDateTime(),
                Approved = ApprovedString.Equals(approved, StringComparison.InvariantCultureIgnoreCase)
            };
        }

        private static async Task<HtmlDocument> GetHtmlDocument(HttpResponseMessage result)
        {
            var stream = await result.Content.ReadAsStreamAsync();
            var doc = new HtmlDocument();
            doc.Load(stream);

            return doc;
        }
    }
}
