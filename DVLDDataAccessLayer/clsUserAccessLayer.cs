using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DVLD.DTOs;
using SharedLayer;

namespace DVLDDataAccessLayer
{
    public static class clsUserAccessLayer
    {

        public static DataTable GetAllUsers()
        {
            DataTable dt;
            dt = new DataTable();

            string Query = @"Select * From Users";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllUsersActives()
        {
            DataTable dt = null;

            string Query = @"Select * From Users Where IsActive = @IsActive";
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

        public static DataTable GetAllInactiveUsers()
        {
            DataTable dt = null;

            string Query = @"Select * From Users Where IsActive = @IsActive";
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


        public static bool FindUserByID(int UserID, out DTOUser dtoUser)
        {
            bool IsFound = false;
            dtoUser = null;

            string Query = @"Select * From Users Where UserID = @UserID";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@UserID", System.Data.SqlDbType.Int).Value = UserID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuildUserFromReader(reader, out dtoUser))
                            IsFound = true;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                dtoUser = null;
                IsFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [FindUserByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsFound;
        }

        public static bool TryBuildUserFromReader(SqlDataReader reader,  out DTOUser User)
        {
            User = null;

            if (reader == null)
                return false;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "UserID", out int value))
            {
                User = new DTOUser();
                User.UserID = value;
            }
            else
                return false;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out value))
                User.PersonID = value;
            else
                return false;

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "UserName", out string stringvalue))
                User.UserName = stringvalue;
            else
                return false;

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "Password", out stringvalue))
                User.Password = stringvalue;
            else
                return false;

            bool? boolvalue = clsMapReader.TryGetBoolFromReaderImediately(reader, "isActive");
            if (boolvalue.HasValue)
                User.IsActive = boolvalue.Value;
            else
                return false;

            return true;
        }

        public static bool FindUserByPersonID(int PersonID, out DTOUser User)
        {
            User = null;
            bool isFound = false;

            string Query = @"Select * From Users Where @PersonID = PersonID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand (Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryBuildUserFromReader(reader, out User))
                        isFound = true;
                    else
                        isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                User = null;
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool FindUserByUserName (string UserName, out DTOUser User)
        {
            User = null;
            bool isFound = false;

            string Query = @"Select * From Users Where @UserName = UserName";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = UserName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryBuildUserFromReader(reader, out User))
                        isFound = true;
                    else
                        isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                User = null;
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool CheckUserNameAndPassword(string UserName, string Password)
        {
            bool isFound = false;

            string Query = "Select 1 From Users Where UserName = @UserName AND Password = @Password And IsActive = 1";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = UserName;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = Password;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isFound = Convert.ToBoolean(result);
                }
            }
            catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool isUserActive(string UserName, string Password)
        {
            bool isFound = false;

            string Query = "Select IsActive From Users Where UserName = @UserName AND Password = @Password";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = UserName;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = Password;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if(result != null && result != DBNull.Value)
                {
                    isFound = Convert.ToBoolean(result);
                }
            }
            catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            
            return isFound;
        }

        public static bool isPersonUser(int PersonID)
        {
            bool isUser = false;

            string Query = @"select case
                                when exists(
                                	select 1 From People Inner join
                                    Users On @PersonID = Users.PersonID
                                	)Then 1
                                	Else 0
                                End As isExists;";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null && result != DBNull.Value)
                {
                    int value;
                    if(int.TryParse(result.ToString(), out value))
                    {
                        isUser = value == 1;
                    }
                    else
                    {
                        isUser = false;
                    }
                }
            }
            catch(Exception ex)
            {
                isUser = false;
            }
            finally
            {
                connection.Close();
            }

            return isUser;
        }

        public static bool isUserNameUnique(string Username, int UserID = -1, bool isUpdateMode = false)
        {
            bool isUnique = false;

                string Query = @"Select 
                                case
                                    When Exists (Select 1 From Users Where UserName = @UserName "
                                    + (isUpdateMode? "AND UserID != @UserID" : "") + @"
                                    )
                                    Then 0
                                    Else 1
                                End As IsUnique";


            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = Username;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                {
                    isUnique = value == 1;
                }

            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return isUnique;
        }

        public static int AddNewUser(DTOUser user)
        {
            int UserID = -1; 
            string Query = @"Insert into Users (PersonID, UserName, Password, IsActive)
                            values (@PersonID, @UserName, @Password, @IsActive);
                            SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = user.PersonID;
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = user.UserName;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = user.Password;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                {
                    UserID = value; 
                }
            }
            catch(Exception ex)
            {
                UserID = -1;
            }
            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUser(DTOUser user)
        {
            int rowsAffected = -1;

            string Query = @"Update Users
                             set
                                UserName = @UserName,
                                Password = @Password,
                                IsActive = @IsActive
                            Where
                                UserID = @UserID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = user.UserName;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = user.Password;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = user.UserID;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                rowsAffected = -1;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > -1);
        }

        public static bool DeleteUserByUserID(int UserID)
        {
            int rowAffected = 0;

            string Query = "Delete From Users Where UserID = @UserID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("UserID", SqlDbType.Int).Value = UserID;

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }

        public static bool isPasswordCurrect(int UserID, string Password)
        {
            bool isCurrect = false;

            string Query = @"Select case 
                            When EXISTS (
                               Select 1 From Users 
                               Where Password = @Password And UserID = @UserID
                                        )
                                        Then 1 Else 0
                                    end;";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = Password;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isCurrect = Convert.ToInt32(result) == 1;
                }
            }
            catch(Exception ex)
            {
                isCurrect = false;
            }
            finally
            {
                connection.Close();
            }
            return isCurrect;
        }

        public static bool UpdatePassword(int UserID, string NewPassword)
        {
            int rowAffected = 0;

            string Query = @"Update Users
                                Set Password = @Password
                             Where UserID = @UserID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            command.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = NewPassword;

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }

        public static bool UserExists(int UserID)
        {
            bool isFound = false;
            string Query = @"Select 
                                Case
                                    When Exists (
                                        Select 1 From Users Where UserID = @UserID
                                    )
                                    Then 1
                                    Else 0
                                    End As UserExists";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query,connection);
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int value))
                    isFound = value == 1 ? true : false;
            }
            catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool TryGetUserNameByUserID(int AppID, out string UserName)
        {
            UserName = null;
            bool isFound = false;

            string Query = @"select UserName From Users U
            Inner join Applications A on A.CreatedByUserID = U.UserID
            Where A.ApplicationID = @AppID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query,connection))
                {
                    command.Parameters.Add("@AppID", SqlDbType.Int).Value = AppID;

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if(result != null)
                    {
                        UserName = result.ToString();
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                UserName = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetUserNameByUserID]", "Info",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
    }
}
