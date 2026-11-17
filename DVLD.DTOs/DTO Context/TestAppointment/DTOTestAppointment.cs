using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Tests
{
    public class DTOTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public string ClassName { get; set; }
        public enTestType TypeTestID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? R_Take_AppID { get; set; }
        public decimal? R_Take_AppFees { get; set; }

        public DTOTestAppointment()
        {
            TestAppointmentID = -1;
            ClassName = null;
            TypeTestID = enTestType.UnKnown;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.MinValue;
            PaidFees = -1m;
            IsLocked = true;
            R_Take_AppID = null;
            R_Take_AppFees = null;
        }
    }
}
