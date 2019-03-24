using System;
using System.Threading.Tasks;
using OmmaLicenseValidator;
using OmmaLicenseValidator.Responses;

namespace ValidatorSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            var validator = new ValidatorChecker();

            Console.WriteLine("Enter a license number to check");
            var license = Console.ReadLine();

            while (!exit)
            {
                validator.GetLicenseDetails(license).Wait();
                Console.WriteLine("Enter another license number or enter 'N' to exit");
                license = Console.ReadLine() ?? "n";

                if (license.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    exit = true;
            }
        }

        
    }

    public class ValidatorChecker
    {
        private readonly LicenseValidator _licenseValidator;

        public ValidatorChecker()
        {
            _licenseValidator = new LicenseValidator();
        }

        public async Task GetLicenseDetails(string license)
        {
            Console.WriteLine("Fetching...");

            var result = await _licenseValidator.GetValidatorResponseAsync(license);

            Console.WriteLine("License Details ---");
            Console.WriteLine($"County Issued: {result.CountyIssued}");
            Console.WriteLine($"Expiration Date: {result.ExpirationDate.ToShortDateString()}");
            Console.WriteLine($"Approved: {result.Approved}");

            Console.WriteLine();
        }
    }
}
