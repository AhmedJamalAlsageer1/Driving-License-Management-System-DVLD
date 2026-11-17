using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classess.Helpers;
using DVLDBisnessLayer;
using SharedLayer;

namespace DVLD.Person_Forms
{
    public partial class frmAddNewOrUpdatePerson : Form
    {
        public int? personid;
        private void UctrlPersonInfo1_PersonAdded(int newPersonID)
        {
            MessageBox.Show($"تم إضافة الشخص بنجاح. المعرف الجديد هو: {newPersonID}");
            uctrlAddUpdatePerson1.OptionChoosed = uctrlAddUpdatePerson.enChooseOption.Update;
            llMode.Text = "Update Person";
            llPersonID.Text = newPersonID.ToString();
            uctrlAddUpdatePerson1.dotPerson.PersonID = newPersonID;
            uctrlAddUpdatePerson1.FillControlsFromDOTPerson();

           personid = newPersonID;
        }



        public Action<int> UpdatedNewPersonIDToFatherForm;
        public Action OnDataUpdated { get; set; }

        private void MergedOperationIDIntoParentForm(int PersonID)
        {
            this.BeginInvoke(new Action(() =>
            {
                UpdatedNewPersonIDToFatherForm?.Invoke(PersonID);
            }));
        }



        public frmAddNewOrUpdatePerson()
        {
            InitializeComponent();
            uctrlAddUpdatePerson1.OptionChoosed = uctrlAddUpdatePerson.enChooseOption.AddNew;
            uctrlAddUpdatePerson1.PersonAdded += UctrlPersonInfo1_PersonAdded;
            uctrlAddUpdatePerson1.returnedUpdatedNewPersonID += MergedOperationIDIntoParentForm;
            uctrlAddUpdatePerson1.FillControlsFromDOTPerson();
            uctrlAddUpdatePerson1.OnDataUpdate = () =>
            {
                this.OnDataUpdated?.Invoke();
            };
        }

        public frmAddNewOrUpdatePerson(int PersonID)
        {
            InitializeComponent();
            llMode.Text = "Update Person";
            llPersonID.Text = PersonID.ToString();
            uctrlAddUpdatePerson1.OptionChoosed = uctrlAddUpdatePerson.enChooseOption.Update;
            uctrlAddUpdatePerson1.dotPerson = LoadDataFromBuisnesByID(PersonID);
            uctrlAddUpdatePerson1.FillControlsFromDOTPerson();
            uctrlAddUpdatePerson1.OnDataUpdate = () =>
            {
                this.OnDataUpdated?.Invoke();
            };  
        }

        uctrlAddUpdatePerson.clsDOTPerson LoadDataFromBuisnesByID(int PersonID)
        {
            Result <clsPerson> resultPerson = clsPerson.FindObjectByID(PersonID);

            if(resultPerson == null || !resultPerson.isSuccess)
            {
                MessageBoxHelper.Show(resultPerson, "خطأ", true);
                this.Close();
                return null;
            }

            clsPerson Person = resultPerson.Value;

            uctrlAddUpdatePerson.clsDOTPerson dotPerson = new uctrlAddUpdatePerson.clsDOTPerson();
            dotPerson.PersonID = PersonID;
            dotPerson.NationalNo = Person._NationalNo;
            dotPerson.FirstName = Person._FirstName;
            dotPerson.SecondName = Person._SecondName;
            dotPerson.ThirdName = Person._ThirdName;
            dotPerson.LastName = Person._LastName;
            dotPerson.DateOfBirth = Person._DateOfBirth;
            dotPerson.Gendor = Person._Gendor;
            dotPerson.Address = Person._Address;
            dotPerson.Phone = Person._Phone;
            dotPerson.Email = Person._Email;
            dotPerson.Country = Person._CountryName;
            dotPerson.ImagePath = Person._ImagePath;

            return dotPerson;
        }
    }
}
