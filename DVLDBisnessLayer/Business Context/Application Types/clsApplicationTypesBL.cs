using DVLD.DTOs;
using DVLDDataAccessLayer;
using SharedLayer;
using SharedLayer.enums;
using System.Data;

namespace DVLDBisnessLayer
{
    public class clsApplicationTypesBL
    {
        public int ID { get; private set; }
        public string Title { get; private set; }
        public decimal fees { get; private set; }


        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesDAL.GetAllApplicationTypes();
        }

        public static Result <DTOApplicationTypes> GetApplicationType(SharedLayer.enums.enApplicationTypes AppType)
        {
            int id = (int)AppType;
            return GetApplicationTypeByID(id);
        }

        public static Result <DTOApplicationTypes> GetApplicationTypeByID(int ID)
        {
            if (clsApplicationTypesDAL.GetAppliationTypeByID(ID, out DTOApplicationTypes dtoType))
            {
                return dtoType != null ?
                    Result<DTOApplicationTypes>.Success(dtoType, string.Empty)
                    : Result<DTOApplicationTypes>.Failure(null, "تم العثور على العنصر ولكن دون بيانات مرتبطة.");
            }
            return Result<DTOApplicationTypes>.Failure(null, "تعذر العثور على نوع البيانات هذا"); ;
        }

        private static Result <clsApplicationTypesBL> ConvertFromDTOtoObject(Result <DTOApplicationTypes> resultDTO)
        {
            if (!resultDTO.isSuccess || resultDTO.Value == null)
                return Result <clsApplicationTypesBL> .Failure(null, resultDTO.Message);

            var dto = resultDTO.Value;
            var BL = new clsApplicationTypesBL
            {
                ID = dto.ID,
                Title = dto.Title,
                fees = dto.fees
            };

            return Result <clsApplicationTypesBL>.Success(BL, "");
        }

        public static Result <clsApplicationTypesBL> GetApplicationTypebyID(int ID)
        {
            return ConvertFromDTOtoObject(GetApplicationTypeByID(ID));
        }

        private OperationResult SetApplicationType(DTOApplicationTypes dtoType)
        {
            if (!string.IsNullOrWhiteSpace(dtoType.Title))
            {
                Title = dtoType.Title;
            }
            else
                OperationResult.Failure("من فضلك تأكد من أن العنوان المراد إدخاله غير فارغ");

            if (fees > 0)
            {
                fees = dtoType.fees;
            }
            else
                return OperationResult.Failure("من فضلك أدخل رقما صالحا أعلى من 1");

            return OperationResult.Failure("حصل خطأ غير معروف من فضلك أخبر فريق التطوير بذلك.");
        }

        public OperationResult UpdateApplicationType(DTOApplicationTypes dtoType)
        {
            OperationResult result = SetApplicationType(dtoType);
            if (!result.IsSuccess)
                return result;

            if (!clsApplicationTypesDAL.UpdateApplicationType(dtoType))
                return OperationResult.Failure("فشلت عملية التحديث يرجى المحاولة مرة أخرى");

            return OperationResult.Seccuss("اكتملت عملية التعديل بنجاج.");
        }

        public static Result <bool> isApplicationTypeExists(enApplicationTypes ApplicationType)
        {
            if (!clsApplicationTypesDAL.IsApplicationTypeExists(ApplicationType))
                return Result<bool>.Failure(false, "هذا النوع من الامتحانات غير موجود", SharedLayer.enums.enMessageType.Error);

            return Result<bool>.Success(true, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result <bool> isApplicationFeesValid(enApplicationTypes ApplicationTypeID,decimal ApplicationFees)
        {
            if (!clsApplicationTypesDAL.IsApplicationFeesValid(ApplicationTypeID, ApplicationFees))
                return Result<bool>.Failure(false, "خطأ في قيمة الطلب", SharedLayer.enums.enMessageType.Error);

            return Result<bool>.Success(true, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result <decimal> GetApplicationFees(enApplicationTypes applicationTypes)
        {
            if(clsApplicationTypesDAL.TryGetApplicationFees(applicationTypes, out decimal Fees))
            {
                if (Fees > 0)
                    return Result<decimal>.Success(Fees, null, SharedLayer.enums.enMessageType.Info);
            }

            return Result<decimal>.Failure(-1, "حدث خطأ أثناء استرجاع رسوم الامتحان", SharedLayer.enums.enMessageType.Error);
        }
    }
}
