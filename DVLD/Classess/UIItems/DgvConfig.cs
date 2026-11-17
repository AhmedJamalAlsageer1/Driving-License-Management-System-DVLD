using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classess.UIItems
{
    public static class DgvConfig
    {
        public static void ApplyBasicStyle(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = true;

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.75F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Clear();
            dgv.AutoGenerateColumns = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static int? TryGetValueCurrentCell(DataGridView dgv, string NameCurrentCell)
        {
            if (dgv.CurrentRow == null)
                return null;

            var CurrentRow = dgv.CurrentRow;
            int rowcount = dgv.AllowUserToAddRows ? dgv.Rows.Count - 1 : dgv.Rows.Count;

            if (CurrentRow.Index >= rowcount)
                return null;

            if (!dgv.Columns.Contains(NameCurrentCell))
                return null;

            var CurrentCell = CurrentRow.Cells[NameCurrentCell];

            if (CurrentCell.Value == null)
                return null;

            if (!int.TryParse(CurrentCell.Value.ToString(), out int ID))
                return null;

            return ID;
        }

        public static void FoucesOnSelectedID(DataGridView dgv, string ColumnName, int SelectedID)
        {
            if (SelectedID <= 0)
                return;

            if (!dgv.Columns.Contains(ColumnName))
                return;

            int rowCount = dgv.AllowUserToAddRows ? dgv.Rows.Count - 1 : dgv.Rows.Count;

            if (rowCount <= 0)
                return;

            DataGridViewRow FoucesRow = null;
            for (int i = 0; i < rowCount; i++)
            {
                var Cell = dgv.Rows[i].Cells[ColumnName];
                if (Cell != null && int.TryParse(Cell.Value.ToString(), out int value)
                    && value == SelectedID)
                {
                    FoucesRow = dgv.Rows[i];
                    break;
                }
            }

            if (FoucesRow == null)
                return;

            dgv.ClearSelection();
            dgv.CurrentCell = FoucesRow.Cells[ColumnName];
            dgv.Rows[FoucesRow.Index].Selected = true;
            dgv.FirstDisplayedScrollingRowIndex = FoucesRow.Index;
        }

        public static int? GetValueInt_FromColumn_DGV(DataGridView dgv, DataGridViewRow CurrentRow, string ColumnName)
        {
            if (CurrentRow == null)
                return null;


            if (CurrentRow.IsNewRow)
                return null;

            if (!dgv.Columns.Contains(ColumnName))
                return null;

            var CurrentCell = CurrentRow.Cells[ColumnName];

            if (CurrentCell.Value == null)
                return null;

            if (!int.TryParse(CurrentCell.Value.ToString(), out int value))
            {
                return null;
            }

            return value;
        }

        public static DataGridViewTextBoxColumn CreateTextBox(string Name, string Header, int _FillWeight)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = Name,
                DataPropertyName = Name,
                HeaderText = Header,
                ReadOnly = true,
                FillWeight = _FillWeight
            };
        }

        public static DataGridViewCheckBoxColumn CreateCheckBoxColumn(string name, string header, int _FillWeight)
        {
            return new DataGridViewCheckBoxColumn
            {
                Name = name,
                DataPropertyName = name,
                HeaderText = header,
                ReadOnly = true,
                FillWeight = _FillWeight
            };
        }

        public static void FoucesOnRow(DataGridView dgv, string ColumnName, int ID)
        {
            if (dgv == null || dgv.Rows.Count == 0)
                return;

            if (!dgv.Columns.Contains(ColumnName))
                return;


            foreach (DataGridViewRow row in dgv.Rows)
            {
                // نتأكد أن الصف ليس صف جديد فارغ
                if (row.IsNewRow)
                    continue;

                // نقارن القيمة في العمود المطلوب مع الـ ID
                if (row.Cells[ColumnName].Value != null &&
                    Convert.ToInt32(row.Cells[ColumnName].Value) == ID)
                {
                    // تحديد الصف
                    row.Selected = true;

                    // جعل التركيز على الصف
                    dgv.CurrentCell = row.Cells[ColumnName];

                    // تمرير العرض ليظهر الصف للمستخدم
                    dgv.FirstDisplayedScrollingRowIndex = row.Index;

                    break; // وجدنا الصف المطلوب
                }
            }
        }
    }
}
