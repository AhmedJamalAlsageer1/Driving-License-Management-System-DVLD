using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal class clsExtentionMethods
    {
        private static Type[] DefaultTypes = { typeof(TextBox), typeof(MaskedTextBox), typeof(ComboBox) };

        public static void BindKeyDownToHandlerInControls(Control container, KeyEventHandler handler, bool IncludeChildControls = true, IEnumerable <Type> TargetControlsTypes = null)
        {
            var accepted = (TargetControlsTypes ?? DefaultTypes).ToArray();

            foreach (Control c in container.Controls)
            {
                if(accepted.Any(t => t.IsAssignableFrom(c.GetType())))
                {
                    c.KeyDown -= handler;
                    c.KeyDown += handler;

                    if (IncludeChildControls == true && c.HasChildren)
                        BindKeyDownToHandlerInControls(c, handler, IncludeChildControls, accepted);
                }
                
            }
        }

        public static void HideChildControls(Control Container, Control ShowThisControl = null)
        {
            foreach(Control c in Container.Controls)
            {
                c.Visible = false;
            }

            if(ShowThisControl != null)
                ShowThisControl.Visible = true;
        }
    }
}
