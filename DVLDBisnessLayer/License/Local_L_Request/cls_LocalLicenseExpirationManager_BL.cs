using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request
{
    internal class cls_LocalLicenseExpirationManager_BL
    {
        public static OperationResult LockLicenseIfExpired(DTO_LocalLicense LocalLicense)
        {
            if (!LocalLicense.IsActive)
                return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);

            DateTime Today = DateTime.Now.Date;
            if(Today > LocalLicense.ExpirationDate.Date)
            {
                OperationResult result_Deactivated = IsDeactivated_LocalLicense(LocalLicense.LocalLicenseID);
                if (!result_Deactivated.IsSuccess)
                    return result_Deactivated;

                LocalLicense.IsActive = false;
                return result_Deactivated;
            }
            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }

        public static OperationResult LockLicenseIfExpired(int LocalLicenseID, DateTime ExpirationDate, ref bool IsActive)
        {
            if (!IsActive)
                return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);

            DateTime Today = DateTime.Now.Date;
            if (Today > ExpirationDate.Date)
            {
                OperationResult result_Deactivated = IsDeactivated_LocalLicense(LocalLicenseID);
                if (!result_Deactivated.IsSuccess)
                    return result_Deactivated;

                IsActive = false;
                return result_Deactivated;
            }
            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }

        private static OperationResult IsDeactivated_LocalLicense(int LocalLicenseID)
        {
            bool? result_DeactvateLocalLicense =  clsLocalLicense_DAL.Deactivate_LocalLicense(LocalLicenseID);
            if (!result_DeactvateLocalLicense.HasValue)
                return OperationResult.Failure("حصل خطأ في قواعد البيانات يرجى المحاولة مرى أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_DeactvateLocalLicense.Value)
                return OperationResult.Failure("هذا الرخصة غير موجودة من فضلك حاول مرة أخرى!", SharedLayer.enums.enMessageType.Warning);

            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }
    }
}
