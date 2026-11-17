using DVLD.DTOs;
using DVLD.DTOs.License.LDL_Application;
using DVLDBisnessLayer.LDL_App;
using DVLDDataAccessLayer;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.New_LocalDriver_App
{
    public class cls_LDL_ApplicationRequest_BL
    {
        private readonly DTO_LDL_App _dto;
        enMode _Mode;

        public cls_LDL_ApplicationRequest_BL(DTO_LDL_App I_LDL_App)
        {
            _dto = I_LDL_App;
            _Mode = enMode.Add;
        }
        public static OperationResult Delete(int LDL_AppID)
        {
            Result<DTO_LDL_App> result_Find = FindBy_AppID(LDL_AppID);
            if (!result_Find.isSuccess)
                return OperationResult.Failure(result_Find.Message, result_Find.messageType);
            DTO_LDL_App dto = result_Find.Value;


            return cls_LDLApplication_Service_BL.Delete_LDLAppID(dto);
        }

        public static Result<(int PersonID, int ApplicationID)> GetAppID_PersonIDBy_DriverID(int DriverID)
        {
            return cls_LDLApplication_Service_BL.GetAppID_PersonIDBy_DriverID(DriverID);
        }

        public static Result<(int PersonID, int ApplicationID)> GetAppID_PersonIDBy_LDLAppID(int LDL_AppID)
        {
            return cls_LDLApplication_Service_BL.GetAppID_PersonIDBy_LDLAppID(LDL_AppID);
        }

        public static Result<int> GetPersonIDBy_AppID(int AppID)
        {
            return cls_LDLApplication_Service_BL.GetPersonIDBy_AppID(AppID);
        }

        public static OperationResult CancellApplication(int LDL_AppID)
        {
            Result<DTO_LDL_App> result_Found = FindBy_LDLAppID(LDL_AppID);
            if(!result_Found.isSuccess)
            {
                return OperationResult.Failure(result_Found.Message, result_Found.messageType);
            }

            DTO_LDL_App Current_LDLApp = result_Found.Value;


            OperationResult result_Cancelled = cls_LDLApplication_Service_BL.TryChangeStatus(Current_LDLApp, enApplicationStatus.Cancelled);
            if (!result_Cancelled.IsSuccess)
                return result_Cancelled;

            return OperationResult.Seccuss("تم إلفاء الطلب بنحاج!", enMessageType.Info);
        }

        public static Result<DTO_LDL_App> FindBy_LDLAppID(int LDL_App)
        {
            return cls_LDLApplication_Service_BL.FindBy_LDLAppID(LDL_App);
        }

        public static Result<DTO_LDL_App> FindBy_AppID(int LDL_App)
        {
            return cls_LDLApplication_Service_BL.FindByAppID(LDL_App);
        }

        public static Result<decimal> GetLicenseClassFeesRelatedBy_LDLAppID(int LDL_AppID)
        {
            return cls_LDLApplication_Service_BL.GetLicenseClassFeesRelatedBy_LDLAppID(LDL_AppID);
        }

        //private OperationResult Update_Status()
        //{
        //    if (!cls_LDLApplication_DAL.UpdateApplicationStatus(this.ApplicationID, ApplicationStatus))
        //        return OperationResult.Failure("فشل تحديث الطلب، يرجى المحاولة مرة أخرى");

        //    if (cls_LDLApplication_DAL.FindBy_LDLAppID(LDL_AppID, out DTO_LDL_App dto))
        //    {
        //        if (dto.LastStatusDate != null)
        //        {
        //            this.LastStatusDate = dto.LastStatusDate;
        //            return OperationResult.Seccuss("تم تحديث الطلب بنجاح");
        //        }
        //        else
        //        {
        //            return OperationResult.Seccuss("تم تحديث الطلب، ولكن لم يتم الحصول على تاريخ آخر تعديل.");
        //        }
        //    }

        //    return OperationResult.Seccuss("تم تحديث الطلب، ولكن تعذّر جلب تفاصيل الحالة.");
        //}

        private OperationResult AddNew_LDLApplication()
        {
            if(_dto.ApplicationTypeID == enApplicationTypes.NewLocalLicense)
            {
                cls_NewLDLApp_BL RequestNewLDLApp = new cls_NewLDLApp_BL(_dto);
                return RequestNewLDLApp.Save();
            }

            cls_NewLDLApp_BL New_LDL_App = new cls_NewLDLApp_BL(_dto);
            OperationResult result_Save = New_LDL_App.Save();
                return result_Save;
        }

        public OperationResult Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    return AddNew_LDLApplication();
                case enMode.Update:
                    return OperationResult.Failure("هذا الحدث لم يكتمل", enMessageType.Error);
                default:
                    return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!", enMessageType.Error);
            }    
        }
    }
}
