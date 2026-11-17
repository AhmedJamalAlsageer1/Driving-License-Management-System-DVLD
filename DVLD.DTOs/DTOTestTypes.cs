using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTOTestTypes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }

        public DTOTestTypes()
        {
            ID = -1;
            Title = string.Empty;
            Description = string.Empty;
            Fees = -1m;
        }

        public void Clear()
        {
            ID = -1;
            Title = string.Empty;
            Description = string.Empty;
            Fees = -1m;
        }
    }
}
