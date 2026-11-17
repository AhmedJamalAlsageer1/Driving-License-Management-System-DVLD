using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.Inputs;
using DVLD.ViewModel;
using DVLDBisnessLayer;
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

namespace DVLD
{
    public partial class frmChangePassword : Form
    {
        private int UserID;
        public frmChangePassword(EditUserInput _input)
        {
            InitializeComponent();

            if( _input == null || _input.UserID <= 0 || _input.PersonID <= 0)
            {
                return;
            }

            UserID = _input.UserID;

            Result<DTOUser> resultUser = clsUser.FindUserByUserID(UserID);

            if(!resultUser.isSuccess || resultUser.Value == null)
            {
                MessageBoxHelper.Show(resultUser.Message, SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                this.Close();
            }

            Result <DTOPerson> resultPerson = clsPerson.FindByUserID(UserID);
            if(!resultPerson.isSuccess || resultPerson.Value == null)
            {
                MessageBoxHelper.Show(resultUser.Message, SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                this.Close();
            }

            DTOPerson dtoPerson = resultPerson.Value;
            uctrlPersonInfo1.LoadData(dtoPerson);
            uctrlLoginInformation1.UpdateControl(_input.GetUserIdDisplay(), _input.UserName, _input.GetStatusActiveText());
        }

        private bool CheckCurrentPassword()
        {
            if(string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "من فضلك ادخل كلمة السر الحالية.");
                return false;
            }

            if(!clsUser.isPasswordCurrect(UserID, txtCurrentPassword.Text.Trim()))
            {
                errorProvider1.SetError(txtCurrentPassword, "كلمة المرور غير صحيحة!");
                return false;
            }

            errorProvider1.SetError(txtCurrentPassword, "");
            return true;
        }

        private bool checkNewPassword()
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, "من فضلك ادخل كلمة المرور الجديدة .");
                return false;
            }

            errorProvider1.SetError(txtNewPassword, "");

            return true;
        }

        private bool CheckConfrimPassword()
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "من فضلك ادخل كلمة المرور الجديدة .");
                return false;
            }

            if(txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPassword, "كلمتا المرور غير متطابقتين!");
                return false;
            }

            errorProvider1.SetError(txtConfirmPassword, "");
            return true;
        }

        private bool CanEnableSaveButton()
        {
            string CurrentPassword = txtCurrentPassword.Text.Trim();
            string NewPassword = txtNewPassword.Text.Trim();
            string ConfirmPassword = txtConfirmPassword.Text.Trim();

            if (!clsUser.isPasswordCurrect(UserID, CurrentPassword))
                return false;

            if (string.IsNullOrWhiteSpace(NewPassword))
                return false;

            if (NewPassword != ConfirmPassword)
                return false;

            return true;
        }

        private void txtCurrentPassword_TextChanged(object sender, EventArgs e)
        {
            CheckCurrentPassword();
            btnSave.Enabled = CanEnableSaveButton();
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            checkNewPassword();
            CheckConfrimPassword();
            btnSave.Enabled = CanEnableSaveButton();
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            CheckConfrimPassword();
            btnSave.Enabled = CanEnableSaveButton();
        }

        private bool SaveNewPassword()
        {
            if (CanEnableSaveButton())
            {
                if (clsUser.UpdatePassword(UserID, txtCurrentPassword.Text, txtNewPassword.Text))
                {
                    return true;
                }
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveNewPassword())
                MessageBox.Show("تم تغيير كلمة المرور بنجاح.");
            else
                MessageBox.Show("فشلت عملية تغيير كلمة المرور، من فضلك حاول مرة أخرى.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
