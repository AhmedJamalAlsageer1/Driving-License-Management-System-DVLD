using DVLD.DTOs;
using DVLDBisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmManageTypeTest : Form
    {
        public frmManageTypeTest()
        {
            InitializeComponent();
        }

        private void HeaderTextOfData()
        {
            if (dataGridView1.Columns.Contains("TestTypeID"))
            {
                dataGridView1.Columns["TestTypeID"].HeaderText = "ID";
                dataGridView1.Columns["TestTypeID"].Name = "ID";
            }

            if(dataGridView1.Columns.Contains("TestTypeTitle"))
            {
                dataGridView1.Columns["TestTypeTitle"].HeaderText = "Title";
                dataGridView1.Columns["TestTypeTitle"].Name = "Title";
            }

            if (dataGridView1.Columns.Contains("TestTypeDescription"))
            {
                dataGridView1.Columns["TestTypeDescription"].HeaderText = "Description";
                dataGridView1.Columns["TestTypeDescription"].Name = "Description";
            }

            if (dataGridView1.Columns.Contains("TestTypeFees"))
            {
                dataGridView1.Columns["TestTypeFees"].HeaderText = "Fees";
                dataGridView1.Columns["TestTypeFees"].Name = "Fees";
            }
        }

        private void LoadDataOfDGV()
        {
            dataGridView1.DataSource = clsTestTypes.GetAllTestTypes();
            HeaderTextOfData();

        }

        private void frmManageTypeTest_Load(object sender, EventArgs e)
        {
            LoadDataOfDGV();
            int rowcount = dataGridView1.AllowUserToAddRows ? dataGridView1.Rows.Count - 1 : dataGridView1.Rows.Count;
            llRecord.Text = rowcount.ToString();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            var CurrentRow = dataGridView1.CurrentRow;
            int rowcount = dataGridView1.Rows.Count;

            if (CurrentRow.Index >= rowcount)
                return;

            if (CurrentRow.Cells["ID"].Value == null)
                return;

            var currentcell = CurrentRow.Cells["ID"];
            
            if(int.TryParse(currentcell.Value.ToString(), out int ID))
            {
                frmEditTestType frm = new frmEditTestType(ID);
                frm.OnDataUpdated = () =>
                {
                    LoadDataOfDGV();
                };
                frm.ShowDialog();
                frm.Dispose();
            }
            
        }
    }
}
