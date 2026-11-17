using DVLD.DTOs.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request.Local_License
{
    internal class cls_LocalLicenseBase : clsBaseLicense_BL
    {
        public string Notes { get; protected set; }
        public enLicenseClass LicenseClass { get; protected set; }
        public enIssueReasonLicense IssueReason { get; protected set; }

        protected cls_LocalLicenseBase()
        {

        }

        internal static cls_LocalLicenseBase CreateFromDTO(DTO_LocalLicense dto)
        {
            return new cls_LocalLicenseBase
            {
                ApplicationID = -1,
                DriverID = dto.DriverID,
                LocalLicenseID = dto.LocalLicenseID,
                IssueDate = DateTime.MinValue,
                ExpirationDate = DateTime.MinValue,
                IsActive = true,
                Notes = dto.Notes,
                LicenseClass = enLicenseClass.Unknown,
                IssueReason = dto.IssueReason
            };
        }
    }
}
