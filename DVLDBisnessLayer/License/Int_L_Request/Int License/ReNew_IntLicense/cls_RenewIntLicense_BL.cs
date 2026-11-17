using DVLD.DTOs.License.Int_License;
using DVLD.DTOs.License.Int_License.DTO_ReNewLocalLicense;
using DVLDBisnessLayer.License.Int_L_Request.New_Int_License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Int_L_Request.Int_License.ReNew_IntLicense
{
    internal class cls_RenewIntLicense_BL : cls_Base_IntLicense_BL
    {
        public int Old_IntLicenseID { get; set; }

        internal cls_RenewIntLicense_BL(DTO_RenewIntLicense dto)
        {
            DateTime Min_Value = DateTime.MinValue;
            this.Int_LicenseID = -1;
            this.LocalLicenseID = dto.Local_LicenseID;
            this.Old_IntLicenseID = dto.Old_IntLicenseID;
            this.ApplicationID = -1;
            this.ApplicationStatus = SharedLayer.enums.enApplicationStatus.New;
            this.ApplicationTypeID = dto.ApplicationType;
            this.DriverID = dto.DriverID;
            this.ExpirationDate = Min_Value;
            this.ApplicationDate = Min_Value;
            this.IssueDate = Min_Value;
            this.LastStatusDate = Min_Value;
            this.IsActive = false;
            this.PersonID = dto.PersonID;
            this.PaidFees_App = dto.AppFees;
            this.CreatedByUserID = dto.CreatedBy;
        }


        OperationResult ReNew_IntLicense()
        {
            return OperationResult.Failure("لا زال لم يتم بناؤه.", SharedLayer.enums.enMessageType.Error);
        }

        public OperationResult Save()
        {
            return ReNew_IntLicense();
        }
    }
}
