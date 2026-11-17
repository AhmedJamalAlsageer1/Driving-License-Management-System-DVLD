using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBisnessLayer.Validation
{
    public class FieldError
    {
        public string ControlName {  get; private set; }
        public string Message { get; private set; }

        public FieldError(string field, string message)
        {
            ControlName = field;
            Message = message;
        }

        public bool isMbox => ControlName == "mbox";
    }
}
