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

namespace DVLD.LicenseForm
{
    public partial class frmShowLicenseDetails : Form
    {
        public frmShowLicenseDetails(DTOLicenseDriverInfo dto)
        {
            InitializeComponent();

            if (dto == null)
            {
                this.Close();
                return;
            }

            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(dto));
        }

        public frmShowLicenseDetails(int LicenseID)
        {
            InitializeComponent();

            Result<DTOLicenseDriverInfo>  result_dtoLicense = clsLicenseDriverInfo_BL.FindLicenseDriverInfoByID(LicenseID);
            if (!result_dtoLicense.isSuccess)
            {
                MessageBoxHelper.Show(result_dtoLicense.Message, result_dtoLicense.messageType  ?? SharedLayer.enums.enMessageType.Error, null,
                    true);
                this.Close();
            }

            uctrlLicenseInfo1.LoadData(new MDL_LicenseDriver_Info(result_dtoLicense.Value));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
