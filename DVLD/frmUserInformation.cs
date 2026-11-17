using DVLD.Inputs;
using DVLDBisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmUserInformation : Form
    {
        public frmUserInformation(int UserID)
        {
            InitializeComponent();
            if(clsUser.FindUserByUserID(UserID, out DTOs.DTOUser dtoUser, false))
            {
                EditUserInput UserInput = new EditUserInput(dtoUser);
                uctrlUserInfo1.LoadData(UserInput);

            }
        }
    }
}
