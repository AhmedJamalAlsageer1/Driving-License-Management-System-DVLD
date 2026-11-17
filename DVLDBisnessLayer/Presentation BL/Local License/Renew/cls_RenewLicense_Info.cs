using DVLD.DTOs.License.ReNew;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Renew;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Presentation_BL.Local_License.Renew
{
    public class cls_RenewLicense_Info
    {
        public static Result<DTO_RenewLicense_Config> Find_RenewLicenseConfig_Info(enApplicationTypes ApplicationType, enLicenseClass LicenseClass)
        {
            bool? result_Config = clsLocalLicense_DAL.Get_RenewLicenseConfig(ApplicationType, LicenseClass, out DTO_RenewLicense_Config dto);
            if (!result_Config.HasValue)
                return Result<DTO_RenewLicense_Config>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_Config.Value || dto == null)
                return Result<DTO_RenewLicense_Config>.Failure(null, "المعطيات غير دقيقة، يرجى المحاولة مرة أخر!", enMessageType.Warning);

            return Result<DTO_RenewLicense_Config>.Success(dto, null, SharedLayer.enums.enMessageType.Error);
        }

        public static Result<DTO_RenewLicense_Info> Find_RenewLicense_Info(int LicenseID)
        {
            bool? result_Find = clsLocalLicense_DAL.Find_RenewLicense_Info(LicenseID, out DTO_RenewLicense_Info dto);
            if (!result_Find.HasValue)
                return Result<DTO_RenewLicense_Info>.TechnicalFailure(null);

            if (!result_Find.Value)
                return Result<DTO_RenewLicense_Info>.Failure(null, "تعذر العثور على هذه الرخصة لعرضها.", enMessageType.Warning);

            return Result<DTO_RenewLicense_Info>.Success(dto, null, enMessageType.Info);
        }
    }
}
