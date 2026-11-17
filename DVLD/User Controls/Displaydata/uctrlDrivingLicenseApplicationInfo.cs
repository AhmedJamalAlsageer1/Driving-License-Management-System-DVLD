using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.DTOs.Applications;
using DVLD.People;
using DVLDBisnessLayer;
using DVLDBisnessLayer.New_LocalDriver_App;
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

namespace DVLD.User_Controls.Displaydata
{
    public partial class uctrlDrivingLicenseApplicationInfo : UserControl
    {
        int App_ID = -1; 

        public uctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadData(DTO_LDLFull_Info dto)
        {
            DisplayApplicationData(dto);
        }

        public void ClearControl()
        {
            lblAppID.Text = "-1";
            lblLDLAppID.Text = "-1";
            lblLicenseClass.Text = "-1";
            lblStatus.Text = "New";
            lblPassedTest.Text = "0/3";
            lblType.Text = "None";
            lblApplicant.Text = "None";
            lblDate.Text = "??/??/????";
            lblStatusDate.Text = "??/??/????";
            lblCreatedBy.Text = "None";
            lblFees.Text = "-1$";

            App_ID = -1;
        }

        private void DisplayApplicationData(DTO_LDLFull_Info dto)
        {
            ClearControl();

            lblAppID.Text = dto.AppID.ToString();
            lblLDLAppID.Text = dto.LDL_AppID.ToString();
            lblLicenseClass.Text = dto.LicenseDrivingName.ToString();
            lblStatus.Text = dto.StatusApplication.ToString();
            lblPassedTest.Text = dto.PassedTest.ToString() + "/3";
            lblType.Text = dto.ApplicationType.ToString();
            lblApplicant.Text = dto.Applicant.ToString();
            lblDate.Text = dto.ApplicationDate.ToString("dd/MM/yyyy");
            lblStatusDate.Text = dto.StatusDate.ToString("dd/MM/yyyy");
            lblCreatedBy.Text = dto.CreatedBy.ToString();
            lblFees.Text = dto.Fees.ToString() + "$";

            App_ID = dto.AppID;
        }

        private void Llbl_ViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (App_ID == -1)
                return;

            Result <int> resultperonsID = cls_LDL_ApplicationRequest_BL.GetPersonIDBy_AppID(App_ID);

            if(!resultperonsID.isSuccess)
            {
                MessageBoxHelper.Show(resultperonsID,"خطأ", true);
                return;
            }

            int personID = resultperonsID.Value;
            frmPersonDetails frm = new frmPersonDetails(personID);
            frm.ShowDialog();
        }
    }
}
