using DVLD.DTOs.License.ReNew;
using DVLD.Model.LocalLicense.Renew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class uctrl_RenewLicense : UserControl
    {
        public uctrl_RenewLicense()
        {
            InitializeComponent();
        }

        public decimal? AppFees
        {
            get
            {
                string sAppFees = lblApplicationFees.Text.Replace("$", "");
                if (!decimal.TryParse(sAppFees, out decimal appFees))
                    return null;
                return appFees;
            }
        }

        public decimal? LicenseFees
        {
            get
            {
                string sLicenseFees = lblLicenseFees.Text.Replace("$", "");
                if (!decimal.TryParse(sLicenseFees, out decimal LicenseFees))
                    return null;
                return LicenseFees;
            }
        }


        public void LoadDefaultData(decimal AppFees)
        {
            string DateNow = DateTime.Now.ToString("dd/MM/yyyy");

            lblR_L_ApplicationID.Text = "-1";
            lblApplicationDate.Text = DateNow;
            lblIssueDate.Text = DateNow;
            lblApplicationFees.Text = AppFees.ToString();
            lblLicenseFees.Text = "[???]";
            lblRenewLicenseID.Text = "-1";
            lblOldLicenseID.Text = "-1";
            lblExpirationDate.Text = "[???]";
            lblCreatedBy.Text = SharedLayer.Context.AppContext.CurrentUser.UserName;
            lblTotalFees.Text = "[???]";
        }

        public void LoadData(MDL_RenewLicense_Info dto)
        {
            lblR_L_ApplicationID.Text = dto.R_L_ApplicationID;
            lblApplicationDate.Text = dto.ApplicationDate;
            lblIssueDate.Text = dto.IssueDate;
            lblApplicationFees.Text = dto.ApplicationFees;
            lblLicenseFees.Text = dto.LicenseFees.ToString();
            lblRenewLicenseID.Text = dto.RenewedLicenseID;
            lblOldLicenseID.Text = dto.OldLicenseID;
            lblExpirationDate.Text = dto.ExpirationDate;
            lblCreatedBy.Text = dto.CreatedBy;
            lblTotalFees.Text = dto.TotalFees;
        }
    }
}
