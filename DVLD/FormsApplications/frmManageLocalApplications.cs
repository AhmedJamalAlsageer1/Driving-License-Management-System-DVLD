using DVLD.Classess.Helpers;
using DVLD.Classess.Search_Builder;
using DVLD.Classess.UIHandlers;
using DVLD.Classess.UIItems;
using DVLD.DTOs;
using DVLD.DTOs.Applications;
using DVLD.DTOs.License;
using DVLD.License;
using DVLD.LicenseForm;
using DVLD.Tests;
using DVLDBisnessLayer;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.New_LocalDriver_App;
using DVLDBisnessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.FormsApplications
{
    public partial class frmManageLDLApplications : Form
    {
        clsSearchApplications SearchManager;
        public frmManageLDLApplications()
        {
            InitializeComponent();
        }

        private void LoadDefaultData()
        {
            SearchManager = new clsSearchApplications();
            if (cbSearchType.Items.Count > 0)
            {
                cbSearchType.SelectedIndex = 0;
            }

            BindControlsInEventEnter();
            cbStatus.DataSource = ComboBoxHelper.GetEnumOptionsForFilterAndAddInFirst<enApplicationStatus>
                (x => Localizer.TranslateEnum(x.ToString()), enApplicationStatus.Unknown);
            
            SetupLDLApplicationColumns();
        }
        private void EvenEnter(object sendor, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                LoadSearchResult();
            }
        }
        private void BindControlsInEventEnter()
        {
            foreach(Control C in panel1.Controls)
            {
                C.KeyDown += EvenEnter;
            }
        }
        private void frmManageLocalApplications_Load(object sender, EventArgs e)
        {
            LoadDefaultData();
        }
        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchManager.ChangedSelectedItem(cbSearchType, txtInput, mtxtInput, cbStatus);

            // إذا كان نوع البحث None، فنفذ مباشرة دون انتظار Enter (لأنه لا يوجد كنترول لإدخال المستخدم)
            if (SearchManager.SearchType == clsSearchApplications.enSearchType.None)
                LoadSearchResult();
        }
        private void mtxtInput_Enter(object sender, EventArgs e)
        {
            if (mtxtInput.SelectionStart >= mtxtInput.Text.Length)
            {
                mtxtInput.SelectionStart = mtxtInput.Text.Length;
                mtxtInput.SelectionLength = 0;
            }
        }
        private void mtxtInput_DoubleClick(object sender, EventArgs e)
        {
            mtxtInput.SelectionStart = 0;
            mtxtInput.SelectionLength = mtxtInput.Text.Length;
        }
        private void SetupLDLApplicationColumns()
        {
            DgvConfig.ApplyBasicStyle(dataGridView1);

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "L_D_LApplicationID",
                HeaderText = "L.D.L.AppID",
                DataPropertyName = "L_D_LApplicationID",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DrivingClass",
                HeaderText = "Driving Class",
                DataPropertyName = "DrivingClass",
                Width = 250,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NationalNo",
                HeaderText = "National No",
                DataPropertyName = "NationalNo",
                Width = 130,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "Full Name",
                DataPropertyName = "FullName",
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ApplicationDate",
                HeaderText = "Application Date",
                DataPropertyName = "ApplicationDate",
                Width = 170,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PassedTests",
                HeaderText = "Passed Tests",
                DataPropertyName = "PassedTests",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                
            });

            dataGridView1.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
        }
        private void FocusRowByAppID(int SelectedAppID)
        {
            if (SelectedAppID == -1)
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (!int.TryParse(row.Cells["L_D_LApplicationID"].Value?.ToString(), out int value))
                    continue;

                if (SelectedAppID != value)
                    continue;

                dataGridView1.CurrentCell = row.Cells["L_D_LApplicationID"];
            }
        }
        private void LoadDataByNone(int SelectedAppID)
        {
            Result<List <DTO_LDLApplicationsDisplay>> resultOfAll = clsApplicationDisplayBL.GetAllApplications();

            ResultDisplayHandler.DisplayFailureMessageLblOrMbox(lblResultDisplay, resultOfAll, "فشل عملية جلب الطلبات", true);
            if(!resultOfAll.isSuccess)
            {
                dataGridView1.DataSource = null;
                return;
            }

            List<DTO_LDLApplicationsDisplay> dto = resultOfAll.Value;
            dataGridView1.DataSource = new SortableBindingList<DTO_LDLApplicationsDisplay>(dto);
            FocusRowByAppID(SelectedAppID);
        }
        private void LoadDataByAppID(int SelectedID)
        {
            if (int.TryParse(mtxtInput.Text, out int LDLID))
            {
                if (LDLID <= 0)
                {
                    mtxtInput.Focus();
                    return;
                }
            }
            else
                return;

            Result<DTO_LDLApplicationsDisplay> resultApplication = clsApplicationDisplayBL.FindApplicationByLDLAppID(LDLID);

            if(!resultApplication.isSuccess || resultApplication.Value == null)
            {
                ResultDisplayHandler.DisplayFailureMessageLblOrMbox(lblResultDisplay, resultApplication, "فشلت عملية جلب الطلبات", true);
                dataGridView1.DataSource = null;
                return;
            }

            lblResultDisplay.Visible = false;
            DTO_LDLApplicationsDisplay dto = resultApplication.Value;
            dataGridView1.DataSource = new List <DTO_LDLApplicationsDisplay> {dto};
            FocusRowByAppID(SelectedID);
        }
        private void LoadDataByNationalNo(int SelectedID)
        {
            if(string.IsNullOrWhiteSpace(txtInput.Text))
            {
                return;
            }

            string NationalNo = txtInput.Text.Trim();
            Result < List <DTO_LDLApplicationsDisplay> > Applications = clsApplicationDisplayBL.FindLDLApplicationByNationalNo(NationalNo);

            if(!Applications.isSuccess)
            {
                ResultDisplayHandler.DisplayFailureMessageLblOrMbox(lblResultDisplay, Applications, "فشلت عملية جلب الطلبات", true);
                dataGridView1.DataSource = null;
                return;
            }

            lblResultDisplay.Visible = false;
            List<DTO_LDLApplicationsDisplay> dto = Applications.Value;
            dataGridView1.DataSource = new SortableBindingList<DTO_LDLApplicationsDisplay>(dto);
            FocusRowByAppID(SelectedID);
        }
        private void LoadSearchResult(int SelectedAppID = -1)
        {
            switch (SearchManager.SearchType)
            {
                case clsSearchApplications.enSearchType.None:
                LoadDataByNone(SelectedAppID);
                break;

                case clsSearchApplications.enSearchType.AppID:
                LoadDataByAppID(SelectedAppID);
                break;

                case clsSearchApplications.enSearchType.NationalNo:
                    LoadDataByNationalNo(SelectedAppID);
                    break;

                case clsSearchApplications.enSearchType.Status:
                    LoadDefaultStatusSearch(SelectedAppID);
                    break;
            }
            // إعطاؤك عدد الصفوف الكلي
            int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;
            lblRecord.Text = rowcount.ToString();
        }
        private void LoadDefaultStatusSearch(int SelectedAppID)
        {
            if (SearchManager.SearchType != clsSearchApplications.enSearchType.Status)
                return;

            cbStatus.SelectedIndex = 0;
            return;
        }
        private void LoadDataStatusApplications(enApplicationStatus? StatusType ,int SelectedAppID = -1)
        {
            if (SearchManager.Status == null)
            {
                LoadDataByNone(SelectedAppID);
                return;
            }

            Result<List<DTO_LDLApplicationsDisplay>> ResultApplications = clsApplicationDisplayBL.GetAllApplicationByStatus(StatusType.Value);

            if(!ResultApplications.isSuccess)
            {
                ResultDisplayHandler.DisplayFailureMessageLblOrMbox(lblResultDisplay, ResultApplications, "خطأ");
                dataGridView1.DataSource = null;
                return;
            }

            List<DTO_LDLApplicationsDisplay> dto = ResultApplications.Value;
            dataGridView1.DataSource = new SortableBindingList<DTO_LDLApplicationsDisplay>(dto);
            FocusRowByAppID(SelectedAppID);
        }
        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchManager.ChanageSelectedStatus(cbStatus);
            LoadDataStatusApplications(SearchManager.Status);
        }
        private void LoadChoosedSearchData(int LDLAppID = -1)
        {
            if (SearchManager.SearchType == clsSearchApplications.enSearchType.Status)
            {
                LoadDataStatusApplications(SearchManager.Status.Value, LDLAppID);
                return;
            }

            LoadSearchResult(LDLAppID);
        }
        private bool TryGetSelectedID(out int ldlAppID)
        {
            ldlAppID = -1;

            var currentRow = dataGridView1.CurrentRow;
            if (currentRow == null)
                return false;

            int rowCount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;
            if (currentRow.Index >= rowCount)
                return false;

            var cell = currentRow.Cells[0];
            var column = cell?.OwningColumn;

            if (column == null || !dataGridView1.Columns.Contains(column))
                return false;

            if (cell.Value == null)
                return false;

            if (!int.TryParse(cell.Value.ToString(), out ldlAppID))
            {
                MessageBoxHelper.Show(
                    "حدث خطأ أثناء استرجاع المعرف العام الخاص بـ LDLAppID، من فضلك تواصل مع المطورين لتحديث ما صنعت.",
                    enMessageType.Error,
                    "خطأ غير متوقع",
                    true
                );
                return false;
            }

            return true;
        }
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedID(out int LDLAppID))
                return;

            if (LDLAppID == -1)
                return;

            OperationResult result_CancellApp = cls_LDL_ApplicationRequest_BL.CancellApplication(LDLAppID);
            
            if (!result_CancellApp.IsSuccess)
            {
                MessageBoxHelper.Show(result_CancellApp.Message, result_CancellApp.MessageType ?? enMessageType.Error, null, true);
                return;
            }

            LoadChoosedSearchData(LDLAppID);
            return;
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedID(out int LDLAppID))
                return;

            if (LDLAppID == -1)
                return;


            if(MessageBox.Show("هل أنت متأكد من أنك تريد حذف هذا الصف؟", "تنويه", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Cancel)
                return;

            OperationResult result_Delete = cls_LDL_ApplicationRequest_BL.Delete(LDLAppID);
            if (!result_Delete.IsSuccess)
            {
                MessageBoxHelper.Show(result_Delete.Message, enMessageType.Warning, "تعذر الحذف", true);
                return;
            }

            MessageBoxHelper.Show(result_Delete.Message, enMessageType.Info, "نجحت عملية الحذف", true);
            LoadChoosedSearchData(LDLAppID);
            return;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sechduleTestToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            scheduleVisionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTestToolStripMenuItem1.Enabled = false;
            scheduleStreetTestToolStripMenuItem2.Enabled = false;

            if (!TryGetSelectedID(out int LDLAppID))
                return;

            if (LDLAppID == -1)
                return;

            Result< clsTestProgressBL> value = clsTestProgressBL.Load(LDLAppID);

            if (!value.isSuccess)
            {
                return;
            }

            clsTestProgressBL CurrentProgress = value.Value;

            scheduleVisionTestToolStripMenuItem.Enabled = CurrentProgress.CanBookAppointment(enTestType.Theory);
            scheduleWrittenTestToolStripMenuItem1.Enabled = CurrentProgress.CanBookAppointment(enTestType.Simulator);
            scheduleStreetTestToolStripMenuItem2.Enabled = CurrentProgress.CanBookAppointment(enTestType.Field);
        }

        private void HandleNewStatusContextMenue(int PassedTestCount)
        {
            contextMenuStrip1.Items["showApplicationDetailsToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["editApplicationToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["deleteApplicationToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["cancelToolStripMenuItem"].Enabled = true;

            if(PassedTestCount == 3)
            {
                contextMenuStrip1.Items["issueDrivingLicenseFirstTimeToolStripMenuItem"].Enabled = true;
                contextMenuStrip1.Items["sechduleTestToolStripMenuItem"].Enabled = false;
            }
            else
            {
                contextMenuStrip1.Items["sechduleTestToolStripMenuItem"].Enabled = true;
                contextMenuStrip1.Items["issueDrivingLicenseFirstTimeToolStripMenuItem"].Enabled = false;
            }
            contextMenuStrip1.Items["showLicensePersonHistoryToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["showLicenseToolStripMenuItem"].Enabled = false;
        }

        private void HandleContextMenuForCancelled()
        {
            contextMenuStrip1.Items["showApplicationDetailsToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["showLicensePersonHistoryToolStripMenuItem"].Enabled = true;

            contextMenuStrip1.Items["editApplicationToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["deleteApplicationToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["cancelToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["sechduleTestToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["issueDrivingLicenseFirstTimeToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["showLicenseToolStripMenuItem"].Enabled = false;
        }

        private void HandleContextMenuForCompleted()
        {
            contextMenuStrip1.Items["showApplicationDetailsToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["showLicensePersonHistoryToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["showLicenseToolStripMenuItem"].Enabled = true;

            contextMenuStrip1.Items["editApplicationToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["deleteApplicationToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["cancelToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["sechduleTestToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["issueDrivingLicenseFirstTimeToolStripMenuItem"].Enabled = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!TryGetSelectedID(out int LDLAppID))
            {
                e.Cancel = true;
                return;
            }

            if (LDLAppID == -1)
            {
                e.Cancel = true;
                return;
            }


            Result<DTO_LDL_App> result_LDLApp = cls_LDL_ApplicationRequest_BL.FindBy_LDLAppID(LDLAppID);
            if (!result_LDLApp.isSuccess || result_LDLApp.Value == null)
            {
                e.Cancel = true;
                return;
            }

            DTO_LDL_App LDL_Applications = result_LDLApp.Value;

            switch (LDL_Applications.ApplicationStatus)
            {
                case enApplicationStatus.New:
                    HandleNewStatusContextMenue(LDL_Applications.PassedTestCount);
                    break;

                case enApplicationStatus.Cancelled:
                    HandleContextMenuForCancelled();
                    break;

                case enApplicationStatus.Completed:
                    HandleContextMenuForCompleted();
                    break;

                default:
                    e.Cancel = true;
                    break;
            }

        }
        private void scheduleTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!TryGetSelectedID(out int LDLAppID))
            {
                return;
            }

            if (LDLAppID == -1)
                return;

            Result <clsTestProgressBL> resultCurrentTest = clsTestProgressBL.Load(LDLAppID);
            if(!resultCurrentTest.isSuccess && resultCurrentTest.Value == null)
            {
                MessageBoxHelper.Show(resultCurrentTest, "خطأ", true);
                return;
            }

            enTestType CurrentTest = resultCurrentTest.Value.NextTestTypeToBook();
            if(CurrentTest == enTestType.UnKnown)
            {
                MessageBoxHelper.Show("خطأ غير مقصود، قيمة غير صحيحة", enMessageType.Error, "خطأ", true);
                return;
            }

            frmShowInfoTestAppointment frm = new frmShowInfoTestAppointment(LDLAppID, CurrentTest);
            frm.OnDataUpdate += () => LoadSearchResult(LDLAppID);
            frm.ShowDialog();
            frm.Close();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? LDL_AppID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "L_D_LApplicationID");
            if (LDL_AppID == null)
                return;

            frmIssueDriverLicense frm = new frmIssueDriverLicense(LDL_AppID.Value);
            frm.OnUpdate += () => LoadSearchResult(LDL_AppID.Value);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? LDL_AppID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "L_D_LApplicationID");
            if(!LDL_AppID.HasValue)
            {
                return;
            }

            Result< DTOLicenseDriverInfo> result_dtoLicenseDriverInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoBy_LDLAppID(LDL_AppID.Value);
            if(!result_dtoLicenseDriverInfo.isSuccess || result_dtoLicenseDriverInfo.Value ==  null)
            {
                MessageBoxHelper.Show(result_dtoLicenseDriverInfo.Message, result_dtoLicenseDriverInfo.messageType ?? enMessageType.Error, "خطأ", true);
                return;
            }

            DTOLicenseDriverInfo dto = result_dtoLicenseDriverInfo.Value;

            using (frmShowLicenseDetails frm = new frmShowLicenseDetails(dto))
            {
                frm.ShowDialog();
            }
        }

        private void UpdateData()
        {
            TryGetSelectedID(out int LDL);
            LoadSearchResult(LDL);
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            using (frmNewLocalDrivingLicenseApplication frm = new frmNewLocalDrivingLicenseApplication())
            {
                frm.ShowDialog();
            }
            UpdateData();
        }

        private void showLicensePersonHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!TryGetSelectedID(out int LDL) || LDL == -1)
            {
                return;
            }

            frmLicenseHistory frm = frmLicenseHistory.From_LDLAppID(LDL);
            if (frm != null)
            {
                using (frm)
                {
                    frm.ShowDialog();
                }
            }

        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.Show("عذرا هذه الصفحة لم يتم إضافتها بعد", enMessageType.Warning, "تنويه", true);
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
