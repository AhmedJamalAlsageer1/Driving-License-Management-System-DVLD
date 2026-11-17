using DVLD.DTOs.International_License;
using DVLD.DTOs.License;
using DVLD.DTOs.License.LicensesHistory;
using DVLDBisnessLayer.License.Int_L_Request;
using DVLDBisnessLayer.License.Local_L_Request;
using DVLDDataAccessLayer.License.LicensesHistory;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.LicencesHistory
{
    public class clsLicensesHistory_BL
    {
        public static Result<List<DTO_LocalLicenseHistory>> GetAllActiveLocalLicenseByDriverID(int ApplicationID)
        {
            List<DTO_LocalLicenseHistory> LocalLicensesHistory = clsLicensesHistory_DAL.GetAllActiveLocalLicensesBy_ApplicationID(ApplicationID);

            if (LocalLicensesHistory == null)
                return Result<List<DTO_LocalLicenseHistory>>.Failure(null, "حصل خطأ في قواعد البيانات", SharedLayer.enums.enMessageType.Error);

            if (LocalLicensesHistory.Count == 0)
                return Result<List<DTO_LocalLicenseHistory>>.Success(LocalLicensesHistory, "لا يوجد بيانات لعرضها", SharedLayer.enums.enMessageType.Info);

            foreach(DTO_LocalLicenseHistory dto in LocalLicensesHistory)
            {
                bool isActive = dto.IsActive;
                OperationResult result_LockLicenseIfExpired = cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto.LicenseID, dto.ExpirationDate, ref isActive);
                if (!result_LockLicenseIfExpired.IsSuccess)
                    return Result<List<DTO_LocalLicenseHistory>>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

                dto.IsActive = isActive;
            }

            return Result<List<DTO_LocalLicenseHistory>>.Success(LocalLicensesHistory, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<List<DTO_InternationalLicenseHistory>> GetAllActiveInternationalLicenseByDriverID(int ApplicationID)
        {
            List<DTO_InternationalLicenseHistory> LocalLicensesHistory = clsLicensesHistory_DAL.GetAllActiveInternationalLicensesBy_ApplicationID(ApplicationID);

            if (LocalLicensesHistory == null)
                return Result<List<DTO_InternationalLicenseHistory>>.Failure(null, "حصل خطأ في قواعد البيانات", SharedLayer.enums.enMessageType.Error);

            if (LocalLicensesHistory.Count == 0)
                return Result<List<DTO_InternationalLicenseHistory>>.Success(LocalLicensesHistory, "لا يوجد بيانات لعرضها", SharedLayer.enums.enMessageType.Info);


            foreach (DTO_InternationalLicenseHistory dto in LocalLicensesHistory)
            {
                bool isActive = dto.IsActive;
                OperationResult result_LockLicenseIfExpired = cls_IntLicenseExpirationManager_BL.LockLicenseIfExpired(dto.InternationalLicenseID, dto.ExpirationDate, ref isActive);
                if (!result_LockLicenseIfExpired.IsSuccess)
                    return Result<List<DTO_InternationalLicenseHistory>>.Failure(null, result_LockLicenseIfExpired.Message, result_LockLicenseIfExpired.MessageType);

                dto.IsActive = isActive;
            }

            return Result<List<DTO_InternationalLicenseHistory>>.Success(LocalLicensesHistory, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
