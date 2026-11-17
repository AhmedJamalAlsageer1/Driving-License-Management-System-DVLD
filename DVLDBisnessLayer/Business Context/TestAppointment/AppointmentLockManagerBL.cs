using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using SharedLayer.Rules;
using System;

namespace DVLDBisnessLayer.Tests
{
    internal class AppointmentLockManagerBL
    {
        public static Result <bool> EnsureLockedIfOverdue(int appointmentID, DateTime AppointmentDate, ref bool? isLocked)
        {
            if(!isLocked.HasValue)
            {
                Result <bool?> result_isLocked = clsTestAppointmentBL.isLocked(appointmentID);
                if (result_isLocked.isSuccess || result_isLocked.Value == null)
                    return Result<bool>.Failure(false, result_isLocked.Message, result_isLocked.messageType);

                isLocked = result_isLocked.Value;
            }

            if(ShouldBeLocked(AppointmentDate, isLocked.Value))
            {
                Result<bool> result_Look = TryLock(appointmentID);
                if(!result_Look.isSuccess)
                {
                    return result_Look;
                }
                // تحدث الحالة الخارجية
                isLocked = true;
            }

            return Result<bool>.Success(true, null, SharedLayer.enums.enMessageType.Info);
        }
        private static bool ShouldBeLocked(DateTime AppointmentDate, bool isLocked)
        {
            if(isLocked)
                return false;

            Result <bool> result = AppointmentRules.isSubmitTestResultOrAutoLock_AllowedNow(AppointmentDate, false);
            if(!result.isSuccess)
                return true;

            return false;
        }
        private static Result<bool> TryLock(int AppointmentID)
        {
            if (!clsTestAppointmentDAL.Lock_TestAppointment(AppointmentID, enApplicationStatus.Completed))
                return Result<bool>.Failure(false, "حصل خطأ أثناء فحص ومعالجة البيانات، يرجى المحاولة مرة أخرى", SharedLayer.enums.enMessageType.Error);

            return Result<bool>.Success(true, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
