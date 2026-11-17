using DVLD.DTOs;
using DVLD.Inputs;
using DVLD.Person_Forms;
using DVLDBisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmManageUsers : Form
    {
        enum enFindBy { None, UserID, PersonID, FullName, UserName, AboutActivity};
        enum enAboutActivity { All, Active, Inactive};

        enFindBy FindBy = enFindBy.None;
        enAboutActivity ActivityStatus = enAboutActivity.All;


        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void HeaderTextOfDataGridView()
        {
            if (dataGridView1.Columns["UserID"] != null)
                dataGridView1.Columns["UserID"].HeaderText = "User ID";

            if (dataGridView1.Columns["PersonID"] != null)
                dataGridView1.Columns["PersonID"].HeaderText = "Person ID";

            if (dataGridView1.Columns["FullName"] != null)
                dataGridView1.Columns["FullName"].HeaderText = "FullName";

            if (dataGridView1.Columns["UserName"] != null)
                dataGridView1.Columns["UserName"].HeaderText = "User Name";

            if (dataGridView1.Columns["IsActive"] != null)
                dataGridView1.Columns["IsActive"].HeaderText = "Active";
        }

        public void LoadDataByNone(int SelectedIndex = -1)
        {

            dataGridView1.DataSource = clsUserInfo.GetAllUserInfo();
            HeaderTextOfDataGridView();

            if (SelectedIndex != -1)
            {
                int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

                if (rowcount <= 0)
                {
                    return;
                }
                if (SelectedIndex >= rowcount)
                    SelectedIndex = rowcount - 1;

                var UserIDCulomn = dataGridView1.Columns["UserID"];

                if (UserIDCulomn != null)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = SelectedIndex;
                    dataGridView1.CurrentCell = dataGridView1.Rows[SelectedIndex].Cells[UserIDCulomn.Index];
                }
            }
        }

        private void LoadDataByUserID()
        {
            if (!int.TryParse(mtxtSearch.Text, out int userid))
            {
                MessageBox.Show("الرجاء إدخال رقم صحيح.");
                mtxtSearch.Focus(); // تعيده ليكتب القيمة مرة أخرى
                return;
            }

            if (clsUserInfo.GetUserInfoByUserID(userid, out DTOUserInfo dtoUserInfo))
            {
                if (dtoUserInfo == null)
                    return;

                HeaderTextOfDataGridView();
                dataGridView1.DataSource = new List<DTOUserInfo> { dtoUserInfo };
                if(dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            }
        }

        private void LoadDataByPersonID()
        {
            if(!int.TryParse(mtxtSearch.Text, out int personid))
            {
                MessageBox.Show("الرجاء إدخال رقم صحيح.");
                mtxtSearch.Focus();
                return;
            }

            if(clsUserInfo.GetUserInfoByPersonID(personid, out DTOUserInfo dtoUserInfo))
            {
                if (dtoUserInfo == null)
                    return;

                HeaderTextOfDataGridView();
                dataGridView1.DataSource = new List <DTOUserInfo> { dtoUserInfo };
                
                if( dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            }

        }

        private void LoadDataByFullName(int SelectedIndex = -1)
        {
            string FullName = TextHelpers.SplitFullName(txtSearch.Text);

            if (string.IsNullOrWhiteSpace(FullName))
                return;

            dataGridView1.DataSource = clsUserInfo.GetUsersByFullName(FullName);
            HeaderTextOfDataGridView();

            if(SelectedIndex != -1)
            {
                int rowscount = dataGridView1.AllowUserToAddRows? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

                if (rowscount <= 0)
                    return;

                if(SelectedIndex >= rowscount)
                    SelectedIndex = rowscount - 1;

                var UserIDCulomn = dataGridView1.Columns["UserID"];
                if(UserIDCulomn != null)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = SelectedIndex;
                    dataGridView1.CurrentCell = dataGridView1.Rows[SelectedIndex].Cells[UserIDCulomn.Index];
                }
            }
        }

        private void LoadDataByUserName()
        {
            if(string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Please, Enter The UserName.");
                return;
            }

            if(clsUserInfo.GetUserInfoByUserName(txtSearch.Text, out DTOUserInfo dtoUserInfo))
            {
                if (dtoUserInfo == null)
                    return;

                HeaderTextOfDataGridView();
                dataGridView1.DataSource = new List<DTOUserInfo> { dtoUserInfo };
                if(dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];

            }
        }

        private void LoadDataByActivety(int SelectedIndex = -1)
        {
            dataGridView1.DataSource = clsUserInfo.GetAllUserActive();
            HeaderTextOfDataGridView();

            if(SelectedIndex != -1)
            {
                int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;
                
                if(rowcount == 0) 
                    return;

                if (SelectedIndex >= rowcount)
                    SelectedIndex = rowcount - 1;

                var columnActivity = dataGridView1.Columns["Active"];

                if (columnActivity != null)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = SelectedIndex;
                    dataGridView1.CurrentCell = dataGridView1.Rows[SelectedIndex].Cells[columnActivity.Index];
                }
            }
        }

        private void LoadDataByInactivety(int SelectedIndex = -1)
        {
            dataGridView1.DataSource = clsUserInfo.GetAllUserInactive();
            HeaderTextOfDataGridView();

            if (SelectedIndex != -1)
            {
                int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

                if (rowcount == 0)
                    return;

                if (SelectedIndex >= rowcount)
                    SelectedIndex = rowcount - 1;

                var columnActivity = dataGridView1.Columns["Active"];

                if (columnActivity != null)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = SelectedIndex;
                    dataGridView1.CurrentCell = dataGridView1.Rows[SelectedIndex].Cells[columnActivity.Index];
                }
            }
        }

        private void OnEnterKeyPressed(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ChooseFindByWhat();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void LoadIntialData()
        {
            LoadDataByNone();
            dataGridView1.Dock = DockStyle.None;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbFindUserBy.SelectedIndex = 0;
            cbSearchActive.SelectedIndex = 0;
            clsExtentionMethods.BindKeyDownToHandlerInControls(panelInputs, OnEnterKeyPressed);
        }


        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            LoadIntialData();
        }

        private void ChooseFindByWhat(int SelectedIndex = -1)
        {
            if (FindBy == enFindBy.None)
            {
                LoadDataByNone(SelectedIndex);
            }

            if (FindBy == enFindBy.UserID)
            {
                LoadDataByUserID();
            }

            if (FindBy == enFindBy.PersonID)
            {
                LoadDataByPersonID();
            }

            if (FindBy == enFindBy.FullName)
            {
                LoadDataByFullName(SelectedIndex);
            }

            if (FindBy == enFindBy.UserName)
            {
                LoadDataByUserName();
            }

            if (FindBy == enFindBy.AboutActivity)
            {
                CbSearchMode();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser();

            frm.OnUserAdded = (UserIDByString) =>
            {
                MessageBox.Show(" تمت إضافة هؤلاء المستخدمين إلى المنظومة بنجاح.: " + UserIDByString);
            };
            frm.ShowDialog();
            frm.Dispose();
            ChooseFindByWhat();
        }

        private void updateUserInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells.Count > 0)
            {
                int UserID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                frmAddNewUser frm = new frmAddNewUser(UserID);
                frm.ShowDialog();
                frm.Dispose();
                ChooseFindByWhat(dataGridView1.CurrentRow.Index);
            }
        }

        private void mtxtSearch_MouseDown(object sender, MouseEventArgs e)
        {
            mtxtSearch.AdjustCusorToValidInputPosotion(e);
        }

        private void mtxtSearch_Enter(object sender, EventArgs e)
        {
            mtxtSearch.BeginInvoke((MethodInvoker)delegate
            {
                mtxtSearch.MoveCursorToFirstEmptyPrompt();
            });
        }

        private bool DeleteUser()
        {
            bool isDeleted = false;
            
            if (dataGridView1.CurrentRow != null)
            {
                int CountRow = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

                if (dataGridView1.CurrentRow.Index < CountRow && dataGridView1.CurrentRow.Cells["UserID"] != null)
                {
                    var cell = dataGridView1.CurrentRow.Cells["UserID"];

                    if (cell.Value != null)
                    {
                        if (int.TryParse(cell.Value.ToString(), out int UserID))
                        {
                                isDeleted = clsUser.DeleteUser(UserID);
                                ChooseFindByWhat(UserID);
                        }
                    }
                }
            }
            return isDeleted;
        }

        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من أنك تريد حذف هذا المستخدم؟", "حذف مستخدم", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DeleteUser())
                {
                    MessageBox.Show("تم حذف المستخدم بنجاح", "تمت العملية بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("حدث خطأ تسبب بفشل عملية الحذف، أرجوا إعادة المحاولة مرة أخرى.", "فشلت العملية", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("تم إلغاء عملية الحذف", "الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void RuleOfShowOrHideControls()
        {
            if (cbFindUserBy.SelectedIndex == 0)
            {
                FindBy = enFindBy.None;
                clsExtentionMethods.HideChildControls(panelInputs);
                LoadDataByNone();
                return;
            }

            if (cbFindUserBy.SelectedIndex == 1)
            {
                FindBy = enFindBy.UserID;
                clsExtentionMethods.HideChildControls(panelInputs, mtxtSearch);
                LoadDataByNone();
                return;
            }

            if (cbFindUserBy.SelectedIndex == 2)
            {
                FindBy = enFindBy.PersonID;
                clsExtentionMethods.HideChildControls(panelInputs, mtxtSearch);
                LoadDataByNone();
                return;
            }

            if (cbFindUserBy.SelectedIndex == 3)
            {
                FindBy = enFindBy.FullName;
                clsExtentionMethods.HideChildControls(panelInputs, txtSearch);
                LoadDataByNone();
                return;
            }

            if (cbFindUserBy.SelectedIndex == 4)
            {
                FindBy = enFindBy.UserName;
                clsExtentionMethods.HideChildControls(panelInputs, txtSearch);
                LoadDataByNone();
                return;
            }

            if (cbFindUserBy.SelectedIndex == 5)
            {
                FindBy = enFindBy.AboutActivity;
                clsExtentionMethods.HideChildControls(panelInputs, cbSearchActive);
                LoadDataByNone();
                return;
            }
        }

        private void cbFindUserBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuleOfShowOrHideControls();
        }

        private void CbSearchMode(int SelectedIndex = -1)
        {
            if (FindBy == enFindBy.AboutActivity)
            {
                if (cbSearchActive.SelectedIndex == 0)
                {
                    ActivityStatus = enAboutActivity.All;
                    LoadDataByNone(SelectedIndex);
                }
                else if (cbSearchActive.SelectedIndex == 1)
                {
                    ActivityStatus = enAboutActivity.Active;
                    LoadDataByActivety(SelectedIndex);
                }
                else if (cbSearchActive.SelectedIndex == 2)
                {
                    ActivityStatus = enAboutActivity.Inactive;
                    LoadDataByInactivety(SelectedIndex);
                }
            }
        }

        private void cbSearchActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            CbSearchMode();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            int CurrentRow = dataGridView1.CurrentRow.Index;
            int RowCount = dataGridView1.AllowUserToAddRows? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

            if (CurrentRow >= RowCount)
                return;

            var cell = dataGridView1.CurrentRow.Cells["UserID"];
            if (cell == null || cell.Value == null)
                return;

            if(int.TryParse(dataGridView1.CurrentRow.Cells["UserID"].Value.ToString(), out int UserID))
            {
                if(clsUser.FindUserByUserID(UserID, out DTOUser dtoUser, false))
                {
                    EditUserInput _inputUser = new EditUserInput(dtoUser);

                    frmChangePassword frm = new frmChangePassword(_inputUser);
                    frm.ShowDialog();
                    frm.Dispose();
                }

            }
        }

        private void showUserInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            int CurrentRow = dataGridView1.CurrentRow.Index;
            int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

            if (CurrentRow >= rowcount) 
                return;

            var CulomnUserID = dataGridView1.CurrentRow.Cells["UserID"];

            if(CulomnUserID == null || CulomnUserID.Value == null) 
                return;

            if(int.TryParse(CulomnUserID.Value.ToString(), out int UserID))
            {
                frmUserInformation frm = new frmUserInformation(UserID);
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void mtxtSearch_Click(object sender, EventArgs e)
        {
            if (mtxtSearch.SelectionStart > mtxtSearch.Text.Length)
                mtxtSearch.SelectionStart = mtxtSearch.Text.Length;
        }

        private void mtxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {

            }
        }
    }
}
