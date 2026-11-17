using DVLD.Model.MDL_RelaseDetainedLicense;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class Uctrl_ReleaseDetainedLicense_Info : UserControl
    {
        public Uctrl_ReleaseDetainedLicense_Info()
        {
            InitializeComponent();
        }

        public decimal? AppFees
        {
            get
            {
                string s_Appfees = lblApplicationFees.Text.Replace("$", "");
                if(decimal.TryParse(s_Appfees, out decimal value))
                    return value;
                return null;
            }
        }

        public decimal? DetainFines
        {
            get
            {
                string DetainFines = lblDetainFines.Text.Replace("$", "");
                if (decimal.TryParse(DetainFines, out decimal value))
                    return value;
                return null;
            }
        }

        public void LoadDefaultData(decimal AppFees)
        {
            lblDetainID.Text = "-1";
            lblDetainDate.Text = "??/??/????";
            lblApplicationFees.Text = AppFees.ToString() + "$";
            lblTotalFees.Text = "$$$$";
            lblLicenseID.Text = "-1";
            lblCreatedby.Text = "-1";
            lblDetainFines.Text = "$$$$";
            lblReleaseDetainedBy.Text = "???";
        }

        public void LoadDefaultData()
        {
            lblDetainID.Text = "-1";
            lblDetainDate.Text = "??/??/????";
            lblApplicationFees.Text = "$$$$";
            lblTotalFees.Text = "$$$$";
            lblLicenseID.Text = "-1";
            lblCreatedby.Text = "-1";
            lblDetainFines.Text = "$$$$";
            lblReleaseDetainedBy.Text = "???";
        }

        public void LoadData(int LicenseID, decimal AppFees)
        {
            lblDetainID.Text = "-1";
            lblDetainDate.Text = "??/??/????";
            lblApplicationFees.Text = "$$$$";
            lblTotalFees.Text = AppFees.ToString() + "$";
            lblLicenseID.Text = LicenseID.ToString();
            lblCreatedby.Text = "-1";
            lblDetainFines.Text = "$$$$";
            lblReleaseDetainedBy.Text = "???";
        }

        public void LoadData(MDL_RelaseDetainedLicense MDL)
        {
            lblDetainID.Text = MDL.DetainID;
            lblDetainDate.Text = MDL.DetainDate;
            lblApplicationFees.Text = MDL.ApplicationFees;
            lblTotalFees.Text = MDL.TotalFees;
            lblLicenseID.Text = MDL.LicenseID;
            lblCreatedby.Text = MDL.Createdby;
            lblDetainFines.Text = MDL.DetainFines;
            lblReleaseDetainedBy.Text = MDL.ReleaseDetainedBy;
        }
    }
}
