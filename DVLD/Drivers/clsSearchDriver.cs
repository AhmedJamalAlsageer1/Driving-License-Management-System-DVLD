using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs.Drivers;
using DVLDBisnessLayer.Driver;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    internal static class clsSearchDriver
    {
        public enum enSearchType
        {
            None,
            DriverID,
            PersonID,
            NationalNo,
            FullName
        }
        private static enSearchType _SearchBy;

        public static enSearchType CurrentSearchBy => _SearchBy;

        private static int? GetSearchResult_ID(string Text)
        {
            if (!int.TryParse(Text, out int ID))
                return null;

            return ID;
        }

        public static bool ChangeSearchType(enSearchType searchType, Panel pnl)
        {
            TextBox txt = null;
            MaskedTextBox mtxt = null;
            foreach(Control ctrl in pnl.Controls)
            {
                if(ctrl is TextBox)
                    txt = ctrl as TextBox;
                if(ctrl is MaskedTextBox)
                    mtxt = ctrl as MaskedTextBox;
            }
            
            if(txt == null || mtxt == null)
                return false;

            _SearchBy = searchType;
            if (_SearchBy  == enSearchType.None)
            {
                txt.Visible = false;
                mtxt.Visible = false;
                txt.Text = "";
                mtxt.Text = "";

            }

            else if (_SearchBy == enSearchType.PersonID || _SearchBy == enSearchType.DriverID)
            {
                txt.Text = "";
                txt.Visible = false;
                mtxt.Visible = true;
                mtxt.Focus();
            }

            else
            {
                mtxt.Text = "";
                mtxt.Visible = false;
                txt.Visible = true;
                txt.Focus();
            }

            return true;
        }

        public static bool Fill_dgv(DataGridView dgv, string value, int SelectedID = -1)
        {
            if(_SearchBy == enSearchType.None)
            {
                LoadAllDriverLicenses(dgv, SelectedID);
                return true;
            }

            else if(_SearchBy == enSearchType.DriverID)
            {
                LoadDataByDriverID(dgv, GetSearchResult_ID(value), SelectedID);
                return true;
            }

            else if (_SearchBy == enSearchType.PersonID)
            {
                LoadDataByPersonID(dgv, GetSearchResult_ID(value), SelectedID);
                return true;
            }

            else if(_SearchBy == enSearchType.NationalNo)
            {
                LoadDataByNationalNo(dgv, value, SelectedID);
                return true;
            }

            else if (_SearchBy == enSearchType.FullName)
            {
                LoadDataByFullName(dgv, value, SelectedID);
                return true;
            }

            return false;
        }

        private static void LoadDataByPersonID(DataGridView dgv, int? ID, int SelectedID)
        {
            if (!ID.HasValue)
            {
                return;
            }

            Result<DTODrivers_Info> result_DriverInfo = clsDriverInfo_BL.GetDriverInfo_ByPersonID(ID.Value);
            if (!result_DriverInfo.isSuccess || result_DriverInfo.Value == null)
            {
                dgv.DataSource = null;
                return;
            }


            dgv.DataSource = new SortableBindingList<DTODrivers_Info>(
                new[] { result_DriverInfo.Value }.ToList());
            FocusOnSelectedID(dgv, SelectedID);
        }

        private static void LoadDataByDriverID(DataGridView dgv, int? ID, int SelectedID)
        {
            if (!ID.HasValue)
            {
                return;
            }

            Result<DTODrivers_Info> result_DriverInfo = clsDriverInfo_BL.GetDriverInfo_ByDriverID(ID.Value);
            if (!result_DriverInfo.isSuccess || result_DriverInfo.Value == null)
            {
                dgv.DataSource = null;
                return;
            }


            dgv.DataSource = new SortableBindingList<DTODrivers_Info>(
                new[] { result_DriverInfo.Value }.ToList());
            FocusOnSelectedID(dgv, SelectedID);
        }

        private static void LoadDataByNationalNo(DataGridView dgv, string Text, int SelectedID)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            Result <DTODrivers_Info> result_DriverInfo = clsDriverInfo_BL.GetDriverInfo_ByNationalNo(Text.Trim());
            if (!result_DriverInfo.isSuccess || result_DriverInfo.Value == null)
            {
                dgv.DataSource = null;
                return;
            }


            dgv.DataSource = new SortableBindingList<DTODrivers_Info>(
                new[] { result_DriverInfo.Value }.ToList());
            FocusOnSelectedID(dgv, SelectedID);
        }

        private static void LoadDataByFullName(DataGridView dgv, string Text, int SelectedID)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            Result<DTODrivers_Info> result_DriverInfo = clsDriverInfo_BL.GetDriverInfo_ByFullName(Text.Trim());
            if (!result_DriverInfo.isSuccess || result_DriverInfo.Value == null)
            {
                dgv.DataSource = null;
                return;
            }

            dgv.DataSource = new SortableBindingList<DTODrivers_Info>(
             new[] { result_DriverInfo.Value }.ToList());
            FocusOnSelectedID(dgv, SelectedID);
        }

        private static void LoadAllDriverLicenses(DataGridView dgv, int SelectedID)
        {
            Result <List<DTODrivers_Info>> result_AllDriver = clsDriverInfo_BL.GetAllDriversInfo();
            if (!result_AllDriver.isSuccess || result_AllDriver.Value == null)
            {
                dgv.DataSource = null;
                return;
            }

            SortableBindingList <DTODrivers_Info> sortableList = new SortableBindingList<DTODrivers_Info>(result_AllDriver.Value);
            dgv.DataSource = sortableList;
            FocusOnSelectedID(dgv, SelectedID);
        }

        private static void FocusOnSelectedID(DataGridView dgv, int SelectedID)
        {
            if (SelectedID < 0)
                return;

            string Culomn;
            if (_SearchBy == enSearchType.PersonID)
                Culomn = "colPersonID";
            else
                Culomn = "colDriverID";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (Convert.ToInt32(row.Cells[Culomn].Value) == SelectedID)
                {
                    row.Selected = true;
                    dgv.CurrentCell = row.Cells[0]; // تحديد الخلية الأولى
                    break;
                }
            }
        }
    }
}
