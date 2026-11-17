using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTOUserInfo
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool isActive { get; set; }

        public DTOUserInfo()
        {
            UserID = -1;
            PersonID = -1;
            FullName = string.Empty;
            UserName = string.Empty;
            isActive = false;
        }

        public void Clear()
        {
            UserID = -1;
            PersonID = -1;
            FullName = string.Empty;
            UserName = string.Empty;
            isActive = false;
        }
    }
}
