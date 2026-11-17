using DVLD.DTOs.Presention_DTO.Licnese.Release_Detain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model.MDL_RelaseDetainedLicense
{
    public class MDL_RelaseDetainedLicense
    {
        public string DetainID { get; set; }
        public string DetainDate { get; set; }
        public string ApplicationFees { get; set; }
        public string TotalFees { get; set; }
        public string LicenseID { get; set; }
        public string Createdby { get; set; }
        public string DetainFines { get; set; }
        public string ReleaseDetainedBy {get; set;}


        public MDL_RelaseDetainedLicense(DTO_ReleaseDetain_Info dto, int _LicenseID)
        {
            DetainID = dto.DetainID.ToString();
            DetainDate = dto.DatainDate.ToString("dd/MM/yyyy");
            ApplicationFees = dto.AppFees.ToString("G29") + "$";
            TotalFees = (dto.AppFees + dto.FineFees).ToString("G29") + "$";
            LicenseID = _LicenseID.ToString();
            Createdby = dto.Createdby.ToString();
            DetainFines = dto.FineFees.ToString("G29") + "$";
            ReleaseDetainedBy = dto.ReleaseByUser ?? "[???]";
        }

        public MDL_RelaseDetainedLicense(DTO_ReleaseDetain_Info dto)
        {
            DetainID = dto.DetainID.ToString();
            DetainDate = dto.DatainDate.ToString("dd/MM/yyyy");
            ApplicationFees = dto.AppFees.ToString("G29") + "$";
            TotalFees = (dto.AppFees + dto.FineFees).ToString("G29") + "$";
            LicenseID = dto.LicenseID.ToString();
            Createdby = dto.Createdby.ToString();
            DetainFines = dto.FineFees.ToString("G29") + "$";
            ReleaseDetainedBy = dto.ReleaseByUser ?? "[???]";
        }
    }
}
