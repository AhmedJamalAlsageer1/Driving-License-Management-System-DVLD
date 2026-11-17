using DVLD.Classess.Helpers;
using DVLD.Classess.UIItems;
using DVLD.DTOs.International_License;
using DVLDBisnessLayer.InternationalLicense;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Internationall_Licenses.frm_ManageInternationalLicenseApplications
{
    public static class clsIntLicenseSearch
    {
        public enum enSearchType : byte
        {
            None = 0,
            Int_LicenseID = 1,
            ApplicationID = 2,
            DriverID = 3,
            L_LicenseID = 4
        }

        private static enSearchType _SearchType;
        public static enSearchType SearchType => _SearchType;

        public static void SetSearchType(enSearchType searchType, MaskedTextBox mtxt)
        {
            _SearchType = searchType;

            if (_SearchType == enSearchType.None)
                mtxt.Visible = false;
            else
                mtxt.Visible = true;
        }

        public static void LoadDataOn_Dgv(DataGridView dgv, string txtInput, Label lblRecordText, int SelectedID = -1)
        {
            if(_SearchType == enSearchType.None)
            {
                Load_AllIntLicenses(dgv, lblRecordText, SelectedID);
            }

            else if (_SearchType == enSearchType.Int_LicenseID)
            {
                Load_IntLicense_By_IntLicenseID(dgv, txtInput, lblRecordText, SelectedID);
            }

            else if (_SearchType == enSearchType.L_LicenseID)
            {
                Load_IntLicense_By_L_LicenseID(dgv, txtInput, lblRecordText, SelectedID);
            }

            else if (_SearchType == enSearchType.ApplicationID)
            {
                Load_IntLicense_By_ApplicationID(dgv, txtInput, lblRecordText, SelectedID);
            }

            else if (_SearchType == enSearchType.DriverID)
            {
                Load_IntLicense_By_DriverID(dgv, txtInput, lblRecordText, SelectedID);
            }
        }

        private static bool GetInputByInt(string mtxtInput, out int? value)
        {
            value = null;
            if (!int.TryParse(mtxtInput, out int Num))
                return false;

            value = Num;
            return true;
        }

        private static void Load_AllIntLicenses(DataGridView dgv, Label lblRecordText, int Selected_IntLicense)
        {
            Result<List<DTO_ShowIntLicenses>> result_AllIntLicenses = clsShow_IntLicense_BL.ShowAll_intLicenses();

            if(!result_AllIntLicenses.isSuccess)
            {
                MessageBoxHelper.Show(result_AllIntLicenses.Message, result_AllIntLicenses.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                dgv.DataSource = null;
                return;
            }

            List<DTO_ShowIntLicenses> All_IntLicenses = result_AllIntLicenses.Value;
            lblRecordText.Text = All_IntLicenses.Count.ToString();

            SortableBindingList<DTO_ShowIntLicenses> data = new SortableBindingList<DTO_ShowIntLicenses>(All_IntLicenses);
            dgv.DataSource = data;

            if(Selected_IntLicense > 0)
            {
                DgvConfig.FoucesOnSelectedID(dgv, "InitLicenseID", Selected_IntLicense);
            }
        }

        private static void Load_IntLicense_By_IntLicenseID(DataGridView dgv, string mtxtInput, Label lblRecordText, int Selected_IntLicense)
        {
            if(!GetInputByInt(mtxtInput, out int? value) || !value.HasValue)
            {
                return;
            }

            int IntLicenseID = value.Value;
            Result<DTO_ShowIntLicenses> result_IntLicense = clsShow_IntLicense_BL.Find_IntLicneseIDBy_IntLicenseID(IntLicenseID);

            if (!result_IntLicense.isSuccess)
            {
                MessageBoxHelper.Show(result_IntLicense.Message, result_IntLicense.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                dgv.DataSource = null;
                return;
            }

            DTO_ShowIntLicenses Int_License = result_IntLicense.Value;
            List<DTO_ShowIntLicenses> ListInt_License = new List<DTO_ShowIntLicenses> {Int_License};
            lblRecordText.Text = ListInt_License.Count.ToString();

            dgv.DataSource = new SortableBindingList<DTO_ShowIntLicenses>(ListInt_License);

            if (Selected_IntLicense > 0)
            {
                DgvConfig.FoucesOnSelectedID(dgv, "InitLicenseID", Selected_IntLicense);
            }
        }

        private static void Load_IntLicense_By_DriverID(DataGridView dgv, string mtxtInput, Label lblRecordText, int Selected_IntLicense)
        {
            if (!GetInputByInt(mtxtInput, out int? value) || !value.HasValue)
            {
                return;
            }

            int DriverID = value.Value;
            Result<List<DTO_ShowIntLicenses>> result_IntLicensesByDriverID = clsShow_IntLicense_BL.Find_ALLIntLicensesBy_DriverID(DriverID);

            if (!result_IntLicensesByDriverID.isSuccess)
            {
                MessageBoxHelper.Show(result_IntLicensesByDriverID.Message, result_IntLicensesByDriverID.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                dgv.DataSource = null;
                return;
            }


            List<DTO_ShowIntLicenses> Int_Licenses = result_IntLicensesByDriverID.Value;
            lblRecordText.Text = Int_Licenses.Count.ToString();


            dgv.DataSource = new SortableBindingList<DTO_ShowIntLicenses>(Int_Licenses);

            if (Selected_IntLicense > 0)
            {
                DgvConfig.FoucesOnSelectedID(dgv, "InitLicenseID", Selected_IntLicense);
            }
        }

        private static void Load_IntLicense_By_L_LicenseID(DataGridView dgv, string mtxtInput, Label lblRecordText, int Selected_IntLicense)
        {
            if (!GetInputByInt(mtxtInput, out int? value) || !value.HasValue)
            {
                return;
            }

            int L_LicenseID = value.Value;
            Result<DTO_ShowIntLicenses> result_L_License = clsShow_IntLicense_BL.Find_IntLicneseIDByL_LicenseID(L_LicenseID);

            if (!result_L_License.isSuccess)
            {
                MessageBoxHelper.Show(result_L_License.Message, result_L_License.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                dgv.DataSource = null;
                return;
            }

            DTO_ShowIntLicenses Int_License = result_L_License.Value;
            List<DTO_ShowIntLicenses> ListInt_License = new List<DTO_ShowIntLicenses> { Int_License };
            lblRecordText.Text = ListInt_License.Count.ToString();


            dgv.DataSource = new SortableBindingList<DTO_ShowIntLicenses>(ListInt_License);

            if (Selected_IntLicense > 0)
            {
                DgvConfig.FoucesOnSelectedID(dgv, "InitLicenseID", Selected_IntLicense);
            }
        }

        private static void Load_IntLicense_By_ApplicationID(DataGridView dgv, string mtxtInput, Label lblRecordText, int Selected_IntLicense)
        {
            if (!GetInputByInt(mtxtInput, out int? value) || !value.HasValue)
            {
                return;
            }

            int I_ApplicationID = value.Value;
            Result<DTO_ShowIntLicenses> result_IntLicenses_ByApplicationID = clsShow_IntLicense_BL.Find_IntLicneseIDBy_ApplicationID(I_ApplicationID);

            if (!result_IntLicenses_ByApplicationID.isSuccess)
            {
                MessageBoxHelper.Show(result_IntLicenses_ByApplicationID.Message, result_IntLicenses_ByApplicationID.messageType ?? SharedLayer.enums.enMessageType.Error,
                    "خطأ", true);
                dgv.DataSource = null;
                return;
            }

            DTO_ShowIntLicenses Int_License = result_IntLicenses_ByApplicationID.Value;
            List<DTO_ShowIntLicenses> ListInt_License = new List<DTO_ShowIntLicenses> { Int_License };
            lblRecordText.Text = ListInt_License.Count.ToString();

            dgv.DataSource = new SortableBindingList<DTO_ShowIntLicenses>(ListInt_License);

            if (Selected_IntLicense > 0)
            {
                DgvConfig.FoucesOnSelectedID(dgv, "InitLicenseID", Selected_IntLicense);
            }
        }
    }
}
