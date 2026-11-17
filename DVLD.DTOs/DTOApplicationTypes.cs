using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTOApplicationTypes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal fees { get; set; }

        public DTOApplicationTypes()
        {
            ID = -1;
            Title = string.Empty;
            fees = -1m;
        }

        public void Clear()
        {
            ID = -1;
            Title = string.Empty;
            fees = -1m;
        }
    }
}
