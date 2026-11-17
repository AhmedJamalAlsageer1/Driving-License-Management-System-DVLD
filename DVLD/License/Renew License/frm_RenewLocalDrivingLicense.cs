using DVLD.Classess.Helpers;
using DVLD.DTOs.License;
using DVLD.DTOs.License.ReNew;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew;
using DVLD.Internationall_Licenses;
using DVLD.License;
using DVLD.Model.LocalLicense.Renew;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLDBisnessLayer;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.Presentation_BL.Local_License.Renew;
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

namespace DVLD.FormsApplications
{
    public partial class frm_RenewLocalDrivingLicense : Form
    {
        private int RenewLicenseID = -1;
        private int Previous_OldLicense = -1;
        public frm_RenewLocalDrivingLicense()
        {
            InitializeComponent();
            ConfigureForm();

        }

        private void ConfigureForm()
        {
            RenewLicenseID = -1;
            Previous_OldLicense = -1;
            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicenseInfo.Enabled = false;
            btnRenew.Enabled = false;

            Result<decimal> result_ApplicationFees = clsApplicationTypesBL.GetApplicationFees(SharedLayer.enums.enApplicationTypes.RenewDrivingLicense);
            if(!result_ApplicationFees.isSuccess)
            {
                MessageBoxHelper.Show(result_ApplicationFees.Message, result_ApplicationFees.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                this.Close();
                return;
            }
            decimal AppFees = result_ApplicationFees.Value;

            uctrl_RenewLicense1.LoadDefaultData(AppFees);
            uctrlLicenseInfo1.LoadDefaultData();
        }

        private void mtxtInput_Click(object sender, EventArgs e)
        {
            if (mtxtInput.SelectionStart > mtxtInput.Text.Length)
                mtxtInput.SelectionStart = mtxtInput.Text.Length;
        }


        private void mtxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ShowDetailLicense();
            }
        }
        private void btnSearchLicense_Click(object sender, EventArgs e)
        {
            ShowDetailLicense();
        }

        private void ShowDetailLicense()
        {
            if (!int.TryParse(mtxtInput.Text, out int value))
                return;

            int LicenseID = value;
            if (LicenseID == Previous_OldLicense)
                return;

            RenewLicenseID = -1;
            Previous_OldLicense = LicenseID;
            
            
            
            Result<DTOLicenseDriverInfo> result_License = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LicenseID);
            if (!result_License.isSuccess)
            {
                MessageBoxHelper.Show(result_License.Message, result_License.messageType ?? SharedLayer.enums.enMessageType.Error, "خطأ", true);
                ConfigureForm();
                return;
            }
            DTOLicenseDriverInfo License = result_License.Value;
            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(License));


            Result<DTO_RenewLicense_Config> result_RenewLicenseConfig = cls_RenewLicense_Info.Find_RenewLicenseConfig_Info(SharedLayer.enums.enApplicationTypes.RenewDrivingLicense, License.LicenseClassID);
            if(!result_RenewLicenseConfig.isSuccess)
            {
                MessageBoxHelper.Show(result_License.Message, result_License.messageType ?? SharedLayer.enums.enMessageType.Error, "خطأ", true);
                ConfigureForm();
                return;
            }
            
            uctrl_RenewLicense1.LoadData(new MDL_RenewLicense_Info(result_RenewLicenseConfig.Value, Previous_OldLicense));
            btnRenew.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        OperationResult FillDTO(DTO_Renew_LocalLicense dto)
        {
            dto.OldLicenseID = Previous_OldLicense;
            dto.IssueReason = enIssueReasonLicense.Renew;
            dto.ApplicationTypes = enApplicationTypes.RenewDrivingLicense;

            decimal? AppFees = uctrl_RenewLicense1.AppFees;
            decimal? LicenseFees = uctrl_RenewLicense1.LicenseFees;

            if (!AppFees.HasValue)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Warning);

            if (!LicenseFees.HasValue)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Warning);

            dto.ApplicationFees = AppFees.Value;
            dto.LicenseFees = LicenseFees.Value;

            
            return OperationResult.Seccuss(null);
        }

        private bool Issue_RenewLicense()
        {
            if(RenewLicenseID != -1)
                return false;


            DTO_Renew_LocalLicense dto = new DTO_Renew_LocalLicense();
            OperationResult resultFill = FillDTO(dto);

            if(!resultFill.IsSuccess)
            {
                MessageBoxHelper.Show(resultFill.Message, resultFill.MessageType ?? enMessageType.Error,
                    "خطأ", true);
                return false;
            }

            cls_LocalLicenseRequest_BL request_renewLicense = new cls_LocalLicenseRequest_BL(dto);
            OperationResult result_Save = request_renewLicense.Save();
            if(!result_Save.IsSuccess)
            {
                MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Error,
                    null, true);
                return false;
            }

            RenewLicenseID = dto.LocalLicenseID;
            MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Info,
                    null, true);

            return true;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if(Issue_RenewLicense())
            {
                if (RenewLicenseID == -1)
                {
                    return;
                }

                Result <DTO_RenewLicense_Info> result_renewInfo = cls_RenewLicense_Info.Find_RenewLicense_Info(RenewLicenseID);
                if(!result_renewInfo.isSuccess)
                {
                    MessageBoxHelper.Show(result_renewInfo.Message, result_renewInfo.messageType ?? enMessageType.Error, null, true);
                    return;
                }
                DTO_RenewLicense_Info dto_renewInfo = result_renewInfo.Value;

                uctrl_RenewLicense1.LoadData(new MDL_RenewLicense_Info(dto_renewInfo, Previous_OldLicense));
                llblShowLicensesHistory.Enabled = true;
                llblShowNewLicenseInfo.Enabled = true;
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = frmLicenseHistory.From_LocalLicenseID(RenewLicenseID);
            if (frm != null)
            {
                using (frm)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void llblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frm_DriverLicense_Info frm = new frm_DriverLicense_Info(RenewLicenseID))
            {
                frm.ShowDialog();
            }
        }
    }
}
