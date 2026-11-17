using DVLD.Classess.UIItems;
using DVLD.Drivers;
using DVLD.FormsApplications;
using DVLD.Inputs;
using DVLD.International_Licenses;
using DVLD.Internationall_Licenses;
using DVLD.License.Detain_License;
using DVLD.License.Release_Detained_License;
using DVLD.License.Replacement_Or_Demage;
using DVLD.Person_Forms;
using DVLD.Police_Actions.Detain_License.Manage_Detained_License;
using DVLD.Properties;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmMainScreen : Form
    {
        private frmLogIn _LoginForm; 

        public frmMainScreen(frmLogIn LoginForm)
        {
            InitializeComponent();

            _LoginForm = LoginForm;
        }

        private void poepleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMangePoeple frm = new frmMangePoeple();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.UserName = string.Empty;
            Settings.Default.password = string.Empty;
            Settings.Default.Save();
            _LoginForm.ResetForm();
            this.Close();
            this.Dispose();
        }

        private void usersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmManageUsers frm = new frmManageUsers();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SharedLayer.Context.AppContext.CurrentUser.IsAuthenticated)
            {
                if (clsUser.FindUserByUserID(SharedLayer.Context.AppContext.CurrentUser.UserID.Value, out DTOs.DTOUser dtoUser, false))
                {
                    SharedLayer.Context.AppContext.SetUser(dtoUser.UserID, dtoUser.UserName);
                    frmUserInformation frm = new frmUserInformation(SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
                    frm.ShowDialog();
                    frm.Dispose();
                }
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharedLayer.Context.AppContext.CurrentUser.IsAuthenticated)
            {
                if (clsUser.FindUserByUserID(SharedLayer.Context.AppContext.CurrentUser.UserID.Value, out DTOs.DTOUser dtoUser, false))
                {
                    SharedLayer.Context.AppContext.SetUser(dtoUser.UserID, dtoUser.UserName);
                    EditUserInput _inputuser = new EditUserInput(dtoUser);
                    frmChangePassword frm = new frmChangePassword(_inputuser);
                    frm.ShowDialog();
                    frm.Dispose();
                }
            }
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTypeTest frm = new frmManageTypeTest();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApplication frm = new frmNewLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frm.Dispose();
        }
        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLDLApplications frm = new frmManageLDLApplications();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frm_ManageInternationalLicenseApplications frm = new frm_ManageInternationalLicenseApplications())
            {
                frm.ShowDialog();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frmDrivers frm = new frmDrivers())
            {
                frm.ShowDialog();
            }

        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication())
            {
                frm.ShowDialog();
            }
        }

        private void reNewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frm_RenewLocalDrivingLicense frm = new frm_RenewLocalDrivingLicense())
            {
                frm.ShowDialog();
            }
        }

        private void replacementForLoasOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmReplacementForLostOrDamagedLicense frm = new frmReplacementForLostOrDamagedLicense())
            {
                frm.ShowDialog();
            }
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDetainLicense frm = new frmDetainLicense())
            {
                frm.ShowDialog();
            }
        }

        private void releaseDetaindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frm_ReleaseDetainedLicense frm = new frm_ReleaseDetainedLicense())
            {
                frm.ShowDialog();
            }
        }

        private void manageDetainLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frm_ManageDetainedLicense frm = new frm_ManageDetainedLicense())
            {
                frm.ShowDialog();
            }
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frm_ReleaseDetainedLicense frm = new frm_ReleaseDetainedLicense())
            {
                frm.ShowDialog();
            }
        }
    }
}
