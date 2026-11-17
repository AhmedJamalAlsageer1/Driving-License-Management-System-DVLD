using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.Local_License.Replacement_For
{
    public class DTO_Replacement_LocalLicense : I_LocalLicense
    {
        public int LocalLicenseID { get; set; }
        public int OldLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public DateTime IssueDate { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public decimal ApplicationFees { get; set; }
        public enLicenseClass LicenseClass { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReasonLicense IssueReason { get; set; }

        // معلومات التجديد
        public enApplicationTypes ApplicationTypes { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
