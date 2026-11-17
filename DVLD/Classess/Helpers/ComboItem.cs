using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Classess.Helpers
{
    internal class ComboItem <T>
    {
        public T value { get; set; }
        public string Display { get; set; }

        public override string ToString() => Display;
    }
}
