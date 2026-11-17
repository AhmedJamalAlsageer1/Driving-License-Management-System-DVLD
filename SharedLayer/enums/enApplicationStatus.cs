using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.enums
{
    public enum enApplicationStatus : byte
    {
        New = 1,
        Cancelled = 2,
        Completed = 3,
        Unknown = 254
    }
}
