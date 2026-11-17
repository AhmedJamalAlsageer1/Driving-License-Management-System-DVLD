using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using DVLD.DTOs.Presention_DTO.Licnese.Release_Detain;
using DVLDDataAccessLayer.License;
using DVLDDataAccessLayer.PoliceActions;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Presentation_BL.Local_License.Release_Detained_License
{
    public class cls_ReleaseDetainedLicense_Info
    {
        public static Result<DTO_ReleaseDetain_Info> GetReleaseDetainConfig_Info(int LicenseID, enApplicationTypes
            ApplicationType)
        {
            bool? result_get = clsLocalLicense_DAL.GetReleaseDetainConfig_Info(LicenseID, ApplicationType, 
                out DTO_ReleaseDetain_Info dto);
            if (!result_get.HasValue)
                return Result<DTO_ReleaseDetain_Info>.TechnicalFailure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_get.Value || dto == null)
                return Result<DTO_ReleaseDetain_Info>.Failure(null, "الرخصة غير موجودة!", enMessageType.Error);

            if(dto.IsReleased)
            {
                return Result<DTO_ReleaseDetain_Info>.Success
                    (dto, "هذه الرخصة تم فك حجزها مسبقًا. لا توجد إجراءات حجز فعّالة حاليًا.", enMessageType.Warning);
            }

            return Result<DTO_ReleaseDetain_Info>.Success(dto, null);
        }

        public static Result<DTO_ReleaseDetain_Info> GetReleaseDetain_Info(int ReleasedApplicationID)
        {
            bool? result_get = clsLocalLicense_DAL.GetReleaseDetain_Info(ReleasedApplicationID,
                out DTO_ReleaseDetain_Info dto);
            if (!result_get.HasValue)
                return Result<DTO_ReleaseDetain_Info>.TechnicalFailure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_get.Value || dto == null)
                return Result<DTO_ReleaseDetain_Info>.Failure(null, "الرخصة غير موجودة!", enMessageType.Error);

            return Result<DTO_ReleaseDetain_Info>.Success(dto, null);
        }
    }
}
