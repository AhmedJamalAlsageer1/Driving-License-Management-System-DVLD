using DVLD.DTOs.License.Local_License;
using SharedLayer;
using SharedLayer.enums;
using System;

namespace DVLD.DTOs.License
{
    public class DTO_LocalLicense : I_LocalLicense
    {
        public int LocalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int LDL_AppID { get; set; }
        public int DriverID { get; set; }
        public enApplicationTypes ApplicationTypes { get; set; }
        public enLicenseClass LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReasonLicense IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

        public DTO_LocalLicense()
        {

        }
    }
}
