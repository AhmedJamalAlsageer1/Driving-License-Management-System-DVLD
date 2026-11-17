using SharedLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Rules
{
    public static class AppointmentRules
    {
        public static Result<bool> IsAppointmentDateValidForBooking(DateTime date)
        {
            if (date.Date < DateTime.Today)
                return Result<bool>.Failure(false, "لا يمكن الحجز في تاريخ مضى.", enums.enMessageType.Warning);

            if (date.Date == DateTime.Today && date.TimeOfDay > new TimeSpan(2, 0, 0))
                return Result<bool>.Failure(false, "لا يمكن الحجز في نفس اليوم بعد الساعة 12 ظهرا.", enums.enMessageType.Warning);

            if (date.DayOfWeek == DayOfWeek.Friday)
                return Result<bool>.Failure(false, "لا يمكن الحجز يوم الجمعة.", enums.enMessageType.Warning);

            return Result<bool>.Success(true, null, enums.enMessageType.Info);
        }

        public static Result <bool> isEditAppointmentDateOrManuallyCancel_AllowedNow(DateTime AppointmentDate, bool Mode_Update)
        {
            DateTime now = DateTime.Now;

            // هل اليوم بعد الموعد؟
            if (AppointmentDate < now.Date)
                return Result<bool>.Failure(false, "لا يمكن إلغاء أو تعديل الموعد بعد أن مرّ تاريخه.", enums.enMessageType.Warning);

            if(AppointmentDate.Date == now.Date)
            {
                DateTime CutOff = AppointmentDate.Add(AppDefaults.AppointmentDateEditGracePeriod);

                if(now >= CutOff)
                    return Result<bool>.Failure(false, "انتهت مهلة الإلغاء أو التعديل اليدوي لهذا الموعد.", enums.enMessageType.Warning);

                if (Mode_Update)
                {
                    if (now.Date.TimeOfDay > new TimeSpan(2, 0, 0))
                        return Result<bool>.Failure(false, "لا يمكن الحجز في نفس اليوم بعد الساعة 12 ظهرا.", enums.enMessageType.Warning);
                }
            }

            return Result <bool>.Success(true, null, enums.enMessageType.Info);
        }

        public static Result <bool> isSubmitTestResultOrAutoLock_AllowedNow(DateTime AppointmentDate, bool Mode_SubmitTestResult = true, bool TakeTest_NOW = false)
        {
            DateTime LatestAllowedTime = AppointmentDate.Add(AppDefaults.TestResultEditGracePeriodHours);

            if (DateTime.Now > LatestAllowedTime)
            {
                return Result<bool>.Failure(false, "انتهت المهلة المسموح بها لإدخال نتيجة هذا الامتحان. لا يمكن الحفظ الآن.",
                    enums.enMessageType.Warning);
            }

            if(Mode_SubmitTestResult && AppointmentDate.Date > DateTime.Now.Date && !TakeTest_NOW)
            {
                return Result<bool>.Failure(false, "هل أنت متأكد من أنك تريد القيام بالامتحان قبل أوانه؟",
                    enums.enMessageType.Warning, enums.enLogicCode.Future_Appointment);
            }

            return Result <bool>.Success(true, null, enums.enMessageType.Info);
        }
    }
}
