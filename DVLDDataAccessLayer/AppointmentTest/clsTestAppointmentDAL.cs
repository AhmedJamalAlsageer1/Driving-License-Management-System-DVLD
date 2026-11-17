using DVLD.DTOs;
using DVLD.DTOs.Tests;
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

namespace DVLDDataAccessLayer.Tests
{
    public static class clsTestAppointmentDAL
    {
        public static bool isRequestRetakeTest(int LDL_AppID, enTestType testType)
        {
            bool isRetakeTest = false;

            const string query = @"SELECT 
                CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM TestAppointments TA
                        WHERE TA.LocalDrivingLicenseApplicationID = @LDL_AppId
                          AND TA.TestTypeID = TestTypeId
                    ) THEN 1
                    ELSE 0
                END AS isRetake;";

            try
            {
                using (var connections = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (var cmd = new SqlCommand(query, connections))
                {
                    cmd.Parameters.Add("@LDL_AppId", SqlDbType.Int).Value = LDL_AppID;
                    cmd.Parameters.Add("@TestTypeId", SqlDbType.Int).Value = testType;

                    connections.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                        isRetakeTest = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception ex)
            {
                isRetakeTest = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [isRequestRetakeTest]",
                    "خطأ", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser?.UserID ?? 0);
            }

            return isRetakeTest;
        }

        public static bool HasOpenAppointment(int LDL_AppID, enTestType testType)
        {
            bool hasAppointment = true;

            string Query = @"SELECT 
                                CASE 
                                    WHEN EXISTS (
                                        SELECT 1 
                                        FROM TestAppointments 
                                        WHERE LocalDrivingLicenseApplicationID = @LDL_AppID 
                                          AND TestTypeID = @TestTypeID
                                          AND IsLocked = 0
                                    ) THEN 1 
                                    ELSE 0 
                                END AS HasAppointmentOpen;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDL_AppId", SqlDbType.Int).Value = LDL_AppID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testType;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int value))
                        hasAppointment = value == 1;
                }
                catch (Exception ex)
                {
                    hasAppointment = true;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ أثناء تشغيل هذه الدالة [GetPassedTestsCount]",
                        "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return hasAppointment;
            }
        }
        public static bool AddNewTestAppointment(DTOTestAppointment appointment)
        {
            bool isAdded = false;

            string Query = @"Insert into TestAppointments
                (TestTypeID, LocalDrivingLicenseApplicationID, 
                AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
            values
                (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
                @PaidFees, @CreatedByUserID, 0, @RetakeTestApplicationID)
                SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = (int) appointment.TypeTestID;
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = appointment.LocalDrivingLicenseApplicationID;
                    command.Parameters.Add("@AppointmentDate", SqlDbType.SmallDateTime).Value = appointment.AppointmentDate;
                    command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = appointment.PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = appointment.CreatedByUserID;
                    command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = DBNull.Value;



                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int value))
                    {
                        appointment.TestAppointmentID = value;
                    }
                    else
                        appointment.TestAppointmentID = -1;

                    isAdded = true;
                }
            }
            catch (Exception ex)
            {
                isAdded = false;
                appointment.TestAppointmentID = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ أثناء تشغيل هذه الدالة [TryBookAppointment]",
               "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isAdded;
        }

        public static bool AddNewReTakeTestAppointment(DTO_LDL_App application, DTOTestAppointment appointment)
        {
            bool IsAdded = false;
            string Query = @"Begin TRANSACTION;
                             Begin Try 
                                        
                                     DECLARE @CurrentDate DATETIME = GETDATE();
                        
                                     Insert Into Applications (
                                     ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                                     ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID )
                                 values 
                                 ( @ApplicationPersonID, @CurrentDate, @ApplicationTypeID,
                                     @ApplicationStatus, @CurrentDate, @AppPaidFees, @CreatedByUserID_App );
                                    
                                Declare @NewApplicationID INT = SCOPE_IDENTITY();
                                    
                                     INSERT INTO TestAppointments 
                                     ( TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                                     PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID )
                                 VALUES (
                                     @TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
                                     @PaidFees, @CreatedByUserID, 0, @NewApplicationID )
                                    
                                Declare @NewAppointmentID  INT = SCOPE_IDENTITY();

                                SELECT @NewApplicationID AS ReTake_ApplicationID,
                                @NewAppointmentID AS TestAppointmentID,
                                @CurrentDate AS StatusDate;
                                

                                COMMIT TRANSACTION;
                            END TRY
                            Begin Catch
                                ROLLBACK TRANSACTION;
                                Throw;
                            END CATCH;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    //ReTake_ApplicationInfo
                    command.Parameters.Add("@ApplicationPersonID", SqlDbType.Int).Value = application.PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = application.ApplicationTypeID;
                    command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = enApplicationStatus.New;
                    command.Parameters.Add("@AppPaidFees", SqlDbType.SmallMoney).Value = application.PaidFees;
                    command.Parameters.Add("@CreatedByUserID_App", SqlDbType.Int).Value = application.CreatedByUserID;

                    //TestAppointmentInfo
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = appointment.TypeTestID;
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = appointment.LocalDrivingLicenseApplicationID;
                    command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointment.AppointmentDate;
                    command.Parameters.Add("@PaidFees", SqlDbType.SmallMoney).Value = appointment.PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = appointment.CreatedByUserID;
                    command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = appointment.IsLocked;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        application.ApplicationID = -1;
                        appointment.TestAppointmentID = -1;
                        application.LastStatusDate = DateTime.MinValue;
                        application.ApplicationDate = DateTime.MinValue;


                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "ReTake_ApplicationID", out int _applicationID))
                            application.ApplicationID = _applicationID;

                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "TestAppointmentID", out int _testAppointmentID))
                            appointment.TestAppointmentID = _testAppointmentID;

                        if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "StatusDate", out DateTime Status_Date))
                        {
                            application.LastStatusDate = Status_Date;
                            application.ApplicationDate = Status_Date;
                        }
                    }
                    reader.Close();
                    IsAdded = true;
                }
            }
            catch (Exception ex)
            {
                IsAdded = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [AddNewReTakeTestAppointment]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsAdded;
        }
        public static bool UpdateAppointmentDate(int AppointmentID, DateTime AppointmentDate)
        {
            int rowAffected = 0;
            string Query = @"Update TestAppointments
                             set AppointmentDate = @AppointmentDate
                             WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = AppointmentID;
                    command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = AppointmentDate;

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                rowAffected = 0;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة [UpdateAppointmentDate]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return (rowAffected > 0);
        }
        public static bool? isLocked(int TestAppointmentID)
        {
            bool? isLocked = null;
            string Query = @"SELECT
                                CASE
                                    WHEN EXISTS (
                                        SELECT 1
                                        FROM TestAppointments
                                        WHERE IsLocked = 1 AND TestAppointmentID = @TestAppointmentID
                                    ) THEN 1
                                    ELSE 0
                                END AS isLocked;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        isLocked = (value == 1);
                }
            }
            catch(Exception ex)
            {
                isLocked = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [isLocked]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isLocked;
        }
        public static bool Lock_TestAppointment(int AppointmentID, enApplicationStatus applicationStatus = enApplicationStatus.Unknown)
        {
            int rowAffected = 0;

            string Query = @"BEGIN TRY
                                SET NOCOUNT ON;
                                SET XACT_ABORT ON;

                                BEGIN TRANSACTION;
                                        DECLARE @ReTakeAppID INT;
                                        DECLARE @affectedRows INT = 0;
                                
                                        SELECT @ReTakeAppID = RetakeTestApplicationID
                                        FROM TestAppointments
                                        WHERE TestAppointmentID = @TestAppointmentID;

                                        UPDATE TestAppointments
                                        SET IsLocked = 1
                                        WHERE TestAppointmentID = @TestAppointmentID;
                                        Set @affectedRows += @@ROWCOUNT;

                                        if @ReTakeAppID is NOT NULL
                                    Begin
                                	    Update Applications
                                	    Set ApplicationStatus = @ApplicationStatus
                                	    Where ApplicationID = @ReTakeAppID;
                                        Set @affectedRows += @@ROWCOUNT;
                                    END
                                
                                Select @affectedRows;

                               Commit Transaction;
                            END TRY
                            Begin Catch
                                    ROLLBACK TRANSACTION;
                                    THROW;
                            END CATCH;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = AppointmentID;

                    if (applicationStatus == enApplicationStatus.Unknown)
                        command.Parameters.Add("@ApplicationStatus", SqlDbType.Int).Value = 0;
                    else
                        command.Parameters.Add("@ApplicationStatus", SqlDbType.Int).Value = (int)applicationStatus;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                        rowAffected = value;
                }
            }
            catch (Exception ex)
            {
                rowAffected = 0;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [LockReTake_TestAppointment]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return (rowAffected >= 1);
        }

        private static bool Fail(out DTOTestAppointment dto)
        {
            dto = null;
            return false;
        }
        private static bool Fail(out DTOTestAppointment_Init dto)
        {
            dto = null;
            return false;
        }

        private static bool TryBuild_TestAppointmentFromReader(SqlDataReader reader, out DTOTestAppointment dto)
        {
            dto = null;

            if (reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "TestAppointmentID", out int AppointmentID))
                return Fail(out dto);

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "ClassName", out string ClassName))
                return Fail(out dto);

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out int LDL_AppID))
                return Fail(out dto);

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "TestTypeID", out int TestTypeID))
                return Fail(out dto);

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "AppointmentDate", out DateTime AppointmentDate))
                return Fail(out dto);

            if (!clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal PaidFees))
                return Fail(out dto);

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "CreatedByUserID", out int CreatedByUserID))
                return Fail(out dto);

            bool? isLocked = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsLocked");
            if(!isLocked.HasValue)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "فشل في قراءة حالة IsLocked في دالة [TryBuild_TestAppointmentFromReader]",
                    "Error", null, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                return Fail(out dto);
            }

            int? R_Take_AppID = clsCommenFunctions.FromSQLToInt(reader, "R_Take_AppID");
            decimal? R_Take_Fees = clsCommenFunctions.FromSQLToDecimal(reader, "R_Take_AppFees");


            dto = new DTOTestAppointment
            {
                TestAppointmentID = AppointmentID,
                ClassName = ClassName,
                LocalDrivingLicenseApplicationID = LDL_AppID,
                TypeTestID = Enum.IsDefined(typeof(enTestType), TestTypeID)? (enTestType) TestTypeID : enTestType.UnKnown,
                AppointmentDate = AppointmentDate,
                PaidFees = PaidFees,
                IsLocked = isLocked.Value,
                CreatedByUserID = CreatedByUserID,
                R_Take_AppID = R_Take_AppID,
                R_Take_AppFees = R_Take_Fees
            };

            return true;
        }

        private static bool TryBuildTestAppointment_Init_FromReader(SqlDataReader reader, out DTOTestAppointment_Init dto, enTestType testType, 
            enMode mode)
        {
            dto = null;

            if (reader == null)
                return false;

            int? AppointmentID = clsCommenFunctions.FromSQLToInt(reader, "TestAppointmentID");

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out int LDL_AppID))
                return Fail(out dto);

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "TestTypeID", out int TestTypeID))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "ClassName", out string ClassName))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string FullName))
                return Fail(out dto);

            if(!clsMapReader.TryGetDecimalFromReaderImediately(reader, "TestFees", out decimal TestFees))
                return Fail(out dto);

            bool? isLocked = clsCommenFunctions.FromSQLToBOOL(reader, "IsLocked");

            DateTime? AppointmentDate = clsCommenFunctions.FromSQLToDateTime(reader, "AppointmentDate");

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "Trial", out int Trial))
                return Fail(out dto);


            int? R_Take_AppID = null;
            if(mode == enMode.Update)
                R_Take_AppID = clsCommenFunctions.FromSQLToInt(reader, "R_Take_AppID");

            decimal? ReTakeTestFees = clsCommenFunctions.FromSQLToDecimal(reader, "R_Take_AppFees");

            dto = new DTOTestAppointment_Init
            {
                TestAppointmentID = AppointmentID,
                DrivingLiceseName = ClassName,
                FullName = FullName,
                isLocked = isLocked,
                TestFees = TestFees,
                AppointmentDate = AppointmentDate,
                Trial = (byte)Trial,
                R_Take_AppID = R_Take_AppID,
                R_Take_AppFees = ReTakeTestFees,
                LocalDrivingLicenseApplicationID = LDL_AppID,
                TypeTestID = Enum.IsDefined(typeof(enTestType), TestTypeID) ? (enTestType)TestTypeID : enTestType.UnKnown
            };

            return true;
        }
        public static bool TryGetTestAppointment_InitializationByLDL_AppID(int LDL_AppID, enTestType typeId, out DTOTestAppointment_Init dto)
        {
            bool isFound = false;
            dto = null;

            string Query = @"
                    SELECT TOP 1
                    TA.TestAppointmentID,
                    L.LocalDrivingLicenseApplicationID,
                    LC.ClassName,
                    TA.IsLocked,
                    TY.TestTypeID,
					TY.TestTypeFees AS TestFees,
                    (P.FirstName + ' ' + P.SecondName + ' ' +
                     CASE 
                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN ''
                        ELSE P.ThirdName + ' ' 
                     END +
                     P.LastName) AS FullName,
                    TA.AppointmentDate,
                    (
                        SELECT COUNT(*) 
                        FROM TestAppointments TA2 
                        WHERE TA2.TestTypeID = @TestType
                          AND TA2.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                    ) AS Trial,
                    (
						Select AT2.ApplicationFees From ApplicationTypes AT2
					    Where AT2.ApplicationTypeID = @AppType
					)AS R_Take_AppFees
                FROM LocalDrivingLicenseApplications L
                INNER JOIN Applications A
                    ON L.ApplicationID = A.ApplicationID
                INNER JOIN People P
                    ON A.ApplicantPersonID = P.PersonID
                INNER JOIN LicenseClasses LC
                    ON L.LicenseClassID = LC.LicenseClassID
                LEFT JOIN TestAppointments TA
                    ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
					AND TA.TestTypeID = @TestType
				Left join TestTypes TY
					 ON TY.TestTypeID = @TestType
				Where L.LocalDrivingLicenseApplicationID = @LDL_AppID
                ORDER BY TA.TestAppointmentID DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestType", SqlDbType.Int).Value = (int) typeId;
                    command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDL_AppID;
                    command.Parameters.Add("@AppType", SqlDbType.Int).Value = SharedLayer.enums.enApplicationTypes.RetakeTest;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuildTestAppointment_Init_FromReader(reader, out dto, typeId, enMode.Add))
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
                isFound = false;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetTestAppointment_InitializationByLDL_AppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool TryGetTestAppointment_InitializationBy_AppointmentID(int AppointmentID, out DTOTestAppointment_Init dto, enTestType Testtype = enTestType.UnKnown)
        {
            bool isFound = false;
            dto = null;

            string Query = @"SELECT TOP 1
                    TA.TestAppointmentID,
                    L.LocalDrivingLicenseApplicationID,
                    LC.ClassName,
                    TA.IsLocked,
                    TA.TestTypeID,
					TY.TestTypeFees AS TestFees,
                    (P.FirstName + ' ' + P.SecondName + ' ' +
                     CASE 
                        WHEN P.ThirdName IS NULL OR P.ThirdName = '' THEN ''
                        ELSE P.ThirdName + ' ' 
                     END +
                     P.LastName) AS FullName,
                    TA.AppointmentDate,
                    (
                        SELECT COUNT(*) 
                        FROM TestAppointments TA2 
                        WHERE TA2.TestTypeID = TA.TestTypeID
                          AND TA2.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                          AND TA2.IsLocked = 1
                    ) AS Trial,
                    TA.RetakeTestApplicationID AS R_Take_AppID,
                    AT_ReTake.ApplicationFees AS R_Take_AppFees
                
                FROM LocalDrivingLicenseApplications L
                INNER JOIN Applications A
                    ON L.ApplicationID = A.ApplicationID
                INNER JOIN People P
                    ON A.ApplicantPersonID = P.PersonID
                INNER JOIN LicenseClasses LC
                    ON L.LicenseClassID = LC.LicenseClassID
                LEFT JOIN TestAppointments TA
                    ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
				Left JOIN TestTypes TY
					ON TA.TestTypeID = TY.TestTypeID
                LEFT JOIN Applications A_ReTake
                    ON TA.RetakeTestApplicationID = A_ReTake.ApplicationID
                LEFT JOIN ApplicationTypes AT_ReTake
                    ON A_ReTake.ApplicationTypeID = AT_ReTake.ApplicationTypeID
                WHERE TA.TestAppointmentID = @TestAppointmentID
                ORDER BY TA.TestAppointmentID DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = AppointmentID;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuildTestAppointment_Init_FromReader(reader, out dto, Testtype, enMode.Update))
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
                isFound = false;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetTestAppointment_InitializationByLDL_AppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
        public static bool TryGetTestAppointmentByID(int TestAppointment, out DTOTestAppointment dto)
        {
            dto = null;
            bool isFound = false;

            string Query = @"
                        SELECT
                            TA.TestAppointmentID,
                        	L.LocalDrivingLicenseApplicationID,
                            TA.TestTypeID,
                            LC.ClassName,
                            (P.FirstName + ' ' + P.SecondName + ' ' +
                             case 
                        		When P.ThirdName is Null OR P.ThirdName = '' 
                        		Then ''
                        		else P.ThirdName + ' ' end +
                             P.LastName) AS FullName,
                            TA.AppointmentDate,
                            TA.CreatedByUserID,
                            TY.TestTypeFees AS PaidFees,
                        	(
                        		Select Count (*) 
                        		From TestAppointments TA2 
                        		Where TA2.TestTypeID = TA.TestTypeID
                        		AND TA2.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                        		AND TA2.IsLocked = 1
                        	) AS Trial,
                            TA.IsLocked,
                        	TA.RetakeTestApplicationID AS R_Take_AppID,
                        	AT_ReTake.ApplicationFees AS R_Take_AppFees
                        FROM TestAppointments TA
                        INNER JOIN TestTypes TY
                            ON TA.TestTypeID = TY.TestTypeID
                        INNER JOIN LocalDrivingLicenseApplications L
                            ON TA.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                        Inner Join LicenseClasses LC
                        	on L.LicenseClassID = LC.LicenseClassID
                        INNER JOIN Applications A
                            ON L.ApplicationID = A.ApplicationID
                        INNER JOIN People P
                            ON P.PersonID = A.ApplicantPersonID
                        Left JOIN Applications A_ReTake
                        	ON TA.RetakeTestApplicationID = A_ReTake.ApplicationID
                        Left JOIN ApplicationTypes AT_ReTake
                        	on A_ReTake.ApplicationTypeID = AT_ReTake.ApplicationTypeID
                        Where TA.TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query,connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointment;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_TestAppointmentFromReader(reader, out dto))
                            isFound = true;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                    dto = null;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetTestAppointmentByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return isFound;
            }
        }
        public static bool isTestAppointmentExists(int TestAppointmentID)
        {
            bool isExists = false;
            string Query = @"SELECT CASE
                                WHEN EXISTS (
                                    SELECT 1
                                    FROM TestAppointments
                                    WHERE TestAppointmentID = @TestAppointmentID
                                ) THEN 1
                                ELSE 0
                            END AS TestAppointment_Exists;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;
                    

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        isExists = value == 1;
                    }
                }
            }
            catch(Exception ex)
            {
                isExists = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetTestAppointmentByID]",
                "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isExists;
        }
        public static bool TryGetAppointmentDateByID(int TestAppointmentID, out DateTime? AppointmentDate)
        {
            bool isFound = false;
            AppointmentDate = null;

            string Query = @"select TestAppointments.AppointmentDate 
                             From TestAppointments
                             Where TestAppointmentID = @TestAppointmentID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && DateTime.TryParse(result.ToString(), out DateTime value))
                    {
                        AppointmentDate = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                AppointmentDate = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetAppointmentDateByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool TryGetReTake_AppIDByID(int  TestAppointmentID, out int? ReTake_AppID)
        {
            bool isFound = false;
            ReTake_AppID = null;

            string Query = @"select RetakeTestApplicationID From TestAppointments
                            Where TestAppointmentID = @TestAppointmentID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    ReTake_AppID = clsCommenFunctions.ToNullableINT(result);
                    isFound = true;
                }
            }
            catch(Exception ex)
            {
                isFound = false;
                ReTake_AppID = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[TryGetReTake_AppIDByID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isFound;
        }

        public static bool TryGetParentApplicationIDsBy_TA_ID(int TestAppointmentID, out int? LDL_AppID, out int? SourceApplicationID)
        {
            LDL_AppID = null;
            SourceApplicationID = null;
            bool isFound = false;

            string Query = @"SELECT TA.LocalDrivingLicenseApplicationID, L.ApplicationID
                            FROM LocalDrivingLicenseApplications L
                            INNER JOIN TestAppointments TA
                                ON TA.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                            WHERE TA.TestAppointmentID = @TestAppointmentID;
";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isFound = true;

                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out int value_TA))
                            isFound = false;

                        if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int value_L))
                            isFound = false;

                        if (isFound)
                        {
                            LDL_AppID = value_TA;
                            SourceApplicationID = value_L;
                        }
                        else
                        {
                            LDL_AppID = null;
                            SourceApplicationID = null;
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                LDL_AppID = null;
                SourceApplicationID = null;
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetParentApplicationIDsBy_TA_ID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? Is_TabelTestAppointmentHas_LDLAppID(int LDLAppID)
        {
            bool? isFound_value = null;
            string Query = @"SELECT CASE 
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM TestAppointments TA
                                    WHERE TA.LocalDrivingLicenseApplicationID = @LDLAppID
                                )
                                THEN CAST(1 AS bit)
                                ELSE CAST(0 AS bit)
                            END AS Is_TestAppointmentTabelHas_LDLAppID;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        isFound_value = (bool) result;
                }
            }
            catch(Exception ex)
            {
                isFound_value = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [Is_TabelTestAppointmentHas_LDLAppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }


            return isFound_value;
        }
    }
}
