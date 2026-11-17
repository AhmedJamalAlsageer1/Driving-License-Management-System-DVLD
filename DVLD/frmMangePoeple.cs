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
using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs;
using DVLD.People;
using DVLD.Person_Forms;
using DVLDBisnessLayer;
using SharedLayer;

namespace DVLD
{
    public partial class frmMangePoeple : Form
    {
        enum enTextBoxOptions { OnlyLetter = 1, OnlyNumber, Mixed };
        enTextBoxOptions TextBoxOptions = enTextBoxOptions.Mixed;

        enum enLoadDataDefultOptions {None, PersonID, NationalNo, FirstName, SecondName, ThirdName,
        LastName, Gendor, Address, Email, Phone, Nationality, ImagePath};
        enLoadDataDefultOptions LoadDefaultDataOptions = enLoadDataDefultOptions.None;

        public frmMangePoeple()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
            dataGridView1.Dock = DockStyle.None;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDefaultData(int SelectedPersonID = -1)
        {
            dataGridView1.DataSource = clsPerson.GetAllPoeple();
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            if (SelectedPersonID != -1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(row.Cells["PersonID"].Value) == SelectedPersonID)
                    {
                        row.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        dataGridView1.CurrentCell = row.Cells[0];
                        break;
                    }
                }
            }
        }

        private void LoadDefaultDataOnYouSearch(int SelectedPersonID = -1)
        {
            if (SelectedPersonID != -1)
            {
                switch (LoadDefaultDataOptions)
                {
                    case enLoadDataDefultOptions.None:
                        LoadDefaultData(SelectedPersonID);
                        break;

                    case enLoadDataDefultOptions.PersonID:
                        LoadDataByPersonID(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.NationalNo:
                        LoadDataByNationalNo(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.FirstName:
                        LoadDataByFirstName(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.SecondName:
                        LoadDataBySecondName(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.ThirdName:
                        LoadDataByThirdName(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.LastName:
                        LoadDataByLastName(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.Gendor:
                        LoadDataByGendor(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.Nationality:
                        LoadDataByNationality(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.Phone:
                        LoadDataByPhone(SelectedPersonID);
                        break;
                    case enLoadDataDefultOptions.Email:
                        LoadDataByEmail(SelectedPersonID);
                        break;
                }
            }
        }

        private void frmMangePoeple_Load(object sender, EventArgs e)
        {
            LoadDefaultData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNewPerson(object sender, EventArgs e)
        {
            frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson();
            frm.UpdatedNewPersonIDToFatherForm = (newPersonID) =>
            {
                LoadDefaultData(newPersonID);
            };
            frm.OnDataUpdated = () =>
            {
                LoadDefaultData();
            };
            frm.ShowDialog();
            frm.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int GetSelectedPersonID()
        {
            object val = dataGridView1.CurrentRow.Cells["PersonID"].Value;
            if(val == null && val == DBNull.Value)
            {
                MessageBox.Show("القيمة فارغة يرجى اختيار صف صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            int PersonID = Convert.ToInt32(val);

            return PersonID;
        }

        private void updatePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = GetSelectedPersonID();

            if (PersonID == -1)
                return;

            frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson(PersonID);
            frm.OnDataUpdated += () =>
            {
                LoadDefaultDataOnYouSearch(PersonID);
            };
            frm.ShowDialog();
            frm.Dispose();
        }

        private int SelectDeletedID()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("الرجاء تحديد صف لحذفه.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            object value = dataGridView1.CurrentRow.Cells["PersonID"].Value;

            if (value == null && value == DBNull.Value)
            {
                MessageBox.Show("المعرف غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1; 
            }

            return Convert.ToInt32(value);
        }

        private bool DeletePersonID(int PersonID, int SelectedIndex = -1)
        {
            bool isDeleted = false;

            if(PersonID == -1)
            {
                isDeleted = false;
            }

            if (MessageBox.Show($"Are you Sure You Want To Delete This PersonID [{PersonID}]", "خطأ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string SelectedImagePathForDeletion = clsPerson.GetImagePath(PersonID);

                if (!string.IsNullOrEmpty(SelectedImagePathForDeletion) && File.Exists(SelectedImagePathForDeletion))
                {
                    try
                    {
                        File.Delete(SelectedImagePathForDeletion);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" حصل خطأ أثناء حذف الصورة من الملف الذي تم تخزينه فيهاوبذلك قد توقف الحذف.", "خطأ", MessageBoxButtons.OK);
                        isDeleted = false;
                    }
                }

                if (clsPerson.DeletePerson(PersonID))
                {
                    MessageBox.Show("تم الحذف بنجاح.");
                    isDeleted = true;
                }
                else
                {
                    MessageBox.Show("فشل في الحذف.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["PersonID"].Value) == PersonID)
                        {
                            row.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                            dataGridView1.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                    
                    isDeleted = false;
                }

            }

            return isDeleted;
        }

        private void ReloadAndFoucesAfterDelete(int PrevRowIndex = -1)
        {

            dataGridView1.DataSource = clsPerson.GetAllPoeple();
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            int newRowIndex = PrevRowIndex;
            if (PrevRowIndex != -1)
            {
                while (newRowIndex >= dataGridView1.Rows.Count)
                    newRowIndex = dataGridView1.Rows.Count - 1;

                if(newRowIndex >= 0)
                {
                    dataGridView1.Rows[newRowIndex].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = newRowIndex;
                    dataGridView1.CurrentCell = dataGridView1.Rows[newRowIndex].Cells[0];
                }
            }
        }

        private void deletePersonToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int PersonID = SelectDeletedID();

            if (PersonID == -1)
                return;

            int RowIndex = dataGridView1.CurrentRow.Index;
            DeletePersonID(PersonID);
            ReloadAndFoucesAfterDelete(RowIndex);
        }

        private void LoadDataByPersonID(int SelectedPersonIndex = 0)
        {
            if(string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال رقم معرف الشخص.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.TryParse(txtFilter.Text, out int PersonID))
            {
                Result<DTOPerson> resultPerson = clsPerson.FindByID(PersonID);

                if (!resultPerson.isSuccess && resultPerson.Value == null)
                {
                    return;
                }

                DTOPerson dtoPerson = resultPerson.Value;

                if(dtoPerson.PeronsID == -1)
                {
                    MessageBoxHelper.Show(resultPerson, "خطأ", true);
                    return;
                }

                 if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                        SelectedPersonIndex = dataGridView1.Rows.Count - 1;

                if (dtoPerson != null)
                {
                    dataGridView1.DataSource = new List <DTOPerson> { dtoPerson };
                    dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
                }
                else
                {
                    MessageBox.Show("لا يوجد شخص بهذا المعرف.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
                MessageBox.Show("أدخل رقم معرف صحيحًا.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LoadDataByNationalNo(int SelectedPersonIndex = 0)
        {
            if(string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال الرقم الوطني.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Result <DTOPerson> result = clsPerson.FindByNationalNo(txtFilter.Text);

            DTOPerson dtoPerson = result.Value;

            if (dtoPerson != null)
            {
                dataGridView1.DataSource = new List <DTOPerson> { dtoPerson };
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("لا يوجد شخص بهذا الرقم الوطني.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void LoadDataByFirstName(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال الاسم الأول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetAllByFirstName(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("لا يوجد أحد لديه هذا الاسم الأول.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void LoadDataBySecondName(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال الاسم الأول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetAllBySecondName(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("لا يوجد أحد لديه هذا الاسم الثاني.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void LoadDataByThirdName(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال الاسم الثاني.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetAllByThirdName(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("لا يوجد أحد لديه هذا الاسم الثالث.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void LoadDataByLastName(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال الاسم الأخير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetAllByLastName(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("لا يوجد أحد لديه هذا الاسم الأخير.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void LoadDataByNationality(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم بلد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetAllByNationlity(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("اسم البلد هذا غير موجود.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDataByGendor(int SelectedPersonIndex = 0)
        {
            bool Gendor = true;
            if (cbGendor.SelectedItem.ToString() == "Male")
                Gendor = false;

            dataGridView1.DataSource = clsPerson.GetAllByGendor(Gendor);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                if(Gendor == true)
                    MessageBox.Show("لا يوجد أحد أنثى هنا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("لا يوجد أحد مذكر هنا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDataByPhone(int SelectedPersonIndex = 0)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم هاتف صالح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView1.DataSource = clsPerson.GetPersonalDataByPhone(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("اسم الهاتف غير موجود.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool isValidEmail(string Email)
        {
            try
            {
                var add = new System.Net.Mail.MailAddress(Email);
                return add.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        private void LoadDataByEmail(int SelectedPersonIndex = 0)
        {
            if (!isValidEmail(txtFilter.Text))
            {
                MessageBox.Show("الرجاء أدخل إيميلا صالحا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                errorProvider1.SetError(txtFilter, "أدخل إيميلا صاحلا");
                return;
            }
            else
                errorProvider1.SetError(txtFilter, "");

             dataGridView1.DataSource = clsPerson.GetAllByEmail(txtFilter.Text);

            if (SelectedPersonIndex >= dataGridView1.Rows.Count)
                SelectedPersonIndex = dataGridView1.Rows.Count - 1;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[SelectedPersonIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[SelectedPersonIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("هذا الإيميل غير موجود.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateInputFilter()
        {
            switch(TextBoxOptions)
            {
                case enTextBoxOptions.OnlyNumber:
                    return txtFilter.Text.All(char.IsDigit) && !string.IsNullOrWhiteSpace(txtFilter.Text);
                case enTextBoxOptions.OnlyLetter:
                    return txtFilter.Text.All(char.IsLetter) && !string.IsNullOrWhiteSpace(txtFilter.Text);
                case enTextBoxOptions.Mixed:
                    return !string.IsNullOrWhiteSpace(txtFilter.Text);

                default:
                    return false;
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!ValidateInputFilter())
                {
                    errorProvider1.SetError(txtFilter, "الرجاء إدخال بيانات صحيحة حسب نوع البحث!");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtFilter, "");
                }

                if (cbFilter.Text == "PersonID")
                {
                    txtFilter.Enabled = true;
                    LoadDataByPersonID();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }

                else if (cbFilter.Text == "National No")
                {
                    txtFilter.Enabled = true;
                    LoadDataByNationalNo();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }

                else if (cbFilter.Text == "FirstName")
                {
                    txtFilter.Enabled = true;
                    LoadDataByFirstName();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (cbFilter.Text == "SecondName")
                {
                    txtFilter.Enabled = true;
                    LoadDataBySecondName();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (cbFilter.Text == "ThirdName")
                {
                    LoadDataByThirdName();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }

                else if (cbFilter.Text == "LastName")
                {
                    LoadDataByLastName();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (cbFilter.Text == "Phone")
                {
                    LoadDataByPhone();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (cbFilter.Text == "Email")
                {
                    LoadDataByEmail();
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                }
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "None")
            {
                txtFilter.Enabled = false;
                txtFilter.Visible = false;
                cbGendor.Visible = false;
                cbGendor.Enabled = false;
                LoadDefaultDataOptions = enLoadDataDefultOptions.None;
                LoadDefaultData();
                txtFilter.Text = "";
            }
            else if (cbFilter.Text == "PersonID" || cbFilter.Text == "Phone")
            {
                txtFilter.Enabled = true;
                txtFilter.Visible = true;
                cbGendor.Visible = false;
                cbGendor.Enabled = false;
                TextBoxOptions = enTextBoxOptions.OnlyNumber;
                LoadDefaultDataOptions = cbFilter.Text == "PersonID" ? enLoadDataDefultOptions.PersonID : enLoadDataDefultOptions.Phone;
            }
            else if (cbFilter.Text == "Gendor")
            {
                txtFilter.Enabled = false;
                txtFilter.Visible = false;
                cbGendor.Visible = true;
                cbGendor.Enabled = true;
                cbGendor.SelectedIndex = 0;
                LoadDefaultDataOptions = enLoadDataDefultOptions.Gendor;
            }
            else
            {
                txtFilter.Enabled = true;
                txtFilter.Visible = true;
                cbGendor.Visible = false;
                cbGendor.Enabled = false;

                TextBoxOptions = enTextBoxOptions.Mixed;
                if (cbFilter.Text == "FirstName")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.FirstName;
                else if (cbFilter.Text == "SecondName")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.SecondName;
                else if (cbFilter.Text == "ThirdName")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.ThirdName;
                else if (cbFilter.Text == "LastName")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.LastName;
                else if (cbFilter.Text == "National No")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.NationalNo;
                else if (cbFilter.Text == "Email")
                    LoadDefaultDataOptions = enLoadDataDefultOptions.Email;
            }
        }

        private void showPersonInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? PersonID = DgvConfig.TryGetValueCurrentCell(dataGridView1, "PersonID");
            if (PersonID is null)
                return;

            using(frmPersonDetails frm = new frmPersonDetails(PersonID.Value))
            {
                frm.ShowDialog();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Close();
        }

        private void cbGendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "Gendor")
            {
                LoadDataByGendor();
            }

        }
    }
}
