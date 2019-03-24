using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OmmaLicenseValidator;

namespace IntegrationTests
{
    public class OmmaIntegrationTests
    {
        [Test]
        public async Task TestLicenseValidatorAsync()
        {
            const string licenseNumber = "AP-FAAA-N1J7-RMCF-B4JX-1QS7-RK";
            var validator = new LicenseValidator();
            try
            {
                var response = await validator.GetValidatorResponseAsync(licenseNumber);
                var isValid = await validator.ValidAsync(licenseNumber);
                var date = new DateTime(2020, 10, 8);

                Assert.IsTrue(response?.ExpirationDate.Date == date);
                Assert.IsTrue(isValid);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }

        [Test]
        public void TestLicenseValidator()
        {
            const string licenseNumber = "AP-FAAA-N1J7-RMCF-B4JX-1QS7-RK";
            var validator = new LicenseValidator();
            try
            {
                var response = validator.GetValidatorResponse(licenseNumber);
                var isValid = validator.Valid(licenseNumber);
                var date = new DateTime(2020, 10, 8);

                Assert.IsTrue(response?.ExpirationDate.Date == date);
                Assert.IsTrue(isValid);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
    }
}