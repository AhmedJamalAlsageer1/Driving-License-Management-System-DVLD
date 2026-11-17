using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.Int_License
{
    public interface I_IntLicense
    {
        int Int_LicenseApplicationID { get; set; }
        int Int_LicenseID { get; set; }
        int Local_LicenseID { get; set; }
        int DriverID { get; set; }
        int PersonID { get; set; }
        enApplicationTypes ApplicationType { get; set; }
        enApplicationStatus ApplicationStatus { get; set; }
        DateTime LastStatusDate { get; set; }
        decimal AppFees { get; set; }
        DateTime IssueDate { get; set; }
        DateTime ExpirationDate { get; set; }
        bool IsActive { get; set; }
        int CreatedBy { get; set; }
    }
}
