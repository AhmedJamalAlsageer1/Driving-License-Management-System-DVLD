using DVLD.DTOs.License.ReNew;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer.License.ReNew_LDL
{
    internal class cls_RenewLDL_DAL
    {
        //public static DTO_Renew_Config LoadConfigurationData(int LicenseID, enApplicationTypes applicationTypes)
        //{
        //    string Query = @"SELECT
        //                    LC.ClassFees AS LicenseFees,
        //                    (SELECT Ap.ApplicationFees FROM ApplicationTypes Ap WHERE Ap.ApplicationTypeID = @applicationTypes) AS ApplicationFees
        //                FROM Licenses L
        //                INNER JOIN LicenseClasses LC ON L.LicenseClass = LC.LicenseClassID
        //                WHERE L.LicenseID = @LicenseID";
        //}
    }
}
