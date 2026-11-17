using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Helpers.Mapper
{
    public class LicenseClassMapper
    {
        private static readonly Dictionary<enLicenseClass, string> _map = new Dictionary<enLicenseClass, string>
        {
            {enLicenseClass.SmallMotorcycle, "Class 1 - Small Motorcycle" },
            {enLicenseClass.HeavyMotorcycleLicense, "Class 2 - Heavy Motorcycle License" },
            {enLicenseClass.OrdinaryDrivingLicense, "Class 3 - Ordinary driving license" },
            {enLicenseClass.Commercial, "Class 4 - Commercial" },
            {enLicenseClass.Agricultural, "Class 5 - Agricultural" },
            {enLicenseClass.SmallAndMediumBus, "Class 6 - Small and medium bus" },
            {enLicenseClass.TruckAndHeavyVehicle, "Class 7 - Truck and heavy vehicle" }
        };

        public static string ToDisplayString(enLicenseClass LicenseClass)
        {
            return _map.TryGetValue(LicenseClass, out string result)? result : "غير معروف";
        }
    }
}
