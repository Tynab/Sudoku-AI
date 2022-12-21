using System.Windows.Forms;
using static Sudoku_AI.Script.Constant;
using static YANF.Script.YANEvent;

namespace Sudoku_AI.Script
{
    internal static class EventHandler
    {
        #region Ctrl
        /// <summary>
        /// MoveFrm_MouseDown sound mode.
        /// </summary>
        internal static void MoveFrmMod_MouseDown(object sender, MouseEventArgs e)
        {
            // base
            MoveFrm_MouseDown(sender, e);
            // sound
            SND_CHG.Play();
        }

        #endregion
    }
}
