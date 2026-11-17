using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.LicensesHistory
{
    public class DTO_InternationalLicenseHistory
    {
        public int InternationalLicenseID { get; set; }
        public int UsingLocalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; } 
        public bool IsActive { get; set; }

        public DTO_InternationalLicenseHistory()
        {

        }
    }
}
