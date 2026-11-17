using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classess.Search_Builder
{
    internal class clsSearchApplications
    {
        public enum enSearchType
        {
            None = 0,
            AppID = 1,
            NationalNo = 2,
            FullName = 3,
            Status = 4,
        }

        private enSearchType _searchType = enSearchType.None;
        private enApplicationStatus? _statusSearch = null;

        public enSearchType SearchType => _searchType;
        public enApplicationStatus? Status => _statusSearch;

        private void SetDefaultControls(TextBox txtInput, MaskedTextBox MtxtBox, ComboBox cbStatus)
        {
            txtInput.Text = string.Empty;
            MtxtBox.Text = string.Empty;

            int count = cbStatus.Items.Count;
            if (count > 0)
            {
                cbStatus.SelectedIndex = 0;
                _searchType = enSearchType.None;
            }
        }

        private void ToggleBetweenControls(Control ToShow, params Control[] controls)
        {
            foreach (Control ctrl in controls)
            {
                bool result = (ctrl == ToShow);
                ctrl.Visible = result;

                if (result)
                {
                    ctrl.Focus();
                }
            }
        }

        public void ChangedSelectedItem(ComboBox cbSearchType, TextBox txtInput, MaskedTextBox mtxtInput, ComboBox cbStatus)
        {
            SetDefaultControls(txtInput, mtxtInput, cbStatus);

            switch (cbSearchType.SelectedIndex)
            {
                case 0:
                    _searchType = enSearchType.None;
                    ToggleBetweenControls(null, txtInput, mtxtInput, cbStatus);
                    break;

                case 1:
                    _searchType = enSearchType.AppID;
                    ToggleBetweenControls(mtxtInput, txtInput, mtxtInput, cbStatus);
                    break;

                case 2:
                    _searchType = enSearchType.NationalNo;
                    ToggleBetweenControls(txtInput, txtInput, mtxtInput, cbStatus);
                    break;

                case 3:
                    _searchType = enSearchType.FullName;
                    ToggleBetweenControls(txtInput, txtInput, mtxtInput, cbStatus);
                    break;

                case 4:
                    _searchType = enSearchType.Status;
                    ToggleBetweenControls(cbStatus, txtInput, mtxtInput, cbStatus);
                    break;
            }
        }

        public void ChanageSelectedStatus(ComboBox CbStatusApplications)
        {
            switch(CbStatusApplications.SelectedIndex)
            {
                case 0:
                    _statusSearch = null;
                    break;

                case 1:
                    _statusSearch = enApplicationStatus.New;
                    break;

                case 2:
                    _statusSearch = enApplicationStatus.Cancelled;
                    break;
                case 3:
                    _statusSearch = enApplicationStatus.Completed;
                    break;
            }
        }
    }
}
