# Omma License Validator

This is a simple web wrapper to interface with the Oklahoma Medical Marijuana Authority license validation application. It only returns details that are accessible from the publicly accessible validation tool at [https://ommaverify.ok.gov](https://ommaverify.ok.gov).

## Getting Started

Operation is relatively straight forward. To get started simply reference the project and initialize a new `LicenseValidator`. From there you can just call the `Valid` or `GetValidatorResponse` method to get details about the entered number.

### Example

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
            
            var result = await _licenseValidator.GetValidatorReponse(license);
            
            Console.WriteLine("License Details ---");
            Console.WriteLine($"County Issued: {result.CountyIssued}");
            Console.WriteLine($"Expiration Date: {result.ExpirationDate.ToShortDateString()}");
            Console.WriteLine($"Approved: {result.Approved}");
            
            Console.WriteLine();
        }
    }

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

