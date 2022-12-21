using System.Windows.Forms;

namespace Sudoku_AI.Screen
{
    public partial class FrmMain
    {
        #region Txt
        /// <summary>
        /// Textbox Sudoku numeric input.
        /// </summary>
        private void TxtNumeric_Keypress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar is (< '1' or > '9') and not (char)8 and not (char)3 and not (char)22 and not (char)24 and not (char)1 and not (char)26)
            {
                e.KeyChar = '\0';
            }
        }
        #endregion
    }
}
