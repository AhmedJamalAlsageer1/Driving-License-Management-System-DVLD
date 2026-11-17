using DVLD.DTOs.License.Int_License;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License
{
    public class DTO_IntLicense : I_IntLicense
    {
        public int Int_LicenseApplicationID { get; set; }
        public int Int_LicenseID { get; set; }
        public int Local_LicenseID { get; set; }
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public enApplicationTypes ApplicationType { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal AppFees { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
