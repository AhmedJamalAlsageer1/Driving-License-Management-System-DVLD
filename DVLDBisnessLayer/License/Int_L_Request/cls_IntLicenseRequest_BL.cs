using DVLD.DTOs;
using DVLD.DTOs.License;
using DVLD.DTOs.License.Int_License;
using DVLD.DTOs.License.Int_License.DTO_ReNewLocalLicense;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.License.Int_L_Request;
using DVLDBisnessLayer.License.Int_L_Request.Int_License.New_IntLicense;
using DVLDDataAccessLayer.InternationalLicense;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Runtime.Remoting.Messaging;

namespace DVLDBisnessLayer.License
{
    public class  cls_IntLicenseRequest_BL
    {
        private readonly I_IntLicense Current_IntLicense;
        private enMode _Mode;

        public cls_IntLicenseRequest_BL(I_IntLicense I_Int)
        {
            Current_IntLicense = I_Int;
            _Mode = enMode.Add;
        }

        
        public static Result<DTO_IntLicense> Find_IntLicenseByID(int Int_License)
        {
            return cls_IntLicenseService_BL.Find_IntLicenseByID(Int_License);
        }

        public static Result<(int PersonID, int ApplicationID)> GetPersonID_ApplicationIDBy_IntLicenseID(int Int_LicenseID)
        {
            return cls_IntLicenseService_BL.GetPersonID_ApplicationIDBy_IntLicenseID(Int_LicenseID);
        }

        public static Result<(byte, decimal)> TryGetNewInternationalLicenseDefaults(int LocalLicenseID, enApplicationTypes ApplicationType)
        {
            return cls_IntLicenseService_BL.TryGetNewInternationalLicenseDefaults(LocalLicenseID, ApplicationType);
        }

        private OperationResult Add_InternationalLicense()
        {
            
            if (Current_IntLicense.ApplicationType == enApplicationTypes.NewInternationalLicense
                && Current_IntLicense is DTO_IntLicense IntLicense)
            {
                cls_NewIntLicense_BL New_Int = new cls_NewIntLicense_BL(IntLicense);
                return New_Int.Save();
            }

            //else if (Current_IntLicense.ApplicationType == enApplicationTypes.RenewDrivingLicense
            //&& Current_IntLicense is DTO_RenewIntLicense dto_Renew)
            //{
            //    cls_NewIntLicense_BL New_Int = new cls_Re(dto_Renew);
            //    return New_Int.Save();
            //}

            return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Error);
        }

        public OperationResult Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    return Add_InternationalLicense();

                case enMode.Update:
                    return OperationResult.Failure("خطأ غير متوقع، يرجى المحاولة مرة أخرى!", enMessageType.Error);

                default:
                    return OperationResult.TechnicalFailure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

        }
    }

}
