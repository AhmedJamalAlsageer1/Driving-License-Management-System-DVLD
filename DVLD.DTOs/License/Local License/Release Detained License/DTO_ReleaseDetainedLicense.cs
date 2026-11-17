using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License.Local_License.Release_Detained_License
{
    public class DTO_ReleaseDetainedLicense : I_LocalLicense
    {
        public enApplicationTypes ApplicationTypes { get; set; }
        public int PersonID { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public decimal Fines { get; set; }
        public int CreatedByUserID { get; set; }
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }
    }
}
