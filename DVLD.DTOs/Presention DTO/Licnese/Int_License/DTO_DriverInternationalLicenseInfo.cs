using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.International_License
{
    public class DTO_DriverInternationalLicenseInfo
    {
        public int Int_LicenseID { get; set; }
        public int LicenseID { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public bool Gendor { get; set; }
        public DateTime IssueDate { get; set; }
        public int ApplicationId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DriverID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ImagePath { get; set; }


        public DTO_DriverInternationalLicenseInfo()
        {

        }
    }
}
