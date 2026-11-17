using DVLD.DTOs;
using DVLD.DTOs.Tests;
using DVLDBisnessLayer.LDL_App;
using DVLDBisnessLayer.Test;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using SharedLayer.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVLDBisnessLayer.Tests
{
    public class clsTestAppointmentBL
    {
        public int TestAppointmentID { get; private set; }
        public SharedLayer.enums.enTestType TestType { get; private set; }
        public int LDL_AppID { get; private set; }
        public DateTime AppointmentDate { get; private set; }
        public decimal PaidFees { get; private set; }
        public int CreatedByUserID { get; private set; }
        public bool IsLocked { get; private set; }
        public int? R_Test_AppID { get; private set; }

        private enMode _Mode;

        enum enMode : byte
        {
            Add = 1,
            Update = 2,
        }

        public clsTestAppointmentBL(DTOTestAppointment dto)
        {
            TestAppointmentID = -1;
            TestType = dto.TypeTestID;
            LDL_AppID = dto.LocalDrivingLicenseApplicationID;
            AppointmentDate = dto.AppointmentDate;
            PaidFees = dto.PaidFees;
            CreatedByUserID = dto.CreatedByUserID;
            IsLocked = false;
            R_Test_AppID = dto.R_Take_AppID;

            _Mode = enMode.Add;
        }

        private clsTestAppointmentBL()
        {
            TestAppointmentID = -1;
            TestType = enTestType.UnKnown;
            LDL_AppID = -1;
            AppointmentDate = DateTime.MinValue;
            PaidFees = -1m;
            IsLocked = false;
            R_Test_AppID = -1;

            _Mode = enMode.Update;
        }
        private Result <bool> CheckPayment(bool RetakeTest, decimal R_Take_Fees)
        {
            Result<bool> resultTestExists = clsTestTypes.isTestTypeExists(this.TestType);
            if (!resultTestExists.isSuccess)
                return resultTestExists;

            Result<decimal> resultTestFees = clsTestTypes.GetTestFees(this.TestType);
            if (!resultTestFees.isSuccess)
                return Result <bool>.Failure(false, resultTestFees.Message, resultTestFees.messageType);

            if (!RetakeTest)
            {
                if(resultTestFees.Value != this.PaidFees)
                    return Result <bool>.Failure(false, "سعر الامتحان لم يكن متطابقا مع ما هو مسجلا عندنا", enMessageType.Error);

                return Result<bool>.Success(true, null, enMessageType.Info);
            }
            else
            {
                Result<bool> resultApplicationExists = clsApplicationTypesBL.isApplicationTypeExists(enApplicationTypes.RetakeTest);
                if (!resultApplicationExists.isSuccess)
                    return resultApplicationExists;

                Result<decimal> resultApplicationFees = clsApplicationTypesBL.GetApplicationFees(enApplicationTypes.RetakeTest);
                if (!resultApplicationFees.isSuccess)
                    return Result<bool>.Failure(false, resultApplicationFees.Message, resultApplicationFees.messageType);

                decimal TotalPaidFees = resultApplicationFees.Value + resultTestFees.Value;

                if (TotalPaidFees != this.PaidFees + R_Take_Fees)
                    return Result<bool>.Failure(false, "مجموع كلا من إعادة الطلب والامتحان غير متكافئين مع ما في قواعد البيانات",
                        enMessageType.Error);


                return Result <bool>.Success(true, null, enMessageType.Info);
            }
        }
        private Result <bool> isDataComplete(bool isRetakeTest, decimal R_Take_Fees)
        {

            Result<bool> resultAppointmentDate = AppointmentRules.IsAppointmentDateValidForBooking(this.AppointmentDate);
            if (!resultAppointmentDate.isSuccess)
                return resultAppointmentDate;

            Result<bool> resultUser = clsUser.IsUserExists(this.CreatedByUserID);
            if(!resultUser.isSuccess)
                return resultUser;

            if(isRetakeTest)
            {
                Result<bool?> result_HasPassedTestBefore = clsTestBL.HasPassedTestBefore(this.LDL_AppID, this.TestType);
                if (result_HasPassedTestBefore.isSuccess || result_HasPassedTestBefore.Value == null)
                    return Result<bool>.Failure(false, result_HasPassedTestBefore.Message, result_HasPassedTestBefore.messageType);

                Result<bool> resultHasNotOpenAppointment = AllAppointmentsAreClosed();
                if (!resultHasNotOpenAppointment.isSuccess)
                    return resultHasNotOpenAppointment;
            }

            Result<bool> resultPayment = CheckPayment(isRetakeTest, R_Take_Fees);
            if(!resultPayment.isSuccess)
                return resultAppointmentDate;


            return Result<bool>.Success(true, null, enMessageType.Info);
        }
        private DTOTestAppointment ConvertToDTO(bool _isRetakeTest)
        {

            DTOTestAppointment dto = new DTOTestAppointment
            {
                TypeTestID = this.TestType,
                LocalDrivingLicenseApplicationID = this.LDL_AppID,
                AppointmentDate = this.AppointmentDate,
                PaidFees = this.PaidFees,
                CreatedByUserID = this.CreatedByUserID,
                R_Take_AppID = null
            };
            
            if(_isRetakeTest )
            {
                dto.R_Take_AppID = this.R_Test_AppID;
            }

            return dto;
        }
        private DTO_LDL_App PrepareRETakeApplication()
        {
            Result<decimal> resultFees = clsApplicationTypesBL.GetApplicationFees(enApplicationTypes.RetakeTest);
            if (!resultFees.isSuccess)
                return null;

            Result <int> result_PersonID = cls_LDLApplication_Service_BL.GetPersonIDBy_LDLAppID(this.LDL_AppID);
            if (!result_PersonID.isSuccess)
                return null;

            int _personID = result_PersonID.Value;

            return new DTO_LDL_App
            {
                ApplicationID = -1,
                PersonID = _personID,
                ApplicationDate = DateTime.MinValue,
                ApplicationTypeID = enApplicationTypes.RetakeTest,
                ApplicationStatus = enApplicationStatus.New,
                PaidFees = resultFees.Value,
                CreatedByUserID = this.CreatedByUserID
            };
            
        }
        private Result <bool> _AddNewAppointment(decimal R_Take_Fees)
        {
            bool _isRetakeTest = isReTakeTest();

            Result<bool> resultCheckData = isDataComplete(_isRetakeTest, R_Take_Fees);
            if (!resultCheckData.isSuccess)
                return resultCheckData;

            DTOTestAppointment TestAppointment;

            if (!_isRetakeTest) // أول موعد امتحان
            {
                TestAppointment = ConvertToDTO(_isRetakeTest);
                if (!clsTestAppointmentDAL.AddNewTestAppointment(TestAppointment))
                {
                    return Result<bool>.Failure(false, "تعذر إضافة موعد، يرجى المحاولة مرة أخرى.", enMessageType.Error);
                }

                if (TestAppointment.TestAppointmentID == -1)
                    return Result<bool>.Success(true, "تم إضافة البيانات بنجاح ولكن معرف الموعد قد فقد", enMessageType.Warning);

                TestAppointmentID = TestAppointment.TestAppointmentID;
                return Result<bool>.Success(true, null, enMessageType.Info);
            }

            // إضافة امتحان معاد

            DTO_LDL_App application = PrepareRETakeApplication();
            TestAppointment = ConvertToDTO(_isRetakeTest);

            if (application == null)
                return Result<bool>.Failure(false, "حدث خطأ أثناء استرجاع بيانات إعادة الطلب", enMessageType.Error);

            if(TestAppointment == null)
                return Result<bool>.Failure(false, "حدث خطأ أثناء التعامل مع بيانات الموعد.", enMessageType.Error);

            if (!clsTestAppointmentDAL.AddNewReTakeTestAppointment(application, TestAppointment))
            {
                return Result<bool>.Failure(false, "تعذر إضافة موعد جديد، يرجى المحاولة مرة أخرى.", enMessageType.Error);
            }

            List<string> Errors = new List<string>();

            if (TestAppointment.TestAppointmentID == -1)
                Errors.Add("نجح إضافة موعد ولكن تعذر جلب المعرف الخاص به.");
            if (application.ApplicationID == -1)
                Errors.Add("نجح إضافة موعد ولكن تعذر جلب معرف إعادة الطلب.");
            if (application.ApplicationDate == DateTime.MinValue)
                Errors.Add("نجح إضافة موعد ولكن تعذر جلب وقت الطلب.");

            if(Errors.Any())
            {
                string ErrorMessage = "نحجت عملية إضافة الموعد ولكن حدثت المشاكل الآتية:\n" + string.Join("\n•  ", Errors);
                return Result<bool>.Success(true, ErrorMessage, enMessageType.Warning);
            }


            this.TestAppointmentID = TestAppointment.TestAppointmentID;
            this.R_Test_AppID = application.ApplicationID;

            return Result<bool>.Success(true, "تم إضافة الموعد بنجاح.", enMessageType.Info);
        }
        public Result <bool> SetNewAppointmentDate(DateTime AppointmentDate)
        {
            Result <bool> resultOfNewAppointmentDate = AppointmentRules.IsAppointmentDateValidForBooking(AppointmentDate);
            if(!resultOfNewAppointmentDate.isSuccess)
                return resultOfNewAppointmentDate;

            if (!clsTestAppointmentDAL.UpdateAppointmentDate(this.TestAppointmentID, AppointmentDate))
            {
                return Result<bool>.Failure(false, "حصل خطأ أثناء عملية تحديث تاريخ الموعد، يرحى المحاولة مرة أخرى.",
                    enMessageType.Error);
            }
            this.AppointmentDate = AppointmentDate;
            return Result <bool>.Success(true, null, enMessageType.Info);
        }
        private Result <bool> _UpdateAppointment()
        {
            Result<bool> ResultAppointmentDate = SharedLayer.Rules.AppointmentRules.isEditAppointmentDateOrManuallyCancel_AllowedNow(this.AppointmentDate, true);
            if (!ResultAppointmentDate.isSuccess)
                return ResultAppointmentDate;

            if (!clsTestAppointmentDAL.UpdateAppointmentDate(this.TestAppointmentID, AppointmentDate))
                return Result<bool>.Failure(false, "حدث خطأ أثناء التحديث", enMessageType.Error);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        private static clsTestAppointmentBL GetObject(DTOTestAppointment dto)
        {
            return new clsTestAppointmentBL
            {
                TestAppointmentID = dto.TestAppointmentID,
                TestType = dto.TypeTestID,
                LDL_AppID = dto.LocalDrivingLicenseApplicationID,
                AppointmentDate = dto.AppointmentDate,
                PaidFees = dto.PaidFees,
                CreatedByUserID = dto.CreatedByUserID,
                IsLocked = dto.IsLocked,
                R_Test_AppID = dto.R_Take_AppID,
            };
        }

        public static Result <clsTestAppointmentBL> FindObjectTestAppointmentByAppoinID(int TestAppointmentID)
        {
            if(clsTestAppointmentDAL.TryGetTestAppointmentByID(TestAppointmentID, out DTOTestAppointment dto))
            {
                if (dto != null)
                {
                    bool? _islocked = dto.IsLocked;
                    Result<bool> result_Locked = AppointmentLockManagerBL.EnsureLockedIfOverdue(dto.TestAppointmentID, dto.AppointmentDate, ref _islocked);
                    if (!result_Locked.isSuccess)
                        return Result <clsTestAppointmentBL>.Failure(null, "حصل خطأ أثناء معالجة البيانات لجلبها من فضلك حاول مرة أخرى.", enMessageType.Error);

                    dto.IsLocked = _islocked.Value;
                }
                   return Result<clsTestAppointmentBL>.Success(GetObject(dto), null, enMessageType.Info);
            }

            return Result<clsTestAppointmentBL>.Failure(null, "هذا الموعد غير موجود من فضلك اختر واحدا آخر.",
                enMessageType.Error);
        }
        private Result <bool> CanCancelAppointment()
        {
            Result <bool?> Result_isLocked = isLocked(TestAppointmentID);
            if (Result_isLocked.isSuccess || Result_isLocked.Value == null)
                return Result<bool>.Failure(false, Result_isLocked.Message, Result_isLocked.messageType);


            Result<bool> Result_CanCancel = AppointmentRules.isEditAppointmentDateOrManuallyCancel_AllowedNow(AppointmentDate, false);
            if(!Result_CanCancel.isSuccess)
                return Result_CanCancel;

            return Result <bool>.Success(true, null, enMessageType.Info);
        }
        private Result <bool> _LockAppointment()
        {
            if (!clsTestAppointmentDAL.Lock_TestAppointment(this.TestAppointmentID, enApplicationStatus.Cancelled))
                return Result<bool>.Failure(false, "فشلت عملية إغلاق الموعد، يرجى المحاولة مرة أخرى", enMessageType.Warning);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        public Result <bool> CancelAppointment()
        {
            Result<bool> CanCancel = CanCancelAppointment();
            if(!CanCancel.isSuccess)
                return CanCancel;

            return _LockAppointment();
        }

        public Result<bool> Save(decimal R_Take_Fees = 0)
        {
            switch (_Mode)
            {
                case enMode.Add:
                    return _AddNewAppointment(R_Take_Fees);
                case enMode.Update:
                    return _UpdateAppointment();

                default:
                    return Result<bool>.Failure(false, "خطأ تقني، هذا الوضع غير موجود", enMessageType.Error);
            }
        }

        public static Result <DTOTestAppointment_Init> GetTestAppointment_InitBy_LDLAppID(int LDLAppID, SharedLayer.enums.enTestType typeID)
        {
            if(DVLDDataAccessLayer.Tests.clsTestAppointmentDAL.TryGetTestAppointment_InitializationByLDL_AppID
                (LDLAppID, typeID, out DTOTestAppointment_Init dto))
            {
                if (dto != null)
                {
                    return Result<DTOTestAppointment_Init>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
                }
            }

            return Result<DTOTestAppointment_Init>.Failure(null, "معرف الرخصة المحلية هذا غير موجود من فضلك تأكد من إدخال معرف صالح.",
                SharedLayer.enums.enMessageType.Error);
        }

        public static Result<DTOTestAppointment_Init> GetTestAppointment_Init_ByAppointmentID(int AppointmentID, SharedLayer.enums.enTestType typeID = SharedLayer.enums.enTestType.UnKnown)
        {
            if (DVLDDataAccessLayer.Tests.clsTestAppointmentDAL.TryGetTestAppointment_InitializationBy_AppointmentID
                (AppointmentID, out DTOTestAppointment_Init dto, typeID))
            {
                if (dto != null)
                {
                    return Result<DTOTestAppointment_Init>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
                }
            }

            return Result<DTOTestAppointment_Init>.Failure(null, "معرف الرخصة المحلية هذا غير موجود من فضلك تأكد من إدخال معرف صالح.",
                SharedLayer.enums.enMessageType.Error);
        }
        public static Result <DTOTestAppointment> FindTestAppointment (int TestAppointmentID)
        {
            if (!clsTestAppointmentDAL.TryGetTestAppointmentByID(TestAppointmentID, out DTOTestAppointment dto))
                return Result<DTOTestAppointment>.Failure(null, "معرف الموعد المطلوب غير موجود. تأكد من إدخال معرف صالح.", enMessageType.Warning);

            if(dto == null)
                return Result<DTOTestAppointment>.Failure(null, "تعذر جلب بيانات الموعد من قاعدة البيانات.", enMessageType.Error);

            bool? _islocked = dto.IsLocked;
            Result<bool> result_Locked = AppointmentLockManagerBL.EnsureLockedIfOverdue(dto.TestAppointmentID, dto.AppointmentDate, ref _islocked);
            if (!result_Locked.isSuccess)
                return Result<DTOTestAppointment>.Failure(null, "حصل خطأ أثناء معالجة البيانات لجلبها من فضلك حاول مرة أخرى.", enMessageType.Error);

            dto.IsLocked = _islocked.Value;
            return Result<DTOTestAppointment>.Success(dto, null, enMessageType.Info);
        }
        
        public Result <bool> isReTakeTest(int LDL_AppID, enTestType TestType)
        {
            if (!clsTestAppointmentDAL.isRequestRetakeTest(LDL_AppID, TestType))
                return Result<bool>.Failure(false, "حدث خطأ أثناء استرجاع معلومات عن الإعادة", enMessageType.Error);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        private bool isReTakeTest()
        {
            if (!clsTestAppointmentDAL.isRequestRetakeTest(this.LDL_AppID, this.TestType))
                return false;

            return true;
        }

        private Result <bool> AllAppointmentsAreClosed()
        {
            if(!clsTestAppointmentDAL.HasOpenAppointment(this.LDL_AppID, this.TestType))
            {
                return Result<bool>.Success(true, null, enMessageType.Info);
            }

            return Result<bool>.Failure(false, "لا يمكنك إضافة موعد جديد لوجود موعد مفتوح", enMessageType.Error);
        }

        public static Result<bool?> isLocked(int AppointmentID)
        {
            bool? result = clsTestAppointmentDAL.isLocked(AppointmentID);
            if(result == null)
                return Result<bool?>.Failure(null, "حصل خطأ تقني لم نتمكن منه من فحص حالة الإغلاق.", enMessageType.Error);

            if(!result.Value)
                return Result<bool?>.Failure(false, null, enMessageType.Info);

            return Result <bool?>.Success(true, "الموعد مغلق بالفعل", enMessageType.Warning);
        }

        internal static Result <bool> isTestAppointmentExists(int TestAppointmentID)
        {
            if (!clsTestAppointmentDAL.isTestAppointmentExists(TestAppointmentID))
                return Result<bool>.Failure(false, "هذا الموعد غير موجود من فضلك ادخل موعدا آخر", enMessageType.Error);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        internal static Result <DateTime?> GetAppointmentDateByID(int TestAppointmentID)
        {
            if(clsTestAppointmentDAL.TryGetAppointmentDateByID(TestAppointmentID, out DateTime? AppointmentDate))
            {
                if (!AppointmentDate.HasValue)
                    return Result<DateTime?>.Failure(null, "طلب موعد الامتحان هذا غير موجود من فضلك أعد المحاولة!", enMessageType.Error);
            }

            return Result<DateTime?>.Success(AppointmentDate.Value, null, enMessageType.Info);
        }

        internal static Result <int?> Get_ReTake_AppID(int TestAppointmentID)
        {
            if (!clsTestAppointmentDAL.TryGetReTake_AppIDByID(TestAppointmentID, out int? ReTake_AppID))
                return Result<int?>.Failure(null, "حصل خطأ عندما محولة جلب بيانات الامتحان", enMessageType.Error);

            return Result <int?>.Success(ReTake_AppID, null, enMessageType.Info);
        }

        internal static bool TryGetParentApplicationIDsBy_TA_ID(int TestAppointmentID, out int? LDL_AppID, out int? App_ID)
        {
            LDL_AppID = null;
            App_ID = null;
            if (clsTestAppointmentDAL.TryGetParentApplicationIDsBy_TA_ID(TestAppointmentID, out LDL_AppID, out LDL_AppID))
            {
                if (LDL_AppID == null || App_ID == null)
                    return false;

                return true;
            }
            return false;
        }

        internal OperationResult Is_TabelTestAppointmentHas_LDLAppID(int LDL_AppID)
        {
            bool? result_isFoundInTabel = clsTestAppointmentDAL.Is_TabelTestAppointmentHas_LDLAppID(LDL_AppID);
            if (!result_isFoundInTabel.HasValue)
                return OperationResult.TechnicalFailure("حصل خطأ في قواعد البيانات يرحى المحاولة مرة أخرى!", enMessageType.Error);

            if (!result_isFoundInTabel.Value)
                return OperationResult.Failure("لا يمكنك حذف طلب له موعد مسجل!", enMessageType.Warning);

            return OperationResult.Seccuss(null);
        }
    }
}
