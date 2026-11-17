using DVLDBisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplicationTypesHeaderText(bool isDataTable = true)
        {
            if (isDataTable)
            {
                if (dataGridView1.Columns.Contains("ApplicationTypeID"))
                {
                    dataGridView1.Columns["ApplicationTypeID"].HeaderText = "ID";
                    dataGridView1.Columns["ApplicationTypeID"].Name = "ID";
                }

                if (dataGridView1.Columns.Contains("ApplicationTypeTitle"))
                {
                    dataGridView1.Columns["ApplicationTypeTitle"].HeaderText = "Title";
                    dataGridView1.Columns["ApplicationTypeTitle"].Name = "Title";
                }

                if (dataGridView1.Columns.Contains("ApplicationFees"))
                {
                    dataGridView1.Columns["ApplicationFees"].HeaderText = "Fees";
                    dataGridView1.Columns["ApplicationFees"].Name = "Fees";
                }
            }

        }

        private void LoadDataOfTable()
        {
            dataGridView1.DataSource = clsApplicationTypesBL.GetAllApplicationTypes();
            ApplicationTypesHeaderText();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            LoadDataOfTable();
            int RowCount = dataGridView1.AllowUserToAddRows? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;
            llRecords.Text = RowCount.ToString();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            var currentrow = dataGridView1.CurrentRow;
            int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;

            if (currentrow.Index >= rowcount)
                return;

            if(!dataGridView1.Columns.Contains("ID"))
            {
                return;
            }

            var Cell = currentrow.Cells["ID"];

            if (Cell == null || Cell.Value == null)
                return;

            if(int.TryParse(Cell.Value.ToString(), out int TypeID))
            {
                frmUpdateApplicationTypes frm = new frmUpdateApplicationTypes(TypeID);
                frm.OnDataUpdated = () =>
                {
                    LoadDataOfTable();
                };
                frm.ShowDialog();
                frm.Dispose();
            }
        }
    }
}
