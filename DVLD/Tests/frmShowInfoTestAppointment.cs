using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs.Applications;
using DVLD.DTOs.Tests;
using DVLDBisnessLayer;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmShowInfoTestAppointment : Form
    {
        enTestType _TestType;
        int _LDL_AppID = -1;

        public event Action OnDataUpdate;
        public string TestTitle
        {
            get => lblTitleTestType.Text;
            set => lblTitleTestType.Text = value;
        }

        public Image ImageTitle
        {
            get => pbTypeTest.Image;
            set
            {
                if(pbTypeTest.Image != null && !ReferenceEquals(pbTypeTest.Image, value))
                {
                    pbTypeTest.Image.Dispose();
                    pbTypeTest.Image = null;
                }
                pbTypeTest.Image = value;
            }
        }

        private void LoadDefaultDataDrivingLicenseApplication()
        {
            Result<DTO_LDLFull_Info> result_LDL = clsApplicationDisplayBL.Find_FullLDL_Info(_LDL_AppID);

            if (!result_LDL.isSuccess || result_LDL.Value == null)
            {
                MessageBoxHelper.Show(result_LDL, "خطأ", true);
                return;
            }

            uctrlDrivingLicenseApplicationInfo1.LoadData(result_LDL.Value);
        }

        public frmShowInfoTestAppointment(int LDLAppID, enTestType TestType)
        {
            InitializeComponent();

            _TestType = TestType;
            _LDL_AppID = LDLAppID;


            ShowInfoTestAppointmentConfigurator.Configure(TestType, this);
            LoadDefaultDataDrivingLicenseApplication();
        }

        private void ConfigureGrid()
        {
            DgvConfig.ApplyBasicStyle(dataGridView1);
            dataGridView1.ReadOnly = false;
            dataGridView1.RowTemplate.Height = 50;

            var colId = new DataGridViewTextBoxColumn
            {
                Name = "TestAppointmentID",
                DataPropertyName = "TestAppointmentID",
                HeaderText = "Test Appointment ID",
                ValueType = typeof(int),
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 170,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            colId.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colDate = new DataGridViewTextBoxColumn
            {
                Name = "AppointmentDate",
                DataPropertyName = "AppointmentDate",
                HeaderText = "Appointment Date",
                ValueType = typeof(DateTime),
                DefaultCellStyle = 
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "yyyy-MM-dd HH:mm" 
                },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            colDate.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            var colFees = new DataGridViewTextBoxColumn
            {
                Name = "PaidFees",
                DataPropertyName = "PaidFees",
                HeaderText = "Paid Fees",
                ValueType = typeof(decimal),
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter},
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 100,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
            colFees.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colLocked = new DataGridViewCheckBoxColumn
            {
                Name = "IsLocked",
                DataPropertyName = "IsLocked",
                HeaderText = "Is Locked",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 90,
                
                ThreeState = false,
                FlatStyle = FlatStyle.Standard,
                ReadOnly = true
            };
            colLocked.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dataGridView1.Columns.AddRange(colId, colDate, colFees, colLocked);


            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name != "IsLocked")
                    col.ReadOnly = true;
            }


            int finalWidth = colDate.Width;
            colDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDate.Width = finalWidth;
        }

        private void frmShowInfoTestAppointment_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadDataTestAppointments_dgv();
        }

        public List<DTODisplayTestAppointment> SortAppointmentsDefault(List<DTODisplayTestAppointment> source)
        {
            return source.OrderBy(x => x.IsLocked).ToList();
        }

        private void LoadDataTestAppointments_dgv()
        {
            if(_LDL_AppID == -1 || _TestType == enTestType.UnKnown)
            {
                MessageBoxHelper.Show("هناك خطأ غير مقصود، يرجى المحاولة مرة أخرى", enMessageType.Error, "خطأ", true);
                return;
            }

            Result <List < DTODisplayTestAppointment>> resultTestAppointments = clsTestAppointmentInfoBL.FindAllTestAppointments(_LDL_AppID, _TestType);

            if(!resultTestAppointments.isSuccess || resultTestAppointments.Value == null)
            {
                MessageBoxHelper.Show("حدث خطأ أثناء تحميل البيانات، يرجى مراجعة هذه الحالة أو المحاولة مجددا", enMessageType.Error, "خطأ", true);
                return;
            }

            List<DTODisplayTestAppointment> sorted = SortAppointmentsDefault(resultTestAppointments.Value);
            BindingList<DTODisplayTestAppointment> Data = new BindingList<DTODisplayTestAppointment>(sorted);
            dataGridView1.DataSource = Data;

            lblRecords.Text = resultTestAppointments.Value.Count.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmScheduleTest frm = new frmScheduleTest(enMode.Add, _TestType, _LDL_AppID);

            frm.OnDataChanged = () =>
            {
                LoadDataTestAppointments_dgv();
            };

            frm.ShowDialog();
            frm.Dispose();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? AppointmentID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "TestAppointmentID");
            if(AppointmentID == null)
            {
                return;
            }

            frmScheduleTest frm = new frmScheduleTest(enMode.Update, _TestType, AppointmentID.Value);
            frm.OnDataChanged += LoadDataTestAppointments_dgv;
            frm.ShowDialog();
            frm.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool TryCancelTestAppointment(int TestAppointmentID)
        {
            Result <clsTestAppointmentBL> result_TestAppointment = clsTestAppointmentBL.FindObjectTestAppointmentByAppoinID(TestAppointmentID);
            if(!result_TestAppointment.isSuccess || result_TestAppointment.Value == null)
            {
                MessageBoxHelper.Show(result_TestAppointment, "خطأ", true);
                return false;
            }

            if (MessageBox.Show("هل أنت متأكد من إلغاء هذا الموعد؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Yes)
            {
                clsTestAppointmentBL TestAppointment = result_TestAppointment.Value;
                Result <bool> result_CancelTestAppointment = TestAppointment.CancelAppointment();
                if(!result_CancelTestAppointment.isSuccess)
                {
                    MessageBoxHelper.Show(result_CancelTestAppointment, null, true);
                    return false;
                }

                MessageBoxHelper.Show("تم إلغاء الموعد بنجاح.", enMessageType.Info,
                         "نجحت عملية إلغاء الموعد", true);

                LoadDataTestAppointments_dgv();
                LoadDefaultDataDrivingLicenseApplication();
                return true;
            }

            return false;
        }

        private void concelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? AppointmentID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "TestAppointmentID");
            if (!AppointmentID.HasValue)
                return;

            TryCancelTestAppointment(AppointmentID.Value);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? AppointmentID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "TestAppointmentID");
            if (!AppointmentID.HasValue)
                return;

            Result<bool?> result_isLocked = clsTestAppointmentBL.isLocked(AppointmentID.Value);
            if(!result_isLocked.Value.HasValue || result_isLocked.isSuccess)
            {
                MessageBoxHelper.Show(result_isLocked, null, true);
                return;
            }

            frmTakeTest frm = new frmTakeTest(AppointmentID.Value, _TestType);
            frm.OnDataUpdated += () =>
            {
                LoadDataTestAppointments_dgv();
                LoadDefaultDataDrivingLicenseApplication();
                OnDataUpdate?.Invoke();
            };
            frm.ShowDialog();
            frm.Close();
        }
    }
}
