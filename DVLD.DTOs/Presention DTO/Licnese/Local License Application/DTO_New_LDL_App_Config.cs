using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTO_New_LDL_App_Config
    {
        public int ApplicationID { get; set; }
        public int PersonID { get; set; }
        public int LDLApplicationID { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public enApplicationTypes ApplicationTypeID { get; set; }
        public SharedLayer.enums.enApplicationStatus ApplicationStatus { get; set; }
        public DateTime? LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public enLicenseClass LicenseClassID { get; set; }
        public int CreatedByUserID { get; set; } 
        public int PassedTestCount { get; set; }

        public DTO_New_LDL_App_Config()
        {
            PersonID = -1;
            ApplicationID = -1;
            ApplicationTypeID = enApplicationTypes.UnKnown;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.Unknown;
            PaidFees = -1;
            LicenseClassID = enLicenseClass.Unknown;
            CreatedByUserID = -1;
            PassedTestCount = -1;
        }

        public DTO_New_LDL_App_Config(int personid, int ApplicationId, DateTime applicationdate, enApplicationTypes applicationTypeId, SharedLayer.enums.enApplicationStatus applicationstatus, 
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
            ApplicationDate = null;
            ApplicationTypeID = enApplicationTypes.UnKnown;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.Unknown;
            LastStatusDate = null;
            PaidFees = -1;
            CreatedByUserID = -1;
            PassedTestCount = -1;
        }

        public DTO_New_LDL_App_Config Clone()
        {
            return new DTO_New_LDL_App_Config
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
