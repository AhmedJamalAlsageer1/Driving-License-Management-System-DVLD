using DVLD.DTOs.International_License;
using DVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class Uctrl_DriverInternationalLicenseInfo : UserControl
    {
        public Uctrl_DriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadData(DTO_DriverInternationalLicenseInfo dto)
        {
            string FormattingDate = "dd/MM/yyyy";

            lblName.Text = dto.FullName;
            lblIntLicenseID.Text = dto.Int_LicenseID.ToString();
            lblLicenseID.Text = dto.LicenseID.ToString();
            lblNationalNo.Text = dto.NationalNo.ToString();
            lblGendor.Text = dto.Gendor == true? "Female" : "Male";
            lblIssueDate.Text = dto.IssueDate.ToString(FormattingDate);
            lblApplicationID.Text = dto.ApplicationId.ToString();
            lblIsActive.Text = dto.IsActive == true? "Yes" : "No";
            lblDateOfBirth.Text = dto.DateOfBirth.ToString(FormattingDate);
            lblDriverID.Text = dto.DriverID.ToString();
            lblExpirationDate.Text = dto.ExpirationDate.ToString(FormattingDate);


            pbImageDriver.Image?.Dispose();
            pbImageDriver.Image = null;


            if (File.Exists(dto.ImagePath))
            {
                using (FileStream fs = new FileStream(dto.ImagePath, FileMode.Open, FileAccess.Read))
                {
                    pbImageDriver.Image = Image.FromStream(fs);
                }
            }
            else
            {
                pbImageDriver.Image = dto.Gendor == true ?
               (Image) Resources.Female_reporter_180.Clone():
               (Image) Resources.Person_Question_180.Clone();
            }
        }
    }
}
