using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License;
using DVLD.DTOs.License.Local_License.New_Local_License;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDBisnessLayer.License.Local_L_Request.Local_License
{
    internal class cls_LocalLicenseService_BL
    {
        public static bool? IsDetainedLicense(int localLicenseID)
        {
             return clsLocalLicense_DAL.IsDetainedLicense(localLicenseID);
        }

        public static bool? isLicenseActive(int LicenseID)
        {
            return clsLocalLicense_DAL.IsLicenseActive(LicenseID);
        }

        public static Result <DTO_NewLicenseRequirements_BL> GetPreviousLicenseInfo(int DriverID ,enLicenseClass LicenseClass)
        {
            bool? result_GetPreviousLicenseInfo = clsLocalLicense_DAL.GetPreviousLicenseInfo(DriverID, LicenseClass, out DTO_NewLicenseRequirements_BL dto);
            if (!result_GetPreviousLicenseInfo.HasValue)
                return Result<DTO_NewLicenseRequirements_BL>.TechnicalFailure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_GetPreviousLicenseInfo.Value)
                return Result<DTO_NewLicenseRequirements_BL>.Failure(null, "لا يوجد رخصة سابقة لهذا السائق من هذه الفئة", enMessageType.Warning);

            return Result<DTO_NewLicenseRequirements_BL>.Success(dto, null, enMessageType.Info);
        }

        public static Result <(int PersonID, int ApplicationID)> GetPersonID_ApplicationIDBy_IntLicenseID(int LocalLicenseID)
        {
            bool? result_get = clsLocalLicense_DAL.GetPersonID_ApplicationID_ByLocalLicenseID(LocalLicenseID, out int ApplicationID, out int PersonID);
            if (!result_get.HasValue)
                return Result<(int PersonID, int ApplicationID)>.TechnicalFailure((-1, -1));
            if (!result_get.Value)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "هذه الرخصة غير موجودة، من فضلك حاول مرة أخرى!", enMessageType.Warning);

            return Result<(int PersonID, int ApplicationID)>.Success((PersonID, ApplicationID), null, enMessageType.Info);
        }
        public static OperationResult IsLastLicenseInClass(int LocalLicenseID)
        {
            bool? result_IsLastLicenseInClass = clsLocalLicense_DAL.IsLastLicenseInClass(LocalLicenseID);
            if (!result_IsLastLicenseInClass.HasValue)
                return OperationResult.Failure("حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_IsLastLicenseInClass.Value)
                return OperationResult.Failure("لا يمكنك التعامل مع رخصة من حيث التجديد أو إلفاء الحجز إذا كان هناك رخصة من حيث الفئة أحدث منها!", enMessageType.Warning);

            return OperationResult.Seccuss(null, enMessageType.Info);
        }

        public static Result<DTO_LocalLicense> FindLicenseByID(int LicenseID)
        {
            bool? Result_License = clsLocalLicense_DAL.FindLicenseByID(LicenseID, out DTO_LocalLicense license);
            if (!Result_License.HasValue)
                return Result<DTO_LocalLicense>.Failure(null, "حصل خطأ في قواعد البيانات", enMessageType.Error);

            if (!Result_License.Value || license == null)
                return Result<DTO_LocalLicense>.Failure(null, "هذا الرخصة غير موجودة، من فضلك اختر واحدة أخرى!", enMessageType.Warning);

            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(license);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_LocalLicense>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

            return Result<DTO_LocalLicense>.Success(license, null, enMessageType.Info);
        }

        internal static Result<clsBaseLicense_BL> FindObjectLicenseByID(int LicenseID)
        {
            bool? Result_License = clsLocalLicense_DAL.FindLicenseByID(LicenseID, out DTO_LocalLicense license);
            if (!Result_License.HasValue)
                return Result<clsBaseLicense_BL>.Failure(null, "حصل خطأ في قواعد البيانات", enMessageType.Error);

            if (!Result_License.Value || license == null)
                return Result<clsBaseLicense_BL>.Failure(null, "هذا الرخصة غير موجودة، من فضلك اختر واحدة أخرى!", enMessageType.Warning);

            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(license);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<clsBaseLicense_BL>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            cls_LocalLicenseBase localLicenseBase = cls_LocalLicenseBase.CreateFromDTO(license);
            return Result<clsBaseLicense_BL>.Success(localLicenseBase, null, enMessageType.Info);
        }

        internal static Result<(decimal LicenseFees, decimal ApplicationFees)> GetLicenseFees_AppFees(enLicenseClass LicenseClass, enApplicationTypes ApplicationType)
        {
            bool? result_getLicenseFees_AppFees = clsLocalLicense_DAL.GetLicenseFees_AppFees(LicenseClass, ApplicationType,
                out decimal LicenseFees, out decimal ApplicationFees);
            if(!result_getLicenseFees_AppFees.HasValue)
            {
                return Result<(decimal LicenseFees, decimal ApplicationFees)>.Failure((-1, -1), "حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرىض", enMessageType.Error);
            }

            if(!result_getLicenseFees_AppFees.Value || LicenseFees == -1 || ApplicationFees == -1)
            {
                return Result<(decimal LicenseFees, decimal ApplicationFees)>.Failure((-1, -1), "خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Warning);
            }

            return Result<(decimal LicenseFees, decimal ApplicationFees)>.Success((LicenseFees, ApplicationFees), null, enMessageType.Info); 
        }

        public static bool? isThereAnActiveLicenseForLicenseClass(int PersonID, enLicenseClass LicenseClassID)
        {
            return clsLocalLicense_DAL.isThereAnActiveLicenseForLicenseClass(PersonID, LicenseClassID);
        }


        public static bool? GetLastLicenseInfo(int PersonID, enLicenseClass licenseClass,
            out DateTime expirationDate, out bool isActive)
        {
            return clsLocalLicense_DAL.GetLastLicenseInfo
                (PersonID, licenseClass, out expirationDate, out isActive);
        }
    }
}
