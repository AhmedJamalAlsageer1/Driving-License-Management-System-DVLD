using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs.Test
{
    public class DTOTest
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public int? ReTake_AppID { get; set; }

        public DTOTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = null;
            CreatedByUserID = -1;
            ReTake_AppID = null;
        }

        public static DTOTest GetDTO(int testid, int testappointmentID, bool testresult, 
            string notes, int createdbyuserid, int? retake_appid)
        {
            return new DTOTest
            {
                TestID = testid,
                TestAppointmentID = testappointmentID,
                TestResult = testresult,
                Notes = notes,
                CreatedByUserID = createdbyuserid,
                ReTake_AppID = retake_appid,
            };
        }
    }
}
