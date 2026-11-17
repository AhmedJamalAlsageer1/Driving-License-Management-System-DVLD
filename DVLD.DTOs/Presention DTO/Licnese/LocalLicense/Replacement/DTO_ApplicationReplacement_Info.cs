using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement
{
    public class DTO_ApplicationReplacement_Info
    {
        public int R_ApplicationID { get; set; }
        public int R_LicenseID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal AppFees { get; set; }
        public decimal Createdby { get; set; }
    }
}
