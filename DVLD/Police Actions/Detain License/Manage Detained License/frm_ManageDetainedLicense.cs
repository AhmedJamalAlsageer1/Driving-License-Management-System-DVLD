using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.License;
using DVLD.License.Detain_License;
using DVLD.License.Release_Detained_License;
using DVLD.LicenseForm;
using DVLD.People;
using DVLDBisnessLayer.PoliceService.clsRequestPoliceAction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Police_Actions.Detain_License.Manage_Detained_License
{
    public partial class frm_ManageDetainedLicense : Form
    {
        enum enSearchType : byte
        {
            None = 0,
            DetainID,
            LicenseID,
            IsReleased,
            NationalNo,
            FullName,
            ReleaseApplicationID
        };

        enSearchType _SearchType;

        public frm_ManageDetainedLicense()
        {
            InitializeComponent();
            ConfigForm();
        }

        private void ConfigForm()
        {
            cbSearchType.Items.AddRange(new object[]
            {
                "None",
                "Detain ID",
                "License ID",
                "Is Released",
                "National No",
                "Full Name",
                "Release Application ID"
            });

            cbIsReleased.Items.AddRange(new object[]
            {
                "Yes",
                "No",
            });

            cbSearchType.SelectedIndex = 0;

            cbIsReleased.SelectedIndexChanged -= cbIsReleased_SelectedIndexChanged;
            cbIsReleased.SelectedIndex = 0;
            cbIsReleased.SelectedIndexChanged += cbIsReleased_SelectedIndexChanged;

            CreateTable();
        }

        private void CreateTable()
        {
            DgvConfig.ApplyBasicStyle(dgv_DetainedLicenses);

            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("DetainID", "D.ID", 100));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("LicenseID", "L.ID", 100));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("DetainDate", "D.Date", 150));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateCheckBoxColumn("IsReleased", "Is Released", 80));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("FineFees", "Fine Fees", 120));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("ReleaseDate", "Release Date", 120));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("NationalNo", "National No", 180));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("FullName", "Full Name", 250));
            dgv_DetainedLicenses.Columns.Add(DgvConfig.CreateTextBox("ReleaseApplicationID", "Rlease App.ID", 120));
        }

        private void ToggleBetweenControl()
        {
            cbIsReleased.Visible = false;
            txtInput.Visible = false;
            mtxtInput.Visible = false;
            txtInput.Text = "";
            mtxtInput.Text = "";

            switch(_SearchType)
            {
                case enSearchType.None:
                    break;

                case enSearchType.IsReleased:
                    cbIsReleased.Visible = true;
                    break;

                case enSearchType.FullName:
                case enSearchType.NationalNo:
                    txtInput.Visible = true;
                    break;

                default:
                    mtxtInput.Visible = true;
                    break;
            };
        }

        private void SetSearchTypeFromComboBox()
        {
            if (cbSearchType.SelectedIndex == 0)
            {
                _SearchType = enSearchType.None;
                ToggleBetweenControl();
                LoadData();
                return;
            }

            else if (cbSearchType.SelectedIndex == 1)
            {
                _SearchType = enSearchType.DetainID;
                ToggleBetweenControl();
                return;
            }

            else if (cbSearchType.SelectedIndex == 2)
            {
                _SearchType = enSearchType.LicenseID;
                ToggleBetweenControl();
                return;
            }

            else if (cbSearchType.SelectedIndex == 3)
            {
                _SearchType = enSearchType.IsReleased;
                ToggleBetweenControl();
                LoadData();
                return;
            }

            else if (cbSearchType.SelectedIndex == 4)
            {
                _SearchType = enSearchType.NationalNo;
                ToggleBetweenControl();
                return;
            }

            else if (cbSearchType.SelectedIndex == 5)
            {
                _SearchType = enSearchType.FullName;
                ToggleBetweenControl();
                return;
            }

            else if (cbSearchType.SelectedIndex == 6)
            {
                _SearchType = enSearchType.ReleaseApplicationID;
                ToggleBetweenControl();
                return;
            }
        }

        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSearchTypeFromComboBox();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SearchType = enSearchType.IsReleased;
            LoadData();
            return;
        }

        private void mtxtInput_Click(object sender, EventArgs e)
        {
            if(mtxtInput.SelectionStart > mtxtInput.Text.Length)
            {
                mtxtInput.SelectionStart = mtxtInput.Text.Length;
            }
        }

        private void mtxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void ShowPersonDetailstoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int? LicenseID =  DgvConfig.TryGetValueCurrentCell(dgv_DetainedLicenses, "LicenseID");
            if (!LicenseID.HasValue)
                return;

            frmPersonDetails frm = frmPersonDetails.ShowPersonDetailsBy_LicenseID(LicenseID.Value);
            if(frm != null)
            {
                using (frm)
                {
                    frm.OnDataUpdated = () =>
                    {
                        LoadData();
                    };

                    frm.ShowDialog();
                    
                }
            }
        }

        private void ShowLicenseDetailstoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int? LicenseID = DgvConfig.TryGetValueCurrentCell(dgv_DetainedLicenses, "LicenseID");
            if (!LicenseID.HasValue)
                return;

            using (frmShowLicenseDetails frm = new frmShowLicenseDetails(LicenseID.Value))
            {
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowPersonLicenseHistorytoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int? LicenseID = DgvConfig.TryGetValueCurrentCell(dgv_DetainedLicenses, "LicenseID");
            if (!LicenseID.HasValue)
                return;

            using (frmLicenseHistory frm = frmLicenseHistory.From_LocalLicenseID(LicenseID.Value))
            {
                if(frm != null)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void ReleaseDetainedLicensetoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int? LicenseID = DgvConfig.TryGetValueCurrentCell(dgv_DetainedLicenses, "LicenseID");
            if (!LicenseID.HasValue)
                return;

            using (frm_ReleaseDetainedLicense frm = new frm_ReleaseDetainedLicense(LicenseID.Value))
            {
                if (frm != null)
                {
                    frm.OnDataUpdate = () =>
                    {
                        LoadData();
                    };
                    frm.ShowDialog();
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int? LicenseID = DgvConfig.TryGetValueCurrentCell(dgv_DetainedLicenses, "LicenseID");
            if (!LicenseID.HasValue)
                return;

            ReleaseDetainedLicensetoolStripMenuItem4.Enabled = false;
            bool? isReleased = cls_RequestPoliceAction_BL.isLicenseReleased(LicenseID.Value);

            if (!isReleased.HasValue)
            {
                MessageBoxHelper.Show("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error,
                    null, true);
                return;
            }

            ReleaseDetainedLicensetoolStripMenuItem4.Enabled = !(isReleased.Value);
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            using (frmDetainLicense frm = new frmDetainLicense())
            {
                frm.OnDataAdded = (int DetainID) =>
                {
                    LoadData();
                    DgvConfig.FoucesOnRow(dgv_DetainedLicenses, "DetainID", DetainID);
                };
                frm.ShowDialog();
            }
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            using (frm_ReleaseDetainedLicense frm = new frm_ReleaseDetainedLicense())
            {
                if (frm != null)
                {
                    frm.OnDataUpdate = () =>
                    {
                        LoadData();
                    };
                    frm.ShowDialog();
                }
            }
        }
    }
}
