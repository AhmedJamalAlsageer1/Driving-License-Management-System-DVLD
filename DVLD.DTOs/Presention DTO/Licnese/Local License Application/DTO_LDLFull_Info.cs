using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Applications
{
    public class DTO_LDLFull_Info
    {
        public int LDL_AppID { get; set; }
        public int AppID { get; set; }
        public string Applicant { get; set; }
        public string LicenseDrivingName { get; set; }
        public byte PassedTest { get; set; }
        public string StatusApplication { get; set; }
        public string ApplicationType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime StatusDate { get; set; }
        public string CreatedBy { get; set; }
        public decimal Fees { get; set; }

        public DTO_LDLFull_Info()
        {
            LDL_AppID = -1;
            AppID = -1;
            LicenseDrivingName = null;
            PassedTest = 0;
            Applicant = null;
            StatusApplication = null;
            ApplicationType = null;
            ApplicationDate = DateTime.MinValue;
            StatusDate = DateTime.MinValue;
            CreatedBy = null;
            Fees = -1;
        }

        public DTO_LDLFull_Info Clone()
        {
            DTO_LDLFull_Info dto = new DTO_LDLFull_Info();

            dto.LDL_AppID = this.LDL_AppID;
            dto.AppID = this.AppID;
            dto.LicenseDrivingName = this.LicenseDrivingName;
            dto.PassedTest = this.PassedTest;
            dto.Applicant = this.Applicant;
            dto.StatusApplication = this.StatusApplication;
            dto.ApplicationType = this.ApplicationType;
            dto.ApplicationDate = this.ApplicationDate;
            dto.StatusDate = this.StatusDate;
            dto.CreatedBy = this.CreatedBy;
            dto.Fees = this.Fees;

            return dto;
        }
    }
}
