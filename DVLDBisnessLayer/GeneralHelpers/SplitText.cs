using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.GeneralHelpers
{
    internal static class SplitText
    {
        public static bool TrySplitFullName(
            string fullName,
            out string firstName,
            out string secondName,
            out string thirdName,
            out string lastName)
        {
            firstName = secondName = thirdName = lastName = null;

            if (string.IsNullOrWhiteSpace(fullName))
                return false;

            var Parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (Parts.Length < 3 || Parts.Length > 4)
                return false;

            firstName = Parts[0];
            secondName = Parts[1];
            thirdName = Parts.Length == 4 ? Parts[2] : null;
            lastName = Parts.Length == 4 ? Parts[3] : Parts[2];

            return true;
        }
    }
}
