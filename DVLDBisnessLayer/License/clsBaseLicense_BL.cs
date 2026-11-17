using DVLDBisnessLayer.BaseLicenseRequest;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License
{
    internal abstract class clsBaseLicense_BL : clsBaseApplication_BL
    {
        public int LocalLicenseID { get; protected set; }
        public int DriverID { get; protected set; }
        public DateTime IssueDate { get; protected set; }
        public DateTime ExpirationDate { get; protected set; }
        public bool IsActive { get; protected set; }
    }
}
