using DVLD.DTOs;
using DVLD.DTOs.License;
using DVLDDataAccessLayer;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer
{
    public class clsLicenseClassesBL
    {
        public int LicenseClassID { get; private set; }
        public string ClassName { get; private set; }
        public string ClassDescription { get; private set; }
        public byte MinimumAllowedAge { get; private set; }
        public byte DefaultValidityLength { get; private set; }
        public decimal ClassFees { get; private set; }

        internal static Result <DTOLicenseClass> FindLicenseClassByID(enLicenseClass licenseClass)
        {
            bool? result_FindLicensesClass = clsLicenseClassesDAL.FindLicenseClassByID(licenseClass, out DTOLicenseClass dto);

            if (!result_FindLicensesClass.HasValue)
                return Result<DTOLicenseClass>.Failure(null, "حصل خطأ في معالجة البيانات، يرجى المحاولة مرة أخرى!",
                    enMessageType.Error);

            if(!result_FindLicensesClass.Value)
                return Result<DTOLicenseClass>.Failure(null, "هذه الفئة غير موجودة من فضلك حاول إدخال فئة أخرى!",
                    enMessageType.Warning);

            return Result<DTOLicenseClass>.Success(dto, null, enMessageType.Info);
        }

        public static List<DTOComboBoxItem> GetAllLicenseClassNames()
        {
            return clsLicenseClassesDAL.GetLicenseClassNames();
        }

        public static bool IsClassExists(enLicenseClass LicenseClass)
        {
            return Enum.IsDefined(typeof(enLicenseClass), LicenseClass) &&
                LicenseClass != enLicenseClass.Unknown;
        }

        public static Result <string> GetLicenseClassNameByID(enLicenseClass licenseClass, out string LicenseName)
        {
            if (clsLicenseClassesDAL.TryGetLicenseClassNameByID(licenseClass, out LicenseName))
            {
                if(LicenseName != null)
                    return Result<string>.Success(LicenseName, null, SharedLayer.enums.enMessageType.Info);
            }

            return Result<string>.Failure(null, "اسم الفئة غير متوفر.", SharedLayer.enums.enMessageType.Error);
        }

        public static Result <byte?> GetDefaultValidityLengthByID(enLicenseClass LicenseClass)
        {
            bool? result_DefaultValidityLength = clsLicenseClassesDAL.GetDefaultValidityLengthByID(LicenseClass, out byte? value);
            if (!result_DefaultValidityLength.HasValue)
                return Result<byte?>.Failure(null, "حصل خطأ أثناء محاولة معالجة البيانات!", enMessageType.Error);

            if (!result_DefaultValidityLength.Value || value == null)
                return Result<byte?>.Failure(null, "هذه الفئة غير موجودة من فضلك اختر واحدة أخرى!", enMessageType.Warning);

            return Result<byte?>.Success(value, null, enMessageType.Info);
        }

        internal static Result<decimal> GetLicenseFees(enLicenseClass licenseClass)
        {
            bool? result_GetFees = clsLicenseClassesDAL.TryGetLicenseClassFeesByID(licenseClass, out decimal? Fees);
            if (!result_GetFees.HasValue )
                return Result<decimal>.Failure(-1m, "حصل خطأ أثناء معالجة البيانات، يرحى المحاولة في وقت آخر", enMessageType.Error);

            else if (!result_GetFees.Value || Fees == null)
                return Result<decimal>.Failure(-1m, "هذا الفئة من الرخص لا وجود لها من فضلك اختر واحدة صالحة", enMessageType.Warning);

            return Result<decimal>.Success(Fees.Value, null);
        }

        internal static Result<int> GetLicenseValidity(enLicenseClass licenseClass)
        {
            bool? result_LicenseValidity = clsLicenseClassesDAL.TryGetLicenseValidity(licenseClass, out int? Fees);
            if (!result_LicenseValidity.HasValue)
                return Result<int>.Failure(-1, "حصل خطأ غير متوقع، يرجى المحاولة في وقت آخر", enMessageType.Error);

            if (result_LicenseValidity.Value && Fees.HasValue)
            {
                if (Fees != null)
                    return Result<int>.Success(Fees.Value, null);
            }

            return Result<int>.Failure(-1, "يرجى التأكد من أن البيانات المدخلة صحيحة", enMessageType.Warning);
        }
    }
}
