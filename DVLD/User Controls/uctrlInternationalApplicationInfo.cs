using DVLD.DTOs.License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class uctrlInternationalApplicationInfo : UserControl
    {
        public uctrlInternationalApplicationInfo()
        {
            InitializeComponent();
        }

        public decimal? Fees_UI
        {
            get
            {
                string Fees = lblFees.Text.Replace("$", "").Trim();
                if (decimal.TryParse(Fees, out decimal value))
                    return value;

                return null;
            }
        }

        public void Initialize_Uctrl_Defaults(DTO_IntLicense dto, bool Initial = true, byte ValidityLength = 0)
        {
            DateTime Now = DateTime.Now.Date;
            string formattedDate = Now.ToString("dd/MM/yyyy");

            lbl_I_LicenseApplicationID.Text = "[???]";
            lblApplicationDate.Text = formattedDate;
            lblIssueDate.Text = formattedDate;
            lbl_ILicenseID.Text = "[???]";
            lblCreatedBy.Text = "[???]";

            if(Initial)
            {
                lblLocalLicenseID.Text = "[???]";
                lblExpiritionDate.Text = "[???]";
                lblFees.Text = dto.AppFees.ToString() + "$";
            }
            else
            {
                lblLocalLicenseID.Text = dto.Local_LicenseID.ToString();
                lblExpiritionDate.Text = (ValidityLength > 0)? Now.AddYears(ValidityLength).ToString("dd/MM/yyyy") : "[???]";
            }
        }

        public void LoadData(DTO_IntLicense dto)
        {
            string FormattingDate = "dd/MM/yyyy";

            lbl_I_LicenseApplicationID.Text = dto.Int_LicenseApplicationID.ToString();
            lblApplicationDate.Text = dto.ApplicationDate.ToString(FormattingDate);
            lbl_ILicenseID.Text = dto.Int_LicenseID.ToString();
            lblIssueDate.Text = dto.IssueDate.ToString(FormattingDate);
            lblCreatedBy.Text = dto.CreatedBy.ToString();
            lblLocalLicenseID.Text = dto.Local_LicenseID.ToString();
            lblExpiritionDate.Text = dto.ExpirationDate.ToString(FormattingDate);
            lblFees.Text = dto.AppFees.ToString() + "$";
        }
    }
}
