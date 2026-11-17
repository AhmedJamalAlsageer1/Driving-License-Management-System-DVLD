using DVLD.DTOs;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public static  class clsApplicationTypesDAL
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = null;

            string Query = "Select * From ApplicationTypes";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);
            
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch(Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "فشل الاتصال بقاعدة البيانات", "Error", ex, "DAL",
                    null);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
        private static bool TryConvertToDTOTypeServices(SqlDataReader reader, out DTOApplicationTypes dtoType)
        {
            dtoType = null;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationTypeID", out int valueint))
            {
                dtoType = new DTOApplicationTypes();
                dtoType.ID = valueint;
            }
            else
            {
                dtoType = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "ApplicationTypeTitle", out string valuestring))
            {
                dtoType.Title = valuestring;
            }
            else
            {
                dtoType = null;
                return false;
            }

            if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal valuedecimal))
            {
                dtoType.fees = valuedecimal;
            }
            else
            {
                dtoType = null;
                return false;
            }

            return true;
        }
        public static bool GetAppliationTypeByID(int id, out DTOApplicationTypes dtoType)
        {
            bool isFound = false;
            dtoType = null;

            string Query = "Select * From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command= new SqlCommand (Query, connection);
            command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = id;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    if(TryConvertToDTOTypeServices(reader, out dtoType))
                    {
                        if (dtoType != null)
                            isFound = true;
                        else
                            dtoType = null;
                    }
                    else
                    {
                        dtoType = null;
                        isFound = false;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                dtoType = null;
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool UpdateApplicationType(DTOApplicationTypes dtoUser)
        {
            if (dtoUser == null) return false;
            int rowsAffected = 0;

            string Query = @"Update ApplicationTypes set 
                            ApplicationTypeTitle = @ApplicationTypeTitle, 
                            ApplicationFees = @ApplicationFees
                            Where ApplicationTypeID = @ApplicationTypeID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand (Query, connection);
            command.Parameters.Add("@ApplicationTypeTitle", SqlDbType.NVarChar, 150).Value = dtoUser.Title;
            command.Parameters.Add("@ApplicationFees", SqlDbType.SmallMoney).Value = dtoUser.fees;
            command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = dtoUser.ID;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static bool IsApplicationTypeExists(enApplicationTypes id)
        {
            bool isFound = false;
            string Query = @"Select IIF (Exists (
                                        Select 1 From ApplicationTypes Where ApplicationTypeID = @ID
                                        ), 1, 0)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        isFound = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "دالة الخطأ [IsApplicationTypeExists]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
        public static bool IsApplicationFeesValid(SharedLayer.enums.enApplicationTypes ApplicationTypeID, decimal ApplicationFees)
        {
            bool isValid = false;
            string Query = @"Select case
                                When Exists(
                                    select 1 From ApplicationTypes
                                    Where ApplicationTypeID = @ApplicationTypeID
                                    AND ApplicationFees = @ApplicationFees
                                    )Then 1
                                    Else 0
                                    End As ApplicationFeesValid";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;
                    command.Parameters.Add("@ApplicationFees", SqlDbType.SmallMoney).Value = ApplicationFees;

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isValid = value == 1;
                }
            }
            catch(Exception ex)
            {
                isValid = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة [IsApplicationFeeValid]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isValid;
        }
        public static bool TryGetApplicationFees(SharedLayer.enums.enApplicationTypes ApplicationTypeID, out decimal ApplicationFees)
        {
            bool isFound = false;
            ApplicationFees = -1;

            string Query = @"Select ApplicationFees From ApplicationTypes
                        Where ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal value))
                    {
                        ApplicationFees = value;
                        isFound = true;
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = false;
                ApplicationFees = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة[TryGetApplicationFees]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
    }
}
