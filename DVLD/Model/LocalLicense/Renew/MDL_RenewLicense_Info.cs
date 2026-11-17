using DVLD.DTOs.License.Int_License.DTO_ReNewLocalLicense;
using DVLD.DTOs.License.ReNew;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model.LocalLicense.Renew
{
    public class MDL_RenewLicense_Info
    {
        public string R_L_ApplicationID { get; set; }
        public string ApplicationDate { get; set; }
        public string IssueDate { get; set; }
        public string ApplicationFees { get; set; }
        public string LicenseFees { get; set; }
        // معلومات التجديد
        public string RenewedLicenseID { get; set; }
        public string OldLicenseID { get; set; }
        public string ExpirationDate { get; set; }
        public string TotalFees { get; set; }
        public string CreatedBy { get; set; }

        public MDL_RenewLicense_Info(DTO_RenewLicense_Config dto, int LicenseID)
        {
            DateTime Now = DateTime.Now;
            string FormattingDate = "dd/MM/yyyy";

            R_L_ApplicationID = "[???]";
            ApplicationDate = Now.ToString(FormattingDate);
            IssueDate = Now.ToString(FormattingDate);
            ApplicationFees = dto.ApplicationFees.ToString() + "$";
            LicenseFees = dto.LicenseFees.ToString() + "$";
            RenewedLicenseID = "-1";
            OldLicenseID = LicenseID.ToString();
            ExpirationDate = Now.AddYears(dto.ValidityLength).ToString(FormattingDate);
            CreatedBy = SharedLayer.Context.AppContext.CurrentUser.UserName;
            TotalFees = (dto.LicenseFees + dto.ApplicationFees).ToString() + "$";
        }

        public MDL_RenewLicense_Info(DTO_RenewLicense_Info dto, int PreviousLicenseID)
        {
            DateTime Now = DateTime.Now;
            string FormattingDate = "dd/MM/yyyy";

            R_L_ApplicationID = dto.RL_ApplicationID.ToString();
            ApplicationDate = Now.ToString(FormattingDate);
            IssueDate = Now.ToString(FormattingDate);
            LicenseFees = dto.LicenseFees.ToString() + "$";
            ApplicationFees = dto.ApplicationFees.ToString() + "$";
            RenewedLicenseID = dto.RenewedLicenseID.ToString();
            OldLicenseID = PreviousLicenseID.ToString() + "$";
            ExpirationDate =  dto.ExpirationDate.ToString(FormattingDate);
            CreatedBy = SharedLayer.Context.AppContext.CurrentUser.UserName;
            TotalFees = (dto.LicenseFees + dto.ApplicationFees).ToString() + "$";
        }
    }

}
