using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs.Presention_DTO.Police_Actions.Detain_License;
using DVLDBisnessLayer.Presentation_BL.Police_Server.Detain_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Police_Actions.Detain_License.Manage_Detained_License
{
    public partial class frm_ManageDetainedLicense
    {
        private void LoadData()
        {
            if (_SearchType == enSearchType.None)
            {
                LoadDataByNone();
                return;
            }

            else if(_SearchType == enSearchType.DetainID)
            {
                LoadDataByDetainID();
                return;
            }

            else if (_SearchType == enSearchType.LicenseID)
            {
                LoadDataByLicenseID();
                return;
            }

            else if (_SearchType == enSearchType.NationalNo)
            {
                LoadDataByNationalNo();
                return;
            }

            else if (_SearchType == enSearchType.FullName)
            {
                LoadDataByFullName();
                return;
            }

            else if (_SearchType == enSearchType.ReleaseApplicationID)
            {
                LoadDataByReleaseApplicationID();
                return;
            }

            else if(_SearchType == enSearchType.IsReleased)
            {
                LoadDataByIsReleased();
                return;
            }
        }

        private void LoadDataByNone()
        {
            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicenses();
            if(!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }

        private bool Get_mtxtInput(out int Input)
        {
            Input = -1;
            if(int.TryParse(mtxtInput.Text, out Input))
            {
                return true;
            }
            return false;
        }

        private void LoadDataByDetainID()
        {
            if(!Get_mtxtInput(out int Input))
            {
                return;
            }

            Result<DTO_ShowDetainedLicenses> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_DetainID(Input);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(new List<DTO_ShowDetainedLicenses> { result_ShowAll.Value} );

            lblRecord.Text = Data.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }


        private void LoadDataByLicenseID()
        {
            if(!Get_mtxtInput(out int Input))
            {
                return;
            }

            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_LicenseID(Input);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }

        private void LoadDataByReleaseApplicationID()
        {
            if (!Get_mtxtInput(out int Input))
            {
                return;
            }

            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_ReleaseApplicationID(Input);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }

        private void LoadDataByNationalNo()
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
                return;

            string Input = txtInput.Text.Trim();

            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_NationalNo(Input);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }

        private void LoadDataByFullName()
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
                return;

            string Input = txtInput.Text.Trim();

            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_FullName(Input);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }

        private void LoadDataByIsReleased()
        {
            if (cbIsReleased.SelectedItem == null)
            {
                return;
            }

            bool isRelease = cbIsReleased.SelectedItem?.ToString() == "Yes";

            Result<List<DTO_ShowDetainedLicenses>> result_ShowAll = cls_ShowDetainedLicenses_BL.ShowAllDetainedLicensesBy_IsReleased(isRelease);
            if (!result_ShowAll.isSuccess)
            {
                MessageBoxHelper.Show(result_ShowAll.Message, result_ShowAll.messageType ?? SharedLayer.enums.enMessageType.Error, null, true);
                return;
            }

            SortableBindingList<DTO_ShowDetainedLicenses> Data =
                new SortableBindingList<DTO_ShowDetainedLicenses>(result_ShowAll.Value);

            lblRecord.Text = result_ShowAll.Value.Count.ToString();
            dgv_DetainedLicenses.DataSource = Data;
        }
    }
}
