using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.Int_License
{
    public class DTO_IntLicenseRequirements
    {
        public enLicenseClass LicenseClass { get; set; }
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public bool IsActive { get; set; }
        public decimal ApplicationFees { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExipritionDate { get; set; }
        public byte LicenseValidityLength { get; set; }
    }
}
