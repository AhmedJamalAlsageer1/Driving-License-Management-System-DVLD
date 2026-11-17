using DVLD.DTOs.License.Int_License;
using DVLD.DTOs.License.Local_License;
using DVLDDataAccessLayer.InternationalLicense;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Int_L_Request
{
    internal class cls_IntLicenseExpirationManager_BL
    {
        public static OperationResult LockLicenseIfExpired(I_IntLicense Int_License)
        {
            if (!Int_License.IsActive)
                return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);

            DateTime Today = DateTime.Now.Date;
            if (Today > Int_License.ExpirationDate.Date)
            {
                OperationResult result_Deactivated = IsDeactivated_IntLicense(Int_License.Int_LicenseID);
                if (!result_Deactivated.IsSuccess)
                    return result_Deactivated;

                Int_License.IsActive = false;
                return result_Deactivated;
            }
            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }

        public static OperationResult LockLicenseIfExpired(int Int_License, DateTime ExpirationDate, ref bool IsActive)
        {
            if (!IsActive)
                return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);

            DateTime Today = DateTime.Now.Date;
            if (Today > ExpirationDate.Date)
            {
                OperationResult result_Deactivated = IsDeactivated_IntLicense(Int_License);
                if (!result_Deactivated.IsSuccess)
                    return result_Deactivated;

                IsActive = false;
                return result_Deactivated;
            }
            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }

        private static OperationResult IsDeactivated_IntLicense(int Int_License)
        {
            bool? result_DeactvateLocalLicense = clsInternationalLicense_DAL.Deactivate_IntLicense(Int_License);
            if (!result_DeactvateLocalLicense.HasValue)
                return OperationResult.Failure("حصل خطأ في قواعد البيانات يرجى المحاولة مرى أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_DeactvateLocalLicense.Value)
                return OperationResult.Failure("هذا الرخصة غير موجودة من فضلك حاول مرة أخرى!", SharedLayer.enums.enMessageType.Warning);

            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }
    }
}
