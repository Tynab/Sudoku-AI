using System.Linq;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Board
    {
        #region Properties
        internal Area[,] Areas { get; set; }
        internal bool IsBreak { get; set; } = false;
        #endregion

        #region Methods
        //
        private void CheckBreak()
        {
            //
            var arrH = new string[9];
            var arrV = new string[9];
            for (var i = 0; i < W; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    if (Areas[i, j].IsBreak)
                    {
                        IsBreak = true;
                        return;
                    }
                    for (var k = 0; k < W; k++)
                    {
                        for (var l = 0; l < H; l++)
                        {
                            arrH[Areas[i, j].Cells[k, l].X] = Areas[i, j].Cells[k, l].Value;
                            arrV[Areas[i, j].Cells[k, l].Y] = Areas[i, j].Cells[k, l].Value;
                        }
                    }
                }
            }
            //
            var valHs = arrH.ToList();
            var valVs = arrV.ToList();
            if (valHs.Count != valHs.Distinct().Count() || valVs.Count != valVs.Distinct().Count())
            {
                IsBreak = true;
                return;
            }
        }
        #endregion
    }
}
