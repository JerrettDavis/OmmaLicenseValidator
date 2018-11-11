using System;

namespace OmmaLicenseValidator.Responses
{
    public class ValidatorResponse
    {
        public string CountyIssued { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Approved { get; set; }
    }
}
