using DVLD.Classess.Helpers;
using DVLD.Person_Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User_Controls.Displaydata
{
    public partial class uctrlDispalyDataPerson : UserControl
    {
        private enSearchType CurrentSearchType;

        public uctrlDispalyDataPerson()
        {
            InitializeComponent();
        }

        public int? Input ()
        {
            if (!int.TryParse(mtxtInput.ToString(), out int Num))
                return null;

            return Num;
        }
        private void ToggleSearchControl()
        {
            enSearchType type = (enSearchType)cbFindBy.SelectedIndex;
            
            switch(type)
            {
                case enSearchType.PersonID:
                    mtxtInput.Visible = true;
                    txtInput.Visible = false;
                    mtxtInput.Focus();
                    break;

                default:
                    txtInput.Visible = true;
                    mtxtInput.Visible = false;
                    txtInput.Focus();
                    break;
            }
        }

        private void LoadDefaultData()
        {
            int idems = cbFindBy.Items.Count;
            if (idems > 0)
            {
                cbFindBy.SelectedIndex = 0;
                CurrentSearchType = (enSearchType)cbFindBy.SelectedIndex;
            }
        }

        private void uctrlDispalyDataPerson_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
               LoadDefaultData();
            }));
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentSearchType = (enSearchType)cbFindBy.SelectedIndex;
            ToggleSearchControl();
        }

        private void mtxtInput_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (mtxtInput.SelectionStart >= mtxtInput.Text.Length)
                    mtxtInput.SelectionStart = mtxtInput.Text.Length;
            }));
        }

        private void mtxtInput_DoubleClick(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                mtxtInput.SelectionStart = 0;
                mtxtInput.SelectionLength = mtxtInput.Text.Length;
            }));
        }
        private void mtxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                RequestSearch();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private bool CheckInputedData(out string Input)
        {
            Input = mtxtInput.Visible ? mtxtInput.Text.Trim() : txtInput.Text.Trim();

            bool isValid = !string.IsNullOrWhiteSpace(Input);

            Control ActiveControl = mtxtInput.Visible? (Control) mtxtInput : (Control) txtInput;

            if (!isValid)
            {
                errorProvider1.SetError(ActiveControl, "الرجاء إدخال قيمة للبحث");
                Input = string.Empty;
                return false;
            }

            errorProvider1.SetError(ActiveControl, "");
            return true;
        }

        private void ControlInputs_Leave(object sender, EventArgs e)
        {
            CheckInputedData(out string Ignore);
        }

        public event Action<enSearchType, string> SearchRequested;

        private void RequestSearch()
        {
            string input; 
            if(!CheckInputedData(out input))
            {
                MessageBox.Show("من فضلك اكتب شيئا لنبحث عنه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SearchRequested?.Invoke(CurrentSearchType, input);
        }

        private void btnFindBy_Click(object sender, EventArgs e)
        {
            RequestSearch();
        }
        private void PreparControlsforAddPerson(int personid)
        {
            mtxtInput.Text = personid.ToString();
            RequestSearch();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson();
            frm.ShowDialog();
            if (frm.personid != null)
            {
                int id = frm.personid.Value;
                PreparControlsforAddPerson(id);
            }
            frm.Dispose();
        }
    }
}
