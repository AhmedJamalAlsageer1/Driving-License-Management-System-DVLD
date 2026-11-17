using DVLD.DTOs;
using DVLD.DTOs.Drivers;
using DVLDBisnessLayer.GeneralHelpers;
using DVLDDataAccessLayer.Driver;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Driver
{
    public static class clsDriverInfo_BL
    {
        public static Result<List<DTODrivers_Info>> GetAllDriversInfo()
        {
            List<DTODrivers_Info> result_DriversInfo = clsDriverInfo_DAL.GetAllDriversInfo();

            if (result_DriversInfo == null)
                return Result<List<DTODrivers_Info>>.Failure(null, "حصل خطأ في قواعد البيانات");

            if (result_DriversInfo.Count == 0)
                return Result<List<DTODrivers_Info>>.Success(result_DriversInfo, "نحجت العملية، ولكن لا يوجد بيانات لعرضها");

            return Result<List<DTODrivers_Info>>.Success(result_DriversInfo, null);
        }

        public static Result <DTODrivers_Info> GetDriverInfo_ByDriverID(int DriverID)
        {
            bool? result_DriverInfo =  clsDriverInfo_DAL.GetDriverInfo_ByDriverID(DriverID, out DTODrivers_Info dto);
            if (!result_DriverInfo.HasValue)
                return Result<DTODrivers_Info>.Failure(null, "حصل خطأ أنثاء محاولة الوصول لقاعدة البيانات!", SharedLayer.enums.enMessageType.Error);

            if (!result_DriverInfo.Value)
                return Result<DTODrivers_Info>.Failure(dto, "يرجى إدخال معرف صحيح من فضلك", SharedLayer.enums.enMessageType.Warning);


            return Result<DTODrivers_Info>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTODrivers_Info> GetDriverInfo_ByPersonID(int PersonID)
        {
            bool? result_DriverInfo = clsDriverInfo_DAL.GetDriverInfo_ByPersonID(PersonID, out DTODrivers_Info dto);
            if (!result_DriverInfo.HasValue)
                return Result<DTODrivers_Info>.Failure(null, "حصل خطأ أنثاء محاولة الوصول لقاعدة البيانات!", SharedLayer.enums.enMessageType.Error);

            if (!result_DriverInfo.Value)
                return Result<DTODrivers_Info>.Failure(null, "يرجى إدخال معرف صحيح من فضلك", SharedLayer.enums.enMessageType.Warning);


            return Result<DTODrivers_Info>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }


        public static Result<DTODrivers_Info> GetDriverInfo_ByNationalNo(string FullName)
        {
            bool? result_DriverInfo = clsDriverInfo_DAL.GetDriverInfo_NationalNo(FullName, out DTODrivers_Info dto);
            if (!result_DriverInfo.HasValue)
                return Result<DTODrivers_Info>.Failure(null, "حصل خطأ أنثاء محاولة الوصول لقاعدة البيانات!", SharedLayer.enums.enMessageType.Error);

            if (!result_DriverInfo.Value)
                return Result<DTODrivers_Info>.Failure(null, "يرجى إدخال معرف صحيح من فضلك", SharedLayer.enums.enMessageType.Warning);


            return Result<DTODrivers_Info>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTODrivers_Info> GetDriverInfo_ByFullName(string FullName)
        {
            if(!SplitText.TrySplitFullName(FullName, out string FirstName, out string SecondName,
                out string ThirdName, out string LastName))
            {
                return Result<DTODrivers_Info>.Failure(null, "من فضلك تأكد من إدخال ثلاث أسماء على الأقل أو 4 على الأكثر", SharedLayer.enums.enMessageType.Warning);
            }


            bool? result_DriverInfo = clsDriverInfo_DAL.GetDriverInfo_FullName(FirstName, 
                SecondName, ThirdName, LastName, out DTODrivers_Info dto);
            if (!result_DriverInfo.HasValue)
                return Result<DTODrivers_Info>.Failure(null, "حصل خطأ أنثاء محاولة الوصول لقاعدة البيانات!", SharedLayer.enums.enMessageType.Error);



            if (!result_DriverInfo.Value)
                return Result<DTODrivers_Info>.Failure(null, "يرجى إدخال معرف صحيح من فضلك", SharedLayer.enums.enMessageType.Warning);



            return Result<DTODrivers_Info>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
