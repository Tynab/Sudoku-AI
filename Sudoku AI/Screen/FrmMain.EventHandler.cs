using System.Windows.Forms;
using static Sudoku_AI.Script.Constant;
using static YANF.Script.YANEvent;

namespace Sudoku_AI.Screen
{
    public partial class FrmMain
    {
        #region Ctrl
        // MoveFrm_MouseDown sound mode
        private void MoveFrmMod_MouseDown(object sender, MouseEventArgs e)
        {
            // base
            MoveFrm_MouseDown(sender, e);
            // sound
            SND_CHG.Play();
        }
        #endregion

        #region Txt
        // Textbox Sudoku numeric input
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
