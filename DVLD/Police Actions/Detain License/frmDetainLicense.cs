using DVLD.Classess.Helpers;
using DVLD.DTOs.License;
using DVLD.DTOs.Police_Actions.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.PoliceService.clsRequestPoliceAction;
using DVLDBisnessLayer.Presentation_BL.Local_License.Detain_License;
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

namespace DVLD.License.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        int _LicenseID = -1;
        int _DetainID = -1;
        public Action <int> OnDataAdded { get; set; }

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void BasicConfig()
        {
            _LicenseID = -1;
            _DetainID = -1;
            uctrlLicenseInfo1.LoadDefaultData();
            uctrl_DetainLicense1.LoadDefaultData();
            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicensesInfo.Enabled = false;
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

        private void LoadData()
        {
            if(!int.TryParse(mtxtInput.Text, out int LicenseID))
            {
                return;
            }

            if (_LicenseID == LicenseID)
                return;

            _LicenseID = LicenseID;
            llblShowLicensesHistory.Enabled = false;
            llblShowNewLicensesInfo.Enabled = false;

            Result<DTOLicenseDriverInfo> result_LicenseDriverInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LicenseID);
            if(!result_LicenseDriverInfo.isSuccess)
            {
                MessageBoxHelper.Show(result_LicenseDriverInfo.Message, result_LicenseDriverInfo.messageType ?? SharedLayer.enums.enMessageType.Error,
                    null, true);
                BasicConfig();
                return;
            }

            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicenseDriverInfo.Value));
            uctrl_DetainLicense1.LoadData(_LicenseID);
        }

        OperationResult FillDTO(DTO_DetainLicense dto)
        {
            decimal? Fines = uctrl_DetainLicense1.DetainFines;
 
            if (!Fines.HasValue || Fines.Value <= 0)
                return OperationResult.Failure("قيمة الغرامة غير صالحة!", enMessageType.Error);

            if(_LicenseID == -1)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            dto.LicenseID = _LicenseID;
            dto.FineFees = Fines.Value;
            dto.PoliceActionType = enenPoliceActionType.DetainLicense;

            return OperationResult.Seccuss(null);
        }

        private bool DetainLicense()
        {
            if (_DetainID != -1)
                return false;

            DTO_DetainLicense dto = new DTO_DetainLicense();
            OperationResult result_Fill = FillDTO(dto);
            if(!result_Fill.IsSuccess)
            {
                MessageBoxHelper.Show(result_Fill.Message, result_Fill.MessageType ?? SharedLayer.enums.enMessageType.Error,
                    null, true);
                return false;
            }


            cls_RequestPoliceAction_BL RequestPoliceAction = new cls_RequestPoliceAction_BL(dto);
            OperationResult result_Save = RequestPoliceAction.Save();
            if(!result_Save.IsSuccess)
            {
                MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? SharedLayer.enums.enMessageType.Error,
                    null, true);
                return false;
            }


            _DetainID = dto.DetainID;
            OnDataAdded?.Invoke(_DetainID);
            MessageBoxHelper.Show(result_Save.Message, result_Save.MessageType ?? enMessageType.Info, null, true);
            return true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(DetainLicense())
            {
                Result<DTO_DetainLicense_Info> result_Detain_Info = cls_DetainLicense_Info.GetDetainLicense_Info(_DetainID);
                if (!result_Detain_Info.isSuccess)
                {
                    MessageBoxHelper.Show(result_Detain_Info.Message, result_Detain_Info.messageType ?? SharedLayer.enums.enMessageType.Error,
                        null, true);
                    BasicConfig();
                    return;
                }


                Result<DTOLicenseDriverInfo> result_LicenseInfo = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(_LicenseID);
                if (!result_LicenseInfo.isSuccess)
                {
                    MessageBoxHelper.Show(result_LicenseInfo.Message, result_LicenseInfo.messageType ?? enMessageType.Error, null, true);
                    BasicConfig();
                    return;
                }


                uctrl_DetainLicense1.LoadData(result_Detain_Info.Value);
                uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_LicenseInfo.Value));
                llblShowLicensesHistory.Enabled = true;
                llblShowNewLicensesInfo.Enabled = true;
            }
        }

        private void llblShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frm_DriverLicense_Info frm = new frm_DriverLicense_Info(_LicenseID))
            {
                frm.ShowDialog();
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
    }
}
