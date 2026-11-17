using DVLD.Model.LocalLicense.Replacement;
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
    public partial class uctrl_ApplicationForLicenseReplacement : UserControl
    {
        public uctrl_ApplicationForLicenseReplacement()
        {
            InitializeComponent();
        }

        public decimal? AppFees
        {
            get
            {
                string s_appfees = lblApplicationFees.Text.Replace("$", "").Trim();
                if(decimal.TryParse(s_appfees, out decimal appfees))
                {
                    return appfees;
                }
                return null;
            }
        }

        public void LoadDefaultData()
        {
            lblR_ApplicationID.Text = "-1";
            lblApplicationDate.Text = "[???]";
            lblApplicationFees.Text = "[???]";
            lblReplacedLicenseID.Text = "-1";
            lblOldLicenseID.Text = "-1";
            lblCreatedby.Text = "[???]";
        }

        public void LoadData(MDL_ReplacementLocalLicense MDL)
        {
            lblR_ApplicationID.Text = MDL.R_ApplicationID;
            lblReplacedLicenseID.Text = MDL.ReplacedLicenseID;
            lblApplicationDate.Text = MDL.ApplicationDate;
            lblApplicationFees.Text = MDL.ApplicationFees + "$";
            lblOldLicenseID.Text = MDL.OldLicenseID;
            lblCreatedby.Text = MDL.Createdby;
        }
    }
}
