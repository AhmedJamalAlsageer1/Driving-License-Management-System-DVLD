using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLDBisnessLayer;
using DVLDBisnessLayer.New_LocalDriver_App;
using DVLDBisnessLayer.Validation;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.FormsApplications
{
    public partial class frmNewLocalDrivingLicenseApplication : Form
    {
        DTO_LDL_App CurrentApplication;
        public Action OnUpdate;

        public frmNewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            uctrlDispalyDataPerson1.SearchRequested += HandledSearchRequested;
        }

        private void HandleMissingApplicationData()
        {
            if (tabControl1.TabPages.Contains(tpApplicationInfo))
            {
                tabControl1.TabPages.Remove(tpApplicationInfo);
                btnSave.Enabled = false;
            }
        }

        private void SetDefaultForUiValuesForNewApplication()
        {
            llApplicationDate.Text = DateTime.Now.ToString("dd - MM - yyyy");
            Result<DTOApplicationTypes> result = clsApplicationTypesBL.GetApplicationType(SharedLayer.enums.enApplicationTypes.NewLocalLicense);
            

            if(!result.isSuccess || result.Value == null)
            {
                MessageBoxHelper.Show(result.Message, SharedLayer.enums.enMessageType.Error, "خطأ غير متوقع", true);
                return;
            }

            decimal Fees = result.Value.fees;
            int id = result.Value.ID;

            llFees.Text = Fees.ToString();
            CurrentApplication.PaidFees = Fees;
            CurrentApplication.ApplicationTypeID = (enApplicationTypes) id;

            llUserName.Text = SharedLayer.Context.AppContext.CurrentUser.UserName;
            CurrentApplication.PersonID = uctrlPersonInfo1.PersonID;

            int Count = cbLicenseClasses.Items.Count;
            if(Count >= 3)
                cbLicenseClasses.SelectedIndex = 2;
        }

        private void EnsureApplicationTabVisible()
        {
            if (!tabControl1.TabPages.Contains(tpApplicationInfo))
            {
                if (CurrentApplication == null)
                    CurrentApplication = new DTO_LDL_App();

                tabControl1.TabPages.Add(tpApplicationInfo);
                PreparApplicationTab();

                if (CurrentApplication.PersonID != -1)
                {
                    btnNext.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("عذرا حصل خطأ من فضلك ادخل بيانات شخص آخر حتى أو أعد البحث حتى يكتمل الربط.", "خطأ في الربط", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool TryLoadDataPersonByID(string personID)
        {
            bool Found = false;
            if(!int.TryParse(Convert.ToString(personID), out int _PersonID))
            {
                MessageBoxHelper.Show("من فضلك أدخل رقما صحيحا", enMessageType.Error, "خطأ"
                   , true);
                return false;
            }

            Result<DTOPerson> resultPerson = clsPerson.FindByID(_PersonID);
            if (!resultPerson.isSuccess || resultPerson == null)
            {
                MessageBoxHelper.Show(resultPerson, "خطأ", true);
                Found = false;
            }
            else
            {
                uctrlPersonInfo1.LoadData(resultPerson.Value);
                CurrentApplication.PersonID = resultPerson.Value.PeronsID;
                Found = true;
            }

            if (Found)
            {
                EnsureApplicationTabVisible();
            }
            else
            {
                HandleMissingApplicationData();
            }

            return Found;
        }

        private bool TryLoadDataPersonByName(string Name)
        {
            bool Found = false;

            Result <DTOPerson> resultPerson = clsPerson.FindByName(Name);
            if (!resultPerson.isSuccess || resultPerson == null)
            {
                MessageBoxHelper.Show(resultPerson, "خطأ", true);
                Found = false;
            }
            else
            {
                uctrlPersonInfo1.LoadData(resultPerson.Value);
                CurrentApplication.PersonID = resultPerson.Value.PeronsID;
                Found = true;  
            }

            if (Found)
            {
                EnsureApplicationTabVisible();
            }
            else
            {
                HandleMissingApplicationData();
            }

            return Found;
        }

        private bool TryLoadDataPersonByNationalNo(string NationalNo)
        {
            bool Found = false;

            Result<DTOPerson> resultPerson = clsPerson.FindByNationalNo(NationalNo);
            if (!resultPerson.isSuccess || resultPerson == null)
            {
                MessageBoxHelper.Show(resultPerson, "خطأ", true);
                Found = false;
            }
            else
            {
                uctrlPersonInfo1.LoadData(resultPerson.Value);
                CurrentApplication.PersonID = resultPerson.Value.PeronsID;
                Found = true;
            }

            if (Found)
            {
                EnsureApplicationTabVisible();
            }
            else
            {
                HandleMissingApplicationData();
            }

            return Found;
        }

        private void HandledSearchRequested(enSearchType searchType, string Input)
        {
            if (CurrentApplication == null)
                CurrentApplication = new DTO_LDL_App();
            else
                CurrentApplication.Clear();

                switch (searchType)
                {
                    case enSearchType.PersonID:
                        TryLoadDataPersonByID(Input);
                        break;
                    case enSearchType.Name:
                        TryLoadDataPersonByName(Input);
                        break;
                    case enSearchType.NationalNo:
                        TryLoadDataPersonByNationalNo(Input);
                        break;
                }
        }

        private void LoadDefaultData()
        {
            tabControl1.TabPages.Remove(tpApplicationInfo);
        }


        private void NewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            LoadDefaultData();
        }

        private void FillLicenseClassComboBox()
        {
            List<DTOComboBoxItem> dtoLicenseClassList = clsLicenseClassesBL.GetAllLicenseClassNames();
            
            cbLicenseClasses.DataSource = null;
            cbLicenseClasses.ValueMember = nameof(DTOComboBoxItem.Value);
            cbLicenseClasses.DisplayMember = nameof(DTOComboBoxItem.Display);
            cbLicenseClasses.DataSource = dtoLicenseClassList;
        }

        private void PreparApplicationTab()
        {
            if (!tabControl1.TabPages.Contains(tpApplicationInfo))
                return;

            btnSave.Enabled = true;
            FillLicenseClassComboBox();
            SetDefaultForUiValuesForNewApplication();
        }

        private void aboutNext()
        {
            if (tabControl1.TabPages.Contains(tpApplicationInfo))
            {
                tabControl1.SelectedTab = tpApplicationInfo;
                btnSave.Focus();
            }

            btnNext.Enabled = tabControl1.SelectedTab != tpApplicationInfo;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            aboutNext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool PopulateCurrentApplicationFromUi()
        {
            if (decimal.TryParse(llFees.Text.ToString(), out decimal valueDecimal))
                CurrentApplication.PaidFees = valueDecimal;
            else
                return false;

            
            if (cbLicenseClasses.SelectedValue is int LicenseClassID)
                CurrentApplication.LicenseClassID = (enLicenseClass) LicenseClassID;
            else
                return false;

            Result<DTOApplicationTypes> result = clsApplicationTypesBL.GetApplicationType(SharedLayer.enums.enApplicationTypes.NewLocalLicense);
            if (!result.isSuccess)
            {
                MessageBoxHelper.Show(result.Message, SharedLayer.enums.enMessageType.Error, "خطأ غير متوقع", true);
                return false;
            }
            DTOApplicationTypes dto_AppType = result.Value;
            CurrentApplication.ApplicationTypeID = enApplicationTypes.NewLocalLicense;
            CurrentApplication.PaidFees = dto_AppType.fees;

            if (CurrentApplication.PersonID == -1)
                return false;


            CurrentApplication.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            return true;
        }

        private bool TrySave()
        {
            if (!PopulateCurrentApplicationFromUi())
            {
                MessageBox.Show("هناك خطأ في تعبئة البيانات أو أن القيم غير صالحة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            cls_LDL_ApplicationRequest_BL Request_NewLDLApp = new cls_LDL_ApplicationRequest_BL(CurrentApplication);
            OperationResult result = Request_NewLDLApp.Save();

            if(result.IsSuccess)
            {
                if (CurrentApplication.PersonID != -1)
                {
                    MessageBox.Show(result.Message, "نجحت عملية الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    llApplicationID.Text = CurrentApplication.LDLApplicationID.ToString();
                    llApplicationDate.Text = CurrentApplication.ApplicationDate.ToString();
                    llUserName.Text = SharedLayer.Context.AppContext.CurrentUser.UserName;
                }
                else
                    MessageBox.Show(result.Message, "تم الإضافة ولكن..", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                OnUpdate?.Invoke();
            }
            else
            {
                MessageBox.Show(result.Message, "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            return result.IsSuccess;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //aboutNext();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == tpApplicationInfo)
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;
        }
    }
}
