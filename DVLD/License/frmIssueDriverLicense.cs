using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.DTOs.Applications;
using DVLD.DTOs.License;
using DVLDBisnessLayer;
using DVLDBisnessLayer.License;
using DVLDBisnessLayer.New_LocalDriver_App;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.License
{

    public partial class frmIssueDriverLicense : Form
    {
        int AppID = -1;

        public frmIssueDriverLicense(int LDL_AppID)
        {
            InitializeComponent();

            if(LDL_AppID <= 0)
            {
                MessageBoxHelper.Show("معرف غير صالح يرجى التأكد منه!", SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                this.Close();
                return;
            }

            Result <DTO_LDLFull_Info> LDL_App_Info = clsApplicationDisplayBL.Find_FullLDL_Info(LDL_AppID);
            if(!LDL_App_Info.isSuccess || LDL_App_Info.Value == null)
            {
                MessageBoxHelper.Show(LDL_App_Info, "خطأ", true);
                this.Close();
                return;
            }

            this.uctrlDrivingLicenseApplicationInfo1.LoadData(LDL_App_Info.Value);

            Result<decimal> Result_Fees = cls_LDL_ApplicationRequest_BL.GetLicenseClassFeesRelatedBy_LDLAppID(LDL_AppID);
            if(!Result_Fees.isSuccess)
            {
                MessageBoxHelper.Show(Result_Fees, "خطأ", true);
                this.Close();
                return;
            }

            lblLicenseFees.Text = "$" + Result_Fees.Value.ToString();
            AppID = LDL_App_Info.Value.AppID;
        }

        public DTO_LocalLicense GetDTO()
        {
            string Notes = string.IsNullOrWhiteSpace(txtNotes.Text.Trim()) ? null : txtNotes.Text.Trim();

            string feesText = lblLicenseFees.Text.Replace("$", "").Trim();
            if (decimal.TryParse(feesText, out decimal UIFees))
            {

                return new DTO_LocalLicense
                {
                    ApplicationID = AppID,
                    Notes = Notes,
                    IssueReason = enIssueReasonLicense.FirstTime,
                    PaidFees = UIFees,
                    ApplicationTypes = enApplicationTypes.NewLocalLicense
                };
            }

            return null;
        }

        private OperationResult IssueLicense()
        {
            if (AppID <= 0)
            {
                return OperationResult.Failure("تأكد من إدخال قيم صالحة من فضلك", enMessageType.Error);
            }

            DTO_LocalLicense dtoLicense = GetDTO();

            if(dtoLicense == null)
            {
                return OperationResult.Failure("الرسوم المدخلة غير صالحة. يرجى التحقق من القيمة الظاهرة.", enMessageType.Warning);
            }

            cls_LocalLicenseRequest_BL request_NewLocalLicense = new cls_LocalLicenseRequest_BL(dtoLicense);
            OperationResult result_Save = request_NewLocalLicense.Save();
            if (!result_Save.IsSuccess)
            {
                return result_Save;
            }

            return result_Save;
        }

        public event Action OnUpdate;

        private void btnIssue_Click(object sender, EventArgs e)
        {
            enMessageType messageType;

            OperationResult result_IssueLicense =  IssueLicense();
            if(!result_IssueLicense.IsSuccess)
            {
                messageType = result_IssueLicense.MessageType.HasValue ? result_IssueLicense.MessageType.Value : enMessageType.Error;
                MessageBoxHelper.Show(result_IssueLicense.Message, messageType, "خطأ", true);
                return;
            }

            OnUpdate?.Invoke();

            if(result_IssueLicense.Message != null)
            {
                messageType = result_IssueLicense.MessageType.HasValue ? result_IssueLicense.MessageType.Value : enMessageType.Warning;
                MessageBoxHelper.Show(result_IssueLicense.Message, messageType, "تنويه", true);
                return;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
