using DVLD.DTOs.License;
using DVLD.DTOs.License.Int_License;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.License.Int_L_Request.New_Int_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDDataAccessLayer.InternationalLicense;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Int_L_Request
{
    internal class cls_IntLicenseService_BL
    {
        internal static bool HasValidInternationalLicense(int LicenseID, int DriverID,
         enLicenseClass LicenseClass)
        {
            bool? result_HasValid_InternationalLicense = clsInternationalLicense_DAL.HasValidInternationalLicense(LicenseID,
                DriverID, LicenseClass);

            if (!result_HasValid_InternationalLicense.HasValue)
                // نسجلها هنا 
                return false;

            if (!result_HasValid_InternationalLicense.Value)
                return false;

            return true;
        }

        public static Result<(byte, decimal)> TryGetNewInternationalLicenseDefaults(int LicenseID, enApplicationTypes ApplicationType)
        {
            bool? result_TryGetDefaultNewInternationalLicense = clsInternationalLicense_DAL.TryGetNewInternationalLicenseDefaults
                (LicenseID, ApplicationType, out byte Validity, out decimal Fees);

            if (!result_TryGetDefaultNewInternationalLicense.HasValue)
                return Result<(byte, decimal)>.Failure((Validity, Fees), "حصل خطأ أثناء محاولة جلب البيانات المهيئة، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_TryGetDefaultNewInternationalLicense.Value || Validity == 0 || Fees == -1)
                return Result<(byte, decimal)>.Failure((Validity, Fees), "يرجى التأكد من أن المدخلات صحيحة، وحاول مرة أخرى بعد ذلك", enMessageType.Warning);


            return Result<(byte, decimal)>.Success((Validity, Fees), null, enMessageType.Info);
        }

        public static Result<DTO_IntLicense> Find_IntLicenseByID(int I_LicenseID)
        {
            bool? result_Find_IntLicene = clsInternationalLicense_DAL.FindInternationalLicense(I_LicenseID, out DTO_IntLicense dto);
            if (!result_Find_IntLicene.HasValue)
                return Result<DTO_IntLicense>.Failure(null, "حدث خطأ أثناء محاولة الوصول إلى قواعد البيانات", enMessageType.Error);

            if(!result_Find_IntLicene.Value)
                return Result<DTO_IntLicense>.Failure(null, "هذا الرخصة غير موجودة، من فضلك اختر رخصة غيرها!",  enMessageType.Warning);


            OperationResult result_LockLicenseIfExpired = cls_IntLicenseExpirationManager_BL.LockLicenseIfExpired(dto);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_IntLicense>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            return Result<DTO_IntLicense>.Success(dto, null, enMessageType.Info);
        }

        public static Result<cls_Base_IntLicense_BL> Find_ObjectIntLicense(int I_LicenseID)
        {
            bool? result_Find_IntLicene = clsInternationalLicense_DAL.FindInternationalLicense(I_LicenseID, out DTO_IntLicense dto);
            if (!result_Find_IntLicene.HasValue)
                return Result<cls_Base_IntLicense_BL>.Failure(null, "حدث خطأ أثناء محاولة الوصول إلى قواعد البيانات", enMessageType.Error);

            if (!result_Find_IntLicene.Value)
                return Result<cls_Base_IntLicense_BL>.Failure(null, "هذا الرخصة غير موجودة، من فضلك اختر رخصة غيرها!", enMessageType.Warning);

            OperationResult result_LockLicenseIfExpired = cls_IntLicenseExpirationManager_BL.LockLicenseIfExpired(dto);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<cls_Base_IntLicense_BL>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            return Result<cls_Base_IntLicense_BL>.Success(cls_Base_IntLicense_BL.CreateFromDTO(dto), null, enMessageType.Info);
        }

        public static Result<DTO_IntLicenseRequirements> IntLicenseRequirements(int LocalLicenseID, enApplicationTypes applicationTypes)
        {
            bool? isFound = clsInternationalLicense_DAL.IntLicenseRequirements(LocalLicenseID, applicationTypes, out DTO_IntLicenseRequirements dto);
            if (!isFound.HasValue)
                return Result<DTO_IntLicenseRequirements>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            else if (!isFound.Value)
                return Result<DTO_IntLicenseRequirements>.Failure(null, "هذه الرخصة غير موجودة من فضلك اختر غيرها", enMessageType.Warning);

            return Result<DTO_IntLicenseRequirements>.Success(dto, null, enMessageType.Info);
        }

        static internal OperationResult BasicValidation(enLicenseClass licenseClass)
        {
            if (licenseClass == enLicenseClass.HeavyMotorcycleLicense ||
                licenseClass == enLicenseClass.SmallMotorcycle)
                return OperationResult.Failure("هذه الفئة غير مؤهلة للرخصة الدولية يرجى اختيار غيرها");


            return OperationResult.Seccuss(null);
        }

        public static Result<(int PersonID, int ApplicationID)> GetPersonID_ApplicationIDBy_IntLicenseID(int Int_LicenseID)
        {
            bool? result = clsInternationalLicense_DAL.GetPersonID_ApplicationIDBy_IntLicenseID(Int_LicenseID, out int? PersonID, out int? ApplicationID);
            if (!result.HasValue)
            {
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى.", enMessageType.Error);
            }

            if (!result.Value || !PersonID.HasValue || !ApplicationID.HasValue)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "الرخصة العالمية التي أدخلتها غير موجودة، من فضلك اختر رخصة غيرها!", enMessageType.Warning);

            return Result<(int PersonID, int ApplicationID)>.Success((PersonID.Value, ApplicationID.Value), null, enMessageType.Info);
        }
    }
}
