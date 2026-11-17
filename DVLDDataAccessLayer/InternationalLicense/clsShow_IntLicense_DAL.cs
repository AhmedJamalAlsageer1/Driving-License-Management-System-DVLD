using DVLD.DTOs.International_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.InternationalLicense
{
    public static class clsShow_IntLicense_DAL
    {
        public static bool TryBuild_IntLicense(SqlDataReader reader, out DTO_ShowIntLicenses dto)
        {
            dto = null;
            if (reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "Int_LicenseID", out int Int_LicenseID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "L_LicenseID", out int L_LicenseID))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if (!IsActive.HasValue)
                return false;

            dto = new DTO_ShowIntLicenses
            {
                Int_LicenseID = Int_LicenseID,
                ApplicationID = ApplicationID,
                DriverID = DriverID,
                L_LicenseID = L_LicenseID,
                ExpirationDate = ExpirationDate,
                IssueDate = IssueDate,
                IsActive = IsActive.Value,
            };

            return true;
        }

        public static List<DTO_ShowIntLicenses> FindAll_IntLicenses()
        {
            List<DTO_ShowIntLicenses> DTO_IntLicenses = new List<DTO_ShowIntLicenses>();

            string Query = @"select IL.InternationalLicenseID AS Int_LicenseID, 
                                IL.ApplicationID, IL.DriverID,
                                IL.IssuedUsingLocalLicenseID AS L_LicenseID,
                                IL.ExpirationDate, IL.IssueDate, IL.IsActive
                                From InternationalLicenses IL;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        DTO_ShowIntLicenses dto;
                        if (TryBuild_IntLicense(reader, out dto))
                        {
                            if(dto != null)
                            {
                                DTO_IntLicenses.Add(dto);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DTO_IntLicenses = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindAll_IntLicenses]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return DTO_IntLicenses;
        }

        public static bool? Find_IntLicensesByInt_LicenseID(int Int_LicenseID, out DTO_ShowIntLicenses dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select IL.InternationalLicenseID AS Int_LicenseID, 
                                IL.ApplicationID, IL.DriverID,
                                IL.IssuedUsingLocalLicenseID AS L_LicenseID,
                                IL.ExpirationDate, IL.IssueDate, IL.IsActive
                                From InternationalLicenses IL
                                Where InternationalLicenseID = @Int_LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@Int_LicenseID", System.Data.SqlDbType.Int).Value = Int_LicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_IntLicense(reader, out dto))
                        {
                            if (dto != null)
                            {
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [Find_IntLicensesByInt_LicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? Find_IntLicensesByL_LicenseID(int L_LicenseID, out DTO_ShowIntLicenses dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select IL.InternationalLicenseID AS Int_LicenseID, 
                                IL.ApplicationID, IL.DriverID,
                                IL.IssuedUsingLocalLicenseID AS L_LicenseID,
                                IL.ExpirationDate, IL.IssueDate, IL.IsActive
                                From InternationalLicenses IL
                                Where IL.IssuedUsingLocalLicenseID = @L_LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@L_LicenseID", System.Data.SqlDbType.Int).Value = L_LicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_IntLicense(reader, out dto))
                        {
                            if (dto != null)
                            {
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [Find_IntLicensesByL_LicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? Find_IntLicensesByApplicationID(int ApplicationID, out DTO_ShowIntLicenses dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select IL.InternationalLicenseID AS Int_LicenseID, 
                                IL.ApplicationID, IL.DriverID,
                                IL.IssuedUsingLocalLicenseID AS L_LicenseID,
                                IL.ExpirationDate, IL.IssueDate, IL.IsActive
                                From InternationalLicenses IL
                                Where IL.ApplicationID = @ApplicationID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", System.Data.SqlDbType.Int).Value = ApplicationID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_IntLicense(reader, out dto))
                        {
                            if (dto != null)
                            {
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [Find_IntLicensesByApplicationID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static List<DTO_ShowIntLicenses> Find_ALLIntLicensesBy_DriverID(int DriverID)
        {
            List<DTO_ShowIntLicenses> IntLicenses = new List<DTO_ShowIntLicenses>();

            string Query = @"select IL.InternationalLicenseID AS Int_LicenseID, 
                                IL.ApplicationID, IL.DriverID,
                                IL.IssuedUsingLocalLicenseID AS L_LicenseID,
                                IL.ExpirationDate, IL.IssueDate, IL.IsActive
                                From InternationalLicenses IL
                                Where IL.DriverID = @DriverID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = DriverID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowIntLicenses dto;
                        if (TryBuild_IntLicense(reader, out dto))
                        {
                            if (dto != null)
                            {
                                IntLicenses.Add(dto);
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                IntLicenses = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [Find_ALLIntLicensesBy_DriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IntLicenses;
        }


    }
}
