using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License.New_Local_License;
using DVLD.DTOs.License.Local_License.Release_Detained_License;
using DVLD.DTOs.License.Local_License.Replacement_For;
using DVLD.DTOs.License.ReNew;
using DVLD.DTOs.Police_Actions.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement;
using DVLD.DTOs.Presention_DTO.Licnese.Release_Detain;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedLayer.clsSimpleEncryptor;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer.License
{
    public static class clsLocalLicense_DAL
    {
        private static bool TryBuild_LocalLicense(SqlDataReader reader, out DTO_LocalLicense License)
        {
            License = null;

            if(reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            if(!clsMapReader.TryGetEnum_Int(reader, "LicenseClass", out enLicenseClass LicenseClass, enLicenseClass.Unknown))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            string Notes = clsMapReader.TryGetString_NullFromReaderImediately(reader, "Notes");

            if(!clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal PaidFees))
                return false;

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if (!IsActive.HasValue)
                return false;

            if(!clsMapReader.TryGetEnum_Byte(reader, "IssueReason", out enIssueReasonLicense IssueReason, enIssueReasonLicense.UnKnown))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "CreatedByUserID", out int CreatedByUserID))
                return false;


            License = new DTO_LocalLicense
            {
                LocalLicenseID = LicenseID,
                ApplicationID = ApplicationID,
                DriverID = DriverID,
                LicenseClass = LicenseClass,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                Notes = Notes,
                PaidFees = PaidFees,
                IsActive = IsActive.Value,
                IssueReason = IssueReason,
                CreatedByUserID = CreatedByUserID,
            };

            return true;
        }

        public static bool? FindLicenseByID(int LicenseID, out DTO_LocalLicense License)
        {
            License = null;
            bool? isFound = false;

            string Query = @"select * From Licenses L
                            Where L.LicenseID = @LicenseID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_LocalLicense(reader, out License))
                        {
                            if(License != null)
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
                License = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [FindLicenseByID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool HasValidLocalLicense(int PersonID, enLicenseClass LicenseClass)
        {
            bool _HasValid = true;


            string Query = @"SELECT CASE 
                                WHEN EXISTS (
                                    SELECT 1
                                    FROM People P
                                    INNER JOIN Drivers DR ON P.PersonID = DR.PersonID
                                    INNER JOIN Licenses L ON DR.DriverID = L.DriverID
                                    WHERE P.PersonID = @PersonID 
                                      AND L.LicenseClass = @LicenseClass
                                      AND L.ExpirationDate > GETDATE()
                                )
                                THEN CAST(1 AS BIT)   -- True
                                ELSE CAST(0 AS BIT)   -- False
                            END AS HasLicense;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = LicenseClass;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        _HasValid = Convert.ToBoolean(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _HasValid = true;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [AddNewLicenseFirstTime]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return _HasValid;

        }

        public static bool? AddNewLicenseFirstTime(DTO_LocalLicense dtoLicense,
    int YearsValidity, int PersonID, DTODriver dtoDriver = null)
        {
            bool? isAdded = false;
            int DriverID = dtoDriver == null? dtoLicense.DriverID : dtoDriver.DriverID;
            bool isDriver = dtoDriver == null ? false : true;

            string Query = @"BEGIN Try;
                                BEGIN TRANSACTION;
                                
                                DECLARE @IssueDate DATETIME = GETDATE();
                                
                                IF @DriverID <= 0
                                    Begin
                                        INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                                        VALUES (@PersonID, @CreatedByUserID_Driver, @IssueDate);
                    
                                        SET @DriverID = SCOPE_IDENTITY();
                                    END
                                
                                UPDATE Applications
                                SET 
                                    LastStatusDate = @IssueDate,
                                    ApplicationStatus = 3
                                WHERE ApplicationID = @ApplicationID;

                                DECLARE @ExpirationDate DateTime = DATEADD(YEAR, @YearsValidity, @IssueDate);

                                INSERT INTO Licenses (
                                ApplicationID,
                                DriverID,
                                LicenseClass,
                                IssueDate,
                                ExpirationDate,
                                Notes,
                                PaidFees,
                                IsActive,
                                IssueReason,
                                CreatedByUserID
                            )
                            VALUES (
                                @ApplicationID,
                                @DriverID,
                                @LicenseClass,
                                @IssueDate,
                                @ExpirationDate,
                                @Notes,
                                @PaidFees,
                                1,
                                @IssueReason,
                                @CreatedByUserID
                            );
                            DECLARE @LicenseID INT = SCOPE_IDENTITY();

                                COMMIT TRANSACTION;

                                Select @DriverID AS DriverID, @ExpirationDate AS ExpirationDate,
                                @IssueDate As IssueDate, @LicenseID As LicenseID,
                                @ApplicationID As ApplicationID;

                           END Try
                           Begin Catch
                                	ROLLBACK TRANSACTION;                        
                                THROW;
                           END CATCH;";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@IsDriver", SqlDbType.Bit).Value = isDriver;
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = dtoDriver == null ? PersonID : dtoDriver.PersonID;
                    command.Parameters.Add("@CreatedByUserID_Driver", SqlDbType.Int).Value = dtoLicense.CreatedByUserID;

                    //License
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = dtoLicense.ApplicationID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)dtoLicense.LicenseClass;
                    command.Parameters.Add("@YearsValidity", SqlDbType.Int).Value = YearsValidity;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(dtoLicense.Notes) ? (object)DBNull.Value : dtoLicense.Notes.Trim();
                    command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = dtoLicense.PaidFees;
                    command.Parameters.Add("@IssueReason", SqlDbType.Int).Value = (int)dtoLicense.IssueReason;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = dtoLicense.CreatedByUserID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (dtoDriver == null)
                        dtoDriver = new DTODriver();

                    if (reader.Read())
                    {
                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int _DriverID))
                            _DriverID = -1;

                        if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                            ExpirationDate = DateTime.MinValue;

                        if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                            IssueDate = DateTime.MinValue;

                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                            LicenseID = -1;

                        dtoDriver.DriverID = _DriverID;
                        dtoDriver.CreatedDate = IssueDate;
                        dtoLicense.LocalLicenseID = LicenseID;
                        dtoLicense.DriverID = DriverID;
                        dtoLicense.IssueDate = IssueDate;
                        dtoLicense.ExpirationDate = ExpirationDate;

                        isAdded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isAdded = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [AddNewLicenseFirstTime]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isAdded;
        }

        public static bool? isThereAnActiveLicenseForLicenseClass(int PersonID, enLicenseClass LicenseClass)
        {
            bool? isThereAnActiveLicense = false;
            string Query = @"select case
	                            When Exists 
	                            	(select 1 From Drivers D
	                            	Inner Join People P  On P.PersonID = D.PersonID
	                            	Inner Join Licenses L On D.DriverID = L.DriverID
	                            	Where L.LicenseClass = @LicenseClass And D.PersonID = @PersonID And L.IsActive = 1)
	                            	Then CAST(1 As Bit)
	                            	Else CAST(0 As Bit)
	                            End As isThereAnActiveLicenseForLicenseClass;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        isThereAnActiveLicense = (bool)result;
                    }
                }
            }
            catch(Exception ex)
            {
                isThereAnActiveLicense = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [isThereAnActiveLicenseForLicenseClass]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isThereAnActiveLicense;
        }

        public static bool? isThereAnActiveLDL_AppForLicneseClass(int PersonID, enLicenseClass LicenseClass)
        {
            bool? isThereAnActiveLDL_AppForLicneseClass = false;
            string Query = @"select case
	                         When Exists 
	                         	(select 1 From Applications A 
	                         	Inner Join LocalDrivingLicenseApplications LDL On A.ApplicationID = LDL.ApplicationID
	                         	Where (LDL.LicenseClassID = @LicenseClass And A.ApplicantPersonID = @PersonID
								And A.ApplicationStatus = 1))
	                         	Then CAST(1 As Bit)
	                         	Else CAST(0 As Bit)
	                         End As isThereAnActiveLDL_AppForLicneseClass;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        isThereAnActiveLDL_AppForLicneseClass = (bool)result;
                    }
                }
            }
            catch (Exception ex)
            {
                isThereAnActiveLDL_AppForLicneseClass = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [isThereAnActiveLDL_AppForLicneseClass]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isThereAnActiveLDL_AppForLicneseClass;
        }

        public static bool? GetLastLicenseInfo(int PersonID, enLicenseClass LicenseClass, out DateTime ExpirationDate,
            out bool IsActive)
        {
            bool? isFound = false;
            ExpirationDate = DateTime.MinValue;
            IsActive = false;

            string Query = @"select Top 1 L.ExpirationDate, L.IsActive From Licenses L
                            Inner Join Applications A On L.ApplicationID = A.ApplicationID
                            Where A.ApplicantPersonID = @PersonID And L.LicenseClass = @LicenseClass
                            order by L.IssueDate Desc;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out ExpirationDate))
                        {
                            bool? _IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
                            if(_IsActive != null)
                            {
                                isFound = true;
                                IsActive = _IsActive.Value;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                ExpirationDate = DateTime.MinValue;
                IsActive = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في قواعد البيانات [GetLastLicenseInfo]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? GetLicenseFees_AppFees(enLicenseClass LicenseClass, enApplicationTypes ApplicationType,
            out decimal LicenseFees, out decimal ApplicationFees)
        {
            bool? isFound = false;
            LicenseFees = -1;
            ApplicationFees = -1;

            string Query = @"Select Lc.ClassFees, AY.ApplicationFees
                            From LicenseClasses LC, ApplicationTypes AY
                        Where LC.LicenseClassID = @LicenseClassID 
                        AND AY.ApplicationTypeID = @ApplicationTypeID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand( Query, connection))
                {
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClass;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationType;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ClassFees", out LicenseFees))
                        {
                            if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out ApplicationFees))
                            {
                                isFound = true;
                            }
                        }
                         
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                ApplicationFees = -1;
                LicenseFees = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [GetLicenseFees_AppFees]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? Deactivate_LocalLicense(int LocalLicenseID)
        {
            bool? isDeactivated = false;
            string Query = @"UPDATE L
                                SET L.IsActive = 0
                             FROM Licenses L
                             WHERE L.LicenseID = @LocalLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LocalLicenseID", SqlDbType.Int).Value = LocalLicenseID;


                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    isDeactivated = (rowAffected > 0);
                }
            }
            catch(Exception ex)
            {
                isDeactivated = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [Deactivate_LocalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isDeactivated;
        }

        public static bool? IsLastLicenseInClass(int LocalLicense)
        {
            bool? _isLastLicenseInClass = null;
            string Query = @"select 
                            	case When L.LicenseID = 
                            		(select Top 1 L2.LicenseID
                            		From Licenses L2
                            		Where L2.DriverID = L.DriverID And L2.LicenseClass = L.LicenseClass
                            		order by L2.IssueDate Desc
                            		) Then Cast (1 as bit)
                            		Else Cast (0 as bit)
                            		End As IsLatestLicense
                            From Licenses L
                            Where L.LicenseID = @LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LocalLicense;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        _isLastLicenseInClass = (bool)result;
                    }
                }
            }
            catch(Exception ex)
            {
                _isLastLicenseInClass = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [IsLastLicenseInClass]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return _isLastLicenseInClass;
        }

        public static bool? IsDetainedLicense(int LocalLicense)
        {
            bool? IsDetainedLicense = null;
            string Query = @"SELECT 
                                CASE 
                                    WHEN EXISTS (
                                        SELECT 1
                                        FROM (
                                            SELECT TOP 1 DL.IsReleased
                                            FROM DetainedLicenses DL
                                            WHERE DL.LicenseID = @LicenseID
                                            ORDER BY DL.DetainDate DESC
                                        ) AS LatestDetain
                                        WHERE LatestDetain.IsReleased = 0
                                    )
                                    THEN CAST(1 AS BIT)
                                    ELSE CAST(0 AS BIT)
                                END AS IsDetainedLicense;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LocalLicense;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        IsDetainedLicense = (bool) result;
                    }
                }
            }
            catch(Exception ex)
            {
                IsDetainedLicense = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [IsDetainedLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }


            return IsDetainedLicense;
        }

        public static bool AddRenewLocalLicense(DTO_Renew_LocalLicense dto, int ValidtyLength)
        {
            bool isAdded = false;
            string Query = @"SET NOCOUNT ON;
                                SET XACT_ABORT ON;
                                
                                DECLARE @DateNow DATETIME = GETDATE();
                                DECLARE @ApplicationID INT;
                                DECLARE @RenewLocalLicenseID INT;
                                DECLARE @ExpirationDate DATETIME;
                                
                                BEGIN TRY
                                    BEGIN TRANSACTION;
                                
                                    -- 1. Disable old license
                                    UPDATE Licenses
                                    SET IsActive = 0
                                    WHERE LicenseID = @OldLicenseID;
                                
                                    -- 2. Insert new application
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
                                        @DateNow,
                                        @ApplicationTypeID,
                                        3, -- UnderProcessing
                                        @DateNow,
                                        @PaidFees,
                                        @CreatedByUserID
                                    );

                                    IF @@ROWCOUNT = 0
                                         THROW 50002, 'فشل إدخال الطلب الجديدة.', 1;
                                
                                    SET @ApplicationID = SCOPE_IDENTITY();
                                
                                    -- 3. Insert new license
                                    SET @ExpirationDate = DATEADD(YEAR, @ValidtyLength, @DateNow);
                                
                                    INSERT INTO Licenses (
                                        ApplicationID,
                                        DriverID,
                                        LicenseClass,
                                        IssueDate,
                                        ExpirationDate,
                                        Notes,
                                        PaidFees,
                                        IsActive,
                                        IssueReason,
                                        CreatedByUserID
                                    )
                                    VALUES (
                                        @ApplicationID,
                                        @DriverID,
                                        @LicenseClass,
                                        @DateNow,
                                        @ExpirationDate,
                                        NULLIF(LTRIM(RTRIM(@Notes)), ''),
                                        @LicenseFees,
                                        1,
                                        @IssueReason,
                                        @CreatedByUserID
                                    );
                                        
                                        IF @@ROWCOUNT = 0
                                            THROW 50002, 'فشل إدخال الرخصة الجديدة.', 1;

                                
                                    SET @RenewLocalLicenseID = SCOPE_IDENTITY();
                                
                                    COMMIT TRANSACTION;
                                

                                    -- Return generated values
                                    SELECT 
                                        @RenewLocalLicenseID AS RenewedLicenseID,
                                        @ApplicationID AS ApplicationID,
                                        @DateNow AS IssueDate,
                                        @ExpirationDate AS ExpirationDate;
                                END TRY
                                BEGIN CATCH
                                    IF @@TRANCOUNT > 0
                                        ROLLBACK TRANSACTION;
                                
                                    THROW;
                                END CATCH;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@OldLicenseID", SqlDbType.Int).Value = dto.OldLicenseID;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;

                    command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = dto.PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = (int) dto.ApplicationTypes;
                    command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
                    
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = dto.DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int) dto.LicenseClass;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(dto.Notes)? (object) DBNull.Value : dto.Notes;
                    command.Parameters.Add("@IssueReason", SqlDbType.Int).Value = (byte)dto.IssueReason;
                    command.Parameters.Add("@ValidtyLength", SqlDbType.Int).Value = ValidtyLength;
                    command.Parameters.Add("@@LicenseFees", SqlDbType.Decimal).Value = dto.LicenseFees;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "RenewedLicenseID", out int RenewedLicenseID))
                        {
                            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                            {
                                if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                                {
                                    if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                                    {
                                        dto.LocalLicenseID = RenewedLicenseID;
                                        dto.ApplicationID = ApplicationID;
                                        dto.ApplicationDate = IssueDate;
                                        dto.LastStatusDate = IssueDate;
                                        dto.IssueDate = IssueDate;
                                        dto.ExpirationDate = ExpirationDate;
                                    }
                                }
                            }
                        }
                        isAdded = true;
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                isAdded = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [AddRenewLocalLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isAdded;
        }

        public static bool? GetPreviousLicenseInfo(int DriverID, enLicenseClass LicenseClass, out DTO_NewLicenseRequirements_BL dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select Top 1 
                                L.LicenseID,
                                L.IsActive,
                                L.ExpirationDate
                                From
                                Licenses L
                                Where L.DriverID = @DriverID
                                And L.LicenseClass = @LicenseClass
                                Order By L.IssueDate Desc";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("LicenseClass", SqlDbType.Int).Value = (int)LicenseClass;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        dto = new DTO_NewLicenseRequirements_BL();
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                        {
                            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
                            if(IsActive.HasValue)
                            {
                                if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                                {
                                    dto.LicenseID = LicenseID;
                                    dto.IsActive = IsActive.Value;
                                    dto.ExpirationDate = ExpirationDate;
                                    isFound = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetPreviousLicenseInfo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }

        public static bool? Get_RenewLicenseConfig(enApplicationTypes applicationType, enLicenseClass LicenseClass, out DTO_RenewLicense_Config dto_RenewConfig)
        {
            bool? isFound = false;
            dto_RenewConfig = null;

            string Query = @"SELECT 
                                AP.ApplicationFees,
                                LC.ClassFees,
                                LC.DefaultValidityLength
                            FROM ApplicationTypes AP
                            JOIN LicenseClasses LC ON LC.LicenseClassID = @LicenseClass
                            WHERE AP.ApplicationTypeID = @applicationType;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = (int) LicenseClass;
                    command.Parameters.Add("@applicationType", SqlDbType.Int).Value = (int) applicationType;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal ApplicationFees))
                        {
                            if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "ClassFees", out decimal ClassFees))
                            {
                                if(clsMapReader.TryGetByteFromReaderImediately(reader, "DefaultValidityLength", out byte ValidityLength))
                                {
                                    dto_RenewConfig = new DTO_RenewLicense_Config();
                                    dto_RenewConfig.ApplicationFees = ApplicationFees;
                                    dto_RenewConfig.LicenseFees = ClassFees;
                                    dto_RenewConfig.ValidityLength = ValidityLength;
                                    isFound = true;
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [Get_RenewLicenseConfig]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }

        public static bool TryBuild_RenewLicense_Info(SqlDataReader reader, out DTO_RenewLicense_Info dto)
        {
            dto = null;
            if (reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "RenewLicenseID", out int RenewLicenseID))
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime ApplicationDate))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            if (!clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal ApplicationFees))
                return false;

            if(!clsMapReader.TryGetDecimalFromReaderImediately(reader, "ClassFees", out decimal ClassFees))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "CreatedByUserID", out int CreatedByUserID))
                return false;

            dto = new DTO_RenewLicense_Info
            {
                RenewedLicenseID = RenewLicenseID,
                RL_ApplicationID = ApplicationID,
                ApplicationDate = ApplicationDate,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                ApplicationFees = ApplicationFees,
                LicenseFees = ClassFees,
                CreatedBy = CreatedByUserID
            };

            return true;
        }

        public static bool? Find_RenewLicense_Info(int LicenseID, out DTO_RenewLicense_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select L.LicenseID  As RenewLicenseID, L.ApplicationID,
                                A.ApplicationDate, L.IssueDate, L.ExpirationDate,
                                AY.ApplicationFees, LC.ClassFees, L.CreatedByUserID
                                 From Licenses L
                                Inner Join Applications A
                                	On A.ApplicationID = L.ApplicationID
                                Inner Join ApplicationTypes AY
                                	On AY.ApplicationTypeID = A.ApplicationTypeID
                                Inner Join LicenseClasses LC
                                	On LC.LicenseClassID = L.LicenseClass
                                Where L.LicenseID = @LicenseID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_RenewLicense_Info(reader, out dto))
                        {
                            if(dto != null)
                            {
                                isFound = true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [Find_RenewLicense_Info]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }

        public static bool? GetPersonID_ApplicationID_ByLocalLicenseID(int LocalLicenseID, out int ApplicationID, 
            out int PersonID)
        {
            bool? isFound = false;
            ApplicationID = -1;
            PersonID = -1;

            string Query = @"select L.ApplicationID, D.PersonID From Licenses L
                             Inner Join Drivers D
                             	On L.DriverID = D.DriverID
                             Where L.LicenseID = @LocalLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LocalLicenseID", SqlDbType.Int).Value = LocalLicenseID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out ApplicationID))
                        {
                            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out PersonID))
                            {
                                isFound = true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetPersonID_ApplicationID_ByLocalLicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID ?? -1);
            }

            return isFound;
        }

        public static bool? GetReplacementLicenseInfo(int LocalLicenseID, out DTO_ApplicationReplacement_Info dto)
        {
            bool? isFound = false;
            dto = null;
            string Query = @"select A.ApplicationID As R_ApplicationID, 
                             L.LicenseID As R_LicenseID, A.ApplicationDate, 
                             A.PaidFees From Licenses L
                             Inner Join Applications A
                             	On L.ApplicationID = A.ApplicationID
                             Where L.LicenseID = @LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", System.Data.SqlDbType.Int).Value = LocalLicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        dto = new DTO_ApplicationReplacement_Info();
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "R_ApplicationID", out int R_ApplicationID))
                        {
                            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "R_LicenseID", out int R_LicenseID))
                            {
                                if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime ApplicationDate))
                                {
                                    if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal PaidFees))
                                    {
                                        dto.R_ApplicationID = R_ApplicationID;
                                        dto.R_LicenseID = R_LicenseID;
                                        dto.ApplicationDate = ApplicationDate;
                                        dto.AppFees = PaidFees;
                                        isFound = true;
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                dto = null;
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetReplacementLicenseInfo]",
                    "Erorr", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool AddReplacementLicense(DTO_Replacement_LocalLicense dto)
        {
            bool isAdded = false;
            string Query = @"SET NOCOUNT ON;
                             SET XACT_ABORT ON;
                             
                             	 
                                  BEGIN TRY
                                      BEGIN TRANSACTION
                             
                             		 DECLARE @ApplicationID INT;
                                     DECLARE @ReplacementLicense INT;
		                             DECLARE @DateNow datetime = GETDATE();
                             	
                             		 UPDATE Licenses
                                        SET IsActive = 0
                                        WHERE LicenseID = @OldLicenseID;
                                        
                                        IF @@ROWCOUNT = 0
                                            THROW 50001, 'لم يتم العثور على الرخصة القديمة لتعطيلها!', 1;
                                        
                             
                             		 
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
                                          @DateNow,
                                          @ApplicationTypeID,
                                          3, -- UnderProcessing
                                          @DateNow,
                                          @PaidFees,
                                          @CreatedByUserID
                                      );
                             
                             		 SET @ApplicationID = SCOPE_IDENTITY();
                             		                                 
                                      INSERT INTO Licenses (
                                          ApplicationID,
                                          DriverID,
                                          LicenseClass,
                                          IssueDate,
                                          ExpirationDate,
                                          Notes,
                                          PaidFees,
                                          IsActive,
                                          IssueReason,
                                          CreatedByUserID
                                      )
                                      VALUES (
                                          @ApplicationID,
                                          @DriverID,
                                          @LicenseClass,
                                          @DateNow,
                                          @ExpirationDate,
                                          NULL,
                                          0,
                                          1,
                                          @IssueReason,
                                          @CreatedByUserID
                                      );
                             
                             		 set @ReplacementLicense = SCOPE_IDENTITY();
                             
                             		 COMMIT TRANSACTION;
                             
                             		 Select @ApplicationID AS ReplacementApplicationID,
                             		 @ReplacementLicense As ReplacementLicenseID;
                                      END TRY
                                   BEGIN CATCH
                             		  IF @@TRANCOUNT > 0
                             			ROLLBACK TRANSACTION;
                                         THROW;
                             END CATCH;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@OldLicenseID", SqlDbType.Int).Value = dto.OldLicenseID;
                    command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = dto.PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = (int) dto.ApplicationTypes;
                    command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = dto.DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = dto.LicenseClass;
                    command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = dto.ExpirationDate;
                    command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = (byte) dto.IssueReason;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ReplacementApplicationID", out int ReplacementApplicationID))
                        {
                            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ReplacementLicenseID", out int ReplacementLicenseID))
                            {
                                dto.LocalLicenseID = ReplacementLicenseID;
                                dto.ApplicationID = ReplacementApplicationID;
                                isAdded = true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isAdded = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [AddReplacementLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isAdded;
        }

        public static bool? IsLicenseActive(int LicenseID)
        {
            bool? isActive = null;
            string Query = @"SELECT L.IsActive
                             FROM Licenses L
                             WHERE L.LicenseID = @LicenseID;
                             ";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        isActive = (bool)result;
                    }
                }
            }
            catch(Exception ex)
            {
                isActive = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [IsLicenseActive]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isActive;
        }

        public static bool ReleaseDetainedLicense(DTO_ReleaseDetainedLicense dto)
        {
            bool isReleased = false;
            string Query = @"SET NOCOUNT ON;
                             SET XACT_ABORT ON;
                             BEGIN TRY
                                 BEGIN TRANSACTION
                             		
                             		DECLARE @DateNow DATETIME = GETDATE();
                             		DECLARE @ApplicationID INT;
                             		
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
                             		    @DateNow,
                             		    @ApplicationTypeID,
                             		    3,
                             		    @DateNow,
                             		    @PaidFees,
                             		    @CreatedByUserID
                             		);
                             
                             		set @ApplicationID = SCOPE_IDENTITY();
                             
                             		update DetainedLicenses
                             		Set IsReleased = 1,
                             			ReleaseDate = @DateNow,
                             			ReleasedByUserID = @CreatedByUserID,
                             			ReleaseApplicationID = @ApplicationID
                             		Where LicenseID = @LicenseID;
                             		
                             		if(@@ROWCOUNT <= 0)
                             			THROW 50001, 'لم يتم العثور على سجل حجز للرخصة المحددة!', 1;
                             
                             	COMMIT TRANSACTION;
                             
                             	select @ApplicationID As ReleasedApplicationID;
                             	
                             End Try
                             BEGIN Catch
                             	IF @@TRANCOUNT > 0
                             		ROLLBACK TRANSACTION;
                             		THROW;
                             END CATCH;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = dto.PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = (int) dto.ApplicationTypes;
                    command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = dto.PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = dto.LicenseID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        dto.ReleaseApplicationID = value;
                        isReleased = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isReleased = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [ReleaseDetainedLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isReleased;
        }

        public static bool? GetReleaseLicenseRequirements(int LicenseID, enApplicationTypes ApplicationType,
            out DTO_ReleaseLicenseRequirements dto)
        {
            bool? isFound = false;
            dto = null;
            string Query = @"select Top 1 DL.DetainID, D.PersonID, DL.IsReleased,
                            AY.ApplicationFees, DL.FineFees
                                From DetainedLicenses DL
                                Inner Join Licenses L
                                	On DL.LicenseID = L.LicenseID
                                Inner Join Drivers D
                                	On L.DriverID = D.DriverID
                                left Join ApplicationTypes AY
                                	On AY.ApplicationTypeID = @ApplicationTypeID
                                Where DL.LicenseID = @LicenseID
                                order by DL.DetainDate Desc;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = (int) ApplicationType;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "DetainID", out int DetainID))
                        {
                            bool? IsReleased = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsReleased");
                            if (IsReleased.HasValue)
                            {
                                if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal ApplicationFees))
                                {
                                    if (clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int PersonID))
                                    {
                                        if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "FineFees", out decimal FineFees))
                                        {
                                            dto = new DTO_ReleaseLicenseRequirements();
                                            dto.DetainID = DetainID;
                                            dto.IsReleased = IsReleased.Value;
                                            dto.Appfees = ApplicationFees;
                                            dto.personID = PersonID;
                                            dto.Fines = FineFees;
                                            isFound = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetDetainedLicenseData]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? GetReleaseDetainConfig_Info(int LicenseID, enApplicationTypes ApplicationType,
            out DTO_ReleaseDetain_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"Select Top 1 DL.DetainID, DL.DetainDate, 
                            DL.FineFees, U.UserName, AY.ApplicationFees,
							DL.IsReleased, DL.ReleaseApplicationID,
							U2.UserName AS ReleasedByUser
                            From DetainedLicenses DL
                            Inner Join Users U
                            	On DL.CreatedByUserID = U.UserID
                            Left Join ApplicationTypes AY
                            	On AY.ApplicationTypeID = @ApplicationType
							Left Join Users U2
								On U2.UserID = DL.ReleasedByUserID
                            Where DL.LicenseID = @LicenseID
							order by DL.DetainDate Desc;";

            try
            {
                using(SqlConnection connection = new SqlConnection (clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand (Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@ApplicationType", SqlDbType.Int).Value = (int) ApplicationType;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "DetainID", out int DetainID))
                        {
                            if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DetainDate", out DateTime DetainDate))
                            {
                                if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "FineFees", out decimal FineFees))
                                {
                                    if(clsMapReader.TryGetStringFromReaderImediately(reader, "UserName", out string UserName))
                                    {
                                        if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "ApplicationFees", out decimal ApplicationFees))
                                        {
                                            bool? IsReleased = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsReleased");
                                            if (IsReleased.HasValue)
                                            {
                                                int? ReleaseApplicationID = clsMapReader.TryGetNULL_Int(reader, "ReleaseApplicationID");
                                                string ReleasedByUser = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ReleasedByUser");

                                                dto = new DTO_ReleaseDetain_Info();
                                                dto.DetainID = DetainID;
                                                dto.DatainDate = DetainDate;
                                                dto.Createdby = UserName;
                                                dto.FineFees = FineFees;
                                                dto.AppFees = ApplicationFees;
                                                dto.ReleaseApplicationID = ReleaseApplicationID;
                                                dto.ReleaseByUser = ReleasedByUser;
                                                dto.IsReleased = IsReleased.Value;
                                                isFound = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetReleaseDetain_Info]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? GetReleaseDetain_Info(int ReleaseApplicationID, out DTO_ReleaseDetain_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select DL.DetainID, DL.LicenseID, 
                            DL.DetainDate, A.PaidFees, DL.FineFees,
                            (Select U1.UserName From Users U1
                            Where U1.UserID = DL.CreatedByUserID
                            ) As CreatedByUser, 
                            U.UserName As ReleasedByUserName
                             From Applications A
                            Inner Join DetainedLicenses DL
                            	On A.ApplicationID = DL.ReleaseApplicationID
                            Left Join Users U
                            	On DL.ReleasedByUserID = U.UserID
                            Where DL.ReleaseApplicationID = @ReleaseApplicationID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int).Value = ReleaseApplicationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "DetainID", out int DetainID))
                        {
                            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                            {
                                if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DetainDate", out DateTime DetainDate))
                                {
                                    if(clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal PaidFees))
                                    {
                                        if(clsMapReader.TryGetStringFromReaderImediately(reader, "CreatedByUser", out string CreatedByUser))
                                        {
                                            if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "FineFees", out decimal FineFees))
                                            {
                                                string ReleasedByUserName = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ReleasedByUserName");
                                                dto = new DTO_ReleaseDetain_Info();
                                                dto.DetainID = DetainID;
                                                dto.LicenseID = LicenseID;
                                                dto.DatainDate = DetainDate;
                                                dto.AppFees = PaidFees;
                                                dto.Createdby = CreatedByUser;
                                                dto.FineFees = FineFees;
                                                dto.ReleaseByUser = ReleasedByUserName;
                                                isFound = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetReleaseDetain_Info]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isFound;
        }

        public static bool? isLicenseRelease(int LicenseID)
        {
            bool? isRelease = null;
            string Query = @"Select Top 1 DL.IsReleased 
                             From DetainedLicenses DL
                             Where DL.LicenseID = @LicenseID
                             order by DL.DetainDate Desc;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        isRelease = (bool) result;
                    }
                }
            }
            catch(Exception ex)
            {
                isRelease = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [isLicenseRelease]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isRelease;
        }
    }
}
