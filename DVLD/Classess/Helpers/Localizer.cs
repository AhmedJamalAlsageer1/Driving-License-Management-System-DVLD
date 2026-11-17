using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Classess.Helpers
{
    internal static class Localizer
    {
        private static readonly Dictionary<string, string> EnumTranslations =
            new Dictionary<string, string>
            {
                    { "New", "جديد" },
                    { "Cancelled", "ملغي" },
                    { "Completed", "مكتمل" },
                    { "Unknown", "غير معروف" },
                    { "Male", "ذكر" },
                    { "Female", "أنثى" },
                    { "Admin", "مسؤول" },
                    { "User", "مستخدم" }
            };

        public static string TranslateEnum(string key)
        {
            return EnumTranslations.TryGetValue(key, out string value) ? value : key;
        }
    }
}
