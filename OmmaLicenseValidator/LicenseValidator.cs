using OmmaLicenseValidator.Responses;
using OmmaLicenseValidator.WebService;
using System.Threading.Tasks;

namespace OmmaLicenseValidator
{
    public class LicenseValidator
    {
        private readonly ServiceWorker _serviceWorker;

        public LicenseValidator()
        {
            _serviceWorker = new ServiceWorker(new HttpClientContainer());
        }

        /// <summary>
        /// Determines if the entered license is valid.
        /// </summary>
        /// <param name="license">The license number of the id to check.</param>
        /// <returns>True if the license is currently valid.</returns>
        public async Task<bool> Valid(string license) =>
            (await _serviceWorker.GetLicenseValidationResponse(license)).Approved;

        /// <summary>
        /// Fetches the given license's details.
        /// </summary>
        /// <param name="license">The license number of the id to fetch.</param>
        /// <returns>A response object denoting the publicly accessible details
        /// about a license. </returns>
        public async Task<ValidatorResponse> GetValidatorReponse(string license) =>
            await _serviceWorker.GetLicenseValidationResponse(license);
    }
}
