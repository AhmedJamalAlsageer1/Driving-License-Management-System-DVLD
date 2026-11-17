using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Classess.Helpers
{
    internal static class ComboBoxHelper
    {
        public static List <ComboItem<Nullable<T>>> GetEnumOptionsForFilterAndAddInFirst <T> (
            Func <T, string> Localizer,
            params T[] Exclude)
            where T : struct, Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Where(x => !Exclude.Contains(x))
                .Select(x => new ComboItem<Nullable<T>>
                {
                    value = x,
                    Display = Localizer(x)
                })
                .Prepend(new ComboItem <T?>
                {
                    value = null,
                    Display = "الكل"
                })
                .ToList();
        }
    }
}
