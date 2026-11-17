using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Contants.Interface
{
    public interface ITestProgressReadOnly
    {
        bool VisionPassed { get; }
        bool WrittenPassed { get; }
        bool StreetPassed { get; }
    }
}
