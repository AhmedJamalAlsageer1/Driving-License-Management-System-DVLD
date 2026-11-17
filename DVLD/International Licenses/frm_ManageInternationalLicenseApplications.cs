using DVLD.Classess.UIItems;
using DVLD.Internationall_Licenses;
using DVLD.Internationall_Licenses.frm_ManageInternationalLicenseApplications;
using DVLD.License;
using DVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.International_Licenses
{
    public partial class frm_ManageInternationalLicenseApplications : Form
    {
        public frm_ManageInternationalLicenseApplications()
        {
            InitializeComponent();

            Config_Form();
        }

        private void Config_Form()
        {
            CreateTable_IntLicenses();

            cbSearchType.Items.Clear();

            cbSearchType.Items.Add("None");
            cbSearchType.Items.Add("Int.LicenseID");
            cbSearchType.Items.Add("L.LicenseID");
            cbSearchType.Items.Add("ApplicationID");
            cbSearchType.Items.Add("DriverID");

            // اختيار القيمة الافتراضية (اختياري)
            cbSearchType.SelectedIndex = 0; // "None"
        }

        private void CreateTable_IntLicenses()
        {
            dgv_IntLicenses.Columns.Clear();
            DgvConfig.ApplyBasicStyle(dgv_IntLicenses);
            dgv_IntLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv_IntLicenses.RowTemplate.Height = 40;


            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Int_LicenseID",
                HeaderText = "Int License ID",
                DataPropertyName = "Int_LicenseID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ApplicationID",
                HeaderText = "Application ID",
                DataPropertyName = "ApplicationID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DriverID",
                HeaderText = "Driver ID",
                DataPropertyName = "DriverID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "L_LicenseID",
                HeaderText = "License ID",
                DataPropertyName = "L_LicenseID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 80
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IssueDate",
                HeaderText = "Issue Date",
                DataPropertyName = "IssueDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 170
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExpirationDate",
                HeaderText = "Expiration Date",
                DataPropertyName = "ExpirationDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 170
            });

            dgv_IntLicenses.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is Active",
                DataPropertyName = "IsActive",

                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 70
            });
        }

        private void SetSearchType(int SelectedID = -1)
        {
            mtxtInput.Text = "";
            if (cbSearchType.SelectedIndex == 0)
            {
                clsIntLicenseSearch.SetSearchType(clsIntLicenseSearch.enSearchType.None, mtxtInput);
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }

            else if (cbSearchType.SelectedIndex == 1)
            {
                clsIntLicenseSearch.SetSearchType(clsIntLicenseSearch.enSearchType.Int_LicenseID, mtxtInput);
            }

            else if (cbSearchType.SelectedIndex == 2)
            {
                clsIntLicenseSearch.SetSearchType(clsIntLicenseSearch.enSearchType.L_LicenseID, mtxtInput);
            }

            else if (cbSearchType.SelectedIndex == 3)
            {
                clsIntLicenseSearch.SetSearchType(clsIntLicenseSearch.enSearchType.ApplicationID, mtxtInput);
            }

            else if (cbSearchType.SelectedIndex == 4)
            {
                clsIntLicenseSearch.SetSearchType(clsIntLicenseSearch.enSearchType.DriverID, mtxtInput);
            }

        }

        private void LoadData(int SelectedID = -1)
        {
            if(clsIntLicenseSearch.SearchType == clsIntLicenseSearch.enSearchType.None)
            {
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }

            else if (clsIntLicenseSearch.SearchType == clsIntLicenseSearch.enSearchType.Int_LicenseID)
            {
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }

            else if (clsIntLicenseSearch.SearchType == clsIntLicenseSearch.enSearchType.L_LicenseID)
            {
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }

            else if (clsIntLicenseSearch.SearchType == clsIntLicenseSearch.enSearchType.ApplicationID)
            {
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }

            else if (clsIntLicenseSearch.SearchType == clsIntLicenseSearch.enSearchType.DriverID)
            {
                clsIntLicenseSearch.LoadDataOn_Dgv(dgv_IntLicenses, mtxtInput.Text, lblRecord, SelectedID);
            }
        }

        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSearchType();
        }

        private void mtxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? DriverID = DgvConfig.GetValueInt_FromColumn_DGV(dgv_IntLicenses, dgv_IntLicenses.CurrentRow, "DriverID");

            if (!DriverID.HasValue || DriverID <= 0)
                return;

            frmPersonDetails frm = frmPersonDetails.ShowPersonDetailsBy_DriverID(DriverID.Value);
            if(frm != null)
            {
                using(frm)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? Int_LicenseID = DgvConfig.GetValueInt_FromColumn_DGV(dgv_IntLicenses, dgv_IntLicenses.CurrentRow, "Int_LicenseID");
            if (!Int_LicenseID.HasValue || Int_LicenseID.Value <= 0)
                return;

            frm_DriverInternationalLicenseInfo frm = new frm_DriverInternationalLicenseInfo(Int_LicenseID.Value);

            if (!frm.IsDataLoadedSuccessfully)
            {
                frm.Dispose();
                return;
            }

            using (frm)
            {
                frm.ShowDialog();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? Int_LicenseID = DgvConfig.GetValueInt_FromColumn_DGV(dgv_IntLicenses, dgv_IntLicenses.CurrentRow, "Int_LicenseID");
            if (!Int_LicenseID.HasValue)
                return;

            frmLicenseHistory frm = frmLicenseHistory.From_IntLicenseID(Int_LicenseID.Value);
            if (frm != null)
            {
                using (frm)
                    { frm.ShowDialog(); }
            }
        }

        private void btnAddNew_IntLicense_Click(object sender, EventArgs e)
        {
            using(frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication())
            {
                frm.isAdded += (id) =>
                {
                    LoadData(id);
                };
                frm.ShowDialog();
            }
        }
    }
}
