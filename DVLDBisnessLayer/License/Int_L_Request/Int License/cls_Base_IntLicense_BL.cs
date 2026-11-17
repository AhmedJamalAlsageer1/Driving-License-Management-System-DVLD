using DVLD.DTOs.License;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Int_L_Request.New_Int_License
{
    internal class cls_Base_IntLicense_BL : clsBaseLicense_BL
    {
        public int Int_LicenseID { get; protected set; }

        protected cls_Base_IntLicense_BL()
        {

        }

        internal static cls_Base_IntLicense_BL CreateFromDTO(DTO_IntLicense dto)
        {
            return new cls_Base_IntLicense_BL
            {
                Int_LicenseID = dto.Int_LicenseID,
                ApplicationID = dto.Int_LicenseApplicationID,
                LocalLicenseID = dto.Local_LicenseID,
                ApplicationTypeID = dto.ApplicationType,
                ApplicationStatus = dto.ApplicationStatus,
                ApplicationDate = dto.ApplicationDate,
                LastStatusDate = dto.LastStatusDate,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                CreatedByUserID = dto.CreatedBy,
                DriverID = dto.DriverID,
                PaidFees_App = dto.AppFees,
                IsActive = dto.IsActive
            };
        }
    }
}
