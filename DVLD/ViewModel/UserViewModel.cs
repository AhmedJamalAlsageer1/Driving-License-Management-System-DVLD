using DVLD.DTOs;
using DVLD.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.ViewModel
{
    public class UserViewModel
    {

        public string UserID { get; set; }

        public string PersonID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ActivationStatus { get; set; }


        public UserViewModel(EditUserInput _UserInput)
        {
            this.UserID = _UserInput.GetUserIdDisplay();
            this.PersonID = _UserInput.GetPersonIdDisplay();
            this.UserName = _UserInput.UserName;
            this.Password = _UserInput.Password;
            this.ActivationStatus = _UserInput.GetStatusActiveText();
        }

        public void Clear()
        {
            UserID = string.Empty;
            PersonID = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            ActivationStatus = string.Empty;
        }
    }
}
