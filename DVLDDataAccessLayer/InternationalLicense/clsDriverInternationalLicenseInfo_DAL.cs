using DVLD.DTOs.International_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.InternationalLicense
{
    public class clsDriverInternationalLicenseInfo_DAL
    {
        private static bool TryBuild_DriverInternationalLicenseInfo(SqlDataReader reader, out DTO_DriverInternationalLicenseInfo dto)
        {
            dto = null;
            if (reader == null)
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "InternationalLicenseID", out int InternationalLicenseID))
                return false;

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string FullName))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "IssuedUsingLocalLicenseID", out int IssuedUsingLocalLicenseID))
                return false;

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out string NationalNo))
                return false;

            bool? Gendor = clsMapReader.TryGetBoolFromReaderImediately(reader, "Gendor");
            if (!Gendor.HasValue)
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "ApplicationID", out int ApplicationID))
                return false;

            bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
            if (!IsActive.HasValue)
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DateOfBirth", out DateTime DateOfBirth))
                return false;

            if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
                return false;

            if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
                return false;

            string ImagePath = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ImagePath");

            dto = new DTO_DriverInternationalLicenseInfo
            {
                Int_LicenseID = InternationalLicenseID,
                LicenseID = IssuedUsingLocalLicenseID,
                FullName = FullName,
                NationalNo = NationalNo,
                Gendor = Gendor.Value,
                IssueDate = IssueDate,
                ApplicationId = ApplicationID,
                IsActive = IsActive.Value,
                DateOfBirth = DateOfBirth,
                ExpirationDate = ExpirationDate,
                DriverID = DriverID,
                ImagePath = ImagePath,
            };

            return true;
        }

        public static bool? GetDriverInternationalLicenseInfo(int Int_LicneseID, out DTO_DriverInternationalLicenseInfo dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select IL.InternationalLicenseID, 
                                P.FirstName + ' ' + P.SecondName + ' ' + 
                                case When P.ThirdName is null Or P.ThirdName = ''
                                	Then Null Else P.ThirdName + ' ' + P.LastName 
                                End As FullName,
                                IL.IssuedUsingLocalLicenseID, P.NationalNo,
                                cast (P.Gendor As Bit) Gendor,
                                IL.IssueDate, IL.ApplicationID,
                                IL.IsActive, cast (P.DateOfBirth As date) AS DateOfBirth,
                                cast (IL.ExpirationDate As date) AS ExpirationDate, IL.DriverID,
                                P.ImagePath
                                From InternationalLicenses IL
                                Inner Join Applications A
                                	On IL.ApplicationID = A.ApplicationID
                                Inner Join People P
                                	On A.ApplicantPersonID = P.PersonID
                                Where IL.InternationalLicenseID = @Int_LicneseID";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@Int_LicneseID", System.Data.SqlDbType.Int).Value = Int_LicneseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        if(TryBuild_DriverInternationalLicenseInfo(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ أثناء تنفيذ الدالة[GetDriverInternationalLicenseInfo]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }
    }
}
