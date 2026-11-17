using DVLD.DTOs;
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
    public class clsTestTypes
    {
        public int ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Fees { get; private set; }

        clsTestTypes(DTOTestTypes dtoType)
        {
            this.ID  = dtoType.ID;
            this.Title = dtoType.Title;
            this.Description = dtoType.Description;
            this.Fees = dtoType.Fees;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesDAL.GetAllTestTypes();
        }

        public static bool GetTestTypeByID(int ID, out DTOTestTypes dtoType)
        {
            clsTestTypesDAL.GetTypeTestByID(ID, out dtoType);
            if (dtoType == null)
                return false;

            return true;
        }

        public static clsTestTypes GetTestTypeByID(int ID)
        {
            if(clsTestTypesDAL.GetTypeTestByID(ID, out DTOTestTypes dtoTestTypes))
            {
                if(dtoTestTypes != null)
                {
                    clsTestTypes TestTypes = new clsTestTypes(dtoTestTypes);
                    return TestTypes;
                }
            }
            return null;
        }

        public bool UpdateData(DTOTestTypes dtoTestTypes)
        {
            return clsTestTypesDAL.UpdateData(this.ID, dtoTestTypes);
        }

        public static Result <bool> isTestTypeExists(enTestType TestID)
        {
            if(!clsTestTypesDAL.isTestTypeExists(TestID))
                return Result<bool>.Failure(false, "هذا الامتحان غير موجود", SharedLayer.enums.enMessageType.Error);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        public static Result <bool> IsTypeTestFeesValid(enTestType testType, decimal TestFees)
        {
            if (!clsTestTypesDAL.IsTestTypeFeesValid(testType, TestFees))
                return Result<bool>.Failure(false, "الرسوم غير متوافقة مع الامتحان", enMessageType.Error);

            return Result<bool>.Success(true, null, enMessageType.Info);
        }

        public static Result <decimal> GetTestFees(enTestType testType)
        {
            if(clsTestTypesDAL.TryGetTestFeesByID(testType, out decimal Fees))
            {
                if (Fees > 0)
                    return Result<decimal>.Success(Fees, null, enMessageType.Info);
            }

            return Result<decimal>.Failure(-1, "حدث خطأ أنثاء استرجاع سعر الامتحان", enMessageType.Error);
        }
    }
}
