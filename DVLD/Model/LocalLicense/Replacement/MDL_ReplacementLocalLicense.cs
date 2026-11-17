using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model.LocalLicense.Replacement
{
    public class MDL_ReplacementLocalLicense
    {
        public string R_ApplicationID { get; set; }
        public string ApplicationDate { get; set; }
        public string ApplicationFees { get; set; }
        public string ReplacedLicenseID { get; set; }
        public string OldLicenseID { get; set; }
        public string Createdby { get; set; }

        public MDL_ReplacementLocalLicense (decimal AppFees)
        {
            R_ApplicationID = "-1";
            ApplicationDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ApplicationFees = AppFees.ToString();
            ReplacedLicenseID = "-1";
            OldLicenseID = "-1";
            Createdby = SharedLayer.Context.AppContext.CurrentUser.UserName;
        }

        public MDL_ReplacementLocalLicense(decimal AppFees, int _OldLicenseID)
        {
            R_ApplicationID = "-1";
            ApplicationDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ApplicationFees = AppFees.ToString();
            ReplacedLicenseID = "-1";
            OldLicenseID = _OldLicenseID.ToString();
            Createdby = SharedLayer.Context.AppContext.CurrentUser.UserName;
        }

        public MDL_ReplacementLocalLicense(DTO_ApplicationReplacement_Info dto, int _OldLicenseID)
        {
            R_ApplicationID = dto.R_ApplicationID.ToString();
            ApplicationDate = dto.ApplicationDate.ToString("dd/MM/yyyy");
            ApplicationFees = dto.AppFees.ToString() + "$";
            ReplacedLicenseID = dto.R_LicenseID.ToString();
            OldLicenseID = _OldLicenseID.ToString();
            Createdby = SharedLayer.Context.AppContext.CurrentUser.UserName;
        }
    }
}
