using DVLD.DTOs.License.Local_License;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.ReNew
{
    public class DTO_RenewLicense_Info
    {
        public int RL_ApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal ApplicationFees { get; set; }
        public decimal LicenseFees { get; set; }
        // معلومات التجديد
        public int RenewedLicenseID { get; set; }
        public int OldLicenseID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CreatedBy { get; set; }

        public DTO_RenewLicense_Info()
        {

        }

        public DTO_RenewLicense_Info(decimal _ApplicationFees)
        {
            DateTime time = DateTime.MinValue;
            RL_ApplicationID = -1;
            ApplicationDate = time;
            IssueDate = time;
            ApplicationFees = _ApplicationFees;
            LicenseFees = -1;
            RenewedLicenseID = -1;
            OldLicenseID = -1;
            ExpirationDate = time;
            CreatedBy = -1;
        }

        public DTO_RenewLicense_Info(DTO_RenewLicense_Config dto)
        {
            DateTime Now = DateTime.MinValue;
            RL_ApplicationID = -1;
            ApplicationDate = Now;
            IssueDate = Now;
            ApplicationFees = dto.ApplicationFees;
            LicenseFees = dto.LicenseFees;
            RenewedLicenseID = -1;
            OldLicenseID = -1;
            ExpirationDate = Now;
            CreatedBy = -1;
        }
    }
}
