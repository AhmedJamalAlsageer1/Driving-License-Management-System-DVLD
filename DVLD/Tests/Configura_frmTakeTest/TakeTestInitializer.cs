using DVLD.Classess.Helpers;
using DVLD.DTOs.Tests;
using DVLD.Properties;
using DVLD.Tests.Configrue_frmReTakeTest;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System.Drawing;

namespace DVLD.Tests.Configura_frmTakeTest
{
    internal class TakeTestInitializer
    {
        public static bool Configure(int AppointmentID, enTestType TestTypeID, vmTakeTest TakeTest)
        {
            if (AppointmentID <= 0)
            {
                MessageBoxHelper.Show("بيانات مدخلة غير صحيحة.", enMessageType.Error, "خطأ تقني", true);
                return false;
            }

            Result<DTOTestAppointment_Init> result_AppointmentID_Into = clsTestAppointmentBL.GetTestAppointment_Init_ByAppointmentID(AppointmentID, TestTypeID);
            if (!result_AppointmentID_Into.isSuccess || result_AppointmentID_Into.Value == null)
            {
                MessageBoxHelper.Show(result_AppointmentID_Into, "خطأ", true);
                return false;
            }

            DTOTestAppointment_Init dto = result_AppointmentID_Into.Value;
            FillvmWithData(dto, TakeTest);
            return true;
        }

        private static void FillvmWithData(DTOTestAppointment_Init dto, vmTakeTest TakeTest)
        {
            TakeTest.LDL_AppID = dto.LocalDrivingLicenseApplicationID.ToString();
            TakeTest.DriverLicenseName = dto.DrivingLiceseName;
            TakeTest.Name = dto.FullName.ToString();
            TakeTest.Trial = dto.Trial.ToString();
            TakeTest.AppointmentDate = dto.AppointmentDate.Value.ToString("dd/MM/yyyy") ?? "??/??/????";
            TakeTest.Fees = dto.TestFees.ToString() + "$";

            SetTestTypeInfo(dto.TypeTestID, TakeTest);
        }

        private static void SetTestTypeInfo(enTestType testType, vmTakeTest takeTest)
        {
            switch(testType)
            {
                case enTestType.Theory:
                    takeTest.TypeTestName = "Vision Test";
                    takeTest.TestTypeImage = (Image) Resources.eye_128.Clone();
                    break;

                case enTestType.Simulator:
                    takeTest.TypeTestName = "Written Test";
                    takeTest.TestTypeImage = (Image)Resources.test_128.Clone();
                    break;

                case enTestType.Field:
                    takeTest.TypeTestName = "Street Test";
                    takeTest.TestTypeImage = (Image)Resources.test_car_128.Clone();
                    break;
            }
        }
    }
}
