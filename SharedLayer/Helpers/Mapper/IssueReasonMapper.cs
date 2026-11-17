using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedLayer.Helpers.Mapper
{
    public class IssueReasonMapper
    {
        private static readonly Dictionary<enIssueReasonLicense, string> _map = new Dictionary<enIssueReasonLicense, string>
        {
            {enIssueReasonLicense.FirstTime, "FirstTime" },
            {enIssueReasonLicense.Renew, "Renew" },
            {enIssueReasonLicense.ReplacementForDamaged, "Replacement for Damaged." },
            {enIssueReasonLicense.ReplacementForLost, "Replacement for Lost." }
        };

        public static string ToDisplayString(enIssueReasonLicense IssueReason)
        {
            return _map.TryGetValue(IssueReason, out string result) ? result : "غير معروف";
        }
    }
}
