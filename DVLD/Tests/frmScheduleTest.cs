using DVLD.Classess.Helpers;
using DVLD.DTOs.Tests;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {
        internal Image ImageTitleTest
        {
            get => pbTestTitle.Image;
            
            set
            {
                if(pbTestTitle.Image != null && !ReferenceEquals(pbTestTitle.Image, value))
                {
                    pbTestTitle.Image.Dispose();
                    pbTestTitle.Image = null;
                }
                pbTestTitle.Image = value;
            }
        }
        internal string TitleTest
        {
            get => lblTestTitle.Text;
            set => lblTestTitle.Text = value;
        }
        internal bool AppointmentLocked
        {
            get => lblAppointmentLocked.Visible;
            set => lblAppointmentLocked.Visible = value;
        }
        internal bool gbRetake
        {
            get => groupBox2.Enabled;
            set => groupBox2.Enabled = value;
        }
        internal string gbTestTypeText
        {
            get => gbTypeTest.Text;
            set => gbTypeTest.Text = value;
        }
        internal string _LDL_AppID
        {
            get => lblD_L_App_ID.Text;
            set => lblD_L_App_ID.Text = value;
        }
        internal string ClassName
        {
            get => lblDrivingClass.Text;
            set => lblDrivingClass.Text = value;
        }
        internal string FullName
        {
            get => lblName.Text;
            set => lblName.Text = value;
        }
        internal string Trial
        {
            get => lblTrial.Text;
            set => lblTrial.Text = value;
        }
        internal bool Date
        {
            get => dateTimePicker1.Enabled;
            set => dateTimePicker1.Enabled = value;
        }
        internal DateTime appointmentDate_dtp
        {
            get => dateTimePicker1.Value;
            set => dateTimePicker1.Value = value;
        }
        internal string Fees
        {
            get => lblFees.Text;
            set => lblFees.Text = value;
        }
        internal string R_Take_AppID
        {
            get => lbl_R_Take_TestAppID.Text;
            set => lbl_R_Take_TestAppID.Text = value;
        }
        internal string R_Take_AppFees
        {
            get => lbl_R_TakeFees.Text;
            set => lbl_R_TakeFees.Text = value;
        }
        internal string TotalFees
        {
            get => lblTotalFees.Text;
            set => lblTotalFees.Text = value;
        }
        internal bool Save
        {
            get => btnSave.Enabled;
            set => btnSave.Enabled = value;
        }

        enMode _mode;
        enTestType _testype;
        DTOTestAppointment_Init dto_inti;
        public Action OnDataChanged { get; set; }

        public frmScheduleTest(enMode Mode, enTestType testType, int ID)
        {
            InitializeComponent();


            this.Load += (sender, e) =>
            {
                if(!ScheduleTestConfigurator.Configure(this, Mode, testType, ID, out DTOTestAppointment_Init dto))
                {
                    this.Close();
                    this.Dispose();
                }
                _mode = Mode;
                _testype = testType;
                dto_inti = dto;
            };
        }

        private DTOTestAppointment FillDataAddNew()
        {
            DTOTestAppointment dto = new DTOTestAppointment();

            dto.LocalDrivingLicenseApplicationID = dto_inti.LocalDrivingLicenseApplicationID;
            dto.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            dto.AppointmentDate = dateTimePicker1.Value;
            dto.TypeTestID = _testype;
            dto.PaidFees = dto_inti.TestFees;

            return dto;
        }

        private bool SaveAddNewAppointment()
        {
            clsTestAppointmentBL NewTestAppointment = new clsTestAppointmentBL(FillDataAddNew());

            decimal ReTake_Fees = dto_inti.R_Take_AppFees ?? 0;
            Result<bool> resultOfSave = NewTestAppointment.Save(ReTake_Fees);
            if (!resultOfSave.isSuccess)
            {
                MessageBoxHelper.Show(resultOfSave, null, true);
                return false;
            }
            return true;
        }

        private bool SaveUpdateAppointment()
        {
            if(dto_inti.TestAppointmentID == null || dto_inti.TestAppointmentID == -1)
            {
                MessageBoxHelper.Show("معرف الموعد فارغ يرجى تحديد موعد صحيح", enMessageType.Error,
                    "خطأ غير مقصود", true);
                return false;
            }

            Result<clsTestAppointmentBL> resultTestAppointment = clsTestAppointmentBL.FindObjectTestAppointmentByAppoinID(dto_inti.TestAppointmentID.Value);
            if(!resultTestAppointment.isSuccess)
            {
                MessageBoxHelper.Show(resultTestAppointment,"خطأ", true);
                return false;
            }
            clsTestAppointmentBL TestAppointment = resultTestAppointment.Value;
            Result <bool> result_UpdateAppointmentDate = TestAppointment.SetNewAppointmentDate(dateTimePicker1.Value);
            if(!result_UpdateAppointmentDate.isSuccess)
            {
                MessageBoxHelper.Show(result_UpdateAppointmentDate, "خطأ", true);
                return false;
            }

            Refresh_dgv();
            return true;
        }

        private void Refresh_dgv()
        {
            OnDataChanged?.Invoke();
            this.Close();
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_mode == enMode.Add)
            {
                if (SaveAddNewAppointment())
                {
                    MessageBoxHelper.Show("تم إضافة الموعد بنجاج", enMessageType.Info, "نجحت العملية", true);
                    Refresh_dgv();
                    return;
                }
            }
            else if (_mode == enMode.Update)
            {
                if(SaveUpdateAppointment())
                {
                    MessageBoxHelper.Show("تم التحديث بنجاح", enMessageType.Info, "نحجت العملية", true);
                    OnDataChanged?.Invoke();
                    return;
                }
            }
        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
