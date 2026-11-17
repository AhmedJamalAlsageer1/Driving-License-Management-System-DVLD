using DVLD.DTOs.License;
using DVLDBisnessLayer.License.Local_L_Request;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License
{
    public class clsLicenseDriverInfo_BL
    {
        public static Result<DTOLicenseDriverInfo> FindLicenseDriverInfoByID(int LicenseID)
        {
            bool? result_GetLicenseInfo = clsLicenseDriverInfo_DAL.GetLicenseDriverInfoByLicenseID(LicenseID, out DTOLicenseDriverInfo dto);
            if(!result_GetLicenseInfo.HasValue)
            {
                return Result<DTOLicenseDriverInfo>.Failure(null, "حصل خطأ أنثاء جلب البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);
            }

            if (!result_GetLicenseInfo.Value || dto == null)
                return Result<DTOLicenseDriverInfo>.Failure(null, "لا يوجد رخصة بهذا المعرف من فضلك اختر واحدة أخرى", SharedLayer.enums.enMessageType.Warning);

            bool isActive = dto.IsActive;
            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.LicenseID, dto.ExpirationDate, ref isActive);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTOLicenseDriverInfo>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

            dto.IsActive = isActive;
            return Result<DTOLicenseDriverInfo>.Success(dto, null);
        }
        public static Result<DTOLicenseDriverInfo> FindLicenseDriverInfoBy_LDLAppID(int LDLAppID)
        {
            bool? result_GetLicenseInfo = clsLicenseDriverInfo_DAL.GetLicenseDriverInfoBy_LDLAppID(LDLAppID, out DTOLicenseDriverInfo dto);
            if (!result_GetLicenseInfo.HasValue)
            {
                return Result<DTOLicenseDriverInfo>.Failure(null, "حصل خطأ أنثاء جلب البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);
            }

            if (!result_GetLicenseInfo.Value || dto == null)
                return Result<DTOLicenseDriverInfo>.Failure(null, "لا يوجد رخصة بهذا المعرف من فضلك اختر واحدة أخرى", SharedLayer.enums.enMessageType.Warning);


            return Result<DTOLicenseDriverInfo>.Success(dto, null);
        }

    }
}
