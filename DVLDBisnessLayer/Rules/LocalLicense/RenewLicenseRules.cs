using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Rules.LocalLicense
{
    internal static class RenewLicenseRules
    {
        public const int MaxRenewalYears = 2;
        public const int RenewGracePeriodDays = 30;
    }
}
