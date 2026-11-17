using DVLD.Classess.Helpers;
using DVLDBisnessLayer;
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
using DVLD.Model.LocalLicense.Replacement;
using DVLDBisnessLayer.License;
using DVLD.DTOs.License;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLD.DTOs.License.Local_License.Replacement_For;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement;
using DVLDBisnessLayer.Presentation_BL.Local_License.Replacement;

namespace DVLD.License.Replacement_Or_Demage
{
    public partial class frmReplacementForLostOrDamagedLicense : Form
    {
        int PreviousLicense = -1;
        int ReplacementLicense = -1;
        enApplicationTypes _ApplicationType = enApplicationTypes.UnKnown;

        public frmReplacementForLostOrDamagedLicense()
        {
            InitializeComponent();
            rbDemagedLicense.Checked = true;
            BasicConfiguration();
            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicensesInfo.Enabled = false;
        }

        private void ChangeAppFees_Uctrl()
        {
            Result<decimal> result_AppFees = clsApplicationTypesBL.GetApplicationFees(_ApplicationType);
            if (!result_AppFees.isSuccess)
            {
                MessageBoxHelper.Show(result_AppFees.Message, result_AppFees.messageType ?? enMessageType.Error, null, true);
                BasicConfiguration();
                return;
            }

            uctrl_ApplicationForLicenseReplacement1.LoadData(new MDL_ReplacementLocalLicense(result_AppFees.Value, PreviousLicense));
        }

        private void rbDemagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Damaged";
            _ApplicationType = enApplicationTypes.ReplacementForDamagedDrivingLicense;
            ChangeAppFees_Uctrl();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Lost";
            _ApplicationType = enApplicationTypes.ReplacementForLostDrivingLicense;
            ChangeAppFees_Uctrl();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mtxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void btnSearchLicense_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BasicConfiguration()
        {
            uctrlLicenseInfo1.LoadDefaultData();
            PreviousLicense = -1;
            ReplacementLicense = -1;
            ChangeAppFees_Uctrl();
        }

        private void LoadData()
        {
            if(!int.TryParse(mtxtInput.Text, out int LicenseID))
            {
                return;
            }

            if (PreviousLicense == LicenseID)
                return;


            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicensesInfo.Enabled = false;
            ReplacementLicense = -1;
            PreviousLicense = LicenseID;

            Result<DTOLicenseDriverInfo> result_LicenseInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LicenseID);
            if(!result_LicenseInfo.isSuccess)
            {
                MessageBoxHelper.Show(result_LicenseInfo.Message, result_LicenseInfo.messageType ?? enMessageType.Error, null, true);
                BasicConfiguration();
                return;
            }

            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicenseInfo.Value));
            ChangeAppFees_Uctrl();
        }

        private OperationResult FillDTO(DTO_Replacement_LocalLicense dto_ReplacementFor)
        {
            decimal? result_appfees = uctrl_ApplicationForLicenseReplacement1.AppFees;
            if (!result_appfees.HasValue)
                return OperationResult.Failure("خطأ تقني يرجى المحاولة مرة أخرى!", enMessageType.Error);

            enIssueReasonLicense  IssueReason = _ApplicationType == enApplicationTypes.ReplacementForDamagedDrivingLicense ?
                enIssueReasonLicense.ReplacementForDamaged : enIssueReasonLicense.ReplacementForLost;


            dto_ReplacementFor.IssueReason = IssueReason;
            dto_ReplacementFor.PaidFees = result_appfees.Value;
            dto_ReplacementFor.ApplicationTypes = _ApplicationType;
            dto_ReplacementFor.OldLicenseID = PreviousLicense;
            return OperationResult.Seccuss(null);
        }

        private bool IssueReplacement()
        {
            if (ReplacementLicense != -1)
                return false;

            DTO_Replacement_LocalLicense dto = new DTO_Replacement_LocalLicense();
            OperationResult result_Fill = FillDTO(dto);
            if(!result_Fill.IsSuccess)
            {
                MessageBoxHelper.Show(result_Fill.Message, result_Fill.MessageType ?? enMessageType.Error, null, true);
                return false;
            }

            cls_LocalLicenseRequest_BL request_replacementfor = new cls_LocalLicenseRequest_BL(dto);
            OperationResult result_Save = request_replacementfor.Save();
            if(!result_Save.IsSuccess)
            {
                MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Error, null, true);
                return false;
            }

            ReplacementLicense = dto.LocalLicenseID;
            MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Info, null, true);
            return true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if(IssueReplacement())
            {
                Result<DTO_ApplicationReplacement_Info> result_replacement = cls_ApplicationReplacement_Info.GetReplacementLicense_Info(ReplacementLicense);
                if(!result_replacement.isSuccess)
                {
                    MessageBoxHelper.Show(result_replacement.Message, result_replacement.messageType ?? enMessageType.Error, null, true);
                    return;
                }
                uctrl_ApplicationForLicenseReplacement1.LoadData(new MDL_ReplacementLocalLicense(result_replacement.Value, PreviousLicense));


                Result<DTOLicenseDriverInfo> result_LicenseInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(PreviousLicense);
                if (!result_LicenseInfo.isSuccess)
                {
                    MessageBoxHelper.Show(result_LicenseInfo.Message, result_LicenseInfo.messageType ?? enMessageType.Error, null, true);
                    BasicConfiguration();
                    return;
                }

                uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicenseInfo.Value));

                llblShowLicensesHistory.Enabled = true;
                llblShowNewLicensesInfo.Enabled = true;
            }
        }

        private void llblShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frm_DriverLicense_Info frm = new frm_DriverLicense_Info(ReplacementLicense))
            {
                frm.ShowDialog();
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = frmLicenseHistory.From_LocalLicenseID(ReplacementLicense);
            if (frm != null)
            {
                using (frm)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void mtxtInput_Click(object sender, EventArgs e)
        {
            if(mtxtInput.SelectionStart > mtxtInput.Text.Length)
                mtxtInput.SelectionStart = mtxtInput.Text.Length;
        }
    }
}
