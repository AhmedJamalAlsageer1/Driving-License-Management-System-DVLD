using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Tests.Configrue_frmReTakeTest
{
    internal class vmTakeTest
    {
        public string LDL_AppID { get; set; }
        public string DriverLicenseName { get; set; }
        public string Name { get; set; }
        public string Trial { get; set; }
        public string AppointmentDate { get; set; }
        public string Fees { get; set; }
        public string TypeTestName { get; set; }
        public Image TestTypeImage { get; set; }

        public vmTakeTest()
        {
            LDL_AppID = null;
            DriverLicenseName = null;
            Name = null;
            Trial = null;
            AppointmentDate = null;
            Fees = null;
            TypeTestName = null;
            TestTypeImage = null;
        }
    }
}
