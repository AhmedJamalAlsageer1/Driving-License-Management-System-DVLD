using DVLD.DTOs.Police_Actions.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.PoliceActions
{
    public class cls_DetainLicense_DAL
    {
        public static bool? GetDetainedLicense_Info(int DetainID, out DTO_DetainLicense_Info dto)
        {
            bool? isFound = false;
            dto = null;

            string Query = @"select DL.LicenseID, DL.DetainDate,
                            DL.FineFees, U.UserName from DetainedLicenses DL
                            Inner Join Users u
                            	On DL.CreatedByUserID = u.UserID
                            Where DL.DetainID = @DetainID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DetainID", SqlDbType.Int).Value = DetainID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        dto = new DTO_DetainLicense_Info();
                        if (clsMapReader.TryGetIntFromReaderImmediately(reader, "LicenseID", out int LicenseID))
                        {
                            if (clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DetainDate", out DateTime DetainDate))
                            {
                                if (clsMapReader.TryGetDecimalFromReaderImediately(reader, "FineFees", out decimal FineFees))
                                {
                                    if (clsMapReader.TryGetStringFromReaderImediately(reader, "UserName", out string UserName))
                                    {
                                        dto.LicenseID = LicenseID;
                                        dto.DetainID = DetainID;
                                        dto.DetainDate = DetainDate;
                                        dto.FineFees = FineFees;
                                        dto.UserName = UserName;
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
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [GetDetainedLicense_Info]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }


            return isFound;
        }

        public static bool AddDetainLicense(DTO_DetainLicense dto)
        {
            bool isDetained = false;
            string Query = @"SET XACT_ABORT ON;
                             	INSERT INTO DetainedLicenses (
                             	    LicenseID,
                             	    DetainDate,
                             	    FineFees,
                             	    CreatedByUserID,
                             	    IsReleased,
                             	    ReleaseDate,
                             	    ReleasedByUserID,
                             	    ReleaseApplicationID
                             	)
                             	VALUES (
                             	    @LicenseID,
                             	    GetDate(),
                             	    @FineFees,
                             	    @CreatedByUserID,
                             	    0,               
                             	    NULL,            
                             	    NULL,            
                             	    NULL             
                             	);

                                IF @@ROWCOUNT = 0
                                    THROW 50001, 'فشل في إضافة السجل في DetainedLicenses', 1;

                             	Select SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("LicenseID", SqlDbType.Int).Value = dto.LicenseID;
                    command.Parameters.Add("FineFees", SqlDbType.Int).Value = dto.FineFees;
                    command.Parameters.Add("CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;


                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int DetainID))
                    {
                        dto.DetainID = DetainID;
                        isDetained = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isDetained = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في الدالة [AddDetainLicense]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isDetained;
        }
    }
}
