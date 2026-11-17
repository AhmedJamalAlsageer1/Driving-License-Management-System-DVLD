using SharedLayer.Helpers.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Presention_DTO.Licnese.Release_Detain
{
    public class DTO_ReleaseDetain_Info
    {
        public int DetainID { get; set; }
        public DateTime DatainDate { get; set; }
        public decimal AppFees { get; set; }
        public decimal FineFees { get; set; }
        public int LicenseID { get; set; }
        public string Createdby { get; set; }
        public bool IsReleased { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public string ReleaseByUser { get; set; }
    }
}
