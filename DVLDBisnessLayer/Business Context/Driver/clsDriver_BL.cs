using DVLD.DTOs.License;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Driver
{
    internal class clsDriver_BL
    {
        public int DriverID { get; private set; }
        public int PersonID { get; private set; }
        public int CreatedByUserID { get; private set; }
        public DateTime CreatedDate { get; private set; }


        internal clsDriver_BL(int personID, int createdByUserID)
        {
            DriverID = -1;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = DateTime.MinValue;
        }

        clsDriver_BL()
        {

        }

        private clsDriver_BL GetObject(DTODriver dto)
        {
            return new clsDriver_BL
            {
                DriverID = dto.DriverID,
                PersonID = dto.PersonID,
                CreatedByUserID = dto.CreatedByUserID,
                CreatedDate = dto.CreatedDate
            };
        }

        internal clsDriver_BL FindDriverByPersonID(int PersonID)
        {
            if(clsDriver_DAL.FindDriverByPersonID(PersonID, out DTODriver dto))
            {
                if (dto != null)
                    return GetObject(dto);
            }

            return null;
        }

        public static OperationResult IsPersonDriver(int personID)
        {
            bool? isPersonDriver = clsDriver_DAL.isPersonDriver(personID);
            if (!isPersonDriver.HasValue)
                return OperationResult.TechnicalFailure("خطأ تقني قد حصل أثناء البحث عن حال هل المتقدم سواق؟",
                    SharedLayer.enums.enMessageType.Error);

           if(!isPersonDriver.Value)
           {
                return OperationResult.Failure("هذا السائق غير موجود",
                    SharedLayer.enums.enMessageType.Warning);
           }

            return OperationResult.Seccuss(null, SharedLayer.enums.enMessageType.Info);
        }
        public static Result<int> GetDriverIDByPersonID(int PersonID)
        {
            int _DriverID = -1;
            bool? result_GetDriverID = clsDriver_DAL.TryGetDriverIDByPersonID(PersonID, out int? DR_ID);
            if(!result_GetDriverID.HasValue)
                return Result <int>.Failure(-1, "حصل خطأ أثناء التعامل محاولة استرجاع معرف السائق", SharedLayer.enums.enMessageType.Error);

            if(!result_GetDriverID.Value)
                return Result<int>.Failure(-1, "هذا السائق غير موجود من فضلك اختر سائقا آخر!", SharedLayer.enums.enMessageType.Warning);


            _DriverID = DR_ID.Value;
            return Result<int>.Success(_DriverID, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
