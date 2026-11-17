using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmDrivers : Form
    {
        public frmDrivers()
        {
            InitializeComponent();


            ConfigurationForm();
        }

        private void ConfigurationForm()
        {
            cbFindBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFindBy.Items.Add("None");
            cbFindBy.Items.Add("Driver ID");
            cbFindBy.Items.Add("Person ID");
            cbFindBy.Items.Add("National No");
            cbFindBy.Items.Add("Full Name");
            cbFindBy.SelectedIndex = 0;
            CreateTable_dgv();
        }

        private void CreateTable_dgv()
        {
            dataGridView1.Columns.Clear();
            DgvConfig.ApplyBasicStyle(dataGridView1);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Driver ID",
                DataPropertyName = "DriverID",
                Name = "colDriverID",
                FillWeight = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Person ID",
                DataPropertyName = "PersonID",
                Name = "colPersonID",
                FillWeight = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "National No",
                DataPropertyName = "NationalNo",
                Name = "colNationalNo",
                FillWeight = 100
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Full Name",
                DataPropertyName = "FullName",
                Name = "colFullName",
                FillWeight = 250
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                DataPropertyName = "CreatedDate",
                Name = "colCreatedDate",
                FillWeight = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "g" } // "g" = General date/time pattern
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Active Licenses",
                DataPropertyName = "ActiveLicenses",
                Name = "colActiveLicenses",
                FillWeight = 100
            });
        }

        private bool ChangedSearchChoosed(int SelectedID = -1)
        {
            if (cbFindBy.SelectedIndex == 0)
            {
                if (clsSearchDriver.ChangeSearchType(clsSearchDriver.enSearchType.None, panel1))
                {
                    clsSearchDriver.Fill_dgv(dataGridView1, null, SelectedID);
                }
            }

            string Text;
            if (clsSearchDriver.CurrentSearchBy == clsSearchDriver.enSearchType.DriverID ||
                clsSearchDriver.CurrentSearchBy == clsSearchDriver.enSearchType.PersonID)
            {
                Text = maskedTextBox1.Text;
            }
            else
                Text = textBox1.Text;

            if (cbFindBy.SelectedIndex == 1)
            {
                if (clsSearchDriver.ChangeSearchType(clsSearchDriver.enSearchType.DriverID, panel1))
                {
                    clsSearchDriver.Fill_dgv(dataGridView1, Text, SelectedID);
                }
            }

            if (cbFindBy.SelectedIndex == 2)
            {
                if (clsSearchDriver.ChangeSearchType(clsSearchDriver.enSearchType.PersonID, panel1))
                {
                    clsSearchDriver.Fill_dgv(dataGridView1, Text, SelectedID);
                }
            }

            if (cbFindBy.SelectedIndex == 3)
            {
                if (clsSearchDriver.ChangeSearchType(clsSearchDriver.enSearchType.NationalNo, panel1))
                {
                    clsSearchDriver.Fill_dgv(dataGridView1, Text, SelectedID);
                }
            }

            if (cbFindBy.SelectedIndex == 4)
            {
                if (clsSearchDriver.ChangeSearchType(clsSearchDriver.enSearchType.FullName, panel1))
                {
                    clsSearchDriver.Fill_dgv(dataGridView1, Text, SelectedID);
                }
            }
            return false;
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangedSearchChoosed();
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangedSearchChoosed();
            }
        }

        private void showDriverLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? num = DgvConfig.TryGetValueCurrentCell(dataGridView1, "colDriverID");

            if (num == null)
                return;

            frmLicenseHistory frm = frmLicenseHistory.From_DriverID(num.Value);
            if (frm != null)
            {
                using (frm)
                {
                    frm.ShowDialog();
                }
            }
        }
    }
}
