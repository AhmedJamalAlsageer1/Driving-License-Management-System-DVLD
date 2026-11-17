using DVLD.DTOs.Presention_DTO.Police_Actions.Detain_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.PoliceActions
{
    public static class cls_ShowDetainedLicenses_DAL
    {
        private static bool TryBuild_ShowDetainedLicenses(SqlDataReader reader, out DTO_ShowDetainedLicenses dto)
        {
            dto = null;

            if (reader == null)
                return false;


            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "DetainID", out int DetainID))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DetainDate", out DateTime DetainDate))
                return false;

            if(!clsMapReader.TryGetDecimalFromReaderImediately(reader, "FineFees", out decimal FineFees))
                return false;

            bool? IsReleased = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsReleased");
            if (!IsReleased.HasValue)
                return false;

            DateTime? ReleaseDate = clsMapReader.TryGetNULL_DateTime(reader, "ReleaseDate");

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out string NationalNo))
                return false;

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string FullName))
                return false;

            int? ReleaseApplicationID = clsMapReader.TryGetNULL_Int(reader, "ReleaseApplicationID");


            dto = new DTO_ShowDetainedLicenses
            {
                LicenseID = LicenseID,
                DetainID = DetainID,
                DetainDate = DetainDate,
                FineFees = FineFees,
                IsReleased = IsReleased.Value,
                ReleaseDate = ReleaseDate,
                NationalNo = NationalNo,
                FullName = FullName,
                ReleaseApplicationID = ReleaseApplicationID
            };

            return true;
        }
        public static bool? ShowDetainedLicenseBy_DetainID(int DetainID, out DTO_ShowDetainedLicenses dto)
        {
            dto = null;
            bool? isFound = false;

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
            Where V.DetainID = @DetainID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DetainID", System.Data.SqlDbType.Int).Value = DetainID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowDetainedLicenseByLicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicenses()
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicenses]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }

        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_LicenseID(int LicenseID)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();
            bool ? isFound = false;

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
            Where V.LicenseID = @LicenseID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", System.Data.SqlDbType.Int).Value = LicenseID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if(TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicensesBy_LicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }

        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_ReleaseApplicationID(int ReleaseApplicationID)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
                            Where V.ReleaseApplicationID = @ReleaseApplicationID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ReleaseApplicationID", System.Data.SqlDbType.Int).Value = ReleaseApplicationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicensesBy_LicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }


        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_IsReleased(bool isReleased)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();
            bool? isFound = false;


            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
                        Where V.IsReleased = @IsReleased;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@IsReleased", System.Data.SqlDbType.Bit).Value = isReleased;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicensesBy_IsReleased]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }

        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_FullName(string FirstName, 
            string SecondName, string ThirdName, string LastName)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();
            bool? isFound = false;

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
                            Where V.FirstName = @FirstName And V.SecondName = @SecondName
                            And (
                            		(V.ThirdName is Null And @ThirdName is null) 
                            			Or 
                            		(V.ThirdName is not Null And V.ThirdName = @ThirdName)
                            	) And 
                            V.LastName = @LastName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 20).Value = FirstName;
                    command.Parameters.Add("@SecondName", System.Data.SqlDbType.NVarChar, 20).Value = SecondName;
                    command.Parameters.Add("@ThirdName", System.Data.SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace( ThirdName)? (object) DBNull.Value : ThirdName;
                    command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 20).Value = LastName;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicensesBy_FullName]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }

        public static List<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_NationalNo(string NationalNo)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = new List<DTO_ShowDetainedLicenses>();
            bool? isFound = false;

            string Query = @"select * From vw_DetainedLicensesWithPersonInfo AS V
            Where V.NationalNo = @NationalNo;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@NationalNo", System.Data.SqlDbType.NVarChar, 20).Value = NationalNo;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_ShowDetainedLicenses dto;
                        if (TryBuild_ShowDetainedLicenses(reader, out dto))
                        {
                            if (dto != null)
                                dto_ShowAll.Add(dto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto_ShowAll = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ShowAllDetainedLicensesBy_NationalNo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return dto_ShowAll;
        }

    }
}
