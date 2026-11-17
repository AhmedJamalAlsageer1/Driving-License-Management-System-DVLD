using DVLDDataAccessLayer;
using DVLDDataAccessLayer.Tests;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer
{
    public class clsTestProgressBL : SharedLayer.Contants.Interface.ITestProgressReadOnly
    {
        public bool VisionPassed { get; private set; }
        public bool WrittenPassed { get; private set; }
        public bool StreetPassed { get; private set; }

        public int PassedTestsCount { get; private set; }

        private clsTestProgressBL(int Count)
        {
            PassedTestsCount = Count;
            ConfigureDefaultBookingData();
        }

        public static Result<clsTestProgressBL> Load(int LDLAppID)
        {
            if (!cls_LDLApplication_DAL.isLDLApp_Active(LDLAppID))
                return Result<clsTestProgressBL>.Failure(null, "الطلب غير موجود.", enMessageType.Error);

            int? Count = cls_LDLApplication_DAL.GetPassedTestsCount(LDLAppID);
            if (!Count.HasValue)
                return Result<clsTestProgressBL>.Failure(null, "تعذر تحميل عدد الاختبارات المجتازة", enMessageType.Error);


            clsTestProgressBL TestsProgress = new clsTestProgressBL(Count.Value);
            return Result<clsTestProgressBL>.Success(TestsProgress, null, enMessageType.Info);
        }

        private void ConfigureDefaultBookingData()
        {
            VisionPassed = PassedTestsCount >= 1;
            WrittenPassed = PassedTestsCount >= 2;
            StreetPassed = PassedTestsCount >= 3;
        }

        public enTestType NextTestTypeToBook()
        {
            foreach (var kvp in SharedLayer.Contants.TestTypeRules.Rules)
                if (kvp.Value(this))
                    return kvp.Key;

            return enTestType.UnKnown;
        }

        public bool CanBookAppointment (enTestType testType)
        {
            if (SharedLayer.Contants.TestTypeRules.Rules.ContainsKey(testType))
                return SharedLayer.Contants.TestTypeRules.Rules[testType](this);

            return false;
        }

        public bool isPassed => VisionPassed && WrittenPassed && StreetPassed;
    }
}
