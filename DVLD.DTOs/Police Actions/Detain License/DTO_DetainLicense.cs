using DVLD.DTOs.Detain_License;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Police_Actions.Detain_License
{
    public class DTO_DetainLicense : I_PoliceRequest
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public enenPoliceActionType PoliceActionType { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
