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
        public ValidatorResponse GetLicenseValidationResponse(string license)
        {
            var result = _httpClient.Client.GetAsync(OmmaVerifyUrl).Result;
            var verificationToken = GetRequestVerificationToken(result);
            var postResult = PostValidationResponse(license, verificationToken);
            var response = GetValidationResponse(postResult);

            return response;
        }

        /// <summary>
        /// Returns the details about the entered license.
        /// </summary>
        /// <param name="license">The license number of the id to check.</param>
        /// <returns>The license's details if valid.</returns>
        public async Task<ValidatorResponse> GetLicenseValidationResponseAsync(string license)
        {
            var result = await _httpClient.Client.GetAsync(OmmaVerifyUrl);
            var verificationToken = await GetRequestVerificationTokenAsync(result);
            var postResult = await PostValidationResponseAsync(license, verificationToken);
            var response = await GetValidationResponseAsync(postResult);

            return response;
        }

        private static string GetRequestVerificationToken(HttpResponseMessage result)
        {
            var doc = GetHtmlDocument(result);
            var node = doc.DocumentNode.SelectSingleNode(Selector);

            return node.GetAttributeValue(ValueName, "");
        }

        private static async Task<string> GetRequestVerificationTokenAsync(HttpResponseMessage result)
        {
            var doc = await GetHtmlDocumentAsync(result);
            var node = doc.DocumentNode.SelectSingleNode(Selector);

            return node.GetAttributeValue(ValueName, "");
        }

        private HttpResponseMessage PostValidationResponse(string license, string verificationToken)
        {
            var content = GetFormUrlEncodedContent(license, verificationToken);

            return _httpClient.Client.PostAsync(OmmaVerifyUrl, content).Result;
        }

        private async Task<HttpResponseMessage> PostValidationResponseAsync(string license, string verificationToken)
        {
            var content = GetFormUrlEncodedContent(license, verificationToken);

            return await _httpClient.Client.PostAsync(OmmaVerifyUrl, content);
        }

        private static FormUrlEncodedContent GetFormUrlEncodedContent(string license, string verificationToken)
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>(LicenseFieldName, license),
                new KeyValuePair<string, string>(TokenFieldName, verificationToken)
            });
        }

        private static ValidatorResponse GetValidationResponse(HttpResponseMessage result)
        {
            var doc = GetHtmlDocument(result);

            return ShapeHtmlDocument(doc);
        }

        private static async Task<ValidatorResponse> GetValidationResponseAsync(HttpResponseMessage result)
        {
            var doc = await GetHtmlDocumentAsync(result);

            return ShapeHtmlDocument(doc);
        }

        private static ValidatorResponse ShapeHtmlDocument(HtmlDocument doc)
        {
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

        private static HtmlDocument GetHtmlDocument(HttpResponseMessage result)
        {
            var stream = result.Content.ReadAsStreamAsync().Result;
            var doc = new HtmlDocument();
            doc.Load(stream);

            return doc;
        }

        private static async Task<HtmlDocument> GetHtmlDocumentAsync(HttpResponseMessage result)
        {
            var stream = await result.Content.ReadAsStreamAsync();
            var doc = new HtmlDocument();
            doc.Load(stream);

            return doc;
        }
    }
}
