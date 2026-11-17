using DVLD.DTOs.License;
using SharedLayer.Helpers.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model.MDL_DriverLicenseInfo
{
    public class MDL_LicenseDriver_Info
    {
        public string LicenseID { get; set; }
        public string LicenseClass { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public string Gender { get; set; }
        public string IssueDate { get; set; }
        public string IssueReason { get; set; }
        public string Notes { get; set; }
        public string IsActive { get; set; }
        public string DateOfBirth { get; set; }
        public string DriverID { get; set; }
        public string ExpirationDate { get; set; }
        public string IsDetained { get; set; }
        public string ImagePath { get; set; }

        public MDL_LicenseDriver_Info(DTOLicenseDriverInfo dto)
        {
            string FormattingDate = "dd/MM/yyyy";
            LicenseID = dto.LicenseID.ToString();
            LicenseClass = LicenseClassMapper.ToDisplayString(dto.LicenseClassID);
            FullName = dto.FullName;
            NationalNo = dto.NationalNo;
            Gender = GendorMapper.ToDispalyString(dto.Gender);
            IssueDate = dto.IssueDate.ToString(FormattingDate);
            IssueReason = IssueReasonMapper.ToDisplayString(dto.IssueReason);
            Notes = dto.Notes;
            IsActive = dto.IsActive ? "مفعلة" : "غير مفعلة";
            DateOfBirth = dto.DateOfBirth.ToString(FormattingDate);
            DriverID = dto.DriverID.ToString();
            ExpirationDate = dto.ExpirationDate.ToString(FormattingDate);
            IsDetained = dto.IsDetained ? "Yes" : "No";
            ImagePath = dto.ImagePath;
        }
    }
}
