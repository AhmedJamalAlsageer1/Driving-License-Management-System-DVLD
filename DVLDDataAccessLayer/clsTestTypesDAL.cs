using DVLD.DTOs;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public static class clsTestTypesDAL
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = null;

            string Query = "Select * From TestTypes";
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
                dt = null;
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        private static bool TryToGetDataFromReader(SqlDataReader reader, out DTOTestTypes dtoTestType)
        {
            dtoTestType = null;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "TestTypeID", out int valueint))
            {
                dtoTestType = new DTOTestTypes();
                dtoTestType.ID = valueint;
            }
            else
                return false;

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "TestTypeTitle", out string valueString))
            {
                dtoTestType.Title = valueString;
                valueString = string.Empty;
            }
            else
            {
                dtoTestType = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "TestTypeDescription", out valueString))
                dtoTestType.Description = valueString;
            else
            {
                dtoTestType = null;
                return false;
            }

            if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "TestTypeFees", out decimal valueDecimal))
                dtoTestType.Fees = valueDecimal;
            else
            {
                dtoTestType = null;
                return false;
            }

            return true;
        }

        public static bool GetTypeTestByID(int ID, out DTOTestTypes dtoTest)
        {
            bool isFound = false;
            dtoTest = null;

            string Query = "Select * From TestTypes Where TestTypeID = @TestTypeID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = ID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryToGetDataFromReader(reader, out dtoTest))
                    {
                        if(dtoTest != null)
                        {
                            isFound = true;
                        }
                    }
                }
                reader.Read();
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

        public static bool UpdateData(int ID, DTOTestTypes dtoTest)
        {
            int rowAffected = 0;

            string Query = @"Update TestTypes
                             set TestTypeTitle = @TestTypeTitle,
                                 TestTypeDescription = @TestTypeDescription,
                                 TestTypeFees = @TestTypeFees
                              Where TestTypeID = @TestTypeID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = dtoTest.ID;
            command.Parameters.Add("@TestTypeTitle", SqlDbType.NVarChar, 100).Value = dtoTest.Title;
            command.Parameters.Add("@TestTypeDescription", SqlDbType.NVarChar, 500).Value = dtoTest.Description;
            command.Parameters.Add("@TestTypeFees", SqlDbType.Decimal).Value = dtoTest.Fees;

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }

        public static bool isTestTypeExists(enTestType testtypeID)
        {
            bool isExists = false;

            string Query = @"Select Case
                                When Exists(
                                select 1 From TestTypes
                                Where TestTypeID = @TestTypeID
                                )Then 1
                                Else 0
                                End As TestExists";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query,connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = (int) testtypeID;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isExists = value == 1;
                }
            }
            catch(Exception ex)
            {
                isExists = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[isTestTypeExists]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isExists;
        }

        public static bool IsTestTypeFeesValid(enTestType testTypeID, decimal fees)
        {
            bool isValid = false;

            string Query = @"Select Case
                                When Exists(
                                    select 1 From TestTypes
                                    Where TestTypeID = @TestTypeID
                                    AND TestTypeFees = @fees
                                    )Then 1
                                    Else 0
                                End As TestExists";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeID;
                    command.Parameters.Add("@fees", SqlDbType.SmallMoney).Value = fees;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isValid = value == 1;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة [IsTestTypeFeesValid]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isValid;
        }

        public static bool TryGetTestFeesByID(enTestType TestID, out decimal Fees)
        {
            bool isFound = false;
            Fees = -1;

            string Query = @"select TestTypeFees From TestTypes
                            Where TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand( Query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestID;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal value))
                    {
                        Fees = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                Fees = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة[TryGetTestFeesByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
    }
}
