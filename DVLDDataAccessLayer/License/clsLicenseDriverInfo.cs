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
    public static class clsLicenseDriverInfo_DAL
    {
		private static bool Fail(DTOLicenseDriverInfo dto)
		{
			dto = null;
			return false;
		}

		private static bool TryBuild_LicenseDriverInfo(SqlDataReader reader, out DTOLicenseDriverInfo dto)
		{
			dto = null;
			if (reader == null)
				return false;

			if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
				return Fail(dto);

			if (!clsMapReader.TryGetEnum_Int(reader, "LicenseClassID", out enLicenseClass LicenseClassID, enLicenseClass.Unknown))
				return Fail(dto);

            if(!clsMapReader.TryGetStringFromReaderImediately(reader, "FullName", out string FullName))
                return Fail(dto);

			if(!clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out string NationalNo))
                return Fail(dto);

			clsMapReader.TryGetByteFromReaderImediately(reader, "Gendor", out byte Gendor);

			if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "IssueDate", out DateTime IssueDate))
				return Fail(dto);

			string Notes = clsMapReader.TryGetString_NullFromReaderImediately(reader, "Notes");

            if (!clsMapReader.TryGetEnum_Byte(reader, "IssueReason", out enIssueReasonLicense IssueReason, enIssueReasonLicense.UnKnown))
                return Fail(dto);

			bool? IsActive = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsActive");
			if(!IsActive.HasValue)
				return Fail(dto);

			if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DateOfBirth", out DateTime DateOfBirth))
				return Fail(dto);

			if(!clsMapReader.TryGetIntFromReaderImmediately(reader, "DriverID", out int DriverID))
				return Fail(dto);

			if(!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "ExpirationDate", out DateTime ExpirationDate))
				return Fail(dto);

			bool? IsDetained = clsMapReader.TryGetBoolFromReaderImediately(reader, "IsDetained");
			if (!IsDetained.HasValue)
				return Fail(dto);

            string ImagePath = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ImagePath");

			if (Gendor != 1 && Gendor != 0)
				return false;

			dto = new DTOLicenseDriverInfo
			{
				LicenseID = LicenseID,
				LicenseClassID = LicenseClassID,
				FullName = FullName,
				NationalNo = NationalNo,
				Gender = Convert.ToBoolean(Gendor),
				IssueDate = IssueDate,
				Notes = Notes,
				IssueReason = IssueReason,
				IsActive = IsActive.Value,
				DateOfBirth = DateOfBirth,
				DriverID = DriverID,
				ExpirationDate = ExpirationDate,
				IsDetained = IsDetained.Value,
				ImagePath = ImagePath
			};

			return true;
        }

        public static bool? GetLicenseDriverInfoByLicenseID(int LicenseID, out DTOLicenseDriverInfo dto)
        {
            bool? isFound = false;
			dto = null;

			string Query = @"select
								Lic.LicenseID,
								LC.LicenseClassID, 
								(P.FirstName + ' ' + P.SecondName + ' ' + 
									ISNULL(P.ThirdName + ' ', '') + P.LastName
								)AS FullName,
								P.NationalNo,
								Gendor,
								Lic.IssueDate,
								Lic.IssueReason,
								Lic.Notes,
								IsActive,
								P.DateOfBirth,
								Lic.DriverID,
								Lic.ExpirationDate,
									case
									When Exists
										(select 1 From DetainedLicenses DLic
										Where DLic.LicenseID = Lic.LicenseID
										AND DLic.IsReleased = 0
										And DLic.DetainDate = (
											Select MAX(D2.DetainDate) From DetainedLicenses D2
											Where D2.LicenseID = @LicenseID
											)
										)
										Then CAST(1 as bit)
										Else CAST(0 as bit)
									End AS
								IsDetained,
								p.ImagePath
								From Licenses Lic
								Inner Join Applications A
									on Lic.ApplicationID = A.ApplicationID
								Inner Join People P
									on A.ApplicantPersonID = P.PersonID
								Left join LocalDrivingLicenseApplications L
									on L.ApplicationID = A.ApplicationID
								Inner Join LicenseClasses LC
									On Lic.LicenseClass = LC.LicenseClassID
								Where Lic.LicenseID = @LicenseID;";

			try
			{
				using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
				using (SqlCommand command = new SqlCommand(Query, connection))
				{
					command.Parameters.Add("@LicenseID", System.Data.SqlDbType.Int).Value = LicenseID;


					connection.Open();
					SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
					{
						if(TryBuild_LicenseDriverInfo(reader, out dto))
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
				dto = null;
				Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[GetLicenseDriverInfoByLicenseID]",
					"Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
			}

			return isFound;
        }

        public static bool? GetLicenseDriverInfoBy_LDLAppID(int LDL_AppID, out DTOLicenseDriverInfo dto)
		{
            bool? isFound = false;
            dto = null;

            string Query = @"select 
								Lic.LicenseID,
								LC.LicenseClassID, 
								(P.FirstName + ' ' + P.SecondName + ' ' + 
									ISNULL(P.ThirdName + ' ', '') + P.LastName
								)AS FullName,
								P.NationalNo,
								P.Gendor,
								Lic.IssueDate,
								Lic.IssueReason,
								Lic.Notes,
								IsActive,
								P.DateOfBirth,
								Lic.DriverID,
								Lic.ExpirationDate,
									CAST (case
										When Exists
											(select 1 From DetainedLicenses DLic
											Where DLic.LicenseID = Lic.LicenseID
											AND DLic.IsReleased = 0)
											Then 1 
											Else 0
										End AS Bit)
								IsDetained,
								p.ImagePath
								From Licenses Lic
								Inner Join Applications A
									on Lic.ApplicationID = A.ApplicationID
								Inner Join People P
									on A.ApplicantPersonID = P.PersonID
								Left join LocalDrivingLicenseApplications L
									on L.ApplicationID = A.ApplicationID
								Inner Join LicenseClasses LC
									On Lic.LicenseClass = LC.LicenseClassID
								Where L.LocalDrivingLicenseApplicationID = @LDL_AppID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LDL_AppID", System.Data.SqlDbType.Int).Value = LDL_AppID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_LicenseDriverInfo(reader, out dto))
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
                isFound = null;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في هذه الدالة[GetLicenseDriverInfoByLicenseID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID.Value);
            }

            return isFound;
        }
    }
}
