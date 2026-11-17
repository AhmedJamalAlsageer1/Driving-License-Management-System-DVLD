using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Presention_DTO.Licnese.Detain_License
{
    public class DTO_ReleaseLicenseRequirements
    {
        public int DetainID { get; set; }
        public bool IsReleased { get; set; }
        public decimal Appfees { get; set; }
        public int personID { get; set; }
        public decimal Fines { get; set; }
    }
}
