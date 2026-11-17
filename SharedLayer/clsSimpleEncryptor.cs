using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer
{
    public static class clsSimpleEncryptor
    {
        public static string Encrypt(string PlainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(PlainText));
        }

        public static string Decrypt(string PlainText)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(PlainText));
        }
        public enum ApplicationTypes : byte
        {
            NewLocalLicense = 1,
            RenewDrivingLicense = 2,
            ReplacementForLost = 3,
            ReplacementForDamaged = 4,
            ReleaseDetainedDriving = 5,
            NewInternationalLicense = 6,
            RetakeTest = 7
        }
    }
}
