using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.BaseLicenseRequest
{
    public class clsBaseApplication_BL
    {
        public int ApplicationID { get; protected set; }
        public int PersonID { get; protected set; }
        public DateTime ApplicationDate{ get; protected set; }
        public enApplicationTypes ApplicationTypeID { get; protected set; }
        public enApplicationStatus ApplicationStatus{ get; protected set; }
        public DateTime LastStatusDate{ get; protected set; }
        public decimal PaidFees_App { get; protected set; }
        public int CreatedByUserID { get; protected set; }
    }
}
