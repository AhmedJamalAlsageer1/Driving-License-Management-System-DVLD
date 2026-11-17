using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using DVLDDataAccessLayer.License;
using DVLDDataAccessLayer.PoliceActions;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Presentation_BL.Local_License.Detain_License
{
    public class cls_DetainLicense_Info
    {
        public static Result<DTO_DetainLicense_Info> GetDetainLicense_Info(int DetainID)
        {
            bool? result_Get = cls_DetainLicense_DAL.GetDetainedLicense_Info(DetainID, out DTO_DetainLicense_Info dto);
            if (!result_Get.HasValue)
                return Result<DTO_DetainLicense_Info>.TechnicalFailure(null);

            if (!result_Get.Value || dto == null)
                return Result<DTO_DetainLicense_Info>.Failure(null, "تعذر العثور على معرف الحجز هذا لعرضه، يرجى المحاولة مرة أخرى!");

            return Result<DTO_DetainLicense_Info>.Success(dto, null);
        }
    }
}
