using DVLD.DTOs;
using DVLD.DTOs.License;
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
    public static class clsLicenseClassesDAL
    {
        static public List <DTOComboBoxItem> GetLicenseClassNames()
        {
            List<DTOComboBoxItem> dtoClassNames = new List <DTOComboBoxItem> ();

            string Query = "Select LicenseClassID, ClassName From LicenseClasses";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dtoClassNames.Add(new DTOComboBoxItem
                    {
                        Value = reader.GetInt32(0),
                        Display = reader.GetString(1),
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                dtoClassNames = new List<DTOComboBoxItem>();
            }
            finally
            {
                connection.Close();
            }

            return dtoClassNames;
        }
        static public bool LicenseClassExistsByID(enLicenseClass ClassID)
        {
            bool isFound = false;

            string Query = @"Select
                            Case
                                When Exists (
                                Select 1 From LicenseClasses Where LicenseClassID = @LicenseClassID
                            )
                            Then 1
                            Else 0
                            End As ClassExists";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@LicenseClassID", System.Data.SqlDbType.Int).Value = (int) ClassID;

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
        public static bool? IsLicenseExpired(int ApplicationID)
        {
            bool? IsLicenseExpired = false;

            string Query = @"select Case 
	                            When Exists
	                            ( Select 1 From Licenses LS
	                            Where LS.ApplicationID = @ApplicationID
	                            AND Cast (LS.ExpirationDate AS Date) < Cast(GETDATE() AS Date)
	                            )Then 1
	                            Else 0
                            End As IsLicenseExpired;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", System.Data.SqlDbType.Int).Value = ApplicationID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        IsLicenseExpired = value == 1;

                }
            }
            catch (Exception ex)
            {
                IsLicenseExpired = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [IsLicenseExpired]",
             "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsLicenseExpired;
        }
        static public bool? IsFirstLicenseForDriverInClass(int DriverID, enLicenseClass LicenseClass)
        {
            bool? isExists = false;
            string Query = @"SELECT
                                CASE 
                                    WHEN EXISTS (
                                        SELECT 1 
                                        FROM Licenses L
                                        WHERE L.DriverID = @DriverID 
                                        AND L.LicenseClass = @LicenseClass
                                    )
                                    THEN 0
                                    ELSE 1
                                END AS IsFirstLicense;";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int) LicenseClass;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isExists = value == 1;
                }
            }
            catch (Exception ex)
            {
                isExists = null;
            }

            return isExists;
        }
        static public bool? IsLatestLicenseRenewable(int DriverID, enLicenseClass LicenseClass, int graceYears, int earlyDays)
        {
            bool? IsRenewable = false;
            string Query = @"select case
                        	When Exists
                        	(
                        		select 1 From Licenses L 
                        		Where L.LicenseID = (
                        			Select Top 1 L2.LicenseID
                        			From Licenses L2
                        			Where L2.DriverID = @DriverID And L2.LicenseClass = @LicenseClass
                        			Order by L2.IssueDate Desc
                        		)
                        		AND (
                        		(L.ExpirationDate < GETDATE() AND L.ExpirationDate >= DATEADD(YEAR, -@graceYears, GETDATE()))
                        		OR (L.ExpirationDate > GETDATE() AND L.ExpirationDate <= DATEADD(Day, @earlyDays, GETDATE()))
                        		)
                        	)Then 1 
                        	Else 0
                        	End IsRenewable;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        IsRenewable = value == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                IsRenewable = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[IsLatestLicenseRenewable]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsRenewable;
        }

        static public bool TryGetLicenseClassNameByID(enLicenseClass LicenseClassID, out string LicenseClassName)
        {
            LicenseClassName = null;
            bool isFound = false;

            string Query = @"Select ClassName From LicenseClasses
            Where LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseClassID", System.Data.SqlDbType.Int).Value = (int) LicenseClassID;



                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        LicenseClassName = result.ToString();
                        isFound = true;
                    }
                }
            }
            catch(Exception ex)
            {   
                isFound = false;
                LicenseClassName = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [GetLicenseClassNameByID]", "Erro", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                return isFound;
            }

            return isFound;
        }
        static public bool? TryGetLicenseClassFeesByID(enLicenseClass licenseClassID, out decimal? Fees)
        {
            Fees = -1m;
            bool? isFound = false;

            string Query = @"select LC.ClassFees 
                            From LicenseClasses LC
                            Where LC.LicenseClassID = @licenseClassID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@licenseClassID", SqlDbType.Int).Value = licenseClassID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal value))
                    {
                        isFound = true;
                        Fees = value;
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Fees = null;
            }

            return isFound;
        }
        public static bool? TryGetLicenseValidity(enLicenseClass LicenseClassID, out int? LicenseValidityLength)
        {
            bool? isFound = false;
            LicenseValidityLength = null;

            string Query = @"Select DefaultValidityLength From LicenseClasses LC
                            Where LC.LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseClassID", System.Data.SqlDbType.Int).Value = (int)LicenseClassID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        LicenseValidityLength = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                LicenseValidityLength = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [IsLicenseExpired]",
                "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        private static bool TryBuild_LicenseClass(SqlDataReader reader, out DTOLicenseClass dto)
        {
            dto = null;

            if (reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseClassID", out int LicenseClassID))
                return false;

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "ClassName", out string ClassName))
                return false;

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "ClassDescription", out string ClassDescription))
                return false;

            if (!clsMapReader.TryGetByteFromReaderImediately(reader, "MinimumAllowedAge", out byte MinimumAllowedAge))
                return false;

            if (!clsMapReader.TryGetByteFromReaderImediately(reader, "DefaultValidityLength", out byte DefaultValidityLength))
                return false;

            if (!clsMapReader.TryGetDecimalFromReaderImediately(reader, "ClassFees", out decimal ClassFees))
                return false;

            dto = new DTOLicenseClass
            {
                LicenseClassID = LicenseClassID,
                ClassName = ClassName,
                ClassDescription = ClassDescription,
                MinimumAllowedAge = MinimumAllowedAge,
                DefaultValidityLength = DefaultValidityLength,
                ClassFees = ClassFees
            };

            return true;
        }

        public static bool? FindLicenseClassByID(enLicenseClass LicenseClass, out DTOLicenseClass dto)
        {
            bool? isFound = false;
            dto = null;
            string Query = @"select * From LicenseClasses LC
                            Where LC.LicenseClassID = @LicenseClass;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_LicenseClass(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[FindLicenseClassByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? GetDefaultValidityLengthByID(enLicenseClass LicenseClass, out byte? DefaultValidityLength)
        {
            bool? isFound = false;
            DefaultValidityLength = null;

            string Query = @"select LC.DefaultValidityLength From LicenseClasses LC
                    Where LC.LicenseClassID = @LicenseClass;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int) LicenseClass;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && byte.TryParse(result.ToString(), out byte value))
                    {
                        DefaultValidityLength = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DefaultValidityLength = null;
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[GetDefaultValidityLengthByID]",
                "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isFound;
        }
    }
}
