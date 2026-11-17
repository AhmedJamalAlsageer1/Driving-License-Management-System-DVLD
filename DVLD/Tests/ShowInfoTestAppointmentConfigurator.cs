using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Tests
{
    internal static class ShowInfoTestAppointmentConfigurator
    {
        public static void Configure(enTestType testype, frmShowInfoTestAppointment frm)
        {
            ConfigrueFormInfoAppointment(testype, frm);
        }
        private static void ConfigrueFormInfoAppointment(enTestType testype, frmShowInfoTestAppointment frm)
        {
            switch (testype)
            {
                case enTestType.Theory:
                    frm.TestTitle = "Vision Test Appointment";
                    frm.ImageTitle = (Image) Properties.Resources.eye_128.Clone();
                    break;

                case enTestType.Simulator:
                    frm.TestTitle = "Written Test Appointment";
                    frm.ImageTitle = (Image) Properties.Resources.test_128.Clone();
                    break;

                case enTestType.Field:
                    frm.TestTitle = "Street Test Appointment";
                    frm.ImageTitle = (Image) Properties.Resources.test_car_128.Clone();
                    break;
            }
        }

    }
}
