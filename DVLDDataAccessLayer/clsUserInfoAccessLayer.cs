using DVLD.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DVLDDataAccessLayer
{
    public static class clsUserInfoAccessLayer
    {
        public static DataTable GetAllUsersInfo()
        {
            DataTable dt = null;

            string Query = @"Select * From vw_GetAllUsersInfo;";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllUsersByFullName( string FullName)
        {
            DataTable dt = null;

            string Query = @"Select * From vw_GetAllUsersInfo
                            Where FullName = @FullName";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 200).Value = FullName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllUsersActive()
        {
            DataTable dt = null;

            string Query = @"Select * From vw_GetAllUsersInfo
                            Where IsActive = @IsActive";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = true;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllUsersIneactive()
        {
            DataTable dt = null;

            string Query = @"Select * From vw_GetAllUsersInfo
                            Where IsActive = @IsActive";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        private static bool TryBuildUserInfoFromReader(SqlDataReader reader, out DTOUserInfo user)
        {
            user = null;
            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "UserID", out int valueint))
            {
                user = new DTOUserInfo();
                user.UserID = valueint;
            }
            else
                return false;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out valueint))
                user.PersonID = valueint;
            else
                return false;

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string valueString))
                user.FullName = valueString;
            else
                return false;

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "UserName", out valueString))
                user.UserName = valueString;
            else
                return false;

            if (clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive") is bool valuebool)
                user.isActive = valuebool;
            else
                return false;

            return true;
        }

        public static bool FindUserInfoByUserID(int UserID, out DTOUserInfo user)
        {
            user = null;
            bool isFound = false;

            string Query = @"Select * From vw_GetAllUsersInfo
                             Where UserID = @UserID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryBuildUserInfoFromReader(reader, out user))
                        isFound = true;
                    else
                    {
                        user.Clear();
                        isFound = false;
                    }
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                user = null;
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool FindUserInfoByPersonID(int PersonID, out DTOUserInfo user)
        {
            user = null;
            bool isFound = false;

            string Query = @"Select * From vw_GetAllUsersInfo
                             Where PersonID = @PersonID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (TryBuildUserInfoFromReader(reader, out user))
                        isFound = true;
                    else
                    {
                        user.Clear();
                        isFound = false;
                    }
                }
                else
                {
                    user = null;
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool FindUserInfoByUserName(string UserName, out DTOUserInfo user)
        {
            user = null;
            bool isFound = false;

            string Query = @"Select * From vw_GetAllUsersInfo
                             Where UserName = @UserName";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserName", SqlDbType.Int).Value = UserName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (TryBuildUserInfoFromReader(reader, out user))
                        isFound = true;
                    else
                    {
                        user.Clear();
                        isFound = false;
                    }
                }
                else
                {
                    isFound = false;
                }    

                reader.Close();
            }
            catch (Exception ex)
            {
                user = null;
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
    }
}
