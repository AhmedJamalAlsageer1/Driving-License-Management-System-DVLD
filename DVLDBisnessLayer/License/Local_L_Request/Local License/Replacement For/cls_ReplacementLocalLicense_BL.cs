using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License.Replacement_For;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.License.Local_L_Request;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Replacement_For
{
    internal class cls_ReplacementLocalLicense_BL : cls_LocalLicenseBase
    {
        public int OldLicense { get; set; }
        private readonly DTO_Replacement_LocalLicense _dto;
        public cls_ReplacementLocalLicense_BL(DTO_Replacement_LocalLicense dto)
        {
            DateTime Value = DateTime.MinValue;
            this.ApplicationID = -1;
            this.ApplicationDate = Value;
            this.ExpirationDate = Value;
            this.ApplicationStatus = SharedLayer.enums.enApplicationStatus.Unknown;
            this.ApplicationTypeID = dto.ApplicationTypes;
            this.LocalLicenseID = -1;
            this.OldLicense = dto.OldLicenseID;
            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            this.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes;
            this.PersonID = -1;
            this.DriverID = -1;
            this.IssueDate = Value;
            this.IssueReason = dto.IssueReason;
            this.PaidFees_App = dto.PaidFees;
            this.IsActive = false;
            this.LastStatusDate = Value;
            this.LicenseClass = SharedLayer.enums.enLicenseClass.Unknown;
            _dto = dto;
        }

        DTO_Replacement_LocalLicense GetDTO()
        {
            return new DTO_Replacement_LocalLicense
            {
                ExpirationDate = this.ExpirationDate,
                ApplicationTypes = this.ApplicationTypeID,
                OldLicenseID = this.OldLicense,
                CreatedByUserID = this.CreatedByUserID,
                Notes = this.Notes,
                PersonID = this.PersonID,
                DriverID = this.DriverID,
                IssueReason = this.IssueReason,
                PaidFees = this.PaidFees_App,
                LicenseClass = this.LicenseClass
            };
        }

        OperationResult FillObject (DTO_LocalLicense dto_OldLocalLicense)
        {
            this.DriverID = dto_OldLocalLicense.DriverID;

            Result<int> result_Person = clsPerson.GetPersonIDByDriverID(this.DriverID);
            if(!result_Person.isSuccess)
                return OperationResult.Failure(result_Person.Message, result_Person.messageType);


            this.PersonID = result_Person.Value;
            this.ExpirationDate = dto_OldLocalLicense.ExpirationDate;
            this.LicenseClass = dto_OldLocalLicense.LicenseClass;

            return OperationResult.Seccuss(null);
        }

        OperationResult Validation(DTO_LocalLicense dto_OldLocalLicense)
        {
            
            if (!dto_OldLocalLicense.IsActive)
            {
                return OperationResult.Failure("لا يمكنك استبدال رخصة غير مفعلة!", SharedLayer.enums.enMessageType.Warning);
            }

            DateTime Today = DateTime.Now.Date;
            if(Today > dto_OldLocalLicense.ExpirationDate)
            {
                cls_LocalLicenseExpirationManager_BL.LockLicenseIfExpired(dto_OldLocalLicense);
                return OperationResult.Failure("لا يمكن استبدال رخصة قد انتهت رخصتها!", SharedLayer.enums.enMessageType.Warning);
            }

            if(this.IssueReason != enIssueReasonLicense.ReplacementForLost &&
                this.IssueReason != enIssueReasonLicense.ReplacementForDamaged)
            {
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Warning);
            }

            Result<decimal> result_appfees = clsApplicationTypesBL.GetApplicationFees(this.ApplicationTypeID);
            if (!result_appfees.isSuccess)
                return OperationResult.Failure(result_appfees.Message, result_appfees.messageType);

            decimal AppFeesUI = this.PaidFees_App;
            this.PaidFees_App = result_appfees.Value;

            if (AppFeesUI != this.PaidFees_App)
                return OperationResult.Failure("الرسوم المدخلة لا تطابق الرسوم المطلوبة!، يرجى تحديث الصفحة.", SharedLayer.enums.enMessageType.Warning);


            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }

        OperationResult AddReplacementLocalLicense()
        {
            Result<DTO_LocalLicense> result_OldLocalLicense = cls_LocalLicenseService_BL.FindLicenseByID(this.OldLicense);
            if (!result_OldLocalLicense.isSuccess)
                return OperationResult.Failure(result_OldLocalLicense.Message, result_OldLocalLicense.messageType);
            DTO_LocalLicense dto_OldLocalLicense = result_OldLocalLicense.Value;


            OperationResult Result_FillOjbect = FillObject(dto_OldLocalLicense);
            if (!result_OldLocalLicense.isSuccess)
                return OperationResult.Failure(result_OldLocalLicense.Message, result_OldLocalLicense.messageType);


            OperationResult result_Validation = Validation(dto_OldLocalLicense);
            if (!result_Validation.IsSuccess)
                return result_Validation;

            DTO_Replacement_LocalLicense dto = GetDTO();

            bool result_Add = clsLocalLicense_DAL.AddReplacementLicense(dto);
            if (!result_Add)
                return OperationResult.Failure("فشلت عملية التحديث، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);


            if (dto.LocalLicenseID == -1)
                return OperationResult.Failure("نحجت عملية الإضافة ولكن فشلت عملية الحصول على معرف الرخصة الجديدة!", SharedLayer.enums.enMessageType.Warning);


            _dto.LocalLicenseID = dto.LocalLicenseID;
            _dto.ApplicationID = dto.ApplicationID;
            _dto.LicenseClass = dto.LicenseClass;
            return OperationResult.Seccuss("نجحت عملية الإضافة بنجاح، معرف الرخصة الجديدة هو " +  dto.LocalLicenseID);
        }

        public OperationResult Save()
        {
            return AddReplacementLocalLicense();
        }
    }
}
