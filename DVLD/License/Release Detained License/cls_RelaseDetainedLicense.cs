using DVLD.Classess.Helpers;
using DVLD.DTOs.License;
using DVLD.DTOs.Presention_DTO.Licnese.Release_Detain;
using DVLDBisnessLayer;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.Presentation_BL.Local_License.Release_Detained_License;
using SharedLayer;
using System;
using DVLD.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Model.MDL_RelaseDetainedLicense;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLD.DTOs.License.Local_License.Release_Detained_License;

namespace DVLD.License.Release_Detained_License
{
    public partial class frm_ReleaseDetainedLicense : Form
    {
        int _LicenseID = -1;
        int _ReleasedApplicationID = -1;
        bool IsLiceneRelease = false;
        public Action OnDataUpdate { get; set; }

        public frm_ReleaseDetainedLicense()
        {
            InitializeComponent();
            BasicConfig();
        }

        public frm_ReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            BasicConfigFromFactory(LicenseID);
        }

        public void BasicConfigFromFactory(int LicenseID)
        {
            mtxtInput.Text = LicenseID.ToString();
            LoadData();
        }

        private void mtxtInput_Click(object sender, EventArgs e)
        {
            if(mtxtInput.SelectionStart > mtxtInput.Text.Length)
                mtxtInput.SelectionStart = mtxtInput.Text.Length;
        }

        private void mtxtInput_DoubleClick(object sender, EventArgs e)
        {
            mtxtInput.SelectionStart = 0;
            mtxtInput.SelectionLength = mtxtInput.Text.Length;
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

        private void BasicConfig()
        {

            Result<decimal> result_Fees = clsApplicationTypesBL.GetApplicationFees
                (SharedLayer.enums.enApplicationTypes.ReleaseDetainedDrivingLicsense);
            if(!result_Fees.isSuccess)
            {
                uctrl_ReleaseDetainedLicense_Info1.LoadDefaultData();
            }


            if (_LicenseID != -1)
            {
                uctrlLicenseInfo1.LoadDefaultData(_LicenseID);
            }
            else
            {
                uctrlLicenseInfo1.LoadDefaultData();
            }


            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicensesInfo.Enabled = false;
            uctrl_ReleaseDetainedLicense_Info1.LoadDefaultData(result_Fees.Value);
        }

        private void LoadData()
        {
            if (!int.TryParse(mtxtInput.Text, out int LicenseID))
                return;

            if (LicenseID == _LicenseID)
                return;

            _LicenseID = LicenseID;
            Result<DTOLicenseDriverInfo> result_LicneseInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(_LicenseID);
            if(!result_LicneseInfo.isSuccess)
            {
                MessageBoxHelper.Show(result_LicneseInfo.Message, result_LicneseInfo.messageType ?? SharedLayer.enums.enMessageType.Error, null,
                    true);
                BasicConfig();
                return;
            }


            Result<DTO_ReleaseDetain_Info> result_releaseInfo = cls_ReleaseDetainedLicense_Info.GetReleaseDetainConfig_Info
                (LicenseID, SharedLayer.enums.enApplicationTypes.ReleaseDetainedDrivingLicsense);
            if(!result_releaseInfo.isSuccess)
            {
                MessageBoxHelper.Show(result_releaseInfo.Message, result_releaseInfo.messageType ?? SharedLayer.enums.enMessageType.Error, null,
                 true);
                BasicConfig();
                return;
            }

            IsLiceneRelease = result_releaseInfo.Value.IsReleased;
            if (IsLiceneRelease)
            {
                MessageBoxHelper.Show(result_releaseInfo.Message, result_releaseInfo.messageType ?? SharedLayer.enums.enMessageType.Warning, null,
                 true);
            } 

            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicneseInfo.Value));
            uctrl_ReleaseDetainedLicense_Info1.LoadData(new MDL_RelaseDetainedLicense(result_releaseInfo.Value, LicenseID));
        }

        private OperationResult FillDTO(DTO_ReleaseDetainedLicense dto)
        {
            dto.LicenseID = _LicenseID;
            dto.ApplicationTypes = SharedLayer.enums.enApplicationTypes.ReleaseDetainedDrivingLicsense;
            
            decimal? DetainFines = uctrl_ReleaseDetainedLicense_Info1.DetainFines;
            decimal? AppFees = uctrl_ReleaseDetainedLicense_Info1.AppFees;

            if (!DetainFines.HasValue)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if(!AppFees.HasValue)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            dto.Fines = DetainFines.Value;
            dto.PaidFees = AppFees.Value;
            return OperationResult.Seccuss(null);
        }

        bool ReleaseLicense()
        {
            if (_LicenseID == -1)
                return false;

            DTO_ReleaseDetainedLicense dto = new DTO_ReleaseDetainedLicense();
            OperationResult reusult_Fill = FillDTO(dto);
            if(!reusult_Fill.IsSuccess)
            {
                MessageBoxHelper.Show(reusult_Fill.Message, reusult_Fill.MessageType ?? SharedLayer.enums.enMessageType.Error, null,
                true);
                return false;
            }

            cls_LocalLicenseRequest_BL Request_release = new cls_LocalLicenseRequest_BL(dto);
            OperationResult result_Save = Request_release.Save();
            if (!result_Save.IsSuccess)
            {
                MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? SharedLayer.enums.enMessageType.Error, null,
                true);
                return false;
            }

            _ReleasedApplicationID = dto.ReleaseApplicationID.Value;
            MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? SharedLayer.enums.enMessageType.Info, null,
            true);
            return true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if(IsLiceneRelease)
            {
                MessageBoxHelper.Show(
                    "هذه الرخصة تم فك حجزها مسبقًا، لا يمكن تنفيذ عملية الحفظ مرة أخرى.",
                    SharedLayer.enums.enMessageType.Error,
                    null,
                    true);
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من فك هذه الرخصة؟", "تنبيه", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.OK)
            {
                if(ReleaseLicense())
                {
                    if (_ReleasedApplicationID == -1)
                        return;

                    Result<DTOLicenseDriverInfo> result_LicneseInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(_LicenseID);
                    if (!result_LicneseInfo.isSuccess)
                    {
                        MessageBoxHelper.Show(result_LicneseInfo.Message, result_LicneseInfo.messageType ?? SharedLayer.enums.enMessageType.Error, null,
                            true);
                        return;
                    }


                    Result<DTO_ReleaseDetain_Info> result_releaseInfo = cls_ReleaseDetainedLicense_Info.GetReleaseDetain_Info
                        (_ReleasedApplicationID);
                    if(!result_releaseInfo.isSuccess)
                    {
                        MessageBoxHelper.Show(result_releaseInfo.Message, result_releaseInfo.messageType ?? SharedLayer.enums.enMessageType.Error, null,
                        true);
                        return;
                    }

                    llblShowLicensesHistory.Enabled = true;
                    llblShowNewLicensesInfo.Enabled = true;
                    uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicneseInfo.Value));
                    uctrl_ReleaseDetainedLicense_Info1.LoadData(new MDL_RelaseDetainedLicense(result_releaseInfo.Value));
                    OnDataUpdate?.Invoke();
                }
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = frmLicenseHistory.From_LocalLicenseID(_LicenseID);
            if (frm != null)
            {
                using (frm)
                {
                    frm.ShowDialog();
                }
            }
        }

        private void llblShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frm_DriverLicense_Info frm = new frm_DriverLicense_Info(_LicenseID))
            {
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
