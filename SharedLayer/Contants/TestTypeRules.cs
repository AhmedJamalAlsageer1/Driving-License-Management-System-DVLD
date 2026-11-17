using SharedLayer.Contants.Interface;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Contants
{
    public static class TestTypeRules
    {
        public static readonly Dictionary<enTestType, Func<ITestProgressReadOnly, bool>> Rules =
         new Dictionary<enTestType, Func<ITestProgressReadOnly, bool>>
         {
                 {enTestType.Theory,    p => !p.VisionPassed },
                 {enTestType.Simulator, p => p.VisionPassed && !p.WrittenPassed },
                 {enTestType.Field,     p => p.VisionPassed && p.WrittenPassed && !p.StreetPassed }
         };

        public static bool isValidTestType(enTestType testType)
        {
            switch(testType)
            {
                case enTestType.Theory:
                case enTestType.Simulator:
                case enTestType.Field:
                    return true;

                    default 
                    : return false;
            }
        }
    }
}
