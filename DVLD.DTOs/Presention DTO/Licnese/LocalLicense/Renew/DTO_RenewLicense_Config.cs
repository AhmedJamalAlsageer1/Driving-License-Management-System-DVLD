using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew
{
    public class DTO_RenewLicense_Config
    {
        public decimal ApplicationFees { get; set; }
        public int ValidityLength { get; set; }
        public decimal LicenseFees { get; set; }
    }
}
