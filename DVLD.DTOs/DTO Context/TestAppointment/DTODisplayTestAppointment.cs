using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Tests
{
    public class DTODisplayTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }

        public DTODisplayTestAppointment()
        {
            TestAppointmentID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = -1m;
            IsLocked = true;
        }
    }
}
