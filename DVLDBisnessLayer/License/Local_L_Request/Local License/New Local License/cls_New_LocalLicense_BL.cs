using DVLD.DTOs;
using DVLD.DTOs.License;
using DVLD.DTOs.License.Local_License;
using DVLD.DTOs.License.Local_License.New_Local_License;
using DVLDBisnessLayer.Driver;
using DVLDBisnessLayer.LDL_App;
using DVLDBisnessLayer.License.Local_L_Request.Local_License;
using DVLDBisnessLayer.Rules.LocalLicense;
using DVLDBisnessLayer.Test;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.License;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.License.Local_L_Request.New_Local_License
{
    internal class cls_New_LocalLicense_BL
    {
        private int ApplicationID { get; set; }
        private int LDL_AppID { get; set; }
        private int LocalLicenseID { get; set; }
        private int DriverID { get; set; }
        private enApplicationTypes ApplicationTypeID { get; set; }
        private enLicenseClass LicenseClass { get; set; }
        private DateTime IssueDate { get; set; }
        private DateTime ExpirationDate { get; set; }
        private bool IsActive { get; set; }
        private string Notes { get; set; }
        private enIssueReasonLicense IssueReason { get; set; }
        public decimal LicenseFees { get; private set; }
        private int CreatedByUserID {get; set;}
        DTO_LocalLicense _dto;

        public cls_New_LocalLicense_BL(DTO_LocalLicense dto)
        {
            this.ApplicationID = dto.ApplicationID;
            this.LDL_AppID = dto.LDL_AppID;
            this.LocalLicenseID = -1;
            this.DriverID = -1;
            this.ApplicationTypeID = dto.ApplicationTypes;
            this.LicenseClass = dto.LicenseClass;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.IsActive = false;
            this.Notes = string.IsNullOrWhiteSpace(dto.Notes)? null : dto.Notes.Trim();
            this.IssueReason = enIssueReasonLicense.FirstTime;
            this.LicenseFees = dto.PaidFees;
            this.CreatedByUserID = -1;
            _dto = dto;
        }

        private DTO_LocalLicense GetDTO()
        {
            return new DTO_LocalLicense()
            {
                ApplicationID = this.ApplicationID,
                LocalLicenseID = this.LocalLicenseID,
                DriverID = this.DriverID,
                ApplicationTypes = this.ApplicationTypeID,
                LicenseClass = this.LicenseClass,
                IssueDate = this.IssueDate,
                ExpirationDate = this.ExpirationDate,
                IsActive = this.IsActive,
                Notes = this.Notes,
                IssueReason = this.IssueReason,
                PaidFees = this.LicenseFees,
                CreatedByUserID = this.CreatedByUserID
            };
        }
        private void SetDTO(DTO_LocalLicense newLocalLicense)
        {
            _dto.ApplicationID = newLocalLicense.ApplicationID;
            _dto.LocalLicenseID = newLocalLicense.LocalLicenseID;
            _dto.DriverID = newLocalLicense.DriverID;
            _dto.ApplicationTypes = newLocalLicense.ApplicationTypes;
            _dto.LicenseClass = newLocalLicense.LicenseClass;
            _dto.IssueDate = newLocalLicense.IssueDate;
            _dto.ExpirationDate = newLocalLicense.ExpirationDate;
            _dto.IsActive = newLocalLicense.IsActive;
            _dto.Notes = newLocalLicense.Notes;
            _dto.IssueReason = newLocalLicense.IssueReason;
            _dto.PaidFees = newLocalLicense.PaidFees;
            _dto.CreatedByUserID = newLocalLicense.CreatedByUserID;
        }

        internal static OperationResult IsFirstLicense(int DriverID, enLicenseClass licenseClass)
        {
            bool? isFirst_L = clsLicenseClassesDAL.IsFirstLicenseForDriverInClass(DriverID, licenseClass);
            if (!isFirst_L.HasValue)
                return OperationResult.Failure("حصل خطأ أثناء محاولة الوصول لقواعد البيانات، يرجى المحاولة مرة أخرى.", enMessageType.Error);

            if (!isFirst_L.Value)
                return OperationResult.Failure("صاحب هذه الرخصة طلب رخصة من هذا النوع من قبل فهذه ليست أول مرة له.", enMessageType.Error);

            return OperationResult.Seccuss(null);
        }

        private OperationResult PrepareData(DTO_LDL_App CurrentApp, bool isPersonDriver, out DTODriver driver)
        {
            driver = null;

            if (string.IsNullOrWhiteSpace(Notes))
                Notes = null;

            IssueReason = enIssueReasonLicense.FirstTime;

            LicenseClass = CurrentApp.LicenseClassID;
            if (!isPersonDriver)
            {
                driver = new DTODriver();
                driver.PersonID = CurrentApp.PersonID;
                driver.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            }
            else
            {
                Result<int> result_DriverID = clsDriver_BL.GetDriverIDByPersonID(CurrentApp.PersonID);
                if (!result_DriverID.isSuccess)
                    return OperationResult.Failure(result_DriverID.Message, result_DriverID.messageType);

                this.DriverID = result_DriverID.Value;
            }

            this.CreatedByUserID = SharedLayer.Context.AppContext.CurrentUser.UserID.Value;
            return OperationResult.Seccuss(null);
        }

        private OperationResult Validation_BL(DTO_LDL_App CurrentApplication, bool isPersonDriver)
        {
            if (this.ApplicationTypeID != enApplicationTypes.NewLocalLicense)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة في وقت آخر!", enMessageType.Error);

            if (clsLocalLicense_DAL.HasValidLocalLicense(CurrentApplication.PersonID, CurrentApplication.LicenseClassID))
                return OperationResult.Failure("لا يمكنك إنشاء رخصة من هذا النوع بوجودة رخصة أخرى سارية منها!", enMessageType.Warning);

            Result<decimal> result_LicenseFees = clsLicenseClassesBL.GetLicenseFees(CurrentApplication.LicenseClassID);
            if (!result_LicenseFees.isSuccess)
                return OperationResult.Failure(result_LicenseFees.Message, result_LicenseFees.messageType);

            decimal LicenseFeesFromUI = this.LicenseFees;
            this.LicenseFees = result_LicenseFees.Value;

            if (this.LicenseFees != LicenseFeesFromUI)
                return OperationResult.Failure("سعر الرخصة ما بين العرض وما في قواعد البيانات غير متزامنين من فضلك أعد مرة أخرى.", enMessageType.Error);


            OperationResult IsCompletedTest = clsTestBL.isTestPassed(CurrentApplication.PassedTestCount);
            if (!IsCompletedTest.IsSuccess)
                return OperationResult.Failure(IsCompletedTest.Message, IsCompletedTest.MessageType);

            if (this.ApplicationTypeID != enApplicationTypes.NewLocalLicense || this.IssueReason != enIssueReasonLicense.FirstTime)
                return OperationResult.Failure("خطأ تقني، يرجى المحاولة مرة أخرى!");

            if (isPersonDriver)
            {
                Result<DTO_NewLicenseRequirements_BL> result_Requirements_PreviousLicense = cls_LocalLicenseService_BL.GetPreviousLicenseInfo(this.DriverID, this.LicenseClass);
                if (!result_Requirements_PreviousLicense.HasValue)
                    return OperationResult.Failure(result_Requirements_PreviousLicense.Message, result_Requirements_PreviousLicense.messageType);

                if(result_Requirements_PreviousLicense.isSuccess)
                {
                    DTO_NewLicenseRequirements_BL dto_PreviousLicense = result_Requirements_PreviousLicense.Value;
                    if(DateTime.Now.Date <= dto_PreviousLicense.ExpirationDate.AddYears(RenewLicenseRules.MaxRenewalYears).Date)
                    {
                        return OperationResult.Failure("لا يمكن إصدار رخصة جديدة لهذه الفئة، إن كانت الرخصة القديمة يمكن تجديدها!", enMessageType.Warning);
                    }

                    bool? result_IsDetainedLicense = cls_LocalLicenseService_BL.IsDetainedLicense(dto_PreviousLicense.LicenseID);

                    if (!result_IsDetainedLicense.HasValue)
                        return OperationResult.Failure("حصل خطأ في قواعد البيانات يرجى المحاولة مرة أخرى!", enMessageType.Error);
                    
                    if (!result_IsDetainedLicense.Value)
                        return OperationResult.Failure("لا يمكن إصدار رخصة جديدة قبل معالجة وضع الرخصة السابقة!", enMessageType.Warning);

                }

                return OperationResult.Seccuss(null);
            }
            

            return OperationResult.Seccuss(null, enMessageType.Info);
        }

        private OperationResult AddNewLocalLicense()
        {
            Result<DTO_LDL_App> result_CurrentApplication = cls_LDLApplication_Service_BL.FindByAppID(ApplicationID);
            if (!result_CurrentApplication.isSuccess)
                return OperationResult.Failure(result_CurrentApplication.Message, result_CurrentApplication.messageType);

            DTO_LDL_App CurrentApplication = result_CurrentApplication.Value;


            OperationResult result_IsDriver = clsDriver_BL.IsPersonDriver(CurrentApplication.PersonID);
            if (!result_IsDriver.HasValue)
                return OperationResult.Failure(result_IsDriver.Message, result_IsDriver.MessageType);
            bool isPersonDriver = result_IsDriver.IsSuccess;

            OperationResult result_PrepareData = PrepareData(CurrentApplication, isPersonDriver, out DTODriver Driver);
            if (!result_PrepareData.IsSuccess)
                return result_PrepareData;

            OperationResult result_ValidationBL = Validation_BL(CurrentApplication, isPersonDriver);
            if (!result_ValidationBL.IsSuccess)
                return result_ValidationBL;

            
            Result<int> result_LengthValidityLicnese = clsLicenseClassesBL.GetLicenseValidity(this.LicenseClass);
            if (!result_LengthValidityLicnese.isSuccess)
                return OperationResult.Failure(result_LengthValidityLicnese.Message, result_LengthValidityLicnese.messageType);


            int LengthValidityLicnese = result_LengthValidityLicnese.Value;

            DTO_LocalLicense NewLocalLicense = GetDTO();
            bool? result_AddFirstTime = clsLocalLicense_DAL.AddNewLicenseFirstTime(NewLocalLicense, LengthValidityLicnese, CurrentApplication.PersonID, Driver);
            if (!result_AddFirstTime.HasValue)
                return OperationResult.Failure("حصل خطأ أثناء محاولة إضافة البيانات لقواعد البيانات.", enMessageType.Error);

            if (!result_AddFirstTime.Value)
                return OperationResult.Failure("تعذر إنشاء رخصة من فضلك حاول مرة أخرى.", enMessageType.Error);

            string messaege;
            this.IsActive = true;
            NewLocalLicense.IsActive = true;
            SetDTO(NewLocalLicense);

            if (NewLocalLicense.LocalLicenseID == -1)
            {
                messaege = "تم إنشاء الرخصة بنجاج، ولكن لم يتم استرجاع LicenseID بسبب خطإ ما!";
                return OperationResult.Seccuss(messaege, enMessageType.Warning);
            }

            messaege = "تم إنشاء الرخصة بنجاج، رقم معرف الرخصة هو: " + NewLocalLicense.LocalLicenseID;
            return OperationResult.Seccuss(messaege, enMessageType.Info);
        }


        public OperationResult Save()
        {
            return AddNewLocalLicense();
        }
    }
}
