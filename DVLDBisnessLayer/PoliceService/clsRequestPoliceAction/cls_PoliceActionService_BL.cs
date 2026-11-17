using DVLD.DTOs.Presention_DTO.Licnese.Detain_License;
using DVLDDataAccessLayer.License;
using DVLDDataAccessLayer.PoliceActions;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.PoliceService.clsRequestPoliceAction
{
    internal class cls_PoliceActionService_BL
    {
        public static Result<DTO_ReleaseLicenseRequirements> GetReleaseLicenseRequirements(int LicenseID, enApplicationTypes ApplicationType)
        {
            bool? result_Get = clsLocalLicense_DAL.GetReleaseLicenseRequirements(LicenseID, ApplicationType,
                out DTO_ReleaseLicenseRequirements dto);
            if (!result_Get.HasValue)
                return Result<DTO_ReleaseLicenseRequirements>.TechnicalFailure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!");

            if (!result_Get.Value)
                return Result<DTO_ReleaseLicenseRequirements>.Failure(null, "هذه الرخصة غير موجودة!", SharedLayer.enums.enMessageType.Warning);

            return Result<DTO_ReleaseLicenseRequirements>.Success(dto, null);
        }

        public static bool? isLicenseReleased(int LicenseID)
        {
            return clsLocalLicense_DAL.isLicenseRelease(LicenseID);
        }
    }
}
