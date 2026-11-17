using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTOComboBoxItem
    {
        public int Value {  get; set; }
        public string Display { get; set; }

        public override string ToString() => Display;

        public DTOComboBoxItem()
        {
            Value = -1;
            Display = null;
        }

        public void Clear()
        {
            Value = -1;
            Display = null;
        }
    }
}
