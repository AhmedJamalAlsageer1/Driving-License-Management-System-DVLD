using DVLD.DTOs.Applications;
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
    public static class LDL_ApplicationsDisplayDAL
    {
        private static bool TryGet_LDLDisplayApplicationFromReader(SqlDataReader reader, out DTO_LDLApplicationsDisplay dtoApplicationsDisplay)
        {
            if (reader == null)
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out int valueint))
            {
                dtoApplicationsDisplay = new DTO_LDLApplicationsDisplay();
                dtoApplicationsDisplay.L_D_LApplicationID = valueint;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "ClassName", out string valueString))
            {
                dtoApplicationsDisplay.DrivingClass = valueString;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out valueString))
            {
                dtoApplicationsDisplay.NationalNo = valueString;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out valueString))
            {
                dtoApplicationsDisplay.FullName = valueString;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime dateTimeValue))
            {
                dtoApplicationsDisplay.ApplicationDate = dateTimeValue;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PassedTestsCount", out valueint))
            {
                dtoApplicationsDisplay.PassedTests = (byte)valueint;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "Status", out valueString))
            {
                dtoApplicationsDisplay.Status = valueString;
            }
            else
            {
                dtoApplicationsDisplay = null;
                return false;
            }


            return true;
        }
        public static List<DTO_LDLApplicationsDisplay> GetAll_LDLApplicationsDAL()
        {
            List<DTO_LDLApplicationsDisplay> Applications = new List<DTO_LDLApplicationsDisplay>();

            string Query = @"select * From vm_LDLApplicationsStatus";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_LDLApplicationsDisplay dto;
                    if (TryGet_LDLDisplayApplicationFromReader(reader, out dto))
                        Applications.Add(dto);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في 'GetAllApplicationsDAL' أي عند جلب بيانات طلبات شخص معين", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                Applications = null;
            }
            finally
            {
                connection.Close();
            }

            return Applications;
        }
        public static List<DTO_LDLApplicationsDisplay> GetAll_LDLApplicationsByStatus(enApplicationStatus status)
        {
            List<DTO_LDLApplicationsDisplay> Applications = new List<DTO_LDLApplicationsDisplay>();

            string Query = @"select * From vm_LDLApplicationsStatus
                            where Status = @Status";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = status.ToString();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_LDLApplicationsDisplay dto;
                    if (TryGet_LDLDisplayApplicationFromReader(reader, out dto))
                        Applications.Add(dto);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Applications = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في 'GetAllApplicationByID' أي عند جلب بيانات طلبات شخص معين", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }
            finally
            {
                connection.Close();
            }

            return Applications;
        }
        public static List<DTO_LDLApplicationsDisplay> FindLDLApplicationByFullName(string FullName)
        {
            List<DTO_LDLApplicationsDisplay> Applications = new List<DTO_LDLApplicationsDisplay>();

            string Query = @"select * From vm_LDLApplicationsStatus
                            where FullName = @FullName";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = FullName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_LDLApplicationsDisplay dto;
                    if (TryGet_LDLDisplayApplicationFromReader(reader, out dto))
                        Applications.Add(dto);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Applications = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في 'GetAllApplicationByID' أي عند جلب بيانات طلبات شخص معين", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }
            finally
            {
                connection.Close();
            }

            return Applications;
        }
        public static List<DTO_LDLApplicationsDisplay> FindLDLApplicationByNationalNo(string NationalNo)
        {
            List<DTO_LDLApplicationsDisplay> Applications = new List<DTO_LDLApplicationsDisplay>();

            string Query = @"select * From vm_LDLApplicationsStatus
                            where NationalNo = @NationalNo";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = NationalNo;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_LDLApplicationsDisplay dto;
                    if (TryGet_LDLDisplayApplicationFromReader(reader, out dto))
                        Applications.Add(dto);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Applications = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في 'GetAllApplicationByID' أي عند جلب بيانات طلبات شخص معين", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }
            finally
            {
                connection.Close();
            }

            return Applications;
        }
        public static bool FindLDLApplicationBy_LDLAppID(int LDL_AppID, out DTO_LDLApplicationsDisplay application)
        {
            bool isFound = false;
            application = null;

            string Query = @"select * From vm_LDLApplicationsStatus
                            where LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDL_AppID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryGet_LDLDisplayApplicationFromReader(reader, out application))
                    {
                        isFound = application != null;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
                application = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في 'GetDispalyApplicationByAppID' أي عند جلب  طلب معين", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                application.Clear();
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static bool TryGetFullLDL_App(SqlDataReader reader, out DTO_LDLFull_Info dto)
        {
            dto = null;
            if (reader == null)
                return false;

            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "LDL_AppID", out int valueint))
            {
                dto = new DTO_LDLFull_Info();
                dto.LDL_AppID = valueint;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "AppID", out valueint))
            {
                dto.AppID = valueint;
            }
            else
            {
                dto = null;
                return false;
            }

            if(clsMapReader.TryGetStringFromReaderImediately(reader, "Applicant", out string ValueString))
            {
                dto.Applicant = ValueString;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "LicenseDrivingName", out ValueString))
            {
                dto.LicenseDrivingName = ValueString;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PassedTestsCount", out valueint))
            {
                dto.PassedTest = (byte) valueint;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "ApplicationStatus", out ValueString))
            {
                dto.StatusApplication = ValueString;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "ApplicationType", out ValueString))
            {
                dto.ApplicationType = ValueString;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime TimeValue))
            {
                dto.ApplicationDate = TimeValue;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "LastStatusDate", out TimeValue))
            {
                dto.StatusDate = TimeValue;
            }
            else
            {
                dto = null;
                return false;
            }

            if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal  ValueDecimal))
            {
                dto.Fees = ValueDecimal;
            }
            else
            {
                dto = null;
                return false;
            }

            if (clsMapReader.TryGetStringFromReaderImediately(reader, "CreatedBy", out ValueString))
            {
                dto.CreatedBy = ValueString;
            }
            else
            {
                dto = null;
                return false;
            }

            return true;
        }
        public static bool Find_FullLDLbyAppID(int LDLAppID, out DTO_LDLFull_Info dto)
        {
            dto = null;
            bool isFound = false;

            string Query = @"Select * From vw_LDLApplicationFullInfo
            Where LDL_AppID = @LDL_AppID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDLAppID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if(reader.Read())
                    {
                        if(TryGetFullLDL_App(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                dto = null;
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [Find_FullLDLbyAppID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

    }
}
