using DVLD.DTOs.License;
using DVLD.DTOs.License.Int_License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.InternationalLicense
{
    public static class clsInternationalLicense_DAL
    {
        public static bool? isFirstTime(int DriverID, enLicenseClass licenseClass)
        {
            bool? isFirstTime = false;

            string Query = @"SELECT CASE
                                WHEN EXISTS (
                                    SELECT 1
                                    FROM InternationalLicenses IL
                                    INNER JOIN Licenses L ON L.LicenseID = IL.IssuedUsingLocalLicenseID
                                    WHERE IL.DriverID = @DriverID
                                      AND L.LicenseClass = @LicenseClass
                                ) THEN 0
                                ELSE 1
                            END AS IsFirstTimeInternational;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", System.Data.SqlDbType.Int).Value = (int)licenseClass;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        isFirstTime = value == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                isFirstTime = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [isFirstTime]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFirstTime;
        }

        public static bool? HasValidInternationalLicense(int LicenseID, int DriverID,
        enLicenseClass licenseClass)
        {
            bool? isValid = null;
            string Query = @"SELECT CASE 
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM InternationalLicenses IL
                                    INNER JOIN Licenses LL ON LL.LicenseID = IL.IssuedUsingLocalLicenseID
                                    WHERE IL.DriverID = @DriverID
                                      AND LL.LicenseClass = @LicenseClass
                                      AND IL.IsActive = 1
                                ) THEN 1
                                ELSE 0
                            END AS ValidInternationalLicense;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", System.Data.SqlDbType.Int).Value = (int)licenseClass;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        isValid = (value == 1);
                    }

                }
            }
            catch (Exception ex)
            {
                isValid = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [HasValidInternationalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isValid;
        }

        public static bool? TryGetNewInternationalLicenseDefaults(int LicenseID, enApplicationTypes ApplicationType, out byte DefaultValidityLength, 
            out decimal ApplicationFees)
        {
            DefaultValidityLength = 0;
            ApplicationFees = -1;
            bool? isFound = false;

            const string Query = @"select LC.DefaultValidityLength,
                            ApT.ApplicationFees
                            From Licenses L
                            Inner Join LicenseClasses LC On L.LicenseClass = Lc.LicenseClassID
                            Cross Join ApplicationTypes ApT
                            WHERE L.LicenseID = @LicenseID
                            	AND ApT.ApplicationTypeID = @ApplicationType;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", System.Data.SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@ApplicationType", System.Data.SqlDbType.Int).Value = (int) ApplicationType;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetByteFromReaderImediately(reader, "DefaultValidityLength", out byte _DefaultValidityLength))
                        {
                            if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal _ApplicationFees))
                            {
                                DefaultValidityLength = _DefaultValidityLength;
                                ApplicationFees = _ApplicationFees;
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DefaultValidityLength = 0;
                ApplicationFees = -1;
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [GetNewLicenseDefault]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? AddNewInternationalLicense(DTO_IntLicense InternationalLicense, byte ValidityLength)
        {
            bool? isAdded = false;

            string Query = @"BEGIN TRY
                                BEGIN TRANSACTION;
                            
                                DECLARE @Now DATETIME = GETDATE();
                            
                                -- 1. إدراج سجل في جدول الطلبات
                                INSERT INTO Applications (
                                    ApplicantPersonID,
                                    ApplicationDate,
                                    ApplicationTypeID,
                                    ApplicationStatus,
                                    LastStatusDate,
                                    PaidFees,
                                    CreatedByUserID
                                )
                                VALUES (
                                    @ApplicantPersonID,
                                    @Now,
                                    @ApplicationTypeID,
                                    1, -- حالة جديدة
                                    @Now,
                                    @PaidFees,
                                    @CreatedByUserID_App
                                );
                            
                                DECLARE @NewApplicationID INT = SCOPE_IDENTITY();
                            
                                -- 2. حساب تاريخ الانتهاء
                                DECLARE @ExpirationDate DATETIME = DATEADD(YEAR, @ValidityLength, @Now);
                            
                                -- 3. إدراج رخصة دولية جديدة
                                INSERT INTO InternationalLicenses (
                                    ApplicationID,
                                    DriverID,
                                    IssuedUsingLocalLicenseID,
                                    IssueDate,
                                    ExpirationDate,
                                    IsActive,
                                    CreatedByUserID
                                )
                                VALUES (
                                    @NewApplicationID,
                                    @DriverID,
                                    @LocalLicenseID,
                                    @Now,
                                    @ExpirationDate,
                                    1,
                                    @CreatedByUserID_I_License
                                );
                            
                                DECLARE @NewInternationalLicenseID INT = SCOPE_IDENTITY();
                            
                                COMMIT TRANSACTION;
                            
                                -- 4. إرجاع القيم المطلوبة
                                SELECT
                                    @NewApplicationID AS NewApplicationID,
                                    @NewInternationalLicenseID AS NewInternationalLicenseID,
                                    @ExpirationDate AS ExpirationDate, 
                                    @Now AS ApplicationDate;
                            END TRY
                            BEGIN CATCH
                                ROLLBACK TRANSACTION;
                                THROW;
                            END CATCH;
                            ";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    //Application
                    command.Parameters.Add("@ApplicantPersonID", System.Data.SqlDbType.Int).Value = InternationalLicense.PersonID;
                    command.Parameters.Add("@ApplicationTypeID", System.Data.SqlDbType.Int).Value = (int) InternationalLicense.ApplicationType;
                    command.Parameters.Add("@PaidFees", System.Data.SqlDbType.SmallMoney).Value = InternationalLicense.AppFees;
                    command.Parameters.Add("@CreatedByUserID_App", System.Data.SqlDbType.Int).Value = InternationalLicense.CreatedBy;

                    //New International License
                    command.Parameters.Add("@DriverID", System.Data.SqlDbType.Int).Value = InternationalLicense.DriverID;
                    command.Parameters.Add("@ValidityLength", SqlDbType.TinyInt).Value = ValidityLength;
                    command.Parameters.Add("@LocalLicenseID", System.Data.SqlDbType.Int).Value = InternationalLicense.Local_LicenseID;
                    command.Parameters.Add("@CreatedByUserID_I_License", System.Data.SqlDbType.Int).Value = InternationalLicense.CreatedBy;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "NewApplicationID", out int NewApplicationID))
                            NewApplicationID = -1;

                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "NewInternationalLicenseID", out int NewInternationalLicenseID))
                            NewInternationalLicenseID = -1;

                        if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                            ExpirationDate = DateTime.MinValue;

                        if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime ApplicationDate))
                            ApplicationDate = DateTime.MinValue;


                        InternationalLicense.Int_LicenseApplicationID = NewApplicationID;
                        InternationalLicense.Int_LicenseID = NewInternationalLicenseID;
                        InternationalLicense.ApplicationDate = ApplicationDate;
                        InternationalLicense.LastStatusDate = ApplicationDate;
                        InternationalLicense.IssueDate = ApplicationDate;
                        InternationalLicense.ExpirationDate = ExpirationDate;
                        isAdded = true;
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isAdded = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ أثناء تنفيذ الدالة [AddNewInternationalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isAdded;
        }

        private static bool TryBuild_InternationalLicense(SqlDataReader reader, out DTO_IntLicense InternationalLicense)
        {
            InternationalLicense = null;
            if (reader == null)
                return false;

            //Internaional License
            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "InternationalLicenseID", out int InternationalLicenseID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "I_ApplicationID", out int I_ApplicationID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "IssuedUsingLocalLicenseID", out int LocalLicenseID))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if (!IsActive.HasValue)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "I_CreatedByUserID", out int I_CreatedByUserID))
                return false;

            
            //Application
            
            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicantPersonID", out int ApplicantPersonID))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime ApplicationDate))
                return false;

            if(!clsMapReader.TryGetEnum_Int(reader, "ApplicationTypeID", out enApplicationTypes ApplicationType, enApplicationTypes.UnKnown) || ApplicationType == enApplicationTypes.UnKnown)
                return false;

            if (!clsMapReader.TryGetEnum_Byte(reader, "ApplicationStatus", out enApplicationStatus ApplicationStatus, enApplicationStatus.Unknown) || ApplicationStatus == enApplicationStatus.Unknown)
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "LastStatusDate", out DateTime LastStatusDate))
                return false;

            if(!clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal PaidFees))
                return false;


            InternationalLicense = new DTO_IntLicense
            {
                Int_LicenseID = InternationalLicenseID,
                Int_LicenseApplicationID = I_ApplicationID,
                DriverID = DriverID,
                Local_LicenseID = LocalLicenseID,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                IsActive = IsActive.Value,
                CreatedBy = I_CreatedByUserID,
                PersonID = ApplicantPersonID,
                ApplicationDate = ApplicationDate,
                LastStatusDate = LastStatusDate,
                ApplicationType = ApplicationType,
                ApplicationStatus = ApplicationStatus,
                AppFees = PaidFees
            };
            return true;
        }

        public static bool? FindInternationalLicense(int I_LicenseID, out DTO_IntLicense InternationalLicense)
        {
            InternationalLicense = null;
            bool? isFound = false;

            string Query = @"SELECT 
                                
                                IL.InternationalLicenseID,
                                IL.ApplicationID AS I_ApplicationID,
                                IL.DriverID,
                                IL.IssuedUsingLocalLicenseID,
                                IL.IssueDate,
                                IL.ExpirationDate,
                                IL.IsActive,
                                IL.CreatedByUserID AS I_CreatedByUserID,
                            
                                
                                A.ApplicantPersonID,
                                A.ApplicationDate,
                                A.ApplicationTypeID,
                                A.ApplicationStatus,
                                A.LastStatusDate,
                                A.PaidFees
                            
                            FROM InternationalLicenses IL
                            INNER JOIN Applications A ON IL.ApplicationID = A.ApplicationID
                            WHERE IL.InternationalLicenseID = @I_licenseID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query,connection))
                {
                    command.Parameters.Add("@I_licenseID", SqlDbType.Int).Value = I_LicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_InternationalLicense(reader, out InternationalLicense))
                        {
                            if (InternationalLicense != null)
                                isFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ أثناء تنفيذ الدالة[FindInternationalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
        public static bool? GetPersonID_ApplicationIDBy_IntLicenseID(int Int_LicenseID, out int? PersonID, out int? ApplicationID)
        {
            bool? isFound = false;
            PersonID = null;
            ApplicationID = null;

            string Query = @"Select A.ApplicantPersonID As PersonID,
                                IL.ApplicationID
                                From InternationalLicenses IL
                                Inner Join Applications A
                                	On IL.ApplicationID = A.ApplicationID
                                Where IL.InternationalLicenseID = @Int_LicenseID";

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
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int _PersonID))
                        {
                            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int _ApplicationID))
                            {
                                PersonID = _PersonID;
                                ApplicationID = _ApplicationID;
                                isFound = true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [GetPersonIDBy_IntLicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? IntLicenseRequirements(int LocalLicense, enApplicationTypes ApplicationTypes,
            out DTO_IntLicenseRequirements dto)
        {
            bool? isFound = false;
            dto = null;
            string Query = @"select L.DriverID, L.LicenseClass, L.IsActive, DR.PersonID,
						L.IssueDate, L.ExpirationDate, LC.DefaultValidityLength,
                            (select AY.ApplicationFees From ApplicationTypes AY
                            	Where  AY.ApplicationTypeID = @ApplicationTypeID)
                            	AS ApplicationFees
                            From Licenses L
							Inner Join LicenseClasses LC
								On L.LicenseClass = LC.LicenseClassID
							Inner Join Drivers DR
								On L.DriverID = DR.DriverID
                            Where L.LicenseID = @LocalLicenseID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = (int)ApplicationTypes;
                    command.Parameters.Add("@LocalLicenseID", SqlDbType.Int).Value = LocalLicense;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                        {
                            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int PersonID))
                            {
                                bool? isActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
                                if (isActive.HasValue)
                                {
                                    if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal ApplicationFees))
                                    {
                                        if (clsMapReader.TryGetEnum_Int(reader, "LicenseClass", out enLicenseClass LicenseClass, enLicenseClass.Unknown))
                                        {
                                            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                                            {
                                                if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                                                {
                                                    if (clsMapReader.TryGetByteFromReaderImediately(reader, "DefaultValidityLength", out byte DefaultValidityLength))
                                                    {
                                                        dto = new DTO_IntLicenseRequirements();
                                                        dto.DriverID = DriverID;
                                                        dto.PersonID = PersonID;
                                                        dto.ApplicationFees = ApplicationFees;
                                                        dto.LicenseClass = LicenseClass;
                                                        dto.IsActive = isActive.Value;
                                                        dto.IssueDate = IssueDate;
                                                        dto.ExipritionDate = ExpirationDate;
                                                        dto.LicenseValidityLength = DefaultValidityLength;
                                                        isFound = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [IntLicenseRequirements]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                isFound = false;
                dto = null;
            }

            return isFound;
        }

        public static bool? Deactivate_IntLicense(int int_LicenseID)
        {
            bool? isDeactivated = false;
            string Query = @"UPDATE IL
                                SET IL.IsActive = 0
                             FROM InternationalLicenses IL
                             WHERE IL.InternationalLicenseID = @intLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@intLicenseID", SqlDbType.Int).Value = int_LicenseID;


                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    isDeactivated = (rowAffected > 0);
                }
            }
            catch (Exception ex)
            {
                isDeactivated = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [Deactivate_LocalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isDeactivated;
        }


    }
}
