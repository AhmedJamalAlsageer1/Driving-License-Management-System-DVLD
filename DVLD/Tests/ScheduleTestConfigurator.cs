using DVLD.Classess.Helpers;
using DVLD.DTOs.Tests;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    internal static class ScheduleTestConfigurator
    {
        public static bool Configure(frmScheduleTest frm, enMode mode, enTestType testType, int ID,
            out DTOTestAppointment_Init dto_Clone)
        {
            dto_Clone = null;

            if (ID == -1)
            {
                MessageBoxHelper.Show("رقم معرف غير صحيح.", enMessageType.Error,
                    "خطأ", true);
                return false;
            }

            Result<DTOTestAppointment_Init> restulAppointment = null;
            if (mode == enMode.Add)
            {
                restulAppointment = clsTestAppointmentBL.GetTestAppointment_InitBy_LDLAppID
                    (ID, testType);

                if (!restulAppointment.isSuccess || restulAppointment.Value == null)
                {
                    MessageBoxHelper.Show(restulAppointment, "خطأ", true);
                    return false;
                }

                if (restulAppointment.Value.isLocked == false)
                {
                    MessageBoxHelper.Show("لك موعد مسجل بالفعل لا يمكنك تسجيل موعد آخر", enMessageType.Warning,
                        "تنويه", true);

                    return false;
                }
            }
            else if(mode == enMode.Update)
            {
                restulAppointment = clsTestAppointmentBL.GetTestAppointment_Init_ByAppointmentID
                    (ID, testType);

                if (!restulAppointment.isSuccess || restulAppointment.Value == null)
                {
                    MessageBoxHelper.Show(restulAppointment, "خطأ", true);
                    return false;
                }
            }

            if (restulAppointment == null)
                return false;

            DTOTestAppointment_Init dto = restulAppointment.Value;
            dto_Clone = dto.Clone();
            ConfiguraImageScheduleTest(testType, frm);

            if (mode == enMode.Update)
                HandleWithUpdateMode(frm, dto, mode);
            else
                HandleWithAddMode(frm, dto, mode);

                return true;
        }

        private static void ConfiguraImageScheduleTest(enTestType testType, frmScheduleTest frm)
        {
            switch(testType)
            {
                case enTestType.Theory:
                    frm.gbTestTypeText = "Vision Test";
                    frm.ImageTitleTest = (Image)Properties.Resources.eye_128.Clone();
                    break;

                case enTestType.Simulator:
                    frm.gbTestTypeText = "Written Test";
                    frm.ImageTitleTest = (Image)Properties.Resources.test_128.Clone();
                    break;

                case enTestType.Field:
                    frm.gbTestTypeText = "Street Test";
                    frm.ImageTitleTest = (Image)Properties.Resources.test_car_128.Clone();
                    break;
            }
        }
        private static void LoadData(frmScheduleTest frm, DTOTestAppointment_Init dto)
        {
            frm._LDL_AppID = dto.LocalDrivingLicenseApplicationID.ToString();
            frm.ClassName = dto.DrivingLiceseName.ToString();
            frm.FullName = dto.FullName.ToString();
            frm.Trial = dto.Trial.ToString();
            frm.Fees = dto.TestFees.ToString() + "$";

            if (dto.Trial > 0)
            {
                frm.TitleTest = "Schedule Retake Test";
                frm.gbRetake = true;
                frm.TotalFees = ((dto.R_Take_AppFees ?? 0) + dto.TestFees).ToString("0.00") + "$";
            }
            else
            {
                frm.TitleTest = "Schedule Test";
                frm.TotalFees = dto.TestFees.ToString("0.00") + "$";
                frm.gbRetake = false;
            }
        }
        private static void HandleWithUpdateMode(frmScheduleTest frm, DTOTestAppointment_Init dto, enMode Mode)
        {
            LoadData(frm, dto);
            frm.R_Take_AppID = dto.R_Take_AppID.HasValue? dto.R_Take_AppID.ToString() : "N/A";
            frm.R_Take_AppFees = (dto.R_Take_AppFees ?? 0).ToString() + "$";
            frm.appointmentDate_dtp = dto.AppointmentDate.Value;

            if (dto.isLocked.Value)
            {
                frm.Date = false;
                frm.Save = false;
                frm.AppointmentLocked = true;
            }
            else
            {
                frm.Date = true;
                frm.Save = true;
                frm.AppointmentLocked = false;
            }
        }

        private static void HandleWithAddMode(frmScheduleTest frm, DTOTestAppointment_Init dto, enMode Mode)
        {
            LoadData(frm, dto);
            frm.AppointmentLocked = false;
            frm.R_Take_AppID = dto.R_Take_AppID.HasValue ? dto.R_Take_AppID.ToString() : "N/A";
            if (dto.Trial > 0)
                frm.R_Take_AppFees = dto.R_Take_AppFees.ToString() + "$";
            else
                frm.R_Take_AppFees = "0$";
        }

    }
}
