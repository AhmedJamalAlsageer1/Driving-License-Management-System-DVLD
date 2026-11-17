using DVLD.DTOs.Tests;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Tests
{
    public static class clsTestAppointmentInfoBL
    {
        public static Result<List<DTODisplayTestAppointment>> FindAllTestAppointments(int LDLAppID, SharedLayer.enums.enTestType testType)
        {
            List<DTODisplayTestAppointment> AllTestAppointment = clsTestAppointmentInfoDAL.FindAllTestAppointments(LDLAppID, testType);

            if (AllTestAppointment == null)
                return Result<List<DTODisplayTestAppointment>>.Failure(null, "تعذر الوصول إلى البيانات، يرجى مراجعة هذه الحالة.",
                    SharedLayer.enums.enMessageType.Error);

            else if (AllTestAppointment.Count == 0)
                return Result<List<DTODisplayTestAppointment>>.Success(AllTestAppointment, null);

            string Message = null;
            foreach (var appointment in AllTestAppointment)
            {
                bool? _islocked = appointment.IsLocked;
                Result<bool> result_Lock = AppointmentLockManagerBL.EnsureLockedIfOverdue(appointment.TestAppointmentID,
                    appointment.AppointmentDate, ref _islocked);
                if (!result_Lock.isSuccess)
                    return Result<List<DTODisplayTestAppointment>>.Failure(null, result_Lock.Message, result_Lock.messageType);

                appointment.IsLocked = _islocked.Value;
            }

            if (Message != null)
                return Result<List<DTODisplayTestAppointment>>.Failure(null, Message, SharedLayer.enums.enMessageType.Error);

            return Result<List<DTODisplayTestAppointment>>.Success(AllTestAppointment, null,
                    SharedLayer.enums.enMessageType.Info);
        }
    }
}
