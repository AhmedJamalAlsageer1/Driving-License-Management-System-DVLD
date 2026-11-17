using DVLD.Classess.Helpers;
using DVLD.DTOs.International_License;
using DVLD.DTOs.License;
using DVLD.License;
using DVLDBisnessLayer;
using DVLDBisnessLayer.InternationalLicense;
using DVLDBisnessLayer.License;
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

namespace DVLD.Internationall_Licenses
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int? I_LicenseID = null;

        public event Action<int> isAdded;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
            btnIssue.Enabled = false;
            llblShowLicenseInfo.Enabled = false;

            Result <decimal> result_ApplicationFees =  clsApplicationTypesBL.GetApplicationFees(SharedLayer.enums.enApplicationTypes.NewInternationalLicense);
            if(!result_ApplicationFees.isSuccess)
            {
                MessageBoxHelper.Show(result_ApplicationFees, "خطأ", true);
                this.Close();
                return;
            }

            DTO_IntLicense dto = new DTO_IntLicense();
            dto.AppFees = result_ApplicationFees.Value;

            uctrlInternationalApplicationInfo1.Initialize_Uctrl_Defaults(dto, true);
        }

        private bool Config_UctrlLicenseInfo(int LicenseID)
        {

            Result<DTOLicenseDriverInfo> result_LicenseDriverInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LicenseID);
            if (!result_LicenseDriverInfo.isSuccess || result_LicenseDriverInfo.Value == null)
            {
                MessageBoxHelper.Show(result_LicenseDriverInfo, "خطأ", true);
                return false;
            }

            DTOLicenseDriverInfo dto = result_LicenseDriverInfo.Value;
            uctrlLicenseInfo1.LoadData(new Model.MDL_DriverLicenseInfo.MDL_LicenseDriver_Info (dto));
            return true;
        }
        private bool Config_UctrlInternationalApplicationLicenseInfo(int LicenseID)
        {
            Result<(byte, decimal)> result_DefaultApplication = cls_IntLicenseRequest_BL.TryGetNewInternationalLicenseDefaults
                (LicenseID, SharedLayer.enums.enApplicationTypes.NewInternationalLicense);
            if (!result_DefaultApplication.isSuccess)
            {
                MessageBoxHelper.Show(result_DefaultApplication.Message, result_DefaultApplication.messageType ?? enMessageType.Error,
                    "خطأ", true);
                return false;
            }

            var (validityLength, applicationFees) = result_DefaultApplication.Value;

            DTO_IntLicense dto_Inter = new DTO_IntLicense();
            dto_Inter.Local_LicenseID = LicenseID;
            dto_Inter.AppFees = applicationFees;

            uctrlInternationalApplicationInfo1.Initialize_Uctrl_Defaults(dto_Inter, false, validityLength);
            return true;
        }

        private void DisplayDataLicense()
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text))
                return;

            string Input = maskedTextBox1.Text.Trim();

            if (!int.TryParse(Input, out int LicenseID))
                return;

            if (!Config_UctrlLicenseInfo(LicenseID))
                return;

            if(!Config_UctrlInternationalApplicationLicenseInfo(LicenseID))
                return;

            btnIssue.Enabled = true;
        }


        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnIssue.Enabled = false;
                DisplayDataLicense();
            }
        }


        private void MoveCaretToEnd()
        {
            if(maskedTextBox1.SelectionStart > maskedTextBox1.Text.Length)
            {
                maskedTextBox1.SelectionStart = maskedTextBox1.Text.Length;
            }
        }

        private void maskedTextBox1_Click(object sender, EventArgs e)
        {
            MoveCaretToEnd();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DTO_IntLicense Fill_DTO()
        {
            decimal? Fees = uctrlInternationalApplicationInfo1.Fees_UI;
            if (!Fees.HasValue)
                return null;

            if(!int.TryParse(maskedTextBox1.Text, out int LicenseID))
                return null;

            return new DTO_IntLicense
            {
                ApplicationType = enApplicationTypes.NewInternationalLicense,
                Local_LicenseID = LicenseID,
                AppFees = Fees.Value,
            };
        }

        private bool result_IssueLicense()
        {
            DTO_IntLicense dto = Fill_DTO();
            if (dto == null)
            {
                MessageBoxHelper.Show("حدث خطأ، بيانات ناقصة!", enMessageType.Error, "خطأ", true);
                return false;
            }

            cls_IntLicenseRequest_BL request_newInt = new cls_IntLicenseRequest_BL(dto);
            OperationResult result_Save = request_newInt.Save();

            if (!result_Save.IsSuccess)
            {
                MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Error, "خطأ", true);
                return false;
            }

            MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Info, "خطأ", true);
            I_LicenseID = dto.Int_LicenseID;
            isAdded?.Invoke(dto.Int_LicenseID);
            return true;
        }

        private bool Config_UctrlInternationalApplicationLicenseInfoBy_I_LicenseID(int I_LicenseID)
        {
            Result<DTO_IntLicense> result_Interantional = cls_IntLicenseRequest_BL.Find_IntLicenseByID(I_LicenseID);
            if (!result_Interantional.isSuccess || result_Interantional.Value == null)
            {
                MessageBoxHelper.Show(result_Interantional, "خطأ", true);
                return false;
            }

            uctrlInternationalApplicationInfo1.LoadData(result_Interantional.Value);
            return true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("هل أنت متأكد من أنك تريد إصدار الرخصة؟", "هل أنت متأكد؟", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == DialogResult.Yes)
            {
                if (result_IssueLicense())
                {
                    maskedTextBox1.Enabled = false;
                    llblShowLicenseInfo.Enabled = true;

                    if (!I_LicenseID.HasValue)
                        return;

                    Config_UctrlInternationalApplicationLicenseInfoBy_I_LicenseID(I_LicenseID.Value);
                }
            }
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!I_LicenseID.HasValue)
                return;

            using(frm_DriverInternationalLicenseInfo frm = new frm_DriverInternationalLicenseInfo(I_LicenseID.Value))
            {
                frm.ShowDialog();
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!I_LicenseID.HasValue)
                return;

            frmLicenseHistory frm = frmLicenseHistory.From_IntLicenseID(I_LicenseID.Value);
            if(frm != null)
            {
                using(frm)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayDataLicense();
        }
    }
}
