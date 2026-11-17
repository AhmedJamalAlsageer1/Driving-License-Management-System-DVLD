using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DVLD.Person_Forms
{
    public partial class frmAddNewUser : Form
    {
        enum enMode { AddNewUser, UpdateUser};
        enMode Mode = enMode.AddNewUser;

        DTOUser dtoUser;
        string NewUserAdded = string.Empty;

        public frmAddNewUser()
        {
            InitializeComponent();
            Mode = enMode.AddNewUser;
            tabControl1.TabPages.Remove(tpLoginInfo);
        }

        private void FillControllsWithData()
        {
            llUserID.Text = dtoUser.UserID.ToString();
            txtUserName.Text = dtoUser.UserName;
            txtPassword.Text = dtoUser.Password;
            txtConfirmPassword.Text = dtoUser.Password;
            chbIsActive.Checked = dtoUser.IsActive;
        }

        private void LoadDefaultForEditMode(int UserID)
        {
            Mode = enMode.UpdateUser;
            txtdata.Enabled = false;
            llMode.Text = "Update User";
            if (clsUser.FindUserByUserID(UserID, out dtoUser))
            {
                if(dtoUser != null)
                {
                    Result<DTOPerson> resultPerson = clsPerson.FindByID(dtoUser.PersonID);

                    if(!resultPerson.isSuccess || resultPerson.Value == null)
                    {
                        MessageBoxHelper.Show(resultPerson,"خطأ", true);
                        return;
                    }

                    uctrlPersonInfo1.LoadData(resultPerson.Value);
                    FillControllsWithData();
                }
            }
            else
            {
                if(MessageBox.Show("Some Thing Wrong Happen, Do You Want To Add a User Beside Edit?", "Wrong", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Mode = enMode.AddNewUser;
                    DefaultOptionsControls();
                }
            }
        }

        public frmAddNewUser(int UserID)
        {
            InitializeComponent();
            LoadDefaultForEditMode(UserID);
        }

        private void FindBy()
        {
            if (string.IsNullOrWhiteSpace(txtdata.Text))
            {
                MessageBox.Show("خطأ، لا يوجد بيانات صالحة، من فضلك أدخل بيانات صالحة لإدخالها.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbFindBy.SelectedItem == null)
            {
                MessageBox.Show("خطأ،من فضلك أختر بأي طريقة تبحث.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DefaultOptionsControls();

            if (cbFindBy.SelectedItem.ToString() == "Person ID")
            {
                if (isValidPesonID())
                {
                    if (!int.TryParse(txtdata.Text, out int personID))
                    {
                        MessageBoxHelper.Show("من فضلك ادخل رقما صالحا", SharedLayer.enums.enMessageType.Error,
                            "خطأ قيمة غير صالحة", true);
                        return;
                    }

                    Result<DTOPerson> resultPerson = clsPerson.FindByID(personID);

                    if (!resultPerson.isSuccess || resultPerson.Value == null)
                    {
                        MessageBoxHelper.Show(resultPerson, "خطأ", true);
                        return;
                    }

                    uctrlPersonInfo1.LoadData(resultPerson.Value);
                    if (!tabControl1.TabPages.Contains(tpLoginInfo))
                        tabControl1.TabPages.Add(tpLoginInfo);
                    
                }

            }
            else if (cbFindBy.SelectedItem.ToString() == "National No")
            {
                Result <DTOPerson> resultPerson = clsPerson.FindByNationalNo(txtdata.Text);

                if (!resultPerson.isSuccess || resultPerson.Value == null)
                {
                    MessageBoxHelper.Show(resultPerson, "خطأ", true);
                    return;
                }

                uctrlPersonInfo1.LoadData(resultPerson.Value);
                if (!tabControl1.TabPages.Contains(tpLoginInfo))
                    tabControl1.TabPages.Add(tpLoginInfo);
                
            }
            else if (cbFindBy.SelectedItem.ToString() == "Name")
            {
                Result<DTOPerson> resultPerson = clsPerson.FindByName(txtdata.Text);

                if (!resultPerson.isSuccess || resultPerson.Value == null)
                {
                    MessageBoxHelper.Show(resultPerson, "خطأ", true);
                    return;
                }

                uctrlPersonInfo1.LoadData(resultPerson.Value);

                if (!tabControl1.TabPages.Contains(tpLoginInfo))
                    tabControl1.TabPages.Add(tpLoginInfo);
            }
        }

        private void btnFindBy_Click(object sender, EventArgs e)
        {
            FindBy();
        }

        private bool isValidPesonID()
        {
            bool isValid = false;

            if (cbFindBy.SelectedItem.ToString() == "Person ID")
            {
                if (!txtdata.Text.All(char.IsDigit))
                {
                    isValid = false;
                    errorProvider1.SetError(txtdata, "من فضلك أدخل رقما صالحا هنا");
                }
                else
                {
                    isValid = true;
                    errorProvider1.SetError(txtdata, "");
                }
            }
            else
            {
                isValid = true;
                errorProvider1.SetError(txtdata, "");
            }

            return isValid;
        }

        private void txtdata_Leave(object sender, EventArgs e)
        {
            isValidPesonID();
        }

        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            if(cbFindBy.Items.Count > 0)
            {
                cbFindBy.SelectedIndex = 0;
            }
        }

        public Action<string> OnUserAdded { get; set; }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(NewUserAdded != string.Empty)
            {
                OnUserAdded?.Invoke(NewUserAdded);
            }
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(uctrlPersonInfo1.dtoGlobalPerson == null)
            {
                MessageBox.Show("لا يوجد شخص لإضافته من فضلك ادخل المعرف الشخص أو اسمه أو الرقم الوطني للشخص الذي تريد جعله مستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if(clsUser.isPersonUser(uctrlPersonInfo1.dtoGlobalPerson.PeronsID))
            {
                MessageBox.Show("هذا الشخص هو مستخدم بالفعل، من فضلك اختر شخصا آخر لجعله مستخدم.", "تنويه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            tabControl1.SelectedTab = tpLoginInfo;
            btnSave.Enabled = true;

            if (string.IsNullOrWhiteSpace(llUserID.Text))
            {
                errorProvider1.SetError(llUserID, "Required");
            }
            else
            {
                errorProvider1.SetError(llUserID, "");
            }

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "Required");
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Required");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void tpLoginInfo_Click(object sender, EventArgs e)
        {

        }

        private bool ValidPasswords()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()) || string.IsNullOrWhiteSpace(txtConfirmPassword.Text.Trim()))
            {
                errorProvider1.SetError(txtPassword, "كلمة المرور مطلوبة.");
                errorProvider1.SetError(txtConfirmPassword, "تأكيد كلمة المرور مطلوب.");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
                errorProvider1.SetError(txtConfirmPassword, "");
            }

            if(txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPassword, "كلمتا المرور غير متطابقتين");
                return false;
            }

            return true;
        }

        private bool ValidUserName()
        {
            bool StatusUserName = false;

            if (txtUserName.Text.Length < 3)
            {
                errorProvider1.SetError(txtUserName, ".اسم المستخدم أقل من أربعة أسطر من فضلك أدخل رقما أكبر");
                return false;
            }
            else
                errorProvider1.SetError(txtUserName, "");
            StatusUserName = true;

            bool isUpdateMode = Mode == enMode.UpdateUser;

            if (isUpdateMode)
            {
                if (!clsUser.isUserNameUnique(txtUserName.Text, dtoUser.UserID, isUpdateMode))
                {
                    errorProvider1.SetError(txtUserName, "اسم المستخدم محجوز، أدخل واحدا آخر.");
                    return false;
                }
                else
                {
                    errorProvider1.SetError(txtUserName, "");
                }
                StatusUserName = true;
            }
            return StatusUserName;
        }

        private bool CheckDataUser()
        {
            if (!ValidUserName())
            {
                return false;
            }

            if (!ValidPasswords())
            {
                return false;
            }
            return true;
        }

        private bool SaveUser()
        {
            bool isSaved = false;

            if (dtoUser == null) { dtoUser = new DTOUser(); }
            dtoUser.UserName = txtUserName.Text.Trim();
            dtoUser.Password = txtPassword.Text.Trim();
            dtoUser.PersonID = uctrlPersonInfo1.dtoGlobalPerson.PeronsID;
            dtoUser.IsActive = chbIsActive.Checked;

            clsUser user = new clsUser(dtoUser.PersonID, dtoUser.UserName, dtoUser.Password, dtoUser.IsActive);
            if (user.Save())
            {
                isSaved = true;
                dtoUser.UserID = user._UserID;
            }
            else
                isSaved = false;

            return isSaved;
        }

        private void DefaultOptionsControls()
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tpPersonInfo"];
            if(dtoUser != null)
                dtoUser.clear();
            if(uctrlPersonInfo1.dtoGlobalPerson != null)
                uctrlPersonInfo1.dtoGlobalPerson.Clear();
            llUserID.Text = "-1";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            btnSave.Enabled = false;
            if(tabControl1.TabPages.ContainsKey("tpLoginInfo"))
                tabControl1.TabPages.Remove(tabControl1.TabPages["tpLoginInfo"]);
        }

        private void FilldtoUserFromControlls()
        {
            dtoUser.UserName = txtUserName.Text.Trim();
            dtoUser.Password = txtPassword.Text.Trim();
            dtoUser.IsActive = chbIsActive.Checked;
        }

        private bool UpdateUser()
        {
            bool isUpdated = false;

            clsUser User = clsUser.FindUserByUserID(dtoUser.UserID, false);
            FilldtoUserFromControlls();
            User.UpdateDetailUser(dtoUser);
            if (User.Save())
            {
                isUpdated = true;
            }
            else
                isUpdated = false;

            return isUpdated;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!CheckDataUser())
            {
                return;
            }

            if (Mode == enMode.AddNewUser)
            {
                if (SaveUser())
                {
                    llUserID.Text = Convert.ToString(dtoUser.UserID);
                    MessageBox.Show("تم إضافة المستخدم بنجاح", "تم الإضافة بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Mode = enMode.UpdateUser;
                    if (NewUserAdded == string.Empty)
                    {
                        NewUserAdded = dtoUser.UserID.ToString();
                    }
                    else
                    {
                        NewUserAdded += ", " + dtoUser.UserID.ToString();
                    }
                    return;
                }
                else
                {
                    MessageBox.Show("عذرا، حصل خطأ أثناء الإضافة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if(UpdateUser())
                {
                    MessageBox.Show("تم تعديل بيانات المستخدم بنجاح", "تم التعديل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("عذرا، حصل خطأ أثناء تحديث بيانات المتسخدم، من فضلك حاول مجددا.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if(!ValidUserName())
            {;
                return;
            }
            if(!ValidPasswords())
            {
                return;
            }
        }

        private void txtdata_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                FindBy();
            }
        }

        private bool HasChanged()
        {
            if (txtUserName.Text.Trim() != dtoUser.UserName)
                return true;

            if (txtPassword.Text.Trim() != dtoUser.Password)
                return true;
            if (chbIsActive.Checked != dtoUser.IsActive)
                return true;

            return false;
        }

        private void AnyControl_Changed(object sender, EventArgs e)
        {
            if (Mode == enMode.UpdateUser)
            {
                btnSave.Enabled = HasChanged();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            ValidPasswords();
        }

        private void LoadDataOfAddedPerson(int PersonID)
        {
            txtdata.Text = PersonID.ToString();
            FindBy();
        }

        private void btnAddNewUpdatePerson_Click(object sender, EventArgs e)
        {
            int PersonID = -1;
            frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson();
            frm.ShowDialog();
            if (frm.personid.HasValue)
                PersonID = frm.personid.Value;

            frm.Dispose();

            LoadDataOfAddedPerson(PersonID);
        }
    }
}
