using DVLD.DTOs;
using DVLD.DTOs.Applications;
using DVLDDataAccessLayer;
using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer
{
    public static class clsApplicationDisplayBL
    {
        public static Result < List <DTO_LDLApplicationsDisplay> > GetAllApplications()
        {
            List<DTO_LDLApplicationsDisplay> applications = LDL_ApplicationsDisplayDAL.GetAll_LDLApplicationsDAL();

            if(applications == null)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "حصل خطأ أثناء تحميل البيانات.", enMessageType.Error);


            if (applications.Count <= 0)
                return Result <List <DTO_LDLApplicationsDisplay>>.Failure(null, "لا يوجد أي طلبات لعرضها، من فضلك أدخل طلبات ليتم عرضها.", null);

            return Result <List<DTO_LDLApplicationsDisplay>>.Success(applications, "", null);
        }
        public static Result<DTO_LDLApplicationsDisplay> FindApplicationByLDLAppID(int LDLApp)
        {
            if (!LDL_ApplicationsDisplayDAL.FindLDLApplicationBy_LDLAppID(LDLApp, out DTO_LDLApplicationsDisplay Application))
            {
                return Result<DTO_LDLApplicationsDisplay>.Failure(null, "عذرا، هذا الطلب غير موجود.", null);
            }

            if (Application == null)
                return Result<DTO_LDLApplicationsDisplay>.Failure(null, "حدث خطأ أثناء إرجاع الكائن.", enMessageType.Error);

            return Result<DTO_LDLApplicationsDisplay>.Success(Application, "", null);
        }
        public static Result< List <DTO_LDLApplicationsDisplay>> FindLDLApplicationByNationalNo(string NationalNo)
        {
            List<DTO_LDLApplicationsDisplay> Applications = LDL_ApplicationsDisplayDAL.FindLDLApplicationByNationalNo(NationalNo);

            if (Applications == null)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "حدث خطأ أثناء إرجاع الكائن", enMessageType.Error);

            if (Applications.Count <= 0)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "لا يوجد بيانات بهذا الاسم", null);

            return Result<List<DTO_LDLApplicationsDisplay>>.Success(Applications, null, null);
        }
        public static Result< List <DTO_LDLApplicationsDisplay>> GetApplicationsPersonByFullName(string FullName)
        {
            List<DTO_LDLApplicationsDisplay> Applications = LDL_ApplicationsDisplayDAL.FindLDLApplicationByFullName(FullName);

            if (Applications == null)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "حدث خطأ أثناء إرجاع الكائن", enMessageType.Error);

            if(Applications.Count <= 0)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "لا يوجد بيانات بهذا الاسم", null);

            return Result<List<DTO_LDLApplicationsDisplay>>.Success(Applications, null, null);
        }
        public static Result <List <DTO_LDLApplicationsDisplay>> GetAllApplicationByStatus(enApplicationStatus ApplicationStatus)
        {
            if (ApplicationStatus == enApplicationStatus.Unknown)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(null, "لا يوجد أبدا هذا النوع من البيانات", null);

            List <DTO_LDLApplicationsDisplay> dto = LDL_ApplicationsDisplayDAL.GetAll_LDLApplicationsByStatus(ApplicationStatus);

            if (dto == null)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(dto, "حصل خطأ أثناء تحميل البيانات", enMessageType.Error);

            if(dto.Count <= 0)
                return Result<List<DTO_LDLApplicationsDisplay>>.Failure(dto, "لا يوجد بيانات لتحميلها", null);


            return Result<List<DTO_LDLApplicationsDisplay>>.Success(dto, "", null);
        }

        public static Result <DTO_LDLFull_Info> Find_FullLDL_Info(int LDLAppID)
        {
            if(LDL_ApplicationsDisplayDAL.Find_FullLDLbyAppID(LDLAppID, out DTO_LDLFull_Info dto))
            {
                if (dto != null)
                    return Result < DTO_LDLFull_Info>.Success(dto, null, enMessageType.Info);
            }

            return Result<DTO_LDLFull_Info>.Failure(null, "هذا الطلب غير موجود.", enMessageType.Error);
        }

    }
}
