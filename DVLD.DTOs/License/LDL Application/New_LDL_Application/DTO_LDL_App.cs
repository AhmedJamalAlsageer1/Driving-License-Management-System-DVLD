using DVLD.DTOs.License.LDL_Application;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTO_LDL_App : I_LDL_Application
    {
        public int ApplicationID { get; set; }
        public int PersonID { get; set; }
        public int LDLApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public enApplicationTypes ApplicationTypeID { get; set; }
        public SharedLayer.enums.enApplicationStatus ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public enLicenseClass LicenseClassID { get; set; }
        public int CreatedByUserID { get; set; } 
        public int PassedTestCount { get; set; }

        public DTO_LDL_App()
        {

        }

        public DTO_LDL_App (int personid, int ApplicationId, DateTime applicationdate, enApplicationTypes applicationTypeId, SharedLayer.enums.enApplicationStatus applicationstatus, 
            DateTime lastStatusdate, decimal paidfees, enLicenseClass licenseClass, int createdbyuserid, int PassedTestCount)
        {
            PersonID = personid;
            ApplicationID = ApplicationId;
            ApplicationDate = applicationdate;
            ApplicationTypeID = applicationTypeId;
            ApplicationStatus = applicationstatus;
            LastStatusDate = lastStatusdate;
            PaidFees = paidfees;
            LicenseClassID = licenseClass;
            CreatedByUserID = createdbyuserid;
            this.PassedTestCount = PassedTestCount;
        }


        public void Clear()
        {
            PersonID = -1;
            ApplicationID = -1;
            ApplicationDate = DateTime.MinValue;
            ApplicationTypeID = enApplicationTypes.UnKnown;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.Unknown;
            LastStatusDate = DateTime.MinValue;
            PaidFees = -1;
            CreatedByUserID = -1;
            PassedTestCount = -1;
        }

        public DTO_LDL_App Clone()
        {
            return new DTO_LDL_App
            {
                ApplicationID = this.ApplicationID,
                PersonID = this.PersonID,
                LDLApplicationID = this.LDLApplicationID,
                ApplicationDate = this.ApplicationDate,
                ApplicationTypeID = this.ApplicationTypeID,
                ApplicationStatus = this.ApplicationStatus,
                LastStatusDate = this.LastStatusDate,
                PaidFees = this.PaidFees,
                LicenseClassID = this.LicenseClassID,
                CreatedByUserID = this.CreatedByUserID,
                PassedTestCount = this.PassedTestCount
            };
        }
    }
}
