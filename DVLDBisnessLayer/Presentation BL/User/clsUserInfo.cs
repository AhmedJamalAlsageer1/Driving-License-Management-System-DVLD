using DVLD.DTOs;
using DVLDDataAccessLayer;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLDBisnessLayer
{
    public static class clsUserInfo
    {
        public static DataTable GetAllUserInfo()
        {
            return clsUserInfoAccessLayer.GetAllUsersInfo();
        }

        public static bool GetUserInfoByUserID(int UserID, out DTOUserInfo user)
        {
            clsUserInfoAccessLayer.FindUserInfoByUserID(UserID, out user);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public static bool GetUserInfoByPersonID(int UserID, out DTOUserInfo user)
        {
            clsUserInfoAccessLayer.FindUserInfoByPersonID(UserID, out user);
            if (user != null)
                return true;

            return false;
        }

        public static bool GetUserInfoByUserName(string UserName, out DTOUserInfo user)
        {
            clsUserInfoAccessLayer.FindUserInfoByUserName(UserName, out user);
            if (user != null)
                return true;

            return false;
        }

        public static DataTable GetUsersByFullName(string FullName)
        {
            return clsUserInfoAccessLayer.GetAllUsersByFullName(FullName);
        }

        public static DataTable GetAllUserActive()
        {
            return clsUserInfoAccessLayer.GetAllUsersActive();
        }

        public static DataTable GetAllUserInactive()
        {
            return clsUserInfoAccessLayer.GetAllUsersIneactive();
        }
    }
       
}
