using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal static class MaskedTextBoxHelpers
    {
        public static void MoveCursorToFirstEmptyPrompt(this MaskedTextBox mtxtbox)
        {
            mtxtbox.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;

            int IndexOfEmptyPrompt = mtxtbox.Text.IndexOf(mtxtbox.PromptChar);

            if(IndexOfEmptyPrompt == -1)
                IndexOfEmptyPrompt = mtxtbox.Text.Length;

            mtxtbox.Select(IndexOfEmptyPrompt, 0);
        }

        public static void AdjustCusorToValidInputPosotion(this MaskedTextBox mtextbox, MouseEventArgs e, string AllowedChars = "0123456789")
        {
            int ClickIndex = mtextbox.GetCharIndexFromPosition(e.Location);
            int LastPostionValid = mtextbox.Text.IndexOfAny(AllowedChars.ToArray());

            if(LastPostionValid == -1)
            {
                LastPostionValid = mtextbox.Text.Length;
            }
            else
            {
                LastPostionValid++;
            }

            if(ClickIndex > mtextbox.Text.Length || ClickIndex > LastPostionValid)
                mtextbox.Select(LastPostionValid, 0);
        }
    }
}
