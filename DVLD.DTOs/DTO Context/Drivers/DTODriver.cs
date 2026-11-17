using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.License
{
    public class DTODriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public DTODriver()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = -1;
        }

        public DTODriver(int _DriverID, int _PersonID, DateTime _CreatedDate, int _CreatedByUserID)
        {
            DriverID = _DriverID;
            PersonID = _PersonID;
            CreatedDate = _CreatedDate;
            CreatedByUserID = _CreatedByUserID;
        }
    }
}
