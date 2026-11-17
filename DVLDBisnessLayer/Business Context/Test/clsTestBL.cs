using DVLD.DTOs.Test;
using DVLDBisnessLayer.Tests;
using DVLDDataAccessLayer.Test;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using SharedLayer.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Test
{
    public class clsTestBL
    {
        public int _TestID { get; private set; }
        public int _TestAppointmentID { get; private set; }
        public bool _TestResult { get; private set; }
        public string _Notes { get; private set; }
        public int _CreatedByUserID { get; private set; }
        public int? _ReTake_AppID { get; private set; }

        public clsTestBL(DTOTest dto)
        {
            _TestID = -1;
            _TestAppointmentID = dto.TestAppointmentID;
            _TestResult = dto.TestResult;
            _Notes = dto.Notes;
            _CreatedByUserID = dto.CreatedByUserID;
            _ReTake_AppID = dto.ReTake_AppID;
        }

        private Result<bool> _CanTakeTest(bool TakeTest_NOW)
        {
            Result<bool> result_TestAppointment_Exists = clsTestAppointmentBL.isTestAppointmentExists(_TestAppointmentID);
            if(!result_TestAppointment_Exists.isSuccess)
                return result_TestAppointment_Exists;

            Result<bool?> result_TestAppointmentLock = clsTestAppointmentBL.isLocked(_TestAppointmentID);
            if (result_TestAppointmentLock.isSuccess || result_TestAppointmentLock.Value == null)
                return Result<bool>.Failure(false, result_TestAppointmentLock.Message, result_TestAppointmentLock.messageType);

            Result<DateTime?> result_AppointmentDate = clsTestAppointmentBL.GetAppointmentDateByID(_TestAppointmentID);
            if(!result_AppointmentDate.isSuccess)
                return Result <bool>.Failure(false, result_AppointmentDate.Message, result_AppointmentDate.messageType);

            DateTime AppointmentDate = result_AppointmentDate.Value.Value;

            Result<bool> result_SubmitTestResult = AppointmentRules.isSubmitTestResultOrAutoLock_AllowedNow(AppointmentDate, true, TakeTest_NOW);
            if (!result_SubmitTestResult.isSuccess)
                return result_SubmitTestResult;

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        private Result <bool> _TakeTest(DTOTest dto, bool TakeTest_NOW)
        {
            if (!clsTestDAL.TakeTest(dto, TakeTest_NOW))
            {
                return Result<bool>.Failure(false, "فشلت عملية إضافة معلومات الاختبار!", enMessageType.Info);
            }

            

            if (dto.TestID == -1)
                return Result<bool>.Success(true, "تم إضافة معلومات الامتان بنجاح، ولكن فشلت عملية إرجاع المفتاح بنجاح.", enMessageType.Warning);

            this._TestID = dto.TestID;
            return Result<bool>.Success(true, "تم إضافة معلومات الاختبار بنجاح", enMessageType.Info);
        }

        internal static Result <bool?> HasPassedTestBefore(int LDL_AppID, enTestType TestType)
        {
            bool? Result_HasPassed = clsTestDAL.HasPassedTestBefore(LDL_AppID, TestType);

            if (!Result_HasPassed.HasValue)
                return Result<bool?>.Failure(null, "حصل خطأ أثناء التحقق من الامتحان", enMessageType.Error);

            if(!Result_HasPassed.Value)
                return Result<bool?>.Failure(false, null, enMessageType.Error);

            return Result<bool?>.Success(true, "هذا الامتحان مجتاز بالفعل لا يمكنك القيام به مرة أخرى!", enMessageType.Warning);
        }

        public Result<bool> TakeTest(bool TakeTest_NOW = false)
        {
            Result<int?> result_ReTake_AppID = clsTestAppointmentBL.Get_ReTake_AppID(this._TestAppointmentID);
            if (!result_ReTake_AppID.isSuccess)
                return Result<bool>.Failure(false, result_ReTake_AppID.Message, result_ReTake_AppID.messageType);

            this._ReTake_AppID = result_ReTake_AppID.Value;

            Result <bool> result_CanTakeResult = _CanTakeTest(TakeTest_NOW);
            if(!result_CanTakeResult.isSuccess)
                return result_CanTakeResult;

            DTOTest dto = DTOTest.GetDTO(_TestID, _TestAppointmentID, _TestResult, _Notes,
                _CreatedByUserID, _ReTake_AppID);

            Result<bool> result_TakeTest = _TakeTest(dto, TakeTest_NOW);
            if (!result_TakeTest.isSuccess)
                return result_TakeTest;

            
            return result_TakeTest;
        }

        public static OperationResult isTestPassed(int LastTestCompleted)
        {
            // الحصول على أكبر امتحان من enum
            int? LastTestType = Enum.GetValues(typeof(enTestType))
            .Cast<enTestType>()
            .Where(e => e != enTestType.UnKnown)
            .Select(e => (int?)e)
            .DefaultIfEmpty(null)
            .Max();


            if (!LastTestType.HasValue)
                return OperationResult.Failure("حدث خطأ أثناء محاولة تحديد آخر امتحان قد أكلمته.");

            if (LastTestType != LastTestCompleted)
                return OperationResult.Failure("لا يمكنك إصدار رخصة من دون إكمال باقي الامتحانات، عليك إكمال باقي الامتحانات أولا.");

            return OperationResult.Seccuss(null);
        }

    }


}
