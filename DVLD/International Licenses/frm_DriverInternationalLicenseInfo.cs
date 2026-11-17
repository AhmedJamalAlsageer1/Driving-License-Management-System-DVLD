using DVLD.Classess.Helpers;
using DVLD.DTOs.International_License;
using DVLDBisnessLayer.InternationalLicense;
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

namespace DVLD.Internationall_Licenses
{
    public partial class frm_DriverInternationalLicenseInfo : Form
    {
        public bool IsDataLoadedSuccessfully { get; set; }
        public frm_DriverInternationalLicenseInfo(int I_LicenseID)
        {
            InitializeComponent();

            if(I_LicenseID <= 0)
            {
                MessageBoxHelper.Show("معرف الرخصة غير صالح يرجى إدخال قيمة صالحة!", SharedLayer.enums.enMessageType.Error, "خطأ", true);
                this.IsDataLoadedSuccessfully = false;
            }


            Result<DTO_DriverInternationalLicenseInfo> result_dto = cls_DriverInternationalLicenseInfo_BL.GetDriverInternationalLicenseInfo(I_LicenseID);
            if (!result_dto.isSuccess || result_dto.Value == null)
            {
                MessageBoxHelper.Show("خطأ غير مقصود يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error, "خطأ", true);
                this.IsDataLoadedSuccessfully = false;
            }

            uctrl_DriverInternationalLicenseInfo1.LoadData(result_dto.Value);
            IsDataLoadedSuccessfully = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
