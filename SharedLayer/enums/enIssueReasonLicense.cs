using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer
{
    public enum enIssueReasonLicense : byte
    {
        FirstTime = 1,
        Renew = 2,
        ReplacementForDamaged = 3,
        ReplacementForLost = 4,
        UnKnown = 254
    };
}
