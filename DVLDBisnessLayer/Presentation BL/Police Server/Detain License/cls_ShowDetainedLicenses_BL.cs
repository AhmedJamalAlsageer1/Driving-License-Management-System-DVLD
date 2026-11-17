using DVLD.DTOs.Presention_DTO.Police_Actions.Detain_License;
using DVLDDataAccessLayer.PoliceActions;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Presentation_BL.Police_Server.Detain_License
{
    public class cls_ShowDetainedLicenses_BL
    {
        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicenses()
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicenses();

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTO_ShowDetainedLicenses> ShowAllDetainedLicensesBy_DetainID(int DetainID)
        {
            bool? result_Show = cls_ShowDetainedLicenses_DAL.ShowDetainedLicenseBy_DetainID(DetainID, out DTO_ShowDetainedLicenses dto);

            if (!result_Show.HasValue)
                return Result<DTO_ShowDetainedLicenses>.TechnicalFailure(null);

            if(!result_Show.Value)
                return Result<DTO_ShowDetainedLicenses>.Failure(null, "هذا القيد غير موجود!", SharedLayer.enums.enMessageType.Warning);

            return Result<DTO_ShowDetainedLicenses>.Success(dto, null);
        }

        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicensesBy_LicenseID(int LicenseID)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicensesBy_LicenseID(LicenseID);

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicensesBy_ReleaseApplicationID(int ReleaseApplicationID)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicensesBy_ReleaseApplicationID(ReleaseApplicationID);

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }


        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicensesBy_IsReleased(bool IsReleased)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicensesBy_IsReleased(IsReleased);

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicensesBy_FullName(string FullName)
        {
            bool result_Spilit = GeneralHelpers.SplitText.TrySplitFullName
                (FullName, out string FirstName, out string SecondName, out string ThirdName, out string LastName);
            if (!result_Spilit)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicensesBy_FullName
                (FirstName, SecondName, ThirdName, LastName);

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<List<DTO_ShowDetainedLicenses>> ShowAllDetainedLicensesBy_NationalNo(string NationalNo)
        {
            List<DTO_ShowDetainedLicenses> dto_ShowAll = cls_ShowDetainedLicenses_DAL.ShowAllDetainedLicensesBy_NationalNo(NationalNo);

            if (dto_ShowAll == null)
                return Result<List<DTO_ShowDetainedLicenses>>.TechnicalFailure(null);

            return Result<List<DTO_ShowDetainedLicenses>>.Success(dto_ShowAll, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
