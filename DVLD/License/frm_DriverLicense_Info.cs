using DVLD.Classess.Helpers;
using DVLD.DTOs.License;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLDBisnessLayer.License;
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

namespace DVLD.License
{
    public partial class frm_DriverLicense_Info : Form
    {
        public frm_DriverLicense_Info(int LocalLicenseID)
        {
            InitializeComponent();

            if (LocalLicenseID == -1)
            {
                this.Close();
                return;
            }

            Result<DTOLicenseDriverInfo> result_Dto = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LocalLicenseID);
            if(!result_Dto.isSuccess)
            {
                MessageBoxHelper.Show(result_Dto.Message, result_Dto.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                this.Close();
                return;
            }

            LoadData(result_Dto.Value);
        }

        private void LoadData(DTOLicenseDriverInfo dto)
        {
            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(dto));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
