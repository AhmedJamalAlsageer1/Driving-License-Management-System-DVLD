using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Replacement;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Presentation_BL.Local_License.Replacement
{
    public class cls_ApplicationReplacement_Info
    {
        public static Result <DTO_ApplicationReplacement_Info> GetReplacementLicense_Info(int LocalLicenseID)
        {
            bool? result_get = clsLocalLicense_DAL.GetReplacementLicenseInfo(LocalLicenseID, out DTO_ApplicationReplacement_Info dto);
            if (!result_get.HasValue)
                return Result<DTO_ApplicationReplacement_Info>.Failure(null, "حصل خطأ في قواعد البيانات، يرحى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_get.Value)
                return Result<DTO_ApplicationReplacement_Info>.Failure(null, "هذه الرخصة غير موجودة، يرحى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Warning);

            return Result<DTO_ApplicationReplacement_Info>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }
    }
}
