using DVLD.DTOs;
using DVLD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Inputs
{
    public class EditUserInput
    {
        public int UserID { get; set; }

        public int PersonID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        public EditUserInput()
        {
            UserID = -1;
            PersonID = -1;
            UserName = string.Empty;
            Password = string.Empty;
            Active = false;
        }

        public EditUserInput(DTOUser dtoUser)
        {
            UserID = dtoUser.UserID;
            PersonID = dtoUser.PersonID;
            UserName = dtoUser.UserName;
            Password = dtoUser.Password;
            Active = dtoUser.IsActive;
        }

        static public EditUserInput ConvertFromDtoUserTo(DTOUser dtoUser)
        {
            if (dtoUser != null)
            {
                EditUserInput _input = new EditUserInput(dtoUser);
                return _input;
            }
            return null;
        }

        public string GetUserIdDisplay()
        {
            return $"#{UserID}";
        }

        public string GetPersonIdDisplay()
        {
            return $"#{PersonID}";
        }

        public string GetStatusActiveText()
        {
            return Active ? "نشط" : "غير نشط";
        }
    }
}
