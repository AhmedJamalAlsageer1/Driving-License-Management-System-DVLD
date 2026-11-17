using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD
{
    internal static class TextHelpers
    {
        public static string SplitFullName(string Input)
        {
            if(string.IsNullOrWhiteSpace(Input))
                return string.Empty;

            string [] Parts = Input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", Parts);
        }
    }
}
