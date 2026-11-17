using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.DTOs;
using DVLD.Properties;
using DVLDBisnessLayer;
using SharedLayer;

namespace DVLD
{
    public partial class frmLogIn : Form
    {
        private bool _idleStarted = false;
        public frmLogIn()
        {
            InitializeComponent();
        }

        private bool checkDataIsFulled()
        {
            bool status = false;

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "الرجاء تعبئة البيانات.");
                status = false;
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
                status = true;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "الرجاء تعبئة البيانات.");
                status = false;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
                status = true;
            }

            return status;
        }

        private void RememberMe()
        {
            Properties.Settings.Default.UserName = clsSimpleEncryptor.Encrypt(txtUserName.Text);
            Properties.Settings.Default.password = clsSimpleEncryptor.Encrypt(txtPassword.Text);


            Settings.Default.Save();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (!checkDataIsFulled())
            {
                return;
            }

            if (clsUser.CheckUserNameAndPasswordExists(txtUserName.Text.Trim(), txtPassword.Text.Trim()))
            {
                errorProvider1.SetError(txtUserName, "");
                errorProvider1.SetError(txtPassword, "");
                if (clsUser.isUserActive(txtUserName.Text.Trim(), txtPassword.Text.Trim()))
                {
                    if (cbRememberMe.Checked == true)
                    {
                        RememberMe();
                    }

                    if (clsUser.FindUserByUserName(txtUserName.Text.Trim(), out DTOUser dtoUser, false))
                    {
                        SharedLayer.Context.AppContext.SetUser(dtoUser.UserID, dtoUser.UserName);
                        frmMainScreen frm = new frmMainScreen(this);
                        frm.ShowDialog();
                        frm.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("المستخدم غير نشط، الرجاء إدخال مستخدم نشط", "غير نشط", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                errorProvider1.SetError(txtUserName, "اسم المستخدم أو كلمة المرور خاطئة.");
                errorProvider1.SetError(txtPassword, "اسم المستخدم أو كلمة المرور خاطئة.");
            }
        }

        private bool LogInImedietly()
        {
            bool isFound = false;
            string UserName = clsSimpleEncryptor.Decrypt(Settings.Default.UserName);
            string Password = clsSimpleEncryptor.Decrypt(Settings.Default.password);

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                isFound = false;
            }
            else
            {
                if (!clsUser.isUserActive(UserName, Password))
                {
                    MessageBox.Show("المستخدم غير نشط، من فضلك اختر مستخدما نشطا.", "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isFound = false;
                }
                else
                {
                    if (clsUser.CheckUserNameAndPasswordExists(UserName, Password))
                    {
                        isFound = true;
                        txtPassword.Enabled = false;
                        txtUserName.Enabled = false;

                        if(clsUser.FindUserByUserName(UserName,out DTOUser dtouser, false))
                        {
                            if (dtouser != null)
                            {
                                SharedLayer.Context.AppContext.SetUser(dtouser.UserID, dtouser.UserName);
                            }
                            else
                            {
                                isFound = false;
                                MessageBox.Show("حدث خطأ حين استرجاع البيانات من قاعدة البيانات إما أن المستخدم غير موجود أو أن هناك خطأ.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            };   
                        }
                        
                    }

                    else
                    {
                        isFound = false;
                        MessageBox.Show("اسم المستخدم وكلمة المرور تم تغييرهما أو حذفهما من فضلك ادخلهم مرة أخرى", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return isFound;
        }

        public void ResetForm()
        {
            SharedLayer.Context.AppContext.CurrentUser.Clear();

            txtPassword.Enabled = true;
            txtUserName.Enabled = true;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            cbRememberMe.Checked = false;
        }

        private  void frmLogIn_Shown(object sender, EventArgs e)
        {
            if (!_idleStarted)
            {
                _idleStarted = true;
                Application.Idle += OnAppIdle;
            }
        }

        private async void OnAppIdle(object sender, EventArgs e)
        {
            txtPassword.Enabled = false;
            txtUserName.Enabled = false;
            cbRememberMe.Enabled = false;
            btnLogIn.Enabled = false;

            Application.Idle -= OnAppIdle;
            await Task.Delay(2000);

            if (LogInImedietly())
            {
                await Task.Delay(2000);
                this.Hide();
                frmMainScreen frm = new frmMainScreen(this);
                frm.ShowDialog();
                frm.Dispose();
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
                cbRememberMe.Enabled = true;
                btnLogIn.Enabled = true;
                this.Show();
            }
            else
            {
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
                cbRememberMe.Enabled = true;
                btnLogIn.Enabled = true;
            }
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            var originalImage = Properties.Resources.close_60;
            var resizedImage = new Bitmap(originalImage, btnClose.Width, btnClose.Height);
            btnClose.Image = resizedImage;
            btnClose.Image = resizedImage;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
