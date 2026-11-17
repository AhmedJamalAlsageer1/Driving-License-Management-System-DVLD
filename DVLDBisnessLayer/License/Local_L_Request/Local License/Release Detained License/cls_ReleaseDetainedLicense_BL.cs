using DVLD.DTOs.License.Local_License.Release_Detained_License;
using DVLD.DTOs.Presention_DTO.Licnese.Detain_License;
using DVLDBisnessLayer.BaseLicenseRequest;
using DVLDBisnessLayer.PoliceService.clsRequestPoliceAction;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request.Local_License.Release_Detained_License
{
    internal class cls_ReleaseDetainedLicense_BL : clsBaseApplication_BL
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public decimal Fines { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }

        private readonly DTO_ReleaseDetainedLicense _dto;

        public cls_ReleaseDetainedLicense_BL(DTO_ReleaseDetainedLicense dto)
        {
            this.LicenseID = dto.LicenseID;
            this.ReleasedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            this.CreatedByUserID = this.ReleasedByUserID.Value;
            this.PaidFees_App = dto.PaidFees;
            this.ApplicationTypeID = dto.ApplicationTypes;
            this.Fines = dto.Fines;
            _dto = dto;
        }

        public DTO_ReleaseDetainedLicense GetDTO()
        {
            return new DTO_ReleaseDetainedLicense
            {
                LicenseID = this.LicenseID,
                ReleasedByUserID = this.ReleasedByUserID,
                PaidFees = this.PaidFees_App,
                Fines = this.Fines,
                ApplicationTypes = this.ApplicationTypeID,
                DetainID = this.DetainID,
                PersonID = this.PersonID,
                CreatedByUserID = this.CreatedByUserID
            };
        }

        OperationResult FillOjbect(DTO_ReleaseLicenseRequirements dto)
        {
            this.DetainID = dto.DetainID;
            this.PersonID = dto.personID;

            decimal AppFeesUI = this.PaidFees_App;
            this.PaidFees_App = dto.Appfees;

            decimal FinesUI = this.Fines;
            this.Fines = dto.Fines;

            if(AppFeesUI != this.PaidFees_App)
                return OperationResult.Failure(
                "لا يوجد تزامن بين البيانات المعروضة على الشاشة والبيانات الفعلية في قاعدة البيانات.",
                SharedLayer.enums.enMessageType.Error);

            if(FinesUI != this.Fines)
                return OperationResult.Failure(
                    "قيمة الغرامات المعروضة لا تتطابق مع القيمة الفعلية المسجلة في قاعدة البيانات.",
                    SharedLayer.enums.enMessageType.Warning);


            return OperationResult.Seccuss(null);
        }

        OperationResult Validation(DTO_ReleaseLicenseRequirements dto)
        {
            if (dto.IsReleased)
                return OperationResult.Failure("هذه الرخصة غير محجوزة.", SharedLayer.enums.enMessageType.Warning);

            if(this.ApplicationTypeID != SharedLayer.enums.enApplicationTypes.ReleaseDetainedDrivingLicsense)
                return OperationResult.Failure("نوع الطلب غير صحيح، يجب أن يكون طلب فك حجز رخصة قيادة.", SharedLayer.enums.enMessageType.Warning);

            return OperationResult.Seccuss(null);
        }

        private OperationResult ReleaseDetainLicense()
        {
            Result<DTO_ReleaseLicenseRequirements> result_get = cls_PoliceActionService_BL.GetReleaseLicenseRequirements(LicenseID, this.ApplicationTypeID);
            if (!result_get.isSuccess)
                return OperationResult.Failure(result_get.Message, result_get.messageType);


            OperationResult result_Fill = FillOjbect(result_get.Value);
            if (!result_Fill.IsSuccess)
                return OperationResult.Failure(result_Fill.Message, result_Fill.MessageType);


            OperationResult result_Validation = Validation(result_get.Value);
            if (!result_get.isSuccess)
                return result_Validation;

            DTO_ReleaseDetainedLicense dto = GetDTO();

            bool result_release = clsLocalLicense_DAL.ReleaseDetainedLicense(dto);
            if (!result_release)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!");

            if (!dto.ReleaseApplicationID.HasValue)
                return OperationResult.Seccuss("نححت عملية الإضافة، ولكن تعذر الوصول إلى معرف طلب الفك!", SharedLayer.enums.enMessageType.Warning);

            _dto.ReleaseApplicationID = dto.ReleaseApplicationID.Value;
            return OperationResult.Seccuss($"تمت عملية فك الحجز بنجاح. رقم الطلب: {_dto.ReleaseApplicationID}");
        }


        public OperationResult Save()
        {
            return ReleaseDetainLicense();
        }
    }
}
