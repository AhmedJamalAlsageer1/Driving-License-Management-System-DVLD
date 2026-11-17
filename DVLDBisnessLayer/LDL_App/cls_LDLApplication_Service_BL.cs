using DVLD.DTOs;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.LDL_App
{
    internal class cls_LDLApplication_Service_BL
    {
        public static bool? isThereAnActiveLDL_AppForLicneseClass(int PersonID, enLicenseClass LicenseClass)
        {
            return clsLocalLicense_DAL.isThereAnActiveLDL_AppForLicneseClass(PersonID, LicenseClass);
        }


        public static OperationResult TryChangeStatus(DTO_LDL_App CurrentApplication, enApplicationStatus NewStatus)
        {
            OperationResult ResultChecked = CanChangeStatusToCancell(CurrentApplication.ApplicationStatus);
            if (!ResultChecked.IsSuccess)
                return ResultChecked;

            OperationResult result_ChangeStatus = ChangeApplicationStatus(CurrentApplication, NewStatus);
            if(!result_ChangeStatus.IsSuccess)
                return result_ChangeStatus;

            return OperationResult.Seccuss(null);
        }

        private static OperationResult ChangeApplicationStatus(DTO_LDL_App CurrentApplication, enApplicationStatus NewApplicationStatus)
        {
            bool? result_UpdateStatus = cls_LDLApplication_DAL.UpdateApplicationStatus(CurrentApplication, NewApplicationStatus);
            if (!result_UpdateStatus.HasValue)
                return OperationResult.Failure("حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى!", enMessageType.Warning);

            if (!result_UpdateStatus.Value)
                return OperationResult.Failure("فشلت عملية التحديث يرجى المحاولة مرة أخرى!", enMessageType.Warning);

            return OperationResult.Seccuss(null);
        }

        public static OperationResult CanChangeStatusToCancell(enApplicationStatus Currentstatus)
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

        public static Result<DTO_LDL_App> FindBy_LDLAppID(int LDLApp)
        {
            bool? result_Find_LDL_App = cls_LDLApplication_DAL.FindBy_LDLAppID(LDLApp, out DTO_LDL_App dtoApplication);
            if(!result_Find_LDL_App.HasValue)
            {
                Result<DTO_LDL_App>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            if(!result_Find_LDL_App.Value)
            {
                return Result<DTO_LDL_App>.Failure(dtoApplication, "هذه الطلب غير موجود من فضلك، اختر واحدا آخر!", enMessageType.Warning);
            }

            return Result<DTO_LDL_App>.Success(dtoApplication, null, enMessageType.Info);
        }

        public static Result<DTO_LDL_App> FindByAppID(int AppID)
        {
            bool? result_Find_LDL_App = cls_LDLApplication_DAL.FindBy_AppID(AppID, out DTO_LDL_App dtoApplication);
            if (!result_Find_LDL_App.HasValue)
            {
                Result<DTO_LDL_App>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            if (!result_Find_LDL_App.Value)
            {
                return Result<DTO_LDL_App>.Failure(dtoApplication, "هذه الطلب غير موجود من فضلك، اختر واحدا آخر!", enMessageType.Warning);
            }

            return Result<DTO_LDL_App>.Success(dtoApplication, null, enMessageType.Info);
        }

        public static OperationResult IsLDLApp_Exists(int LDLAppID)
        {
            if (!cls_LDLApplication_DAL.isLDLApp_Exists(LDLAppID))
                return OperationResult.Failure("الطلب المحدد غير موجود.");

            return OperationResult.Seccuss(null);
        }

        public static OperationResult CanDelete_LDLAppID(DTO_LDL_App LDL_App)
        {
            if (LDL_App.ApplicationStatus == enApplicationStatus.Completed)
                return OperationResult.Failure("لا يمكن حذف طلب مكتمل.");

            if (!cls_LDLApplication_DAL.CanDeleteLDLAppID(LDL_App.LDLApplicationID))
                return OperationResult.Failure("لا يمكن الحذف لأنه مرتبط ببيانات أخرى.");

            return OperationResult.Seccuss(null);
        }

        public static OperationResult Delete_LDLAppID(DTO_LDL_App LDL_App)
        {
            OperationResult ResultCanBeDeleted = cls_LDLApplication_Service_BL.CanDelete_LDLAppID(LDL_App);
            if (!ResultCanBeDeleted.IsSuccess)
                return ResultCanBeDeleted;

            bool isDeleted = cls_LDLApplication_DAL.Delete_LDLAppByID(LDL_App.LDLApplicationID);
            if (!isDeleted)
                return OperationResult.Failure("حدث خطأ أثناء حذف الطلب، يرجى المحاولة لاحقا.");

            return OperationResult.Seccuss("تم حذف الطلب بنجاح.");
        }
        public static Result<enApplicationStatus> Get_LDLApplicationStatus(int LDLAppID)
        {
            if (!cls_LDLApplication_DAL.Get_LDLApplicationStatus(LDLAppID, out enApplicationStatus status) || status == enApplicationStatus.Unknown)
                return Result<enApplicationStatus>.Failure(enApplicationStatus.Unknown, "الطلب غير موجود", enMessageType.Error);

            return Result<enApplicationStatus>.Success(status, null, enMessageType.Info);
        }

        public static Result<int> GetPersonIDBy_AppID(int AppID)
        {
            bool? result_PersonID = cls_LDLApplication_DAL.TryGetPersonIDBy_AppID(AppID, out int personID);
            if (!result_PersonID.HasValue)
            {
                return Result<int>.Failure(-1, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            else if (!result_PersonID.Value)
            {
                return Result<int>.Failure(-1, "!هذا الشخص غير موجود", enMessageType.Warning);
            }

            return Result<int>.Success(personID, null, enMessageType.Info);
        }

        public static Result<int> GetPersonIDBy_LDLAppID(int LDLAppID)
        {
            bool? result_PersonID = cls_LDLApplication_DAL.TryGetPersonIDByLDL_AppID(LDLAppID, out int? personID);
            if (!result_PersonID.HasValue)
            {
                return Result<int>.Failure(-1, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }

            else if (!result_PersonID.Value || personID == null)
            {
                return Result<int>.Failure(-1, "!هذا الشخص غير موجود", enMessageType.Warning);
            }

            int _PersonID = personID.Value;
            return Result<int>.Success(_PersonID, null, enMessageType.Info);
        }


        public static Result<decimal> GetLicenseClassFeesRelatedBy_LDLAppID(int LDLAppID)
        {
            bool? result_LDLApp_ClassFees = cls_LDLApplication_DAL.TryGetLicenseClassFeesRelated_ByLDLAppID(LDLAppID, out decimal Fees);
            if (!result_LDLApp_ClassFees.HasValue)
            {
                return Result<decimal>.Failure(-1m, "حصل خطأ أثناء محاولة الحصول على سعر الرخصة", SharedLayer.enums.enMessageType.Error);
            }

            if (!result_LDLApp_ClassFees.Value)
                return Result<decimal>.Failure(-1m, "هذه الرخصة غير موجودة من فضلك اختر غيرها!", enMessageType.Warning);


            return Result<decimal>.Success(Fees, null, enMessageType.Info);
        }

        public static int? GetPassedTestsCount(int LDL_AppID)
        {
            return cls_LDLApplication_DAL.GetPassedTestsCount(LDL_AppID);
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

            if (PassedTestCount.Value != LastTestType)
                return Result<bool?>.Failure(false, "هذا الشخص لم يكمل جميع امتحاناته.",
                enMessageType.Warning);

            return Result<bool?>.Success(true, null);
        }

        public static OperationResult IsApplicationStatus_New(int AppID)
        {
            bool? result_ApplicationStatus = cls_LDLApplication_DAL.IsApplicationStatus_New(AppID);
            if (!result_ApplicationStatus.HasValue)
                return OperationResult.Failure("حصل خطأ أثناء محاولة معالجة بعض البيانات");

            if (!result_ApplicationStatus.Value)
                return OperationResult.TechnicalFailure("حالة الطلب مغلق أو أنه مكتمل بالفعل");

            return OperationResult.Seccuss(null);
        }

        public static Result<(int PersonID, int ApplicationID)> GetAppID_PersonIDBy_LDLAppID(int LDL_AppID)
        {
            bool? result_Data = cls_LDLApplication_DAL.TryGetAppID_PersonIDBy_LDLAppID(LDL_AppID, out int? ApplicationID, out int? PersonID);
            if (!result_Data.HasValue)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", enMessageType.Error);

            if (!result_Data.Value)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "طلب الرخصة المحلية غير صالح، من فضلك اختر واحدا آخر!", enMessageType.Warning);

            return Result<(int PersonID, int ApplicationID)>.Success((PersonID.Value, ApplicationID.Value), null, enMessageType.Info);
        }

        public static Result<(int PersonID, int ApplicationID)> GetAppID_PersonIDBy_DriverID(int DriverID)
        {
            bool? result_Data = cls_LDLApplication_DAL.TryGetAppID_PersonIDBy_DriverID(DriverID, out int? ApplicationID, out int? PersonID);
            if (!result_Data.HasValue)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى", enMessageType.Error);

            if (!result_Data.Value)
                return Result<(int PersonID, int ApplicationID)>.Failure((-1, -1), "طلب الرخصة المحلية غير صالح، من فضلك اختر واحدا آخر!", enMessageType.Warning);

            return Result<(int PersonID, int ApplicationID)>.Success((PersonID.Value, ApplicationID.Value), null, enMessageType.Info);
        }

    }
}
