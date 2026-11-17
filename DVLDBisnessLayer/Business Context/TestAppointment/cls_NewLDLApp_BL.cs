using DVLD.DTOs;
using DVLDBisnessLayer.BaseLicenseRequest;
using DVLDBisnessLayer.Validation;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace DVLDBisnessLayer
{
    public class cls_NewLDLApp_BL : clsBaseApplication_BL
    {
        public int LDL_AppID { get; protected set; }
        public decimal PaidFees_Class { get; protected set; }
        public enLicenseClass LicenseClassID { get; protected set; }
        public int PassedTestCount { get; protected set; }
        enum enMode { Add, Update };
        enMode Mode;
        private cls_NewLDLApp_BL()
        {
            PersonID = -1;
            ApplicationID = -1;
            LDL_AppID = -1;
            ApplicationTypeID = enApplicationTypes.UnKnown;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.New;
            PaidFees_Class = -1m;
            CreatedByUserID = -1;
            LicenseClassID = enLicenseClass.Unknown;
            PassedTestCount = -1;

            Mode = enMode.Add;
        }
        public cls_NewLDLApp_BL(DTO_New_LDL_App dto)
        {
            PersonID = dto.PersonID;
            ApplicationID = -1;
            LDL_AppID = -1;
            ApplicationTypeID =  dto.ApplicationTypeID;
            ApplicationStatus = SharedLayer.enums.enApplicationStatus.New;
            PaidFees_Class = dto.PaidFees;
            CreatedByUserID = dto.CreatedByUserID;
            LicenseClassID = dto.LicenseClassID;
            PassedTestCount = -1;
            Mode = enMode.Add;
        }
        internal static cls_NewLDLApp_BL GetCurrentObject(DTO_NewOrEdit_LDLApplications dto)
        {
            cls_NewLDLApp_BL Current = new cls_NewLDLApp_BL();

            Current.PersonID = dto.PersonID;
            Current.ApplicationID = dto.ApplicationID;
            Current.LDL_AppID = dto.LDLApplicationID;
            Current.ApplicationDate = dto.ApplicationDate;
            Current.ApplicationTypeID = dto.ApplicationTypeID;
            Current.ApplicationStatus = dto.ApplicationStatus;
            Current.LastStatusDate = dto.LastStatusDate;
            Current.PaidFees_Class = dto.PaidFees;
            Current.CreatedByUserID = dto.CreatedByUserID;
            Current.LicenseClassID = dto.LicenseClassID;
            Current.PassedTestCount = dto.PassedTestCount;

            Current.Mode = enMode.Update;
            return Current;
        }
        private DTO_NewOrEdit_LDLApplications GetDTOLDLApplication()
        {
            DTO_NewOrEdit_LDLApplications dto = new DTO_NewOrEdit_LDLApplications();

            dto.PersonID = PersonID;
            dto.ApplicationID = ApplicationID;
            dto.ApplicationDate = ApplicationDate;
            dto.ApplicationTypeID = ApplicationTypeID;
            dto.ApplicationStatus = ApplicationStatus;
            dto.LastStatusDate = LastStatusDate;
            dto.PaidFees = PaidFees_Class;
            dto.CreatedByUserID = CreatedByUserID;
            dto.LDLApplicationID = LDL_AppID;
            dto.LicenseClassID = LicenseClassID;
            dto.PassedTestCount = PassedTestCount;

            return dto;
        }
        public static OperationResult CanChangeStatusTo(enApplicationStatus Currentstatus)
        {
            switch (Currentstatus)
            {
                case enApplicationStatus.New:
                    return OperationResult.Seccuss(null);

                case enApplicationStatus.Cancelled:
                case enApplicationStatus.Completed:
                    return OperationResult.Failure("لا يمكن تعديل حالة طلب مكتمل أو ملغى.");

                case enApplicationStatus.Unknown:
                    return OperationResult.Failure("الوضع الحالي غير معروف.");


                default:
                    return OperationResult.Failure("لا يسمح بتعديل هذه الحالة.");
            }
        }
        private OperationResult ValidateAddNewApplicationRequest()
        {
            if (!clsPerson.PersonExists(PersonID))
                return OperationResult.Failure("المعرف الشخصي الذي أدخلته غير موجود");

            if (!clsUser.UserExists(CreatedByUserID))
                return OperationResult.Failure("هذا المستخدم غير موجود");

            if (!clsLicenseClassesBL.ClassExists(this.LicenseClassID))
                return OperationResult.Failure("هذا النوع من الأصناف غير موجود");

            if (!clsApplicationTypesDAL.IsApplicationTypeExists((enApplicationTypes) ApplicationTypeID))
                return OperationResult.Failure("هذا النوع من الطلبات غير موجود.");

            if (!clsNewOrEdit_LDL_ApplicationsDAL.CanAddNew_LDL_Application(PersonID, LicenseClassID))
                return OperationResult.Failure("طلبك إما أن يكون قيد التنفيذ أو مكتملا، فلا يسمح لك بتقديم مثل هذا الطلب حتى يلغى.");

            return OperationResult.Seccuss("اكتملت عملية الإضافة بنجاح");
        }
        public static List<FieldError> ValidateApplicationFields(DTO_NewOrEdit_LDLApplications dto, string Name_C_PersonID, string Name_C_UserID,
            string Name_C_Classes, string Name_C_Status)
        {
            List<FieldError> errors = new List<FieldError>();

            if (!clsPerson.PersonExists(dto.PersonID))
                errors.Add(new FieldError(Name_C_PersonID, "هذا الشخص غير موجود"));

            if (!clsUser.UserExists(dto.CreatedByUserID))
                errors.Add(new FieldError(Name_C_UserID, "هذا المستخدم غير موجود"));

            if (!clsLicenseClassesBL.ClassExists(dto.LicenseClassID))
                errors.Add(new FieldError(Name_C_Classes, "هذا النوع من فئات القيادة غير موجود"));

            if (!clsApplicationTypesDAL.IsApplicationTypeExists(dto.ApplicationTypeID))
                errors.Add(new FieldError("mbox", "هذا النوع من الطلبات غير موجود"));

            if (!clsNewOrEdit_LDL_ApplicationsDAL.CanAddNew_LDL_Application(dto.PersonID, dto.LicenseClassID))
                errors.Add(new FieldError(Name_C_Status, "نظرًا لكون الطلب الحالي مكتمل أو لا يزال قيد التنفيذ ولم يتم إلغاؤه، فلا يُسمح بتقديم طلب جديد من نفس النوع حتى يتم إلغاؤه.."));

            return errors;
        }

        private OperationResult AddNewApplication()
        {
            OperationResult ValidationResult = ValidateAddNewApplicationRequest();
            if (!ValidationResult.IsSuccess)
                return ValidationResult;


            DTO_NewOrEdit_LDLApplications dto = GetDTOLDLApplication();
            if (!clsNewOrEdit_LDL_ApplicationsDAL.AddNew_LDL_Applicaton(dto))
                return OperationResult.Failure("حدث خطأ أثناء حفظ الطلب يرجى المحاولة لاحقا.");

            if (dto.ApplicationID == -1 && dto.LDLApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرفي الطلب والرخصة.");

            else if (dto.ApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرف الطلب.");

            else if (dto.LDLApplicationID == -1)
                return OperationResult.Seccuss("نحجت العملية ولكن فشل إرجاع معرف الرخصة المحلية.");

            this.ApplicationID = dto.ApplicationID;
            this.LDL_AppID = dto.LDLApplicationID;
            return OperationResult.Seccuss("تم إضافة طلبك بنجاح.");
        }

        public OperationResult TryChangeStatus(enApplicationStatus NewStatus)
        {
            OperationResult ResultChecked = CanChangeStatusTo(this.ApplicationStatus);
            if (!ResultChecked.IsSuccess)
                return ResultChecked;

            this.ApplicationStatus = NewStatus;
            return OperationResult.Seccuss(null);
        }

        private OperationResult Update_Status()
        {
            if (!clsNewOrEdit_LDL_ApplicationsDAL.UpdateApplicationStatus(this.ApplicationID, ApplicationStatus))
                return OperationResult.Failure("فشل تحديث الطلب، يرجى المحاولة مرة أخرى");

            if (clsNewOrEdit_LDL_ApplicationsDAL.FindBy_LDLAppID(LDL_AppID, out DTO_NewOrEdit_LDLApplications dto))
            {
                if (dto.LastStatusDate != null)
                {
                    this.LastStatusDate = dto.LastStatusDate;
                    return OperationResult.Seccuss("تم تحديث الطلب بنجاح");
                }
                else
                {
                    return OperationResult.Seccuss("تم تحديث الطلب، ولكن لم يتم الحصول على تاريخ آخر تعديل.");
                }
            }

            return OperationResult.Seccuss("تم تحديث الطلب، ولكن تعذّر جلب تفاصيل الحالة.");
        }

        public static Result<DTO_NewOrEdit_LDLApplications> FindByLDLApp(int LDLApp)
        {
            if (clsNewOrEdit_LDL_ApplicationsDAL.FindBy_LDLAppID(LDLApp, out DTO_NewOrEdit_LDLApplications dtoApplication))
            {
                if (dtoApplication != null)
                {
                    return Result<DTO_NewOrEdit_LDLApplications>.Success(dtoApplication, "");
                }
                else
                    return Result<DTO_NewOrEdit_LDLApplications>.Failure(dtoApplication, "تم إرجاع الكائن بنجاح ولكنه فارغ.");
            }
            return Result<DTO_NewOrEdit_LDLApplications>.Failure(dtoApplication, "هذا الطلب بهذا المعرف غير موجود.");
        }

        public static cls_NewLDLApp_BL GetByLDLApplication(int LDLApp)
        {
            Result<DTO_NewOrEdit_LDLApplications> result = FindByLDLApp(LDLApp);

            if (!result.isSuccess)
                return null;

            return GetCurrentObject(result.Value);
        }

        public static Result<DTO_NewOrEdit_LDLApplications> FindByAppID(int AppID)
        {
            if (clsNewOrEdit_LDL_ApplicationsDAL.FindBy_AppID(AppID, out DTO_NewOrEdit_LDLApplications dtoApplication))
            {
                if (dtoApplication != null)
                {
                    return Result<DTO_NewOrEdit_LDLApplications>.Success(dtoApplication, "");
                }
                else
                    return Result<DTO_NewOrEdit_LDLApplications>.Failure(dtoApplication, "تم إرجاع الكائن بنجاح ولكنه فارغ.");
            }
            return Result<DTO_NewOrEdit_LDLApplications>.Failure(dtoApplication, "هذا الطلب بهذا المعرف غير موجود.");
        }

        public static cls_NewLDLApp_BL GetByApplicationID(int AppID)
        {
            Result<DTO_NewOrEdit_LDLApplications> result = FindByAppID(AppID);

            if (!result.isSuccess)
                return null;

            return GetCurrentObject(result.Value);
        }

        public OperationResult Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    return AddNewApplication();
                case enMode.Update:
                    return Update_Status();
                default:
                    return OperationResult.Failure("هذا الوضع غير مدعوم");
            }
        }

        public static OperationResult IsLDLApp_Exists(int LDLAppID)
        {
            if (!clsNewOrEdit_LDL_ApplicationsDAL.isLDLApp_Exists(LDLAppID))
                return OperationResult.Failure("الطلب المحدد غير موجود.");

            return OperationResult.Seccuss(null);
        }

        public static OperationResult CanDelete_LDLAppID(int LDL_AppID, enApplicationStatus status)
        {
            if (status == enApplicationStatus.Completed)
                return OperationResult.Failure("لا يمكن حذف طلب مكتمل.");

            if (!clsNewOrEdit_LDL_ApplicationsDAL.CanDeleteLDLAppID(LDL_AppID))
                return OperationResult.Failure("لا يمكن الحذف لأنه مرتبط ببيانات أخرى.");

            return OperationResult.Seccuss(null);
        }

        public OperationResult Delete_LDLAppID()
        {
            OperationResult ExistenceResult = IsLDLApp_Exists(this.LDL_AppID);
            if (!ExistenceResult.IsSuccess)
                return ExistenceResult;

            OperationResult ResultCanBeDeleted = CanDelete_LDLAppID(this.LDL_AppID, this.ApplicationStatus);
            if(!ResultCanBeDeleted.IsSuccess)
                return ResultCanBeDeleted;

            bool isDeleted = clsNewOrEdit_LDL_ApplicationsDAL.Delete_LDLAppByID(this.LDL_AppID);
            if(!isDeleted)
                return OperationResult.Failure("حدث خطأ أثناء حذف الطلب، يرجى المحاولة لاحقا.");

            return OperationResult.Seccuss("تم حذف الطلب بنجاح.");
        }

        public static Result <enApplicationStatus> Get_LDLApplicationStatus(int LDLAppID)
        {
            if (!clsNewOrEdit_LDL_ApplicationsDAL.Get_LDLApplicationStatus(LDLAppID, out enApplicationStatus status) || status == enApplicationStatus.Unknown)
                return Result<enApplicationStatus>.Failure(enApplicationStatus.Unknown, "الطلب غير موجود", enMessageType.Error);

            return Result<enApplicationStatus>.Success(status, null, enMessageType.Info);
        }

        public static Result <int?> GetPersonIDByLDL_AppID(int LDL_AppID)
        {
            if(clsNewOrEdit_LDL_ApplicationsDAL.TryGetPersonIDByLDL_AppID(LDL_AppID, out int? personID))
            {
                if (personID.HasValue)
                    return Result<int?>.Success(personID.Value, null, enMessageType.Info);
            }

            return Result<int?>.Failure(null, "هذا الشخص غير موجود.", enMessageType.Error);
        }

        public static Result <int?> GetPersonIDBy_AppID(int AppID)
        {
            if(clsNewOrEdit_LDL_ApplicationsDAL.TryGetPersonIDBy_AppID(AppID, out int? personID))
            {
                if (personID.HasValue)
                    return Result<int?>.Success(personID.Value, null, enMessageType.Info);

            }

            return Result<int?>.Failure(null, "هذا الشخص غير موجود", enMessageType.Warning);
        }

        public static Result<decimal?> GetLicenseClassFeesRelatedBy_LDLAppID(int LDLAppID)
        {
            if (clsNewOrEdit_LDL_ApplicationsDAL.TryGetLicenseClassFeesRelated_ByLDLAppID(LDLAppID, out decimal? Fees))
            {
                if (!Fees.HasValue || Fees.Value <= 0)
                    return Result<decimal?>.Failure(null, "حصل خطأ أثناء محاولة الحصول على سعر الرخصة", SharedLayer.enums.enMessageType.Error);


                return Result<decimal?>.Success(Fees.Value, null, enMessageType.Info);
            }
            
            return Result<decimal?>.Failure(null, "من فضلك يرجى التأكد من أن الرخصة التي وضعتها صالحة.", SharedLayer.enums.enMessageType.Warning);
        }

        internal static int? GetPassedTestsCount(int LDL_AppID)
        {
            return clsNewOrEdit_LDL_ApplicationsDAL.GetPassedTestsCount(LDL_AppID);
        }

        public static Result<bool?> isCompletedAllTests(int LDL_AppID)
        {
            int? PassedTestCount = GetPassedTestsCount(LDL_AppID);
            if (!PassedTestCount.HasValue)
                return Result<bool?>.Failure(null, "حصل خطأ أنثاء محاولة جلب الحالة، يرجى المحاولة مرة أخرى.",
                    enMessageType.Error);

            // الحصول على أكبر امتحان من enum
            int LastTestType = Enum.GetValues(typeof(enTestType))
                .Cast<enTestType>().Where(e => e != enTestType.UnKnown)
                .Max(e => (int)e);

            if(PassedTestCount.Value != LastTestType)
                return Result<bool?>.Failure(false, "هذا الشخص لم يكمل جميع امتحاناته.",
                enMessageType.Warning);

            return Result<bool?>.Success(true, null);
        }

        public static OperationResult IsApplicationStatus_New(int AppID)
        {
            bool? result_ApplicationStatus = clsNewOrEdit_LDL_ApplicationsDAL.IsApplicationStatus_New(AppID);
            if (!result_ApplicationStatus.HasValue)
                return OperationResult.Failure("حصل خطأ أثناء محاولة معالجة بعض البيانات");

            if (!result_ApplicationStatus.Value)
                return OperationResult.TechnicalFailure("حالة الطلب مغلق أو أنه مكتمل بالفعل");

            return OperationResult.Seccuss(null);
        }

        public static Result<(int PersonID, int ApplicationID)> GetAppID_PersonIDBy_LDLAppID(int LDL_AppID)
        {
            bool? result_Data = clsNewOrEdit_LDL_ApplicationsDAL.TryGetAppID_PersonIDBy_LDLAppID(LDL_AppID, out int? ApplicationID, out int? PersonID);
            if (!result_Data.HasValue)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", enMessageType.Error);

            if (!result_Data.Value)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "طلب الرخصة المحلية غير صالح، من فضلك اختر واحدا آخر!", enMessageType.Warning);

            return Result<(int PersonID, int ApplicationID)>.Success((PersonID.Value, ApplicationID.Value), null, enMessageType.Info);
        }
    }
}