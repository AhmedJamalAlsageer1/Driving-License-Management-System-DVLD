using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Helpers.Mapper
{
    public class GendorMapper
    {
        public static string ToDispalyString(bool Gendor)
        {
            return Gendor ? "Female" : "Male";
        }
    }
}
