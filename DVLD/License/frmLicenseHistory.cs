using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs;
using DVLD.DTOs.International_License;
using DVLD.DTOs.License.LicensesHistory;
using DVLDBisnessLayer;
using DVLDBisnessLayer.InternationalLicense;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.License.LicencesHistory;
using DVLDBisnessLayer.New_LocalDriver_App;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.License
{
    public partial class frmLicenseHistory : Form
    {
        private frmLicenseHistory()
        {
            InitializeComponent();

            CreateTableLocalLicense();
            CreateTableInternationalLicences();
        }

        public static frmLicenseHistory From_DriverID(int DriverID)
        {
            if (DriverID <= 0)
                return null;

            frmLicenseHistory frm = new frmLicenseHistory();

            Result<(int PersonID, int ApplicationID)> result_Data = cls_LDL_ApplicationRequest_BL.GetAppID_PersonIDBy_DriverID(DriverID);
            if (!result_Data.isSuccess)
            {
                MessageBoxHelper.Show(result_Data, "خطأ", true);
                return null;
            }

            int PersonID = result_Data.Value.PersonID;
            int ApplicationID = result_Data.Value.ApplicationID;

            if (!frm.ConfiguraionForm(PersonID, ApplicationID))
            {
                MessageBoxHelper.Show("حدث خطأ أثناء تحميل بيانات الرخص. سيتم إغلاق الشاشة.", SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                frm.Close();
                return null;
            }
            return frm;
        }


        public static frmLicenseHistory From_LDLAppID(int LDL_AppID)
        {
            if (LDL_AppID <= 0)
                return null;

            frmLicenseHistory frm = new frmLicenseHistory();

            Result<(int PersonID, int ApplicationID)> result_Data =  cls_LDL_ApplicationRequest_BL.GetAppID_PersonIDBy_LDLAppID(LDL_AppID);
            if(!result_Data.isSuccess)
            {
                MessageBoxHelper.Show(result_Data, "خطأ", true);
                return null;
            }

            int PersonID = result_Data.Value.PersonID;
            int ApplicationID = result_Data.Value.ApplicationID;

            if (!frm.ConfiguraionForm(PersonID, ApplicationID))
            {
                MessageBoxHelper.Show("حدث خطأ أثناء تحميل بيانات الرخص. سيتم إغلاق الشاشة.", SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                frm.Close();
                return null;
            }
            return frm;
        }

        public static frmLicenseHistory From_IntLicenseID(int int_LicenseID)
        {
            if(int_LicenseID <= 0)
                return null;


            Result <(int PersonID, int ApplicationID)> result_Data = cls_IntLicenseRequest_BL.GetPersonID_ApplicationIDBy_IntLicenseID(int_LicenseID);
            if(!result_Data.isSuccess)
            {
                MessageBoxHelper.Show(result_Data, "خطأ", true);
                return null;
            }

            int PersonID = result_Data.Value.PersonID;
            int ApplicationID = result_Data.Value.ApplicationID;


            frmLicenseHistory frm = new frmLicenseHistory();
            if (!frm.ConfiguraionForm(PersonID, ApplicationID))
            {
                MessageBoxHelper.Show("حدث خطأ أثناء تحميل بيانات الرخص. سيتم إغلاق الشاشة.", SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                frm.Close();
                return null;
            }
            return frm;
        }

        public static frmLicenseHistory From_LocalLicenseID(int LocalLicenseID)
        {
            if (LocalLicenseID <= 0)
                return null;


            Result<(int PersonID, int ApplicationID)> result_Data = cls_LocalLicenseRequest_BL.GetPersonID_ApplicationIDBy_IntLicenseID(LocalLicenseID);
            if (!result_Data.isSuccess)
            {
                MessageBoxHelper.Show(result_Data, "خطأ", true);
                return null;
            }

            int PersonID = result_Data.Value.PersonID;
            int ApplicationID = result_Data.Value.ApplicationID;


            frmLicenseHistory frm = new frmLicenseHistory();
            if (!frm.ConfiguraionForm(PersonID, ApplicationID))
            {
                MessageBoxHelper.Show("حدث خطأ أثناء تحميل بيانات الرخص. سيتم إغلاق الشاشة.", SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                frm.Close();
                return null;
            }
            return frm;
        }


        private void CreateTableLocalLicense()
        {
            dgvLocal.Columns.Clear();
            DgvConfig.ApplyBasicStyle(dgvLocal);
            dgvLocal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLocal.RowTemplate.Height = 50;

            dgvLocal.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Lic.ID",
                DataPropertyName = "LicenseID",
                Name = "colLicenseID",
                Width = 70
            });

            dgvLocal.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "App.ID",
                DataPropertyName = "ApplicationID",
                Name = "colApplicationID",
                Width = 70
            });

            dgvLocal.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Class Name",
                DataPropertyName = "ClassName",
                Name = "colClassName",
                FillWeight = 250
            });

            dgvLocal.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Issue Date",
                DataPropertyName = "IssueDate",
                Name = "colIssueDate",
                FillWeight = 150
            });

            dgvLocal.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Expiration Date",
                DataPropertyName = "ExpirationDate",
                Name = "colExpirationDate",
                FillWeight = 150
            });

            dgvLocal.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Is Active",
                DataPropertyName = "IsActive", // must be bool
                Name = "colIsActive",
                Width = 80,
                ReadOnly = true // ❗ اختياري: لتجنب تعديل المستخدم عليه
            });
        }
        private void CreateTableInternationalLicences()
        {
            dgvInternational.Columns.Clear();
            DgvConfig.ApplyBasicStyle(dgvInternational);
            dgvInternational.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInternational.RowTemplate.Height = 50;

            dgvInternational.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Int.License ID",
                DataPropertyName = "InternationalLicenseID",
                Name = "colIntLicenseID",
                FillWeight = 110
            });

            dgvInternational.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Application ID",
                DataPropertyName = "ApplicationID",
                Name = "colApplicationID",
                FillWeight = 100
            });

            dgvInternational.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "L.License ID",
                DataPropertyName = "UsingLocalLicenseID",
                Name = "colLocalLicenseID",
                FillWeight = 100
            });

            dgvInternational.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Issue Date",
                DataPropertyName = "IssueDate",
                Name = "colIssueDate",
                FillWeight = 200
            });

            dgvInternational.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Expiration Date",
                DataPropertyName = "ExpirationDate",
                Name = "colExpirationDate",
                FillWeight = 200
            });

            dgvInternational.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Is Active",
                DataPropertyName = "IsActive",
                Name = "colIsActive",
                FillWeight = 80,
                ReadOnly = true
            });
        }

        private bool ConfiguraionForm(int PersonID, int ApplicationID)
        {
            if(!LoadDataUctrlPersonInfo(PersonID))
                return false;
            if(!LoadData_LocalLicencesHistory(ApplicationID))
                return false;
            if (!LoadData_InternationalLicencesHistory(ApplicationID))
                return false;

            return true;
        }

        private bool LoadDataUctrlPersonInfo(int PersonID)
        {
            Result<DTOPerson> result_person = clsPerson.FindByID(PersonID);
            if(!result_person.isSuccess || result_person.Value == null)
                return false;

            DTOPerson person = result_person.Value;

            uctrlPersonInfo1.LoadData(person);
            return true;
        }

        private bool LoadData_LocalLicencesHistory(int ApplicationID)
        {
            if (ApplicationID <= 0)
                return false;

            Result<List<DTO_LocalLicenseHistory>> result_LocalLicenses = clsLicensesHistory_BL.GetAllActiveLocalLicenseByDriverID(ApplicationID);
            if (!result_LocalLicenses.isSuccess || result_LocalLicenses.Value == null)
                return false;

            SortableBindingList <DTO_LocalLicenseHistory> sortableList = new SortableBindingList <DTO_LocalLicenseHistory>(result_LocalLicenses.Value);
            dgvLocal.DataSource = sortableList;

            lblLocalRecord.Text = result_LocalLicenses.Value.Count.ToString();
            return true;
        }

        private bool LoadData_InternationalLicencesHistory(int ApplicationID)
        {
            if (ApplicationID <= 0)
                return false;

            Result<List<DTO_InternationalLicenseHistory>> result_InternationalLicenses = clsLicensesHistory_BL.GetAllActiveInternationalLicenseByDriverID(ApplicationID);
            if (!result_InternationalLicenses.isSuccess || result_InternationalLicenses.Value == null)
                return false;


            SortableBindingList<DTO_InternationalLicenseHistory> sortableList = new SortableBindingList<DTO_InternationalLicenseHistory>(result_InternationalLicenses.Value);
            dgvInternational.DataSource = sortableList;

            lblInternationalRecord.Text = result_InternationalLicenses.Value.Count.ToString();
            return true;
        }
    }
}
