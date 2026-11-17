using DVLD.DTOs.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.License
{
    public static class clsDriver_DAL
    {
        public static bool? isPersonDriver(int PersonID)
        {
            bool? isDriver = false;

            string Query = @"Select Case
	                            When Exists
	                                (Select 1 From Drivers D
	                                Where D.PersonID = @PersonID
	                                ) Then 1
	                            else 0
	                            End As isPersonDriver;";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = PersonID;

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isDriver = value == 1;
                }
            }
            catch (Exception ex)
            {
                isDriver = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[isHeDriver].",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isDriver;
        }
        public static bool? TryGetDriverIDByPersonID(int PersonID, out int? DriverID)
        {
            bool? isFound = false;
            DriverID = null;

            string Query = @"Select D.DriverID From Drivers D
                            Where D.PersonID = @PersonID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = PersonID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        DriverID = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DriverID = null;
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetDriverIDByPersonID]",
                "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        private static bool TryBuild_Driver(SqlDataReader reader, out DTODriver dtoDriver)
        {
            dtoDriver = null;

            if (reader == null)
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int PersonID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "CreatedByUserID", out int CreatedByUserID))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "CreatedDate", out DateTime CreatedDate))
                return false;

            dtoDriver = new DTODriver
            {
                DriverID = DriverID,
                PersonID = PersonID,
                CreatedByUserID = CreatedByUserID,
                CreatedDate = CreatedDate,
            };

            return true;
        }
        public static bool FindDriverByPersonID(int PersonID, out DTODriver dtoDriver)
        {
            bool isFound = false;
            dtoDriver = null;

            string Query = @"Select * From Drivers Where DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = PersonID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_Driver(reader, out dtoDriver))
                        {
                            if (dtoDriver != null)
                                isFound = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[FindDriverByPersonID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? TryGetPersonIDByDriverID(int DriverID, out int? PersonID)
        {
            bool? isFound = false;
            PersonID = null;

            string Query = @"select DR.PersonID from Drivers DR 
                                where DR.DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = DriverID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        PersonID = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                PersonID = null;
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetPersonIDByDriverID]",
                "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }


        //public static decimal? TryGetLicenseClassByID(int ClassID)
        //{
        //    bool isFound = false;
        //    string Query = @"Select LC.ClassFees From LicenseClasses LC
        //                Where LC.LicenseClassID = @LicenseClassID";


        //}
    }
}
