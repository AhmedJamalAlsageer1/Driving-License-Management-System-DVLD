using DVLD.DTOs;
using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License;
using DVLD.DTOs.License.Local_License.Release_Detained_License;
using DVLD.DTOs.License.Local_License.Replacement_For;
using DVLD.DTOs.License.ReNew;
using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.License.Int_L_Request.Int_License.ReNew_IntLicense;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License.Detain_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License.Release_Detained_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License.ReNew_Local_License;
using DVLDBisnessLayer.License.Local_L_Request.New_Local_License;
using DVLDBisnessLayer.License.Replacement_For;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLDBisnessLayer.License
{
    public class cls_LocalLicenseRequest_BL
    {
        private readonly I_LocalLicense _LocalLicense;

        enMode _Mode;
        public cls_LocalLicenseRequest_BL(I_LocalLicense localLicense)
        {
            _LocalLicense = localLicense;
            _Mode = enMode.Add;
        }

        public static Result<(int PersonID, int ApplicationID)> GetPersonID_ApplicationIDBy_IntLicenseID(int LocalLicenseID)
        {
            return cls_LocalLicenseService_BL.GetPersonID_ApplicationIDBy_IntLicenseID(LocalLicenseID);
        }

        private OperationResult Request_AddNewLocalLicense()
        {

            if (_LocalLicense is DTO_LocalLicense DTO_NewLocal && DTO_NewLocal.IssueReason == enIssueReasonLicense.FirstTime)
            {
                cls_New_LocalLicense_BL NewLocalLicense = new cls_New_LocalLicense_BL(DTO_NewLocal);
                return NewLocalLicense.Save();
            }


            else if (_LocalLicense is DTO_Renew_LocalLicense dto_Renew && dto_Renew.IssueReason == enIssueReasonLicense.Renew)
            {
                cls_Renew_LocalLicense_BL RenewLicense = new cls_Renew_LocalLicense_BL(dto_Renew);
                return RenewLicense.Save();
            }

            else if (_LocalLicense is DTO_Replacement_LocalLicense dto_ReplacementLocalLicense && 
                (dto_ReplacementLocalLicense.IssueReason == enIssueReasonLicense.ReplacementForDamaged ||
                dto_ReplacementLocalLicense.IssueReason == enIssueReasonLicense.ReplacementForLost))
            {
                cls_ReplacementLocalLicense_BL ReplacementLocalLicense = new cls_ReplacementLocalLicense_BL(dto_ReplacementLocalLicense);
                return ReplacementLocalLicense.Save();
            }

            else if (_LocalLicense is DTO_ReleaseDetainedLicense dto_release)
            {
                cls_ReleaseDetainedLicense_BL Release_BL = new cls_ReleaseDetainedLicense_BL(dto_release);
                return Release_BL.Save();
            }


            return OperationResult.Failure("خطأ تقني، يرحى المحاولة مرة أخرى", enMessageType.Error);
        }

        public OperationResult Save()
        {
            if (_Mode == enMode.Add)
                return Request_AddNewLocalLicense();
            else if (_Mode == enMode.Update)
                return OperationResult.Failure("لا يوجد خطة لإنشائه، ووصولك له خطأ تقني.", enMessageType.Error);

            return OperationResult.Failure("اختيار غير موجود، يرجى المحاولة مرة أخرى!", enMessageType.Error);
        }
    }
}
