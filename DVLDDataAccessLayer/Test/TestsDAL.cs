using DVLD.DTOs.Test;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.Test
{
    public static class clsTestDAL
    {
		//static public bool FindTestByTestID(int TestAppointmentID)
		//{
		//	string Query = @"";
		//}

		public static bool IsTestLinkedToAppointment(int TestAppointmentID)
		{
			bool _isTakenTest = false;

			string Query = @"Select Case
								When Exists(
									Select 1 
									From Tests
									Where TestAppointmentID = @TestAppointmentID
								)Then 0

								Else 1
							END AS isTakenTest;";

			try
			{
				using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
				using (SqlCommand command = new SqlCommand(Query, connection))
				{
					command.Parameters.Add("@TestAppointmentID", System.Data.SqlDbType.Int).Value = TestAppointmentID;


					connection.Open();
					object result = command.ExecuteScalar();
					if(result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
					{
						_isTakenTest = value == 1;
                    }
                }
			}
			catch (Exception ex)
			{
				_isTakenTest = false;
				Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [IsTestLinkedToAppointment]",
					"Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
			}

			return _isTakenTest;
        }

        public static bool? HasPassedTestBefore(int LDL_AppID, enTestType testType)
        {
            bool? PassedTestBefore = null;

            string Query = @"select case
	                            When Exists
	                            	(
	                            	Select 1 From LocalDrivingLicenseApplications L
	                            	Inner Join TestAppointments TA
	                            		On L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
	                            	Inner Join Tests T
	                            		ON T.TestAppointmentID = TA.TestAppointmentID
	                            	Where L.LocalDrivingLicenseApplicationID = @LDL_AppID
	                            	AND TA.TestTypeID = @TestTypeID
	                            	AND T.TestResult = 1
	                            	)Then 1
	                            	else 0
	                            	End As HasPassedTestBefore;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDL_AppID;
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = (int)testType;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int value))
                    {
                        PassedTestBefore = value == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                PassedTestBefore = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [HasPassedTestBefore]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return PassedTestBefore;
        }
        public static bool TakeTest(DTOTest test, bool TakeTest_NOW)
        {
			bool isAdded = false;
            string UpdateDate = TakeTest_NOW ? "AppointmentDate = GETDATE(), " : "";

            string Query = $@"SET XACT_ABORT ON;

							Declare @ReTake_AppID INT;
							Declare @NewTestID INT;
							
							Begin TRY
								Begin Transaction;
							
								Insert Into Tests(TestAppointmentID, TestResult, Notes, CreatedByUserID)
								values (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
							
								SET @NewTestID = SCOPE_IDENTITY();
							
								UPDATE TestAppointments
							    SET {UpdateDate} IsLocked = 1
							    WHERE TestAppointmentID = @TestAppointmentID;
							
								select @ReTake_AppID = RetakeTestApplicationID
								From TestAppointments
								Where TestAppointmentID = @TestAppointmentID;
							
								if @ReTake_AppID is NOT NULL
								Begin
									Update Applications 
									Set ApplicationStatus = 3,
										LastStatusDate = GETDATE()
									Where ApplicationID = @ReTake_AppID;
								END
							
								commit;
							
								Select @NewTestID AS NewTestID;
							
							End TRY
							Begin CATCH
								IF(@@TRANCOUNT > 0)
									ROLLBACK;
							
								THROW;
							END CATCH;";

			try
			{
				using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
				using (SqlCommand command = new SqlCommand(Query, connection))
				{
					command.Parameters.Add("@TestAppointmentID", System.Data.SqlDbType.Int).Value = test.TestAppointmentID;
                    command.Parameters.Add("@TestResult", System.Data.SqlDbType.Bit).Value = test.TestResult;
					command.Parameters.Add("@Notes", System.Data.SqlDbType.NVarChar, 500).Value = clsCommenFunctions.ToDBNullIfEmpty(test.Notes);
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = test.CreatedByUserID;

					connection.Open();
					object resutl = command.ExecuteScalar();

                    if(resutl != null && resutl != DBNull.Value && int.TryParse(resutl.ToString(), out int _testid))
					{
						test.TestID = _testid;
						isAdded = true;
					}
                }
            }
			catch(Exception ex)
			{
				isAdded = false;
				test.TestID = -1;
				Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ في هذه الدالة [TakeTest]",
					"Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

			return isAdded;
        }

    }
}
