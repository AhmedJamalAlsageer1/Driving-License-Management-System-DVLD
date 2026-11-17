using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Context
{
    public static class AppDefaults
    {
        public static readonly TimeSpan AppointmentDateEditGracePeriod = TimeSpan.FromHours(1);
        public static readonly TimeSpan TestResultEditGracePeriodHours = TimeSpan.FromHours(6);
    }
}
