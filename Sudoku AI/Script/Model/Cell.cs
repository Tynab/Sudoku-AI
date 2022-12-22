using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Cell
    {
        #region Properties
        internal int X { get; set; }
        internal int Y { get; set; }
        internal string Value { get; set; }
        internal List<string> Values { get; set; } = new List<string>(BASE_NUMS);
        #endregion
    }
}
