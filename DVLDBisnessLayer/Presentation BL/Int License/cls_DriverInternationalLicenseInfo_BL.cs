using DVLD.DTOs.International_License;
using DVLD.DTOs.License.LicensesHistory;
using DVLDBisnessLayer.License.Local_L_Request;
using DVLDDataAccessLayer.InternationalLicense;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.InternationalLicense
{
    public static class cls_DriverInternationalLicenseInfo_BL
    {
        public static Result<DTO_DriverInternationalLicenseInfo> GetDriverInternationalLicenseInfo(int Int_LicneseID)
        {
            bool? result_DriverInternationalLicense = clsDriverInternationalLicenseInfo_DAL.
                GetDriverInternationalLicenseInfo(Int_LicneseID, out DTO_DriverInternationalLicenseInfo dto);

            if (!result_DriverInternationalLicense.HasValue)
                return Result<DTO_DriverInternationalLicenseInfo>.Failure(null, "حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            else if (!result_DriverInternationalLicense.Value || dto == null)
                return Result<DTO_DriverInternationalLicenseInfo>.Failure(null, "هذه الرخصة غير موجودة من أدخل رخصة موجودة!", SharedLayer.enums.enMessageType.Warning);


            bool isActive = dto.IsActive;
            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.LicenseID, dto.ExpirationDate, ref isActive);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_DriverInternationalLicenseInfo>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

            dto.IsActive = isActive;
            return Result<DTO_DriverInternationalLicenseInfo>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }
    }
}
