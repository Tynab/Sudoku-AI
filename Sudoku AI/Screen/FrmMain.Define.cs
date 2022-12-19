using YANF.Control;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Screen
{
    public partial class FrmMain
    {
        private void InitItems() => InitTxtCells();

        #region Txt
        private YANTxt[,] _txtCells;
        // Initialize array txtCell
        private void InitTxtCells() => _txtCells = new YANTxt[MAX_W, MAX_H]
            {
                { txtCell00, txtCell01, txtCell02, txtCell03, txtCell04, txtCell05, txtCell06, txtCell07, txtCell08 },
                { txtCell10, txtCell11, txtCell12, txtCell13, txtCell14, txtCell15, txtCell16, txtCell17, txtCell18 },
                { txtCell20, txtCell21, txtCell22, txtCell23, txtCell24, txtCell25, txtCell26, txtCell27, txtCell28 },
                { txtCell30, txtCell31, txtCell32, txtCell33, txtCell34, txtCell35, txtCell36, txtCell37, txtCell38 },
                { txtCell40, txtCell41, txtCell42, txtCell43, txtCell44, txtCell45, txtCell46, txtCell47, txtCell48 },
                { txtCell50, txtCell51, txtCell52, txtCell53, txtCell54, txtCell55, txtCell56, txtCell57, txtCell58 },
                { txtCell60, txtCell61, txtCell62, txtCell63, txtCell64, txtCell65, txtCell66, txtCell67, txtCell68 },
                { txtCell70, txtCell71, txtCell72, txtCell73, txtCell74, txtCell75, txtCell76, txtCell77, txtCell78 },
                { txtCell80, txtCell81, txtCell82, txtCell83, txtCell84, txtCell85, txtCell86, txtCell87, txtCell88 }
            };
        #endregion
    }
}
