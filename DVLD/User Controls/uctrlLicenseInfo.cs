using DVLD.DTOs.License;
using DVLD.Model.MDL_DriverLicenseInfo;
using DVLD.Properties;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class uctrlLicenseInfo : UserControl
    {
        public uctrlLicenseInfo()
        {
            InitializeComponent();

            ShowDefaultImage();
        }

        public void LoadDefaultData()
        {
            lblClassName.Text = "[???]";
            lblName.Text = "[???]";
            lblLicenseID.Text = "-1";
            lblNationalNo.Text = "[???]";
            lblGendor.Text = "[???]";
            lblIssueDate.Text = "[??/??/????]";
            lblIssueReason.Text = "[???]";
            lblIsActive.Text = "[???]";
            lblDateOfBirth.Text = "[??/??/????]";
            lblDriverID.Text = "-1";
            lblExpirationDate.Text = "[??/??/????]";
            lblIsDetained.Text = "[???]";

            if (pbImagePerson != null)
            {
                if (pbImagePerson.Image != null)
                {
                    pbImagePerson.Image.Dispose();
                    pbImagePerson.Image = null;
                }

                pbImagePerson.Image = (Image)Resources.Person_Question_180.Clone();
            }
        }

        public void LoadDefaultData(int LicenseID)
        {
            lblClassName.Text = "[???]";
            lblName.Text = "[???]";
            lblLicenseID.Text = LicenseID.ToString();
            lblNationalNo.Text = "[???]";
            lblGendor.Text = "[???]";
            lblIssueDate.Text = "[??/??/????]";
            lblIssueReason.Text = "[???]";
            lblIsActive.Text = "[???]";
            lblDateOfBirth.Text = "[??/??/????]";
            lblDriverID.Text = "-1";
            lblExpirationDate.Text = "[??/??/????]";
            lblIsDetained.Text = "[???]";
        }

        public void LoadData(MDL_LicenseDriver_Info LicenseDriverInfo)
        {
            if (LicenseDriverInfo == null)
                return;

            DisplayData(LicenseDriverInfo);
        }

        private void DisplayData(MDL_LicenseDriver_Info dto)
        {
            lblClassName.Text = dto.LicenseClass;
            lblName.Text = dto.FullName;
            lblLicenseID.Text = dto.LicenseID;
            lblNationalNo.Text = dto.NationalNo;
            lblGendor.Text = dto.Gender;
            lblIssueDate.Text = dto.IssueDate;
            lblIssueReason.Text = dto.IssueReason;
            lblIsActive.Text = dto.IsActive;
            lblDateOfBirth.Text = dto.DateOfBirth;
            lblDriverID.Text = dto.DriverID.ToString();
            lblExpirationDate.Text = dto.ExpirationDate;
            lblIsDetained.Text = dto.IsDetained;

            if(!string.IsNullOrWhiteSpace(dto.ImagePath) && File.Exists(dto.ImagePath))
            {
                pbImagePerson.Image?.Dispose();

                using (var img = Image.FromFile(dto.ImagePath))
                {
                    pbImagePerson.Image = new Bitmap(img);
                }
            }
            else
            {
                ShowDefaultImage();
            }
        }

        private void ShowDefaultImage()
        {
            if(!string.IsNullOrWhiteSpace(lblGendor.Text))
            {

                string gender = lblGendor.Text?.Trim().ToUpper();
                pbImagePerson.Image?.Dispose();

                Image img;

                if (gender.StartsWith("F"))
                {
                    img = (Image)Resources.Female_reporter_180.Clone();
                }
                else
                {
                    img = (Image)Resources.Person_Question_180.Clone();
                }

                pbImagePerson.Image = img;
            }
        }
    }
}
