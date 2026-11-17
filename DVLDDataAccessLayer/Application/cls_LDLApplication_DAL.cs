using DVLD.DTOs;
using DVLD.DTOs.Applications;
using Microsoft.SqlServer.Server;
using SharedLayer;
using SharedLayer.Context;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer
{
    public class cls_LDLApplication_DAL
    {
        public static bool CanAddNew_LDL_Application(int PersonID, enLicenseClass LicenseClassID)
        {
            bool isAllowed = false;

            string Query = @"select
                            	case
                               	When EXists(
                            	Select 1 
                            	    From Applications A Inner join LocalDrivingLicenseApplications L
                            	    on A.ApplicationID = L.ApplicationID
                            	    Where ApplicantPersonID = @ApplicantPersonID
                            	    AND L.LicenseClassID = @LicenseClassID
                            	    AND A.ApplicationStatus IN (1, 3)
                            	)
                            	Then 0
                            	Else 1
                            END AS CanAdd;
                                ";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@ApplicantPersonID", System.Data.SqlDbType.Int).Value = PersonID;
            command.Parameters.Add("@LicenseClassID", System.Data.SqlDbType.Int).Value = (int) LicenseClassID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if(result != null)
                {
                    isAllowed = Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                isAllowed = false;
            }
            finally
            {
                connection.Close();
            }

            return isAllowed;
        }
        public static bool AddNew_LDL_Applicaton(DTO_LDL_App Application)
        {
            bool isAdded = false;

            string Query = @"Begin TRY
                                Begin Transaction;

                                DECLARE @Now DATETIME = GETDATE();
                                
                                Insert Into Applications
                                     (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus,
                                     LastStatusDate, PaidFees, CreatedByUserID)
                                values
                                     (@ApplicantPersonID, @Now, @ApplicationTypeID, @ApplicationStatus,
                                     @Now, @PaidFees, @CreatedByUserID)
                                     
                                Declare @AppID INT = SCOPE_IDENTITY();
                                     
                                Insert Into LocalDrivingLicenseApplications
                                     (ApplicationID, LicenseClassID)
                                values 
                                     (@AppID, @LicenseClassID)
                                DECLARE @LDLAppID INT = SCOPE_IDENTITY();

                                COMMIT TRANSACTION;

                                Select @AppID As ApplicatonID,
                                @LDLAppID AS LocalDrivingLicenseApplicationID,
                                @Now AS ApplicationTimestamp;

                             END TRY
                             BEGIN CATCH
                                     ROLLBACK TRANSACTION;
                                 
                                     THROW;
                             END CATCH;
                            ";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query,connection);
            command.Parameters.Add("@ApplicantPersonID", System.Data.SqlDbType.Int).Value = Application.PersonID;
            command.Parameters.Add("@ApplicationTypeID", System.Data.SqlDbType.Int).Value = Application.ApplicationTypeID;
            command.Parameters.Add("@ApplicationStatus", System.Data.SqlDbType.TinyInt).Value = Application.ApplicationStatus;
            command.Parameters.Add("@PaidFees", System.Data.SqlDbType.SmallMoney).Value = Application.PaidFees;
            command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = Application.CreatedByUserID;
            command.Parameters.Add("@LicenseClassID", System.Data.SqlDbType.Int).Value = Application.LicenseClassID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicatonID", out int value))
                    {
                        Application.ApplicationID = value;
                    }

                    if(clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out value))
                    {
                        Application.LDLApplicationID = value;
                    }

                    if(clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationTimestamp", out DateTime vDate))
                    {
                        Application.ApplicationDate = vDate;
                        Application.LastStatusDate = vDate;
                    }

                    isAdded = true;
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ أثناء تنفيذ هذه الدالة في AddNew_LDL_Applicaton", "خطأ أثناء إضافة طلب رخصة محلية", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                isAdded = false;
            }
            finally
            {
                connection.Close();
            }

            return isAdded;
        }

        public static bool? UpdateApplicationStatus(DTO_LDL_App dto, enApplicationStatus NewStatus)
        {
            bool? isUpdated = false;
            string Query = @"UPDATE Applications
                            SET ApplicationStatus = @Status, LastStatusDate = GETDATE()
                            WHERE ApplicationID = @ID
                            ";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = dto.ApplicationID;
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = NewStatus;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null && result !=  DBNull.Value && DateTime.TryParse(result.ToString(), out DateTime value))
                {
                    dto.LastStatusDate = value;
                    isUpdated = true;
                }
            }
            catch(Exception ex)
            {
                isUpdated = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "دالة 'UpdateApplicationStatus'", "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }
            finally
            {
                connection.Close();
            }

            return isUpdated;
        }

        private static bool TryGetApplicationStatus(SqlDataReader reader, string CurrentCell, out enApplicationStatus? status)
        {
            status = null;
            byte? valueByte = clsCommenFunctions.FromSQLToByte(reader, CurrentCell);

            if (!valueByte.HasValue)
                return false;

            status =  Enum.IsDefined(typeof(enApplicationStatus), valueByte) ?
                (enApplicationStatus)valueByte
                : enApplicationStatus.Unknown;
            return true;
        }

        private static bool TryGetInfoApplicationFromReader(SqlDataReader reader, out DTO_LDL_App dtoApplications)
        {
            if (reader == null)
            {
                dtoApplications = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int valueint))
            {
                dtoApplications = new DTO_LDL_App();
                dtoApplications.ApplicationID = valueint;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicantPersonID", out valueint))
            {
                dtoApplications.PersonID = valueint;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ApplicationDate", out DateTime dateTimeValue))
            {
                dtoApplications.ApplicationDate = dateTimeValue;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationTypeID", out valueint))
            {
                dtoApplications.ApplicationTypeID = (enApplicationTypes) valueint;
            }

            else
            {
                dtoApplications = null;
                return false;
            }

            
            if (clsMapReader.TryGetByteFromReaderImediately(reader, "ApplicationStatus", out byte sbytevalue))
            {
                dtoApplications.ApplicationStatus = (SharedLayer.enums.enApplicationStatus)sbytevalue;
            }
            else
            {
                dtoApplications = null;
                return false;
            }
            

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "LastStatusDate", out dateTimeValue))
            {
                dtoApplications.LastStatusDate = dateTimeValue;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if (TryGetApplicationStatus(reader, "ApplicationStatus", out enApplicationStatus? Status))
            {
                if (Status == null || Status.Value == enApplicationStatus.Unknown)
                {
                    dtoApplications = null;
                    return false;
                }
            }
            else
            {
                dtoApplications.ApplicationStatus = Status.Value;
            }


            if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal Decimalvalue))
                dtoApplications.PaidFees = Decimalvalue;

            else
            {
                dtoApplications = null;
                return false;
            }
            
            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "CreatedByUserID", out valueint))
                dtoApplications.CreatedByUserID = valueint;
            else
            {
                dtoApplications = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseClassID", out valueint))
            {
                if (Enum.IsDefined(typeof(enLicenseClass), valueint))
                    dtoApplications.LicenseClassID = (enLicenseClass)valueint;
                else
                    dtoApplications.LicenseClassID = enLicenseClass.Unknown;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "LocalDrivingLicenseApplicationID", out valueint))
            {
                dtoApplications.LDLApplicationID = valueint;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "PassedTestCount", out valueint))
            {
                dtoApplications.PassedTestCount = valueint;
            }
            else
            {
                dtoApplications = null;
                return false;
            }

            return (dtoApplications != null);
        }

        public static bool? FindBy_LDLAppID(int LDLAppID, out DTO_LDL_App dtoApplicaton)
        {
            bool? isFound = false;
            dtoApplicaton = null;

            string Query = @"select * From vm_Full_LDLApp
                            Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";


            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = LDLAppID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    if(TryGetInfoApplicationFromReader(reader, out dtoApplicaton))
                    {
                        if (dtoApplicaton != null)
                            isFound = true;
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "FindBy_LDLAppID", "خطأ", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                isFound = null;
                dtoApplicaton = null;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool FindBy_AppID(int AppID, out DTO_LDL_App dtoApplicaton)
        {
            bool isFound = false;
            dtoApplicaton = null;

            string Query = @"select * From vm_Full_LDLApp
                            Where ApplicationID = @ApplicationID";


            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = AppID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (TryGetInfoApplicationFromReader(reader, out dtoApplicaton))
                    {
                        if (dtoApplicaton != null)
                            isFound = true;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "FindBy_LDLAppID", "خطأ", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool CanDeleteLDLAppID(int LDLAppID)
        {
            bool canDelete = false;
            string Query = @"Select Case
                                When Exists(
                                Select 1 
                                    From TestAppointments TA
                                    Where TA.LocalDrivingLicenseApplicationID = @LDLAppID
                                )Then 0
                                Else 1
                              END AS CanDelete";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                    canDelete = Convert.ToInt32(result) == 1;
            }
            catch(Exception ex)
            {
                canDelete = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [CanDeleteLDLAppID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return canDelete;
        }
                                          
        public static bool Delete_LDLAppByID(int LDLAppID)
        {
            bool isDeleted = false;

            string Query = @"BEGIN TRY
                                Begin TRANSACTION;

                                DECLARE @Deleted_App INT = 0;

                                Delete A 
                                From Applications A 
                                Inner Join LocalDrivingLicenseApplications L
                                    On A.ApplicationID = L.ApplicationID
                                Where L.LocalDrivingLicenseApplicationID = @LDLAppID;

                                    SET @Deleted_App = @@ROWCOUNT;

                                 IF @Deleted_App = 0
                                 BEGIN
                                     ROLLBACK TRANSACTION;
                                     SELECT 0 AS IsDeleted;
                                     RETURN;
                                 END
                                
                                Commit Transaction
                                SELECT 1 AS IsDeleted;

                            End TRY
                            BEGIN CATCH
                                Rollback Transaction
                                SELECT 0 AS IsDeleted;
                                Throw
                            End CATCH
                            ";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int value))
                        isDeleted = value == 1;
                        
                }
                catch (Exception ex)
                {
                    isDeleted = false;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [DeleteApplicationByLDLAppID]", "Error",
                        ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return isDeleted;
            }
        }

        public static bool isLDLApp_Exists(int LDLAppID)
        {
            bool isFound = false;
            string Query = @"Select case
	                            when exists(
	                            Select 1
	                            	From LocalDrivingLicenseApplications L 
	                            	where L.LocalDrivingLicenseApplicationID = @LDLAppID
	                            	)then 1
	                            	else 0
	                            End As IsExists";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;
                
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int value))
                        isFound = value == 1;
                }
                catch(Exception ex)
                {
                    isFound = false;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [isLDLApp_Exists]", "Error",
                     ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return isFound;
            }
        }

        public static bool isLDLApp_Active(int LDLAppID)
        {
            bool isFound = false;
            string Query = @"Select Cast(
                                case when exists(
	                                Select 1
                                        From  LocalDrivingLicenseApplications AS L
                                        Inner Join Applications AS A
                                        On A.ApplicationID = L.ApplicationID
                                        Where A.ApplicationStatus = 1
                                          AND L.LocalDrivingLicenseApplicationID = @LDLAppID
	                                	)
                                 then 1 else 0
	                         End As bit) As IsActive";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                        isFound = Convert.ToBoolean(result);
                }
                catch (Exception ex)
                {
                    isFound = false;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [isLDLApp_Active]", "Error",
                     ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return isFound;
            }
        }

        public static bool Get_LDLApplicationStatus(int LDLAppID, out enApplicationStatus status)
        {
            status = enApplicationStatus.Unknown;
            bool isFound = false;

            string Query = @"Select A.ApplicationStatus
                            From Applications A
                            Inner join LocalDrivingLicenseApplications L
                                ON A.ApplicationID = L.ApplicationID
                            WHERE L.LocalDrivingLicenseApplicationID = @LDLAppID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte value))
                    {
                        if(Enum.IsDefined(typeof (enApplicationStatus), value))
                        {
                            status = (enApplicationStatus)value;
                            isFound = true;
                        }
                        else
                        {
                            status = enApplicationStatus.Unknown;
                            isFound = false;
                        }
                    }    
                }
                catch (Exception ex)
                {
                    status = enApplicationStatus.Unknown;
                    isFound = false;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [isLDLApp_Active]", "Error",
                     ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return isFound;
            }
        }

        public static bool TryGetPersonIDByLDL_AppID(int LDL_AppID, out int? PersonID)
        {
            bool isFound = false;
            PersonID = null;

            string Query = @"Select A.ApplicantPersonID From Applications A
                            Inner Join LocalDrivingLicenseApplications L
                            On A.ApplicationID = L.ApplicationID
                            Where LocalDrivingLicenseApplicationID = 45";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = LDL_AppID;

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value && int.TryParse(Convert.ToString(result), out int value))
                    {
                        PersonID = value;
                        isFound = true;
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = false;
                PersonID = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [TryGetPersonIDByAppID]", "Error",
                 ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? TryGetPersonIDBy_AppID(int  AppID, out int PersonID)
        {
            bool? isFound = false;
            PersonID = -1;

            string Query = @"select ApplicantPersonID From Applications A
            Where A.ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = AppID;

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int _personID))
                    {
                        isFound = true;
                        PersonID = _personID;
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                PersonID = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetPersonIDBy_AppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? TryGetAppID_PersonIDBy_DriverID(int DriverID, out int? ApplicationID, out int? PersonID)
        {
            bool? isFound = false;
            ApplicationID = null;
            PersonID = null;

            string Query = @"Select LDL.ApplicationID, A.ApplicantPersonID As PersonID
                            From LocalDrivingLicenseApplications LDL
                            Inner Join Applications A
                            	On LDL.ApplicationID = A.ApplicationID
							Inner join Drivers D
								On D.PersonID = A.ApplicantPersonID
							Where D.DriverID = @DriverID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;


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
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                PersonID = null;
                ApplicationID = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [TryGetAppID_PersonIDBy_DriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }


        public static bool? TryGetAppID_PersonIDBy_LDLAppID(int LDLAppID, out int? ApplicationID, out int? PersonID)
        {
            bool? isFound = false;
            ApplicationID = null;
            PersonID = null;

            string Query = @"Select LDL.ApplicationID, A.ApplicantPersonID As PersonID
                            From LocalDrivingLicenseApplications LDL
                            Inner Join Applications A
                            	On LDL.ApplicationID = A.ApplicationID
                            Where LDL.LocalDrivingLicenseApplicationID = @LDL_AppID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDLAppID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if(clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int _PersonID))
                        {
                            if(clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int _ApplicationID))
                            {
                                PersonID = _PersonID;
                                ApplicationID = _ApplicationID;
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                PersonID = null;
                ApplicationID = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [TryGetAppID_PersonIDBy_LDLAppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        public static bool? TryGetLicenseClassFeesRelated_ByLDLAppID(int LDLAppID, out decimal Fees)
        {
            bool? isFound = false;
            Fees = -1m;

            string Query = @"Select Lc.ClassFees From LocalDrivingLicenseApplications L
                            Inner Join LicenseClasses LC
                            	On L.LicenseClassID = LC.LicenseClassID
                            Where L.LocalDrivingLicenseApplicationID = @LDLAppID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal value))
                    {
                        Fees = value;
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Fees = -1;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [TryGetLicenseClassFees_ByLDLAppID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
        public static bool? IsApplicationStatus_New(int AppID)
        {
            bool? IsNew = false;
            string Query = @"select case
                            	When Exists
                            		(Select 1 From Applications A
                            		Where A.ApplicationID = @AppID
                            		AND A.ApplicationStatus = 1
                            		)Then 1
                            		else 0
                            		End As IsApplicationStatusNew;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@AppID", SqlDbType.Int).Value = AppID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        IsNew = value == 1;
                    }
                }
            }
            catch(Exception ex)
            {
                IsNew = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[IsApplicationStatus_New]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsNew;
        }
        public static int? GetPassedTestsCount(int LDL_AppID)
        {
            int? TestPassedCount = null;

            string Query = @"select PassedTestsCount From vm_LDLApplicationsStatus
                            where LocalDrivingLicenseApplicationID = @LDL_AppID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDL_AppID;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int value))
                        TestPassedCount = value;
                }
                catch (Exception ex)
                {
                    TestPassedCount = null;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ أثناء تشغيل هذه الدالة [GetPassedTestsCount]",
                        "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }

                return TestPassedCount;
            }
        }

    }
}
