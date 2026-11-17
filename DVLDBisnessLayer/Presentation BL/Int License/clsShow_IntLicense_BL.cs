using DVLD.DTOs.International_License;
using DVLDBisnessLayer.License.Local_L_Request;
using DVLDDataAccessLayer.InternationalLicense;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.InternationalLicense
{
    public static class clsShow_IntLicense_BL
    {
        public static Result<List<DTO_ShowIntLicenses>> ShowAll_intLicenses()
        {
            List<DTO_ShowIntLicenses> intLicenses_Licenses = clsShow_IntLicense_DAL.FindAll_IntLicenses();

            if (intLicenses_Licenses == null)
                return Result<List<DTO_ShowIntLicenses>>.Failure(null, "حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            if (intLicenses_Licenses.Count == 0)
                return Result<List<DTO_ShowIntLicenses>>.Success(intLicenses_Licenses, null, SharedLayer.enums.enMessageType.Info);

            foreach (DTO_ShowIntLicenses dto in intLicenses_Licenses)
            {
                bool isActive = dto.IsActive;
                OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.Int_LicenseID, dto.ExpirationDate, ref isActive);
                if (!result_LockLicenseIfExpired.IsSuccess)
                    return Result<List<DTO_ShowIntLicenses>>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

                dto.IsActive = isActive;
            }

            return Result<List<DTO_ShowIntLicenses>>.Success(intLicenses_Licenses, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTO_ShowIntLicenses> Find_IntLicneseIDBy_IntLicenseID(int IntLicenseID)
        {
            bool? result_Find = clsShow_IntLicense_DAL.Find_IntLicensesByInt_LicenseID(IntLicenseID, out DTO_ShowIntLicenses dto);
            if (!result_Find.HasValue)
                return Result<DTO_ShowIntLicenses>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            if (!result_Find.Value || dto == null)
                return Result<DTO_ShowIntLicenses>.Failure(null, "هذه الرخصة الدولية غير موجودة، يرجى إدخال غيرها!", SharedLayer.enums.enMessageType.Warning);


            bool isActive = dto.IsActive;
            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.Int_LicenseID, dto.ExpirationDate, ref isActive);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_ShowIntLicenses>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            dto.IsActive = isActive;
            return Result<DTO_ShowIntLicenses>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTO_ShowIntLicenses> Find_IntLicneseIDByL_LicenseID(int L_LicenseID)
        {
            bool? result_Find = clsShow_IntLicense_DAL.Find_IntLicensesByL_LicenseID(L_LicenseID, out DTO_ShowIntLicenses dto);
            if (!result_Find.HasValue)
                return Result<DTO_ShowIntLicenses>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            if (!result_Find.Value || dto == null)
                return Result<DTO_ShowIntLicenses>.Failure(null, "هذه الرخصة المحلية غير موجودة، أو أنه لا يوجد رخصة دولية مرتبطة بها، يرجى إدخال غيرها!!", SharedLayer.enums.enMessageType.Warning);


            bool isActive = dto.IsActive;
            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.Int_LicenseID, dto.ExpirationDate, ref isActive);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_ShowIntLicenses>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            dto.IsActive = isActive;
            return Result<DTO_ShowIntLicenses>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTO_ShowIntLicenses> Find_IntLicneseIDBy_ApplicationID(int ApplicationID)
        {
            bool? result_Find = clsShow_IntLicense_DAL.Find_IntLicensesByApplicationID(ApplicationID, out DTO_ShowIntLicenses dto);
            if (!result_Find.HasValue)
                return Result<DTO_ShowIntLicenses>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            if (!result_Find.Value || dto == null)
                return Result<DTO_ShowIntLicenses>.Failure(null, "هذا الطلب غير موجود، يرجى إدخال قيمة غيرها!", SharedLayer.enums.enMessageType.Warning);


            bool isActive = dto.IsActive;
            OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.Int_LicenseID, dto.ExpirationDate, ref isActive);
            if (!result_LockLicenseIfExpired.IsSuccess)
                return Result<DTO_ShowIntLicenses>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);


            dto.IsActive = isActive;
            return Result<DTO_ShowIntLicenses>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<List<DTO_ShowIntLicenses>> Find_ALLIntLicensesBy_DriverID(int DriverID)
        {
            List < DTO_ShowIntLicenses > Int_Licenses = clsShow_IntLicense_DAL.Find_ALLIntLicensesBy_DriverID(DriverID);
            if (Int_Licenses == null)
                return Result<List<DTO_ShowIntLicenses>>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            if (Int_Licenses.Count == 0)
                return Result<List<DTO_ShowIntLicenses>>.Failure(null, "هذا السائق إما أنه غير موجود، أو أنه لا رخصة له، يرجى إدخال شخص آخر!!", SharedLayer.enums.enMessageType.Warning);


            foreach (DTO_ShowIntLicenses dto in Int_Licenses)
            {
                bool isActive = dto.IsActive;
                OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.Int_LicenseID, dto.ExpirationDate, ref isActive);
                if (!result_LockLicenseIfExpired.IsSuccess)
                    return Result<List<DTO_ShowIntLicenses>>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

                dto.IsActive = isActive;
            }


            return Result <List<DTO_ShowIntLicenses >>.Success(Int_Licenses, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
