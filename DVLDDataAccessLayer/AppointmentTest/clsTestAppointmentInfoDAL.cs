using DVLD.DTOs.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer.Tests
{
    public static class clsTestAppointmentInfoDAL
    {

        private static bool Fail(out DTODisplayTestAppointment dto)
        {
            dto = null;
            return false;
        }
        private static bool TryBuildDisplayTestAppointmentsFromReader(SqlDataReader reader, out DTODisplayTestAppointment dto)
        {
            dto = null;

            if (reader == null)
                return false;

            if (clsMapReader.TryGetIntFromReaderImmediately(reader, "TestAppointmentID", out int valueint))
            {
                dto = new DTODisplayTestAppointment();
                dto.TestAppointmentID = valueint;
            }
            else
                return Fail(out dto);

            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "AppointmentDate", out DateTime valueDateTime))
                dto.AppointmentDate = valueDateTime;
            else
                return Fail(out dto);

            if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "PaidFees", out decimal valuedecimal))
                dto.PaidFees = valuedecimal;
            else
                return Fail(out dto);

            bool? valuebool = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsLocked");
            if (valuebool.HasValue)
            {
                dto.IsLocked = valuebool.Value;
            }
            else
                return Fail(out dto);

            return true;
        }
        public static bool FindTestActiveAppointment(int LDLAppID, SharedLayer.enums.enTestType testType, out DTODisplayTestAppointment ActiveAppointment)
        {
            bool isFound = false;
            ActiveAppointment = null;

            string Query = @"SELECT TOP 1 
                                   TestAppointmentID, 
                                   AppointmentDate, 
                                   PaidFees, 
                                   IsLocked
                            FROM   TestAppointments
                            WHERE  IsLocked = 0 
                              AND  LocalDrivingLicenseApplicationID = @LDLAppID
                              AND  TestTypeID = @TestTypeID
                            ORDER BY AppointmentDate DESC;
                            ";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDLAppID", SqlDbType.Int).Value = LDLAppID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testType;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuildDisplayTestAppointmentsFromReader(reader, out ActiveAppointment))
                        {
                            if(ActiveAppointment != null)
                            {
                                isFound = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                    
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "حدث خطأ أثناء تشغيل هذه الدالة [GetActiveAppointment]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                }
            }

            return isFound;
        }
        public static List <DTODisplayTestAppointment> FindAllTestAppointments(int LDL_AppID, enTestType testType)
        {
            List<DTODisplayTestAppointment> AllTestAppointments = new List<DTODisplayTestAppointment> ();

            string Query = @"select TA.TestAppointmentID, TA.AppointmentDate,
            TA.PaidFees, TA.IsLocked
            From TestAppointments TA
            Where TA.LocalDrivingLicenseApplicationID = @LDL_AppID
            AND TA.TestTypeID = @TestType;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                command.Parameters.Add("@LDL_AppID", SqlDbType.Int).Value = LDL_AppID;
                command.Parameters.Add("@TestType", SqlDbType.Int).Value = testType;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DTODisplayTestAppointment dto;
                        if (TryBuildDisplayTestAppointmentsFromReader(reader, out dto))
                        {
                            AllTestAppointments.Add(dto);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    AllTestAppointments = null;
                    Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في دالة [FindAllTestAppointments]",
                        "خطأ", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser?.UserID ?? 0);
                }

                return AllTestAppointments;
            }
        }

        
    }
}
