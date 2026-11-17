using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License
{
    public class DTOLicenseDriverInfo
    {
        public int LicenseID { get; set; }
        public enLicenseClass LicenseClassID { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public bool Gender { get; set; }
        public DateTime IssueDate { get; set; }
        public enIssueReasonLicense IssueReason { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DriverID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsDetained { get; set; }
        public string ImagePath { get; set; }

        public DTOLicenseDriverInfo()
        {

        }
    }
}
