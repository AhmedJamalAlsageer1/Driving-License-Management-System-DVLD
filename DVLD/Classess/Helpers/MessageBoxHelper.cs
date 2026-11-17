using SharedLayer;
using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classess.Helpers
{
    internal static class MessageBoxHelper
    {
        public static void Show (string Message, enMessageType messageType = enMessageType.Info, string Caption = null, bool AlignRTL = false)
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;

            Caption = Caption ?? GetDefaultCaption(messageType);
            MessageBoxIcon Icon = GetIconByType(messageType);

            if(!AlignRTL)
                MessageBox.Show(Message, Caption, MessageBoxButtons.OK, Icon, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show(Message, Caption, MessageBoxButtons.OK, Icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static void Show <T> (Result <T> result, string Caption = null, bool AlginRTL = false)
        {
            if (result == null)
                return;

            Show(result.Message, result.messageType ?? enMessageType.Info, Caption, AlginRTL);
        }

        private static string GetDefaultCaption(enMessageType messageType)
        {
            switch (messageType)
            {
                case enMessageType.Error:
                    return "خطأ";
                case enMessageType.Warning:
                    return "تنبيه";
                case enMessageType.Info:
                    return "معلومة";

                default:
                    return "لاشيء";
            }
        }

        public static MessageBoxIcon GetIconByType(enMessageType messageType)
        {
            switch (messageType)
            {
                case enMessageType.Error:
                    return MessageBoxIcon.Error;

                case enMessageType.Warning:
                    return MessageBoxIcon.Warning;

                case enMessageType.Info:
                    return MessageBoxIcon.Information;

                    default:
                    return MessageBoxIcon.None;
            }
        }
    }
}
