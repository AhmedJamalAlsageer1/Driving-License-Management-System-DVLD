using DVLD.DTOs.Drivers;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.Driver
{
    public static class clsDriverInfo_DAL
    {
        private static bool TryBuild_DriverInfo(SqlDataReader reader, out DTODrivers_Info dto)
        {
            dto = null;

            if (reader == null)
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int PersonID))
                return false;

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string FullName))
                return false;

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out string NationalNo))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "CreatedDate", out DateTime CreatedDate))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "ActiveLicenses", out int ActiveLicenses))
                return false;


            dto = new DTODrivers_Info()
            {
                DriverID = DriverID,
                PersonID = PersonID,
                FullName = FullName,
                NationalNo = NationalNo,
                CreatedDate = CreatedDate,
                ActiveLicenses = ActiveLicenses
            };
            return true;
        }

        public static List<DTODrivers_Info> GetAllDriversInfo()
        {
            List < DTODrivers_Info> drivers_Info = new List<DTODrivers_Info> ();

            string Query = @"Select DR.DriverID, DR.PersonID,
                                P.NationalNo, 
                                	 P.FirstName + ' ' + 
                                    P.SecondName + ' ' + 
                                    CASE 
                                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN '' 
                                        ELSE P.ThirdName + ' '
                                    END +
                                    P.LastName AS FullName,
                                DR.CreatedDate, 
                                LocalCount.ActiveLocalLicenses + InterCount.ActiveInternationalLicenses AS ActiveLicenses
                                 From Drivers DR
                                Inner Join People P
                                	On P.PersonID = DR.PersonID
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveLocalLicenses
                                	From Licenses L 
                                	Where DR.DriverID = L.DriverID AND L.IsActive = 1
                                )AS LocalCount
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveInternationalLicenses
                                	From InternationalLicenses IL 
                                	Where DR.DriverID = IL.DriverID AND IL.IsActive = 1
                                )AS InterCount";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        DTODrivers_Info dto;
                        if(TryBuild_DriverInfo(reader, out dto))
                        {
                            if (dto != null)
                                drivers_Info.Add(dto);
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                drivers_Info = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في تنفيذ هذه الدالة [GetAllDriversInfo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return drivers_Info;
        }

        public static bool? GetDriverInfo_ByDriverID(int DriverID, out DTODrivers_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"Select DR.DriverID, DR.PersonID,
                                P.NationalNo, 
                                	 P.FirstName + ' ' + 
                                    P.SecondName + ' ' + 
                                    CASE 
                                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN '' 
                                        ELSE P.ThirdName + ' '
                                    END +
                                    P.LastName AS FullName,
                                DR.CreatedDate, 
                                LocalCount.ActiveLocalLicenses + InterCount.ActiveInternationalLicenses AS ActiveLicenses
                                 From Drivers DR
                                Inner Join People P
                                	On P.PersonID = DR.PersonID
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveLocalLicenses
                                	From Licenses L 
                                	Where DR.DriverID = L.DriverID AND L.IsActive = 1
                                )AS LocalCount
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveInternationalLicenses
                                	From InternationalLicenses IL 
                                	Where DR.DriverID = IL.DriverID AND IL.IsActive = 1
                                )AS InterCount
								Where DR.DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = DriverID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_DriverInfo(reader, out dto))
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
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في تنفيذ هذه الدالة [GetDriverInfo_ByDriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }

        public static bool? GetDriverInfo_ByPersonID(int PersonID, out DTODrivers_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"Select DR.DriverID, DR.PersonID,
                                P.NationalNo, 
                                	 P.FirstName + ' ' + 
                                    P.SecondName + ' ' + 
                                    CASE 
                                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN '' 
                                        ELSE P.ThirdName + ' '
                                    END +
                                    P.LastName AS FullName,
                                DR.CreatedDate, 
                                LocalCount.ActiveLocalLicenses + InterCount.ActiveInternationalLicenses AS ActiveLicenses
                                 From Drivers DR
                                Inner Join People P
                                	On P.PersonID = DR.PersonID
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveLocalLicenses
                                	From Licenses L 
                                	Where DR.DriverID = L.DriverID AND L.IsActive = 1
                                )AS LocalCount
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveInternationalLicenses
                                	From InternationalLicenses IL 
                                	Where DR.DriverID = IL.DriverID AND IL.IsActive = 1
                                )AS InterCount
								Where DR.PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = PersonID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_DriverInfo(reader, out dto))
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
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في تنفيذ هذه الدالة [GetDriverInfo_ByPersonID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }
        public static bool? GetDriverInfo_NationalNo(string NationalNo, out DTODrivers_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"Select DR.DriverID, DR.PersonID,
                                P.NationalNo, 
                                	 P.FirstName + ' ' + 
                                    P.SecondName + ' ' + 
                                    CASE 
                                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN '' 
                                        ELSE P.ThirdName + ' '
                                    END +
                                    P.LastName AS FullName,
                                DR.CreatedDate, 
                                LocalCount.ActiveLocalLicenses + InterCount.ActiveInternationalLicenses AS ActiveLicenses
                                 From Drivers DR
                                Inner Join People P
                                	On P.PersonID = DR.PersonID
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveLocalLicenses
                                	From Licenses L 
                                	Where DR.DriverID = L.DriverID AND L.IsActive = 1
                                )AS LocalCount
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveInternationalLicenses
                                	From InternationalLicenses IL 
                                	Where DR.DriverID = IL.DriverID AND IL.IsActive = 1
                                )AS InterCount
								Where P.NationalNo = @NationalNo";

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
                        if (TryBuild_DriverInfo(reader, out dto))
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
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في تنفيذ هذه الدالة [GetDriverInfo_NationalNo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }

        public static bool? GetDriverInfo_FullName(string FirstName, string SecondName, string ThirdName,
            string LastName, out DTODrivers_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"Select DR.DriverID, DR.PersonID,
                                P.NationalNo, 
                                	 P.FirstName + ' ' + 
                                    P.SecondName + ' ' + 
                                    CASE 
                                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN '' 
                                        ELSE P.ThirdName + ' '
                                    END +
                                    P.LastName AS FullName,
                                DR.CreatedDate, 
                                LocalCount.ActiveLocalLicenses + InterCount.ActiveInternationalLicenses AS ActiveLicenses
                                 From Drivers DR
                                Inner Join People P
                                	On P.PersonID = DR.PersonID
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveLocalLicenses
                                	From Licenses L 
                                	Where DR.DriverID = L.DriverID AND L.IsActive = 1
                                )AS LocalCount
                                
                                Cross Apply (
                                	Select Count(*) AS ActiveInternationalLicenses
                                	From InternationalLicenses IL 
                                	Where DR.DriverID = IL.DriverID AND IL.IsActive = 1
                                )AS InterCount
								Where P.FirstName = @FirstName AND
								    P.SecondName = @SecondName AND
								    (
								        (@ThirdName is Null AND P.ThirdName is Null) OR
										(@ThirdName is Not NULL AND P.ThirdName = @ThirdName)
									)AND
									P.LastName = @LastName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 20).Value = FirstName;
                    command.Parameters.Add("@SecondName", System.Data.SqlDbType.NVarChar, 20).Value = SecondName;
                    command.Parameters.Add("@ThirdName", System.Data.SqlDbType.NVarChar, 20).Value = ThirdName;
                    command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 20).Value = LastName;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (TryBuild_DriverInfo(reader, out dto))
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
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في تنفيذ هذه الدالة [GetDriverInfo_NationalNo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }


    }
}
