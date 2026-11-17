using DVLD.Classess.Helpers;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classess.UIHandlers
{
    internal static class ResultDisplayHandler
    {
        public static void DisplayFailureMessageLblOrMbox <T>(Label lbl, Result<T> result, string Caption = null, bool AlignRTL = false)
        {
            if (result == null)
            {
                lbl.Visible = false;
                return;
            }

            if (result.messageType != null)
            {
                lbl.Visible = false;
                MessageBoxHelper.Show(result, Caption, AlignRTL);
                return;
            }

            lbl.Text = result.Message;
            lbl.Visible = true;
        }
    }
}
