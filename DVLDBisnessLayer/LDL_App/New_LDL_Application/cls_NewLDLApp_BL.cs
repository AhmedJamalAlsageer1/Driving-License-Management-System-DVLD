using DVLD.DTOs;
using DVLDBisnessLayer.BaseLicenseRequest;
using DVLDBisnessLayer.LDL_App;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDBisnessLayer.Validation;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.License;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace DVLDBisnessLayer
{
    internal class cls_NewLDLApp_BL : clsBaseApplication_BL
    {
        public int LDL_AppID { get; protected set; }
        public enLicenseClass LicenseClassID { get; protected set; }
        public int PassedTestCount { get; protected set; }
        enum enMode { Add, Update };
        enMode Mode;
        DTO_LDL_App _dto;

        public cls_NewLDLApp_BL(DTO_LDL_App dto)
        {
            PersonID = dto.PersonID;
            ApplicationID = -1;
            LDL_AppID = -1;
            ApplicationTypeID =  dto.ApplicationTypeID;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.New;
            PaidFees_App = dto.PaidFees;
            CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            LicenseClassID = dto.LicenseClassID;
            PassedTestCount = 0;
            _dto = dto;
            Mode = enMode.Add;
        }
        private DTO_LDL_App GetDTO_LDLApplication()
        {
            DTO_LDL_App dto = new DTO_LDL_App();

            dto.PersonID = PersonID;
            dto.ApplicationID = ApplicationID;
            dto.ApplicationDate = ApplicationDate;
            dto.ApplicationTypeID = ApplicationTypeID;
            dto.ApplicationStatus = ApplicationStatus;
            dto.LastStatusDate = LastStatusDate;
            dto.PaidFees = PaidFees_App;
            dto.CreatedByUserID = CreatedByUserID;
            dto.LDLApplicationID = LDL_AppID;
            dto.LicenseClassID = LicenseClassID;
            dto.PassedTestCount = PassedTestCount;

            return dto;
        }

        private OperationResult isThereAnActiveLDL_AppForLicneseClass()
        {
            bool? result = cls_LDLApplication_Service_BL.isThereAnActiveLDL_AppForLicneseClass(PersonID, LicenseClassID);
            if (result == null)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result.Value)
                return OperationResult.Failure(null, enMessageType.Info);

            return OperationResult.Seccuss("لا يمكنك تقديم طلب جديد لهذه الفئة، لوجود طلب مفتوح قيد المعالجة بالفعل.", enMessageType.Warning);
        }

        private OperationResult isThereAnActiveLicenseForLicenseClass()
        {
            bool? result = cls_LocalLicenseService_BL.isThereAnActiveLicenseForLicenseClass(PersonID, LicenseClassID);
            if (result == null)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result.Value)
                return OperationResult.Failure(null, enMessageType.Info);

            return OperationResult.Seccuss("لا يمكن تقديم طلب جديد لهذه الفئة، لوجود رخصة مفعّلة منها بالفعل.", enMessageType.Warning);
        }

        private Result<(DateTime, bool)> GetLastLicenseInfo()
        {
            bool? result_GetLastLicenseInfo = cls_LocalLicenseService_BL.GetLastLicenseInfo(PersonID, LicenseClassID,
            out DateTime expirationDate, out bool isActive);

            if (!result_GetLastLicenseInfo.HasValue)
                return Result<(DateTime, bool)>.TechnicalFailure((DateTime.MinValue, false));

            if (!result_GetLastLicenseInfo.Value)
                return Result<(DateTime, bool)>.Failure((DateTime.MinValue, false), "خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Error);


            return Result<(DateTime, bool)>.Success((expirationDate, isActive), null, enMessageType.Info);
        }

        private OperationResult ValidateAddNewApplicationRequest()
        {
            if (this.ApplicationTypeID != enApplicationTypes.NewLocalLicense)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Error);


            if (!clsUser.UserExists(CreatedByUserID))
                return OperationResult.Failure("هذا المستخدم غير موجود");

            if (!clsPerson.PersonExists(PersonID))
                return OperationResult.Failure("هذا الشخص غير موجود!", enMessageType.Error);

            if (!clsLicenseClassesBL.IsClassExists(this.LicenseClassID))
                return OperationResult.Failure("هذا النوع من الأصناف غير موجود");

            Result<decimal> result_AppFees = clsApplicationTypesBL.GetApplicationFees(enApplicationTypes.NewLocalLicense);
            if (!result_AppFees.isSuccess)
                return OperationResult.Failure(result_AppFees.Message, result_AppFees.messageType);

            decimal AppFees_UI = this.PaidFees_App;
            PaidFees_App = result_AppFees.Value;

            if (AppFees_UI != PaidFees_App)
                return OperationResult.Failure("لا يمكن متابعة الطلب، لعدم تزامن الرسوم المدفوعة مع الرسوم المحددة.", enMessageType.Warning);

            OperationResult result_isThereAnActiveLDL_AppForLicneseClass = isThereAnActiveLDL_AppForLicneseClass();
            if (result_isThereAnActiveLDL_AppForLicneseClass.IsSuccess)
                return OperationResult.Failure(result_isThereAnActiveLDL_AppForLicneseClass.Message, result_isThereAnActiveLDL_AppForLicneseClass.MessageType);


            //OperationResult result_isThereAnActiveLicenseForLicenseClass = isThereAnActiveLicenseForLicenseClass();
            //if (result_isThereAnActiveLicenseForLicenseClass.IsSuccess)
            //    return OperationResult.Failure(result_isThereAnActiveLicenseForLicenseClass.Message, result_isThereAnActiveLicenseForLicenseClass.MessageType);

            Result<(DateTime, bool)> result_LicenseInfo = GetLastLicenseInfo();
            if (!result_LicenseInfo.HasValue)
                return OperationResult.Failure(result_LicenseInfo.Message, result_LicenseInfo.messageType);

            if(!result_LicenseInfo.isSuccess)
            {
                return OperationResult.Seccuss(null);
            }


            DateTime ExpirationDate = result_LicenseInfo.Value.Item1;
            bool isActive = result_LicenseInfo.Value.Item2;

            if (isActive)
                return OperationResult.Failure("لا يمكن إصدار رخصة جديدة لهذه الفئة، لوجود رخصة فعالة مسبقًا.", enMessageType.Warning);

            if (!isActive)
            {
                if (ExpirationDate <= DateTime.Now.Date.AddYears(Rules.LocalLicense.NewLocalLicenseRules.MinYearsAfterExpirationForNew))
                    return OperationResult.Failure("لا يمكنك إنشاء طلب جديد لرخصة قابلة للتجديد.", enMessageType.Warning);
            }


            return OperationResult.Seccuss(null);
        }

        private OperationResult AddNewApplication()
        {
            OperationResult ValidationResult = ValidateAddNewApplicationRequest();
            if (!ValidationResult.IsSuccess)
                return ValidationResult;


            DTO_LDL_App dto = GetDTO_LDLApplication();
            if (!cls_LDLApplication_DAL.AddNew_LDL_Applicaton(dto))
                return OperationResult.Failure("حدث خطأ أثناء حفظ الطلب يرجى المحاولة لاحقا.");

            if (dto.ApplicationID == -1 && dto.LDLApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرفي الطلب والرخصة.");

            else if (dto.ApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرف الطلب.");

            else if (dto.LDLApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرف الرخصة المحلية.");


            _dto.ApplicationID = dto.ApplicationID;
            _dto.LDLApplicationID = dto.LDLApplicationID;
            _dto.ApplicationDate = dto.ApplicationDate;
            return OperationResult.Seccuss("تم إضافة طلبك بنجاح.");
        }



        public OperationResult Save()
        {
            return AddNewApplication();
        }

    }
}