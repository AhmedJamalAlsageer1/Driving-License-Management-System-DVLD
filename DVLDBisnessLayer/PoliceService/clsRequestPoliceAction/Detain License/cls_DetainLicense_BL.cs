using DVLD.DTOs.License;
using DVLD.DTOs.Police_Actions.Detain_License;
using DVLDDataAccessLayer.License;
using DVLDDataAccessLayer.PoliceActions;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request.Local_License.Detain_License
{
    public class cls_DetainLicense_BL
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }

        private readonly DTO_DetainLicense _dto;

        public cls_DetainLicense_BL(DTO_DetainLicense dto)
        {
            this.FineFees = dto.FineFees;
            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            this.LicenseID = dto.LicenseID;
            _dto = dto;
        }

        DTO_DetainLicense GetDTO()
        {
            return new DTO_DetainLicense
            {
                LicenseID = this.LicenseID,
                CreatedByUserID = this.CreatedByUserID,
                FineFees = this.FineFees
            };
        }

        private OperationResult IsDetainedLicense()
        {
            bool? result_IsDetained = cls_LocalLicenseService_BL.IsDetainedLicense(LicenseID);

            if (!result_IsDetained.HasValue)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);
            if (!result_IsDetained.Value)
                return OperationResult.Failure(null, SharedLayer.enums.enMessageType.Info);

            return OperationResult.Seccuss("لا يمكن حجز رخصة محجوزة مسبقا!", SharedLayer.enums.enMessageType.Warning);
        }

        private OperationResult IsLicenseActive()
        {
            bool? result_License = cls_LocalLicenseService_BL.isLicenseActive(LicenseID);
            if (!result_License.HasValue)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_License.Value)
                return OperationResult.Failure("لا يمكن حجز رخصة غير مفعلة، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Warning);

            return OperationResult.Seccuss(null);
        }

        OperationResult Validation()
        {
            OperationResult result_IsDetained = IsDetainedLicense();
            if (result_IsDetained.IsSuccess)
                return OperationResult.Failure(result_IsDetained.Message, result_IsDetained.MessageType);

            if (FineFees <= 0)
                return OperationResult.Failure("الغرامة غير صحيحة!", SharedLayer.enums.enMessageType.Warning);

            OperationResult result_IsLicenseActive = IsLicenseActive();
            if (!result_IsLicenseActive.IsSuccess)
                return result_IsLicenseActive;

            return OperationResult.Seccuss(null);
        }

        OperationResult AddDetainedLicense()
        {
            OperationResult result_Validation = Validation();
            if (!result_Validation.IsSuccess)
                return result_Validation;

            DTO_DetainLicense dto = GetDTO();

            bool result_Add = cls_DetainLicense_DAL.AddDetainLicense(dto);
            if (!result_Add)
                return OperationResult.TechnicalFailure("فشلت عملية الحجز، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if (dto.DetainID == -1)
                return OperationResult.Failure("نحجت عملية الإضافة، ولكن تعذر الوصول إلى معرف الحجز!", SharedLayer.enums.enMessageType.Error);

            _dto.DetainID = dto.DetainID;
            return OperationResult.Seccuss("نحجت عملية الحجز، معرف الرخصة هو: " +  dto.DetainID + ".");
        }

        public OperationResult Save()
        {
            return AddDetainedLicense();
        }
    }
}
