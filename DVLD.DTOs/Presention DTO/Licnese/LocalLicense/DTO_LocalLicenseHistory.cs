using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.LicensesHistory
{
    public class DTO_LocalLicenseHistory
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public string ClassName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public DTO_LocalLicenseHistory()
        {

        }
    }
}
