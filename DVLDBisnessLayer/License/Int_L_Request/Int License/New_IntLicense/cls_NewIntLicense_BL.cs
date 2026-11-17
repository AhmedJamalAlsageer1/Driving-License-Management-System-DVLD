using DVLD.DTOs.License;
using DVLD.DTOs.License.Int_License;
using DVLDBisnessLayer.License.Int_L_Request.New_Int_License;
using DVLDDataAccessLayer.InternationalLicense;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Int_L_Request.Int_License.New_IntLicense
{
    internal class cls_NewIntLicense_BL : cls_Base_IntLicense_BL
    {
        DTO_IntLicense NewIntLicense;

        public cls_NewIntLicense_BL(DTO_IntLicense IntLicense)
        {
            NewIntLicense = IntLicense;
            this.ApplicationID = -1;
            this.Int_LicenseID = -1;
            this.LocalLicenseID = IntLicense.Local_LicenseID;
            this.ApplicationDate = DateTime.MinValue;
            this.LastStatusDate = DateTime.MinValue;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.PersonID = -1;
            this.DriverID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.ApplicationTypeID = IntLicense.ApplicationType;
            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            this.IsActive = false;
            this.PaidFees_App = IntLicense.AppFees;
        }

        private DTO_IntLicense GetDTO()
        {
            return new DTO_IntLicense
            {
                Int_LicenseApplicationID = this.ApplicationID,
                Int_LicenseID = this.Int_LicenseID,
                Local_LicenseID = this.LocalLicenseID,
                ApplicationDate = this.ApplicationDate,
                LastStatusDate = this.LastStatusDate,
                IssueDate = this.IssueDate,
                ExpirationDate = this.ExpirationDate,
                PersonID = this.PersonID,
                DriverID = this.DriverID,
                ApplicationStatus = this.ApplicationStatus,
                ApplicationType = this.ApplicationTypeID,
                CreatedBy = this.CreatedByUserID,
                IsActive = this.IsActive,
                AppFees = this.PaidFees_App
            };
        }

        private OperationResult FillObject(out DTO_IntLicenseRequirements IntLicenseRequirements)
        {
            IntLicenseRequirements = null;

            Result<DTO_IntLicenseRequirements> result_IntLicenseRequirements = cls_IntLicenseService_BL.IntLicenseRequirements(this.LocalLicenseID, enApplicationTypes.NewInternationalLicense);
            if (!result_IntLicenseRequirements.isSuccess)
                return OperationResult.Failure(null, enMessageType.Error);

            IntLicenseRequirements = result_IntLicenseRequirements.Value;


            decimal AppFeesUI = this.PaidFees_App;
            this.PaidFees_App = IntLicenseRequirements.ApplicationFees;
            this.DriverID = IntLicenseRequirements.DriverID;
            this.PersonID = IntLicenseRequirements.PersonID;
            this.IsActive = IntLicenseRequirements.IsActive;
            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;

            if (AppFeesUI != this.PaidFees_App)
            {
                return OperationResult.Failure("رسوم الرخصة العالمية غير متزامن مع ما هو موجود في قواعد البيانات", enMessageType.Warning);
            }

            return OperationResult.Seccuss(null);
        }
        private OperationResult Validation_NewInternationalLicense(DTO_IntLicenseRequirements dto_IntLicenseRequirements)
        {
            OperationResult result_BasicValidation = cls_IntLicenseService_BL.BasicValidation(dto_IntLicenseRequirements.LicenseClass);
            if (!result_BasicValidation.IsSuccess)
                return OperationResult.Failure(result_BasicValidation.Message, result_BasicValidation.MessageType);

            if (!dto_IntLicenseRequirements.IsActive)
                return OperationResult.Failure("لا يمكنك إنشاء نسخة دولية على حساب رخصة محلية غير مفعلة!.", enMessageType.Warning);

            if (dto_IntLicenseRequirements.ExipritionDate < DateTime.Today)
                return OperationResult.Failure("لا يمكن إنشاء رخصة دولية على رخصة محلية منتهية!", enMessageType.Warning);

            bool result_HasValid = cls_IntLicenseService_BL.HasValidInternationalLicense(this.LocalLicenseID,
                this.DriverID, dto_IntLicenseRequirements.LicenseClass);
            if (result_HasValid)
                return OperationResult.Failure("لا يمكنك إنشاء رخصة دولية من هذه الفئة لأنها مفعلة.", enMessageType.Warning);


            return OperationResult.Seccuss(null);
        }

        private OperationResult AddNewInternationalLicense()
        {
            OperationResult result_FIllObject = FillObject(out DTO_IntLicenseRequirements dto_IntLicenseRequirements);
            if (!result_FIllObject.IsSuccess && dto_IntLicenseRequirements == null)
                return OperationResult.Failure(result_FIllObject.Message, result_FIllObject.MessageType);

            OperationResult result_Validation = Validation_NewInternationalLicense(dto_IntLicenseRequirements);
            if (!result_Validation.IsSuccess)
                return result_Validation;


            DTO_IntLicense dto_NewInternationalLicense = GetDTO();

            bool? result_AddNew = clsInternationalLicense_DAL.AddNewInternationalLicense(dto_NewInternationalLicense, dto_IntLicenseRequirements.LicenseValidityLength);
            if (!result_AddNew.HasValue)
            {
                return OperationResult.Failure("حدث خطأ أثناء محاولة إضافة طلب رخصة دولية، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            else if (!result_AddNew.Value)
            {
                return OperationResult.Failure("فشلت عملية إضافة رخصة دولية، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            if (dto_NewInternationalLicense.Int_LicenseID <= 0)
                return OperationResult.Seccuss("نجحت عملية الإضافة ولكن تعذر الحصول على رقم معرفها");


            this.ApplicationID = dto_NewInternationalLicense.Int_LicenseApplicationID;
            NewIntLicense.Int_LicenseApplicationID = this.ApplicationID;

            this.Int_LicenseID = dto_NewInternationalLicense.Int_LicenseID;
            NewIntLicense.Int_LicenseID = this.Int_LicenseID;

            this.ApplicationDate = dto_NewInternationalLicense.ApplicationDate;
            this.LastStatusDate = dto_NewInternationalLicense.LastStatusDate;
            this.IssueDate = dto_NewInternationalLicense.IssueDate;
            this.ExpirationDate = dto_NewInternationalLicense.ExpirationDate;


            return OperationResult.Seccuss("نجحت عملية إضافة رخصة دولية، معرف الرخصة الدولية هو " + dto_NewInternationalLicense.Int_LicenseID, enMessageType.Info);
        }

        public OperationResult Save()
        {
            return AddNewInternationalLicense();
        }
    }
}
