using DVLD.Classess.Helpers;
using DVLD.DTOs.Test;
using DVLD.DTOs.Tests;
using DVLD.Tests.Configrue_frmReTakeTest;
using DVLD.Tests.Configura_frmTakeTest;
using DVLDBisnessLayer.Test;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using DrawingImage = System.Drawing.Image;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        int _TestAppointmentID = -1;
        public event Action OnDataUpdated;
        public frmTakeTest(int TestAppointmentID, enTestType testType)
        {
            InitializeComponent();
            vmTakeTest Taketest = new vmTakeTest();

            if (!TakeTestInitializer.Configure(TestAppointmentID, testType, Taketest))
                this.Close();

            _TestAppointmentID = TestAppointmentID;
            DisplayDataOnControl(Taketest);
        }

        private void DisplayDataOnControl(vmTakeTest TakeTest)
        {
            lblD_L_App_ID.Text = TakeTest.LDL_AppID;
            gbNameTest.Text = TakeTest.TypeTestName;
            lblDrivingClass.Text = TakeTest.DriverLicenseName;
            lblName.Text = TakeTest.Name;
            lblTrial.Text = TakeTest.Trial;
            // حفظ الصورة القديمة
            DrawingImage _OldImage = pbImageTest.Image;
            // إدخال الصورة الجديدة
            pbImageTest.Image = TakeTest.TestTypeImage;
            lblAppointmentDate.Text = TakeTest.AppointmentDate;
            lblFees.Text = TakeTest.Fees;
            lblTestID.Text = "Not Taken Yet";
            rbPass.Checked = true;

            // حذف إن كان الصورة القديمة ليست null
            if (_OldImage != null)
                _OldImage.Dispose();
        }

        private DTOs.Test.DTOTest GetInputData()
        {
            return new DTOs.Test.DTOTest
            {
                TestAppointmentID = _TestAppointmentID,
                TestResult = rbPass.Checked? true : false,
                Notes = txtNotes.Text,
                CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID ?? -1,
            };
        }

        private bool HandleFutureAppointmentConfirmation(Result <bool> result, clsTestBL TestBL)
        {
            enMessageType messageType = result.messageType ?? enMessageType.Warning;

            if (MessageBox.Show(result.Message, "تنويه", MessageBoxButtons.YesNo,
                MessageBoxHelper.GetIconByType(messageType), MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Yes)
            {

                Result<bool> result_TakeTest = TestBL.TakeTest(true);
                if (!result_TakeTest.isSuccess)
                {
                    MessageBoxHelper.Show(result_TakeTest, null, true);
                    return false;
                }

                MessageBoxHelper.Show("تم إضافة بيانات الاختبار بنجاح.", enMessageType.Info, "نجحت العملية", true);
                return true;
            }

            return false;
        }

        private bool TakeTest(ref DTOTest dto)
        {
            dto = GetInputData();

            clsTestBL Test = new clsTestBL(dto);

            Result<bool> result_TakeTest = Test.TakeTest();
            if (!result_TakeTest.isSuccess)
            {
                if (result_TakeTest.LogicCode != null)
                {
                    bool isAdded_Successfully = HandleFutureAppointmentConfirmation(result_TakeTest, Test);
                    dto.TestID = Test._TestID;
                    return isAdded_Successfully;
                }
                else
                {
                    MessageBoxHelper.Show(result_TakeTest, null, true);
                    return false;
                }
            }

            MessageBoxHelper.Show("تم إضافة بيانات الاختبار بنجاج.", enMessageType.Info, 
                "نحجت العملية", true);
            return true;
        }

        private void IfResultTestSuccessfully()
        {
            btnSave.Enabled = false;
            rbFail.Enabled = false;
            rbPass.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DTOTest dto = null;
            if (TakeTest(ref dto))
            {
                lblTestID.Text = dto.TestID.ToString();
                IfResultTestSuccessfully();
            }

            OnDataUpdated?.Invoke();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
