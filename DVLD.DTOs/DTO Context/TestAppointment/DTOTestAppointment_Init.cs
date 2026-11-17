using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Tests
{
    public class DTOTestAppointment_Init
    {
        public int? TestAppointmentID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public SharedLayer.enums.enTestType TypeTestID { get; set; }
        public string DrivingLiceseName { get; set; }
        public decimal TestFees { get; set; }
        public string FullName { get; set; }
        public bool? isLocked { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public byte Trial { get; set; }
        public int? R_Take_AppID { get; set; }
        public decimal? R_Take_AppFees { get; set; }

        public DTOTestAppointment_Init()
        {
            TestAppointmentID = null;
            LocalDrivingLicenseApplicationID = -1;
            TypeTestID = SharedLayer.enums.enTestType.UnKnown;
            DrivingLiceseName = null;
            TestFees = -1m;
            FullName = null;
            isLocked = null;
            AppointmentDate = null;
            Trial = 0;
            R_Take_AppID = null;
            R_Take_AppFees = null;
        }

        public DTOTestAppointment_Init Clone()
        {
            return new DTOTestAppointment_Init
            {
                TestAppointmentID = this.TestAppointmentID,
                LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID,
                TypeTestID = this.TypeTestID,
                DrivingLiceseName = this.DrivingLiceseName,
                TestFees = this.TestFees,
                FullName = this.FullName,
                isLocked = this.isLocked,
                AppointmentDate = this.AppointmentDate,
                Trial = this.Trial,
                R_Take_AppID = this.R_Take_AppID,
                R_Take_AppFees = this.R_Take_AppFees,
            };
        }
    }
}
