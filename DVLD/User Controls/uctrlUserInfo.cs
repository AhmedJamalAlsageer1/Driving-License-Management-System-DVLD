using DVLD.Classess.Helpers;
using DVLD.DTOs;
using DVLD.Inputs;
using DVLDBisnessLayer;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class uctrlUserInfo : UserControl
    {
        public uctrlUserInfo()
        {
            InitializeComponent();
        }

        public void LoadData(EditUserInput _userinput)
        {
            if (_userinput != null)
            {
                uctrlLoginInformation1.UpdateControl(_userinput.GetUserIdDisplay(), _userinput.UserName, _userinput.GetStatusActiveText());

                Result<DTOPerson> resultPerson = clsPerson.FindByID(_userinput.PersonID);

                if (!resultPerson.isSuccess || resultPerson.Value == null)
                {
                    MessageBoxHelper.Show(resultPerson, "خطأ", true);
                }

                uctrlPersonInfo1.LoadData(resultPerson.Value);
            }
        }
    }
}
