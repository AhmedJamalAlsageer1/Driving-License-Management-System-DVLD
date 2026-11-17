using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.Local_License.New_Local_License
{
    public class DTO_NewLicenseRequirements_BL
    {
        public int LicenseID { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
