using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.Person_Forms;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmPersonDetails : Form
    {
        int _PersonID = -1;
        public Action OnDataUpdated { get; set; }

        private void LoadData(DTOPerson dto_person)
        {
            _PersonID = dto_person.PeronsID;
            uctrlPersonInfo1.LoadData(dto_person);
            uctrlPersonInfo1.OnDataUpdate = () =>
            {
                OnDataUpdated?.Invoke();
            };
        }


        public frmPersonDetails(int PersonID)
        {
            InitializeComponent();

            if (PersonID <= 0)
                this.Close();

            Result<DTOPerson> ResultPerson = clsPerson.FindByID(PersonID);

            if (!ResultPerson.isSuccess || ResultPerson == null)
            {
                MessageBoxHelper.Show(ResultPerson, "خطأ", true);
                this.Close();
            }

            this.LoadData(ResultPerson.Value);
        }

        private frmPersonDetails()
        {
            InitializeComponent();
        }

        public static frmPersonDetails ShowPersonDetailsBy_DriverID(int DriverID)
        {
            if(DriverID <= 0)
            {
                return null;
            }

            Result<DTOPerson> result_Person = clsPerson.FindByDriverID(DriverID);
            if(!result_Person.isSuccess || result_Person.Value == null)
            {
                MessageBoxHelper.Show(result_Person, "خطأ", true);
                return null;
            }


            DTOPerson dto_person = result_Person.Value;

            frmPersonDetails frm = new frmPersonDetails();
            frm.LoadData(dto_person);

            return frm;
        }

        public static frmPersonDetails ShowPersonDetailsBy_LicenseID(int LicenseID)
        {
            if (LicenseID <= 0)
            {
                return null;
            }

            Result<DTOPerson> result_Person = clsPerson.FindByLicenseID(LicenseID);
            if (!result_Person.isSuccess || result_Person.Value == null)
            {
                MessageBoxHelper.Show(result_Person, "خطأ", true);
                return null;
            }


            DTOPerson dto_person = result_Person.Value;

            frmPersonDetails frm = new frmPersonDetails();
            frm.LoadData(dto_person);

            return frm;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uctrlPersonInfo1_Load(object sender, EventArgs e)
        {
            if (_PersonID == -1)
                return;

            using (frmAddNewOrUpdatePerson frm = new frmAddNewOrUpdatePerson(_PersonID))
            {
                frm.OnDataUpdated = () =>
                {
                    this.OnDataUpdated?.Invoke();
                };
                frm.ShowDialog();
            }
        }
    }
}
