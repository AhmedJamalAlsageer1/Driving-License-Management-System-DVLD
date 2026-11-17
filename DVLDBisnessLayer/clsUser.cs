using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccessLayer;
using DVLD.DTOs;
using System.Dynamic;
using System.Data;
using System.Xml.Linq;
using System.Xml.Serialization;
using SharedLayer;

namespace DVLDBisnessLayer
{
    public class clsUser
    {
        enum enMode {Update, AddNew};
        private enMode _Mode;
        public int _UserID { get; private set; }
        public int _PersonID { get; private set; }
        public string _UserName { get; private set; }
        public string _password { get; private set; }
        public bool _isActive { get; private set; }

        public clsUser(int PersonID, string UserName, string Password, bool isActive)
        {
            _UserID = -1;
            _PersonID = PersonID;
            _UserName = UserName;
            _password = Password;
            _isActive = isActive;
            _Mode = enMode.AddNew;
        }
        
        private clsUser()
        {
            _UserID = -1;
            _PersonID = -1;
            _UserName = "";
            _password = "";
            _isActive = false;
            _Mode = enMode.Update;
        }

        private clsUser(DTOUser User)
        {
            _UserID = User.UserID;
            _PersonID = User.PersonID;
            _UserName = User.UserName;
            _password = User.Password;
            _isActive = User.IsActive;
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool isActive)
        {
            _UserID = UserID;
            _PersonID = PersonID;
            _UserName = UserName;
            _password = Password;
            _isActive = isActive;
            _Mode = enMode.AddNew;
        }

        public static DTOUser ConvertFromUserToDTO(clsUser User)
        {
            DTOUser dtoUser = new DTOUser();
            dtoUser.UserID = User._UserID;
            dtoUser.PersonID = User._PersonID;
            dtoUser.UserName = User._UserName;
            dtoUser.Password = User._password;
            dtoUser.IsActive = User._isActive;

            return dtoUser;
        }

        private bool AddnewUser()
        {
            DTOUser dtoUser = ConvertFromUserToDTO(this);
            this._UserID = clsUserAccessLayer.AddNewUser(dtoUser);
            dtoUser.UserID = this._UserID;
            return (_UserID != -1);
        }

        public void UpdateDetailUser(DTOUser dtoUser)
        {
            _PersonID = dtoUser.PersonID;
            _UserName = dtoUser.UserName;
            _password = dtoUser.Password;
            _isActive = dtoUser.IsActive;
        }

        private bool UpdateUser()
        {
            DTOUser dtoUser = ConvertFromUserToDTO(this);
            return clsUserAccessLayer.UpdateUser(dtoUser);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    return AddnewUser();
                default:
                    return UpdateUser();
            }     
        }

        public static DataTable GetAllUsers()
        {
            return clsUserAccessLayer.GetAllUsers();
        }

        static private void RemovePassword(ref DTOUser user, bool IncludePassword)
        {
            user.Password = null;
        }

        private void RemovePassword(bool IncludePassword)
        {
                this._password = null;
        }

        public static bool FindUserByUserID(int UserID, out DTOUser User, bool IncludePassword = true)
        {
            if(DVLDDataAccessLayer.clsUserAccessLayer.FindUserByID(UserID, out User))
            {
                if (User != null)
                {
                    if (!IncludePassword)
                    {
                        RemovePassword(ref User, IncludePassword);
                    }
                    return true;
                }
            }
            return false;
        }

        public static bool FindUserByPersonID(int PersonID, out DTOUser User, bool IncludePassword = true)
        {
            if(clsUserAccessLayer.FindUserByPersonID(PersonID, out User))
            {
                if (User != null)
                {
                    if (!IncludePassword)
                    {
                        RemovePassword(ref User, IncludePassword);
                    }
                    return true;
                }
            }
            return false;
        }

        public static bool FindUserByUserName(string UserName, out DTOUser user, bool IncludePassword)
        {
            if(clsUserAccessLayer.FindUserByUserName(UserName, out user))
            {
                if (user != null)
                {
                    if (!IncludePassword)
                    {
                        RemovePassword(ref user, IncludePassword);
                    }
                    return true;
                }
            }
            return false;
        }

        public static clsUser FindUserByUserID(int UserID, bool IncludePassword = true)
        {
            if(DVLDDataAccessLayer.clsUserAccessLayer.FindUserByID(UserID, out DTOUser dtoUser))
            {
                if (dtoUser != null)
                {
                    var user = new clsUser(dtoUser);
                    if (!IncludePassword)
                    {
                        user.RemovePassword(IncludePassword);
                    }
                    return user;
                }
            }
            return null;
        }

        public static Result <DTOUser> FindUserByUserID(int UserID)
        {
            if (DVLDDataAccessLayer.clsUserAccessLayer.FindUserByID(UserID, out DTOUser dtoUser))
            {
                if (dtoUser != null)
                {
                    var user = new clsUser(dtoUser);
                    if (true)
                    {
                        user.RemovePassword(true);
                        return Result<DTOUser>.Success(dtoUser, null, SharedLayer.enums.enMessageType.Info);
                    }
                }
            }
            return Result<DTOUser>.Failure(null, "هذا المستخدم غير موجود", SharedLayer.enums.enMessageType.Info);
        }

        public static clsUser FindUserByPersonID(int PersonID, bool IncludePassword = true)
        {
            if(clsUserAccessLayer.FindUserByPersonID(PersonID, out DTOUser dtoUser))
            {
                if (dtoUser != null)
                {
                    var user = new clsUser(dtoUser);
                    if (!IncludePassword)
                    {
                        user.RemovePassword(IncludePassword);
                    }
                    return user;
                }
            }
            return null;
        }

        public static clsUser FindUserByUserName(string UserName, bool IncludePassword)
        {
            if(clsUserAccessLayer.FindUserByUserName(UserName, out DTOUser dtouser))
            {
                if (dtouser != null)
                {
                    var user = new clsUser(dtouser);
                    if (!IncludePassword)
                    {
                        user.RemovePassword(IncludePassword);
                    }
                    return user;
                }
            }
            return null;
        }

        public static DataTable GetAllUserByActive()
        {
            return clsUserAccessLayer.GetAllUsersActives();
        }

        public static DataTable GetAllInactiveUsers()
        {
            return clsUserAccessLayer.GetAllInactiveUsers();
        }

        public static bool CheckUserNameAndPasswordExists(string UserName, string Password)
        {
            return clsUserAccessLayer.CheckUserNameAndPassword(UserName, Password);
        }

        public static bool isUserActive(string UserName, string Password)
        {
            return clsUserAccessLayer.isUserActive(UserName, Password);
        }

        public static bool isPersonUser(int PersonID)
        {
            return clsUserAccessLayer.isPersonUser(PersonID);
        }

        public static bool isUserNameUnique(string UserName, int UserID = -1, bool isUpdateMode = false)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                return false;

            if (isUpdateMode && UserID <= 0)
                return false;

            return clsUserAccessLayer.isUserNameUnique(UserName, UserID, isUpdateMode);
        }

        public static bool DeleteUser(int UserID)
        {
            if(UserID == -1)
                return false;

            return clsUserAccessLayer.DeleteUserByUserID(UserID);
        }

        public static bool isPasswordCurrect(int UserID, string Password)
        {
            return clsUserAccessLayer.isPasswordCurrect(UserID, Password);
        }

        public static bool UpdatePassword(int UserID, string CurrentPassword, string NewPassword)
        {
            string _currentpassword = CurrentPassword.Trim();
            string _newpassword = NewPassword.Trim();

            if(isPasswordCurrect(UserID, _currentpassword))
            {
                if(clsUserAccessLayer.UpdatePassword(UserID, _newpassword))
                    return true;
            }
            return false;
        }

        public static bool UserExists(int userid)
        {
            return clsUserAccessLayer.UserExists(userid);
        }

        public static Result <bool> IsUserExists(int userid)
        {
            if (!clsUserAccessLayer.UserExists(userid))
                return Result<bool>.Failure(false, "حصل خطأ أثناء البحث عن مستخدم هذا النظام بحيث لم نجده", SharedLayer.enums.enMessageType.Error);

            return Result<bool>.Success(true, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result <string> GetUserNameByUserID(int AppID)
        {
            if(clsUserAccessLayer.TryGetUserNameByUserID(AppID, out string UserName))
            {
                if (UserName != null)
                    return Result<string>.Success(UserName, null, SharedLayer.enums.enMessageType.Info);
            }

            return Result<string>.Failure(null, "هذا المستخدم غير موجود", SharedLayer.enums.enMessageType.Error);
        }
    }
}
