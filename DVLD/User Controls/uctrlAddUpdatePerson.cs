using DVLD.Classess.Helpers;
using DVLD.Properties;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class uctrlAddUpdatePerson : UserControl
    {
        public enum enChooseOption { AddNew = 1, Update = 2}
        public enChooseOption OptionChoosed;
        private bool _HasCustomImage = false;
        private bool _isPictrueDelete = false;
        private string _LastUsedFolder = "";
        private string _PathOfTheIamgeDelete = "";
        private string _SelectedImageSourcePath = "";
        private string _DestinationImageFullPath = "";
        public clsDOTPerson dotPerson = new clsDOTPerson();
        public Action OnDataUpdate { get; set; }

        public class clsDOTPerson
        {
            public int PersonID { get; set; }
            public string NationalNo { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string ThirdName { get; set; }
            public string LastName { get; set; }
            public bool Gendor { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Country { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string ImagePath { get; set; }

            public clsDOTPerson()
            {
                PersonID = 0;
                NationalNo = "";
                FirstName = "";
                SecondName = "";
                ThirdName = "";
                LastName = "";
                Gendor = false;
                DateOfBirth = DateTime.Now;
                Country = "";
                Address = "";
                Email = "";
                Phone = "";
            }

        }

        public event Action<int> PersonAdded;

        private void LoadGendor()
        {

           pbGendor.Image.Dispose();
           pbGendor.Image = null;

            if (dotPerson.Gendor == false && string.IsNullOrEmpty(dotPerson.ImagePath))
                pbGendor.Image = Resources.Person_Question_180;
            else if (dotPerson.Gendor == true && string.IsNullOrEmpty(dotPerson.ImagePath))
                pbGendor.Image = Resources.Female_reporter_180;
        }

        private void LoadDataToScreen()
        {
            txtNationalNo.Text = dotPerson.NationalNo;
            txtFirst.Text = dotPerson.FirstName;
            txtSecond.Text = dotPerson.SecondName;
            txtThird.Text = dotPerson.ThirdName;
            txtLast.Text = dotPerson.LastName;
            dtpDateOfBirth.Value = dotPerson.DateOfBirth;
            LoadGendor();
            txtPhone.Text = dotPerson.Phone;
            txtEmail.Text = dotPerson.Email;
            txtAddress.Text = dotPerson.Address;
            txtPhone.Text = dotPerson.Phone;
            LoadImagePathInModeUpdate();
        }

        public void FillControlsFromDOTPerson()
        {

            if (OptionChoosed == enChooseOption.AddNew)
            {
                FillComboBoxByCountries();
            }
            else
            {

                DataTable dt = clsPerson.GetAllCountries();
                cbCountry.DataSource = dt;
                cbCountry.DisplayMember = "CountryName";
                cbCountry.ValueMember = "CountryID";

                int Idx = cbCountry.FindStringExact(dotPerson.Country);
                if (Idx > 0)
                {
                    cbCountry.SelectedIndex = Idx;
                }

                LoadDataToScreen();
            }
        }

        public uctrlAddUpdatePerson()
        {
            InitializeComponent();
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
        }

        private void FillComboBoxByCountries()
        {
            DataTable dt = clsPerson.GetAllCountries();

            cbCountry.DataSource = dt;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";

            int Idx = cbCountry.FindStringExact("Libya");
            if(Idx > 0)
            {
                cbCountry.SelectedIndex = Idx;
            }
        }

        private void radiobutton_ClickedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked == true && _HasCustomImage != true)
            {
                pbGendor.Image = Resources.Person_Question_180;
                dotPerson.Gendor = false;
            }
            else if (rbFemale.Checked == true && _HasCustomImage != true)
            {
                pbGendor.Image = Resources.Female_reporter_180;
                dotPerson.Gendor = true;
            }
        }

        private void LoadImagePathInModeUpdate()
        {
            if (!string.IsNullOrEmpty(dotPerson.ImagePath) && File.Exists(dotPerson.ImagePath))
            {
                pbGendor.Image?.Dispose();
                pbGendor.Image = null;
                pbGendor.Image = Image.FromFile(dotPerson.ImagePath);
            }
            else
            {
                if (dotPerson.Gendor == false)
                    pbGendor.Image = Resources.Person_Question_180;
                else if (dotPerson.Gendor == true)
                    pbGendor.Image = Resources.Female_reporter_180;
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_SelectedImageSourcePath) && !string.IsNullOrEmpty(_DestinationImageFullPath))
            {
                MessageBox.Show("If you Want To Delete This Picture, Please Press Delete", "Note", MessageBoxButtons.OK);
                return;
            }

            string ImegesFolder = Path.Combine(Application.StartupPath, "PersonImeges");
            if(!Directory.Exists(ImegesFolder))
            {
                Directory.CreateDirectory(ImegesFolder);
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "اختر صورة";
            openFileDialog.InitialDirectory = _LastUsedFolder;
            openFileDialog.Filter = "صور (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _SelectedImageSourcePath = openFileDialog.FileName.ToString();
                _LastUsedFolder = Path.GetDirectoryName(openFileDialog.FileName);
                string FileName =  Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "_" + Guid.NewGuid().ToString("N");
                string NewFileName = FileName + Path.GetExtension(openFileDialog.FileName);
                _DestinationImageFullPath = Path.Combine(ImegesFolder, NewFileName);
                _HasCustomImage = true;
                dotPerson.ImagePath = _DestinationImageFullPath;

                pbGendor.Image = Image.FromFile(_SelectedImageSourcePath);
            }
        }

        private void ValidateTextBox(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox txt = sender as TextBox;
                if (string.IsNullOrEmpty(txt.Text))
                {
                    errorProvider1.SetError(txt, "Required");
                }
                else
                {
                    errorProvider1.SetError(txt, "");
                }
            }
        }

        private bool ValidateTextBox(TextBox txt)
        {
            bool isFull = false;
 
            if (string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "Required");
                isFull = false;
            }
            else
            {
                errorProvider1.SetError(txt, "");
                isFull = true;
            }

            return isFull;
        }

        private bool CheckcbCountry()
        {
            string SelctedCountry = cbCountry.Text.Trim();
            var dt = cbCountry.DataSource as DataTable;
            bool exists = false;
            if(dt != null)
            {
                exists = dt.AsEnumerable()
                    .Any(r => string.Equals(
                        r.Field <string> ("CountryName"),
                        SelctedCountry,
                        StringComparison.OrdinalIgnoreCase));

                if(!exists)
                {
                    errorProvider1.SetError(cbCountry, "This Country Is Not Exists");

                }
                else
                {
                    errorProvider1.SetError(cbCountry, "");
                }

                return true;
            }
            return false;
        }

        private bool validateEmail()
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "");
                return true;
            }

            try
            {
                var MAdd = new System.Net.Mail.MailAddress(txtEmail.Text);
                errorProvider1.SetError(txtEmail, "");
                bool isValid = MAdd.Address == txtEmail.Text;

                if (isValid)
                {
                    errorProvider1.SetError(txtEmail, "");
                }
                else
                    errorProvider1.SetError(txtEmail, "Error In Email");

                return isValid;
            }
            catch(Exception ex)
            {
                errorProvider1.SetError(txtEmail, "Error In Email");
                return false;
            }
        }

        private bool ConfirmFullData()
        {
            bool isValid = true;
            isValid &= ValidateTextBox(txtFirst);
            isValid &= ValidateTextBox(txtSecond);
            isValid &= ValidateTextBox(txtLast);
            isValid &= ValidateTextBox(txtNationalNo);
            isValid &= ValidateTextBox(txtPhone);
            isValid &= validateEmail();
            isValid &= ValidateTextBox(txtAddress);
            isValid &= CheckcbCountry();

            return isValid;
        }

        private clsPerson ConvertFromdotPersonToBusnissPerson(uctrlAddUpdatePerson.clsDOTPerson dotPerson)
        {
            Result<clsPerson> resultPerson = clsPerson.FindObjectByID(dotPerson.PersonID);

            if (!resultPerson.isSuccess && resultPerson == null)
            {
                MessageBoxHelper.Show(resultPerson, "خطأ", true);
            }
            clsPerson person = resultPerson.Value;
            person.UpdateDetails(dotPerson.NationalNo, dotPerson.FirstName, dotPerson.SecondName, dotPerson.ThirdName,
                dotPerson.LastName, dotPerson.DateOfBirth, dotPerson.Gendor, dotPerson.Address, dotPerson.Phone,
                dotPerson.Email, dotPerson.Country, dotPerson.ImagePath);
            return person;
        }

        private bool checkIfUniqueData(clsPerson person)
        {
            bool CheckedState = true;

            if (!person.isUniqueNationalNo(dotPerson.PersonID))
            {
                errorProvider1.SetError(txtNationalNo, "الرقم الوطني مكرر، الرجاء إدخال رقم آخر.");
                CheckedState = false;
            }
            if (!person.isUniqueFullName(dotPerson.PersonID))
            {
                errorProvider1.SetError(txtFirst, "هذا الاسم قد سبق إدخاله من فضلك اختر اسما غيره.");
                errorProvider1.SetError(txtSecond, "هذا الاسم قد سبق إدخاله من فضلك اختر اسما غيره.");
                errorProvider1.SetError(txtThird, "هذا الاسم قد سبق إدخاله من فضلك اختر اسما غيره.");
                errorProvider1.SetError(txtLast, "هذا الاسم قد سبق إدخاله من فضلك اختر اسما غيره.");
                CheckedState = false;
            }
            return CheckedState;
        }

        private void LoadDataFromControlsTodotPerson()
        {
            dotPerson.NationalNo = txtNationalNo.Text;
            dotPerson.FirstName = txtFirst.Text;
            dotPerson.SecondName = txtSecond.Text;
            dotPerson.ThirdName = txtThird.Text;
            dotPerson.LastName = txtLast.Text;
            dotPerson.DateOfBirth = dtpDateOfBirth.Value;
            dotPerson.Gendor = rbMale.Checked == true? false : true;
            dotPerson.Phone = txtPhone.Text;
            dotPerson.Email = txtEmail.Text;
            dotPerson.Address = txtAddress.Text;
            dotPerson.Country = cbCountry.Text;
        }

        private void LoadPictureFromTheNewData()
        {
            if (!string.IsNullOrEmpty(_DestinationImageFullPath) && !string.IsNullOrEmpty(_SelectedImageSourcePath)
                 && File.Exists(_SelectedImageSourcePath))
            {
                pbGendor.Image.Dispose();
                pbGendor.Image = null;
                File.Copy(_SelectedImageSourcePath, _DestinationImageFullPath, true);
                pbGendor.Image = Image.FromFile(_DestinationImageFullPath);
            }
        }

        private void DeleteDeletedImageFromFile()
        {
            try
            {
                if (_isPictrueDelete == true)
                {
                    if (File.Exists(_PathOfTheIamgeDelete))
                    {
                        File.Delete(_PathOfTheIamgeDelete);
                    }
                    else
                    {
                        if (dotPerson.ImagePath == _PathOfTheIamgeDelete)
                            dotPerson.ImagePath = "";
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حذف الصورة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ConfirmFullData())
            {
                return;
            }

            if (OptionChoosed == enChooseOption.Update)
            {
                LoadDataFromControlsTodotPerson();
                DeleteDeletedImageFromFile();

                clsPerson person = ConvertFromdotPersonToBusnissPerson(dotPerson);
                if (checkIfUniqueData(person))
                {
                    if (person.Save())
                    {
                        LoadPictureFromTheNewData();
                        OnDataUpdate?.Invoke();
                        MessageBox.Show("The Data is Updated Seccufully", "Updated Done Seccusfully");
                        
                    }
                    else
                        MessageBox.Show("Something Wrong Happend.");
                }
                else
                {
                    return;
                }
            }
            else
            {
                LoadDataFromControlsTodotPerson();
                clsPerson person = new clsPerson(dotPerson.NationalNo, dotPerson.FirstName, dotPerson.SecondName, dotPerson.ThirdName, dotPerson.LastName,
                    dotPerson.DateOfBirth, dotPerson.Gendor, dotPerson.Address, dotPerson.Email, dotPerson.Phone, dotPerson.Country, dotPerson.ImagePath);
                if (checkIfUniqueData(person))
                {
                    if (person.Save())
                    {
                        dotPerson.PersonID = person._ID;
                        returnedUpdatedNewPersonID?.Invoke(dotPerson.PersonID);
                        LoadPictureFromTheNewData();
                        PersonAdded?.Invoke(dotPerson.PersonID);
                    }
                    else
                    {
                        MessageBox.Show("خطأ ما، من فضلك احفظ البيانات مرة أخرى!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    return;    
            }
        }

        public event Action<int> returnedUpdatedNewPersonID;
        private void btnClose_Click(object sender, EventArgs e)
        {
            var parentForm = this.FindForm();
            parentForm?.Close();
        }

        private void llDeleteImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            pbGendor.Image?.Dispose();
            pbGendor.Image = null;

            _PathOfTheIamgeDelete = dotPerson.ImagePath;
            _HasCustomImage = false;
            _isPictrueDelete = true;
            if (rbMale.Checked)
                pbGendor.Image = Resources.Person_Question_180;
            else if (rbFemale.Checked)
                pbGendor.Image = Resources.Female_reporter_180;

            if (!string.IsNullOrEmpty(_PathOfTheIamgeDelete) && File.Exists(_PathOfTheIamgeDelete))
            {
                try
                {
                    File.Delete(_PathOfTheIamgeDelete);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"خطأ أثناء حذف الصورة من مجلد PersonImeges: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _SelectedImageSourcePath = "";
            _DestinationImageFullPath = "";
            dotPerson.ImagePath = "";
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            validateEmail();
        }
    }
}
