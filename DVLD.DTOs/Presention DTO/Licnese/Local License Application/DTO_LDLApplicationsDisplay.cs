using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Applications
{
    public class DTO_LDLApplicationsDisplay
    {
        public int L_D_LApplicationID { get; set; }
        public string DrivingClass { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public byte PassedTests { get; set; }
        public string Status { get; set; }

        public DTO_LDLApplicationsDisplay()
        {
            L_D_LApplicationID = -1;
            DrivingClass = null;
            NationalNo = null;
            FullName = null;
            ApplicationDate = DateTime.Now;
            PassedTests = 0;
            Status = null;
        }

        public void Clear()
        {
            L_D_LApplicationID = -1;
            DrivingClass = null;
            NationalNo = null;
            FullName = null;
            ApplicationDate = DateTime.Now;
            PassedTests = 0;
            Status = null;
        }
    }
}
