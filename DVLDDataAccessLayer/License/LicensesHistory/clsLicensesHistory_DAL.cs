using DVLD.DTOs.License.LicensesHistory;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.License.LicensesHistory
{
    public static class clsLicensesHistory_DAL
    {
        private static bool Fail(out DTO_LocalLicenseHistory dto)
        {
            dto = null;
            return false;
        }

        private static bool TryBuild_LocalLicensesHistoryActivily(SqlDataReader reader, out DTO_LocalLicenseHistory dto)
        {
            dto = null;
            if(reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                return Fail(out dto);

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "ClassName", out string ClassName))
                return Fail(out dto);

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return Fail(out dto);

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return Fail(out dto);

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if(!IsActive.HasValue)
                return Fail(out dto);

            dto = new DTO_LocalLicenseHistory
            {
                LicenseID = LicenseID,
                ApplicationID = ApplicationID,
                ClassName = ClassName,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                IsActive = IsActive.Value,
            };

            return true;
        }

        public static List<DTO_LocalLicenseHistory> GetAllActiveLocalLicensesBy_ApplicationID(int ApplicationID)
        {
            List<DTO_LocalLicenseHistory> List_LocalLicenses_Info = new List<DTO_LocalLicenseHistory>();
            string Query = @"SELECT L.LicenseID,
                                   L.ApplicationID,
                                   LC.ClassName,
                                   L.IssueDate,
                                   L.ExpirationDate,
                                   L.IsActive
                            FROM Licenses L
							Inner Join Applications A
								On L.ApplicationID = A.ApplicationID
							Inner Join LicenseClasses LC
								On L.LicenseClass = LC.LicenseClassID
							Where A.ApplicantPersonID = 
                                (
								select A2.ApplicantPersonID From Applications A2
								Where A2.ApplicationID = @ApplicationID
								)
							order by l.IssueDate desc;";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", System.Data.SqlDbType.Int).Value = ApplicationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        DTO_LocalLicenseHistory dto;
                        if (TryBuild_LocalLicensesHistoryActivily(reader, out dto))
                        {
                            if (dto != null)
                                List_LocalLicenses_Info.Add(dto);
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [GetActiveLocalLicensesByDriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
                List_LocalLicenses_Info = null;
            }

            return List_LocalLicenses_Info;
        }

        private static bool TryBuild_LocalLicensesHistoryActivily(SqlDataReader reader, out DTO_InternationalLicenseHistory dto)
        {
            dto = null;

            if (reader == null)
                return false;

            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "InternationalLicenseID", out int InternationalLicenseID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "IssuedUsingLocalLicenseID", out int LicenseID))
                return false;

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if(!IsActive.HasValue)
                return false;


            dto = new DTO_InternationalLicenseHistory
            {
                InternationalLicenseID = InternationalLicenseID,
                UsingLocalLicenseID = LicenseID,
                ApplicationID = ApplicationID,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                IsActive = IsActive.Value,
            };

            return true;
        }

        public static List<DTO_InternationalLicenseHistory> GetAllActiveInternationalLicensesBy_ApplicationID(int LocalApplicationID)
        {
            List<DTO_InternationalLicenseHistory> List_InternationalLicenses_Info = new List<DTO_InternationalLicenseHistory>();
            string Query = @"SELECT IL.InternationalLicenseID,
                                       IL.ApplicationID,
                                       IL.IssuedUsingLocalLicenseID,
                                       IL.IssueDate,
                                       IL.ExpirationDate,
                                       IL.IsActive
                                FROM InternationalLicenses IL
								Inner Join Drivers D
									On IL.DriverID = D.DriverID
								Where IL.DriverID = (
									select DR.DriverID From Drivers DR
                                    Inner Join Applications A
                                    	On Dr.PersonID = A.ApplicantPersonID
                                    Where A.ApplicationID = @ApplicationID
								)
								Order by IL.IssueDate Desc;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@ApplicationID", System.Data.SqlDbType.Int).Value = LocalApplicationID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        DTO_InternationalLicenseHistory dto;
                        if(TryBuild_LocalLicensesHistoryActivily(reader, out dto))
                        {
                            if (dto != null)
                                List_InternationalLicenses_Info.Add(dto);
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                List_InternationalLicenses_Info = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة [GetActiveInternationalLicensesByDriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return List_InternationalLicenses_Info;
        }
    }
}
