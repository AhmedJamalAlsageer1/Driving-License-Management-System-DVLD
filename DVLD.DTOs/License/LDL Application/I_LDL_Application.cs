using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.LDL_Application
{
    public interface I_LDL_Application
    {
        int ApplicationID { get; set; }
        int PersonID { get; set; }
        int LDLApplicationID { get; set; }
        DateTime ApplicationDate { get; set; }
        enApplicationTypes ApplicationTypeID { get; set; }
        SharedLayer.enums.enApplicationStatus ApplicationStatus { get; set; }
        DateTime LastStatusDate { get; set; }
        decimal PaidFees { get; set; }
        enLicenseClass LicenseClassID { get; set; }
        int CreatedByUserID { get; set; }
    }
}
