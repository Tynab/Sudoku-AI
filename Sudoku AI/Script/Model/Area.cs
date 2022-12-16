using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Area
    {
        #region Properties
        internal Cell[,] Cells { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }
        internal bool IsBreak { get; set; } = false;
        #endregion

        #region Methods
        //
        private void CheckBreak()
        {
            var xMin = X * W;
            var xMax = xMin + W;
            var yMin = Y * H;
            var yMax = yMin + H;
            var vals = new List<string>();
            for (var i = xMin; i < xMax; i++)
            {
                for (var j = yMin; j < yMax; j++)
                {
                    var val = Cells[i, j].Value;
                    if (vals.Contains(val))
                    {
                        IsBreak = true;
                        return;
                    }
                    vals.Add(val);
                }
            }
        }
        #endregion
    }
}
