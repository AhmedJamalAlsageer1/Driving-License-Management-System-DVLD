using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.Properties;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Person_Forms
{
    public partial class uctrlPersonInfo : UserControl
    {
        public DTOPerson dtoGlobalPerson { get; private set; }
        int _PersonID = -1;

        public Action OnDataUpdate { get; set; }

        public int PersonID
        {
            get { return _PersonID; }
        }

        public uctrlPersonInfo()
        {
            InitializeComponent();
        }

        public void ClearControls()
        {
            llPersonID.Text = "????";
            llNationalNo.Text = "????";
            llName.Text = "????";
            llDateOfBirth.Text = "????";
            llGendor.Text = "????";
            llEmail.Text = "????";
            llPhone.Text = "????";
            llAddress.Text = "????";
            llCountry.Text = "????";
            pbGendor.Image.Dispose();
            pbGendor.Image = null;
            pbGendor.Image = Resources.Person_Question_180;
        }

        public void LoadData(DTOPerson person)
        {
            ReresentingDataInControls(person);
        }

        private void LoadPersonImagePath(string ImagePath, bool Gendor)
        {
            pbGendor.Image.Dispose();
            pbGendor.Image = null;

            if(!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                pbGendor.ImageLocation = ImagePath;
            }
            else
            {
                if (Gendor == true)
                    pbGendor.Image = Resources.Female_reporter_180;
                else
                    pbGendor.Image = Resources.Person_Question_180;
            }
        }

        public void ReresentingDataInControls(DTOPerson dtoPerson)
        {
            ClearControls();

            llPersonID.Text = dtoPerson.PeronsID.ToString();
            llNationalNo.Text = dtoPerson.NationalNo.ToString();
            llName.Text = dtoPerson.FirstName + " " + dtoPerson.SecondName + " " + dtoPerson.ThirdName + " " + dtoPerson.LastName;
            llDateOfBirth.Text = dtoPerson.DateOfBirth.ToShortDateString();
            llGendor.Text = dtoPerson.Gendor == false ? "Male" : "Female";
            llEmail.Text = dtoPerson.Email;
            llPhone.Text = dtoPerson.Phone;
            llAddress.Text = dtoPerson.Address;
            llCountry.Text = dtoPerson.CountryName;

            _PersonID = dtoPerson.PeronsID;
            dtoGlobalPerson = dtoPerson;
            LoadPersonImagePath(dtoPerson.ImagePath, dtoPerson.Gendor);
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dtoGlobalPerson == null)
            {
                MessageBox.Show("من فضلك اختر شخصا لتحديث معلوماته.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson(dtoGlobalPerson.PeronsID))
            {
                frm.OnDataUpdated = () =>
                {
                    OnDataUpdate?.Invoke();
                };
                frm.ShowDialog();
                LoadingDataAfterModification();
            }

        }
        private void LoadingDataAfterModification()
        {
            Result<DTOPerson> resultPerson = clsPerson.FindByID(_PersonID);

            if (!resultPerson.isSuccess || resultPerson.Value == null)
            {
                MessageBoxHelper.Show("حدث خطأ أثناء استرجاع البيانات", SharedLayer.enums.enMessageType.Error, "خطأ", true);
                return;
            }

            LoadData(resultPerson.Value);
        }
    }
}
