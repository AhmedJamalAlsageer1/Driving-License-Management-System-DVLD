using DVLD.DTOs;
using DVLD.ViewModel;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Mapper
{
    internal class VMAppliationMapper
    {
        public static vmApplication ToVMAppliation(DTO_LDL_App dto)
        {
            vmApplication vmApplication = new vmApplication();
            vmApplication.ApplicationID = dto.ApplicationID.ToString();
            vmApplication.PersonID = dto.PersonID.ToString();
            vmApplication.ApplicationDate = dto.ApplicationDate.ToString();
            vmApplication.ApplicationTypeID = dto.ApplicationTypeID.ToString();
            vmApplication.ApplicationStatus = GetApplicationStatus(dto.ApplicationStatus);
            vmApplication.LastStatusDate = dto.LastStatusDate.ToString();
            vmApplication.PaidFees = dto.PaidFees.ToString();
            vmApplication.CreatedByUserID = dto.CreatedByUserID.ToString();

            return vmApplication;
        }

        private static string GetApplicationStatus(SharedLayer.enums.enApplicationStatus ApplicationStatus)
        {
            if (ApplicationStatus == enApplicationStatus.New)
                return "New";
            else if (ApplicationStatus == enApplicationStatus.Cancelled)
                return "Cancelled";
            else if (ApplicationStatus == enApplicationStatus.Completed)
                return "Completed";

            else
                return "خطأ قيمة غير محسوبة، يرجى مراجعة الحالة.";
        }
    }
}
