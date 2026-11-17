using DVLD.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.ViewModel
{
    internal class vmApplication
    {
        public string PersonID { get; set; }

        public string ApplicationID { get; set; }

        public string ApplicationDate { get; set; }

        public string ApplicationTypeID { get; set; }

        public string ApplicationStatus { get; set; }

        public string LastStatusDate { get; set; }

        public string PaidFees { get; set; }

        public string CreatedByUserID { get; set; }
    }
}
