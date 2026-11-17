using DVLD.DTOs.Detain_License;
using DVLD.DTOs.Police_Actions.Detain_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License.Detain_License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.PoliceService.clsRequestPoliceAction
{
    public class cls_RequestPoliceAction_BL
    {
        private readonly I_PoliceRequest _dto;
        enMode _Mode;

        public cls_RequestPoliceAction_BL(I_PoliceRequest dto)
        {
            _dto = dto;
            _Mode = enMode.Add;
        }

        public static bool? isLicenseReleased(int LicenseID)
        {
            return cls_PoliceActionService_BL.isLicenseReleased(LicenseID);
        }

        private OperationResult AddPoliceRequest()
        {
            if(_dto.PoliceActionType == enenPoliceActionType.DetainLicense && 
                _dto is DTO_DetainLicense dto_detainLicense)
            {
                cls_DetainLicense_BL detainLicense_BL = new cls_DetainLicense_BL(dto_detainLicense);
                return detainLicense_BL.Save();
            }

            return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!");
        }

        public OperationResult Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    return AddPoliceRequest();
                case enMode.Update:
                    return OperationResult.TechnicalFailure("وضع التحديث لا زال غير مدعوم!");

                default:
                    return OperationResult.TechnicalFailure("خطأ تقني، يرجى المحاولة مرة أخرى!");
            }
        }

    }
}
