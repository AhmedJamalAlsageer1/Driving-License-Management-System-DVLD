using DVLD.DTOs.License;
using DVLD.DTOs.License.ReNew;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.Rules.LocalLicense;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request.Local_License.ReNew_Local_License
{
    internal class cls_Renew_LocalLicense_BL : cls_LocalLicenseBase
    {
        private DTO_Renew_LocalLicense _dto;
        public int OldLicenseID { get; private set; }
        public decimal LicenseFees { get; private set; }

        public cls_Renew_LocalLicense_BL(DTO_Renew_LocalLicense dto)
        {
            DateTime MinDate = DateTime.MinValue;
            this.LocalLicenseID = -1;
            this.OldLicenseID = dto.OldLicenseID;
            this.LicenseClass = enLicenseClass.Unknown;
            this.ApplicationDate = MinDate;
            this.IssueDate = MinDate;
            this.ExpirationDate = MinDate;
            this.ApplicationStatus = enApplicationStatus.Unknown;
            this.ApplicationTypeID = enApplicationTypes.RenewDrivingLicense;
            this.DriverID = -1;
            this.Notes = string.IsNullOrWhiteSpace(dto.Notes)? null : dto.Notes.Trim();
            this.CreatedByUserID = -1;
            this.PersonID = -1;
            this.IssueReason = enIssueReasonLicense.Renew;
            this.PaidFees_App = dto.ApplicationFees;
            this.LicenseFees = dto.LicenseFees;
            this.IssueDate = dto.IssueDate;
            this.IsActive = false;
            this.CreatedByUserID = -1;
            _dto = dto;
        }


        OperationResult FillObject(DTO_LocalLicense dto_OldLocalLicense)
        {
            Result <int> result_PersonID = clsPerson.GetPersonIDByDriverID(dto_OldLocalLicense.DriverID);
            if (!result_PersonID.isSuccess)
                return OperationResult.Failure(result_PersonID.Message, result_PersonID.messageType);

            Result<(decimal LicenseFees, decimal AppFees)> result_LicenseFees_AppFees = cls_LocalLicenseService_BL.GetLicenseFees_AppFees(dto_OldLocalLicense.LicenseClass, ApplicationTypeID);
            if (!result_LicenseFees_AppFees.isSuccess)
                return OperationResult.Failure(result_LicenseFees_AppFees.Message, result_LicenseFees_AppFees.messageType);

            this.LicenseClass = dto_OldLocalLicense.LicenseClass;
            this.DriverID = dto_OldLocalLicense.DriverID;
            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            this.PersonID = result_PersonID.Value;
            decimal AppFees_UI = this.PaidFees_App;
            decimal LicenseFees_UI = this.LicenseFees;
            this.PaidFees_App = result_LicenseFees_AppFees.Value.AppFees;
            this.LicenseFees = result_LicenseFees_AppFees.Value.LicenseFees;

            if (AppFees_UI != this.PaidFees_App)
                return OperationResult.Failure("تم تعديل رسوم الطلب، يرجى إعادة تحميل الصفحة.");

            if (LicenseFees_UI != this.LicenseFees)
                return OperationResult.Failure("تم تعديل رسوم الرخصة، يرجى إعادة تحميل الصفحة.");

            return OperationResult.Seccuss(null, enMessageType.Info);
        }

        OperationResult IsDetainedLicense()
        {
            bool? result_isDetained = cls_LocalLicenseService_BL.IsDetainedLicense(this.OldLicenseID);
            if (!result_isDetained.HasValue)
                return OperationResult.Failure("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_isDetained.Value)
                return OperationResult.Failure(null, enMessageType.Info);

            return OperationResult.Seccuss("لا يمكنك التجديد لرخصة محجوزة بالفعل!، يرجى فك الحجز أولا!", enMessageType.Warning);
        }

        OperationResult validation(DTO_LocalLicense dto_OldLocalLicense)
        {
            DateTime Today = DateTime.Now.Date;
            if (Today < dto_OldLocalLicense.ExpirationDate.Date.AddDays(- RenewLicenseRules.RenewGracePeriodDays))
                return OperationResult.Failure("لا يمكنك تجديد رخصة لا زالت صلاحيتها بعيدة!", enMessageType.Warning);

            if (Today > dto_OldLocalLicense.ExpirationDate.AddYears(RenewLicenseRules.MaxRenewalYears))
                return OperationResult.Failure("انتهت فترة سماح تجديد الرخصة (عامين)، يجب إنشاء رخصة جديدة.", enMessageType.Warning);

            OperationResult result_isLastLicenseInClass = cls_LocalLicenseService_BL.IsLastLicenseInClass(this.OldLicenseID);
            if (!result_isLastLicenseInClass.IsSuccess)
                return result_isLastLicenseInClass;

            if(!dto_OldLocalLicense.IsActive)
            {
                if (Today <= dto_OldLocalLicense.ExpirationDate.Date)
                    return OperationResult.Failure("لا يمكن تجديد رخصة محجوزة، يجب فك حجزها أولا!", enMessageType.Warning);


                OperationResult result_IsDetaind = IsDetainedLicense();
                if (!result_IsDetaind.HasValue || result_IsDetaind.IsSuccess)
                    return OperationResult.Failure(result_IsDetaind.Message, result_IsDetaind.MessageType);
            }

            return OperationResult.Seccuss(null, enMessageType.Info);
        }

        DTO_Renew_LocalLicense GetDTO()
        {
            DateTime MinDate = DateTime.MinValue;
            return new DTO_Renew_LocalLicense
            {
                LocalLicenseID = -1,
                ApplicationID = -1,
                OldLicenseID = this.OldLicenseID,
                LicenseClass = this.LicenseClass,
                ApplicationDate = MinDate,
                IssueDate = MinDate,
                ExpirationDate = MinDate,
                ApplicationStatus = enApplicationStatus.Unknown,
                ApplicationTypes = this.ApplicationTypeID,
                DriverID = this.DriverID,
                Notes = this.Notes,
                CreatedByUserID = this.CreatedByUserID,
                PersonID = this.PersonID,
                PaidFees = this.PaidFees_App,
                ApplicationFees = this.PaidFees_App,
                IssueReason = this.IssueReason,
                IsActive = false
            };
        }

        OperationResult AddRenewLocalLicense()
        {
            Result<DTO_LocalLicense> result_LocalLicense = cls_LocalLicenseService_BL.FindLicenseByID(this.OldLicenseID);
            if (!result_LocalLicense.isSuccess)
                return OperationResult.Failure(result_LocalLicense.Message, result_LocalLicense.messageType);

            DTO_LocalLicense dto_OldLocalLicense = result_LocalLicense.Value;


            OperationResult result_validation = validation(dto_OldLocalLicense);
            if (!result_validation.IsSuccess)
                return result_validation;

            OperationResult result_FillObject = FillObject(dto_OldLocalLicense);
            if (!result_FillObject.IsSuccess)
                return result_FillObject;

            DTO_Renew_LocalLicense dto_Renew_LocalLicense = GetDTO();

            Result<int> result_validityLength = clsLicenseClassesBL.GetLicenseValidity(this.LicenseClass);
            if (!result_validityLength.isSuccess)
                return OperationResult.Failure(result_validityLength.Message, result_validityLength.messageType);

            int ValidityLength = result_validityLength.Value;

            if(!clsLocalLicense_DAL.AddRenewLocalLicense(dto_Renew_LocalLicense, ValidityLength))
                return OperationResult.Failure("فشلت عملية تجديد الرخصة، يرجى المحاولة مرة أخرى!", enMessageType.Error);


            _dto.IsActive = true;
            if (dto_Renew_LocalLicense.LocalLicenseID == -1)
            {
                return OperationResult.Seccuss("نجحت عملية تجديد الرخصة ولكن فشلت عملية جلب معرف الرخصة الجديدة!", enMessageType.Warning);
            }

            _dto.ApplicationID = dto_Renew_LocalLicense.ApplicationID;
            _dto.LocalLicenseID = dto_Renew_LocalLicense.LocalLicenseID;
            return OperationResult.Seccuss("نحجت عملية تجديد الرخصة، معرف الرخصة الجديد هو " + _dto.LocalLicenseID + "!", enMessageType.Info);
        }

        public OperationResult Save()
        {
            return AddRenewLocalLicense();
        }
    }
}
