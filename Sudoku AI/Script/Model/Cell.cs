using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Cell
    {
        #region Properties
        internal int X { get; set; } = 0;
        internal int Y { get; set; } = 0;
        internal string Value { get; set; } = string.Empty;
        internal List<string> AvailableValues { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        public void Prcs() => AvailableValues = string.IsNullOrWhiteSpace(Value) ? new List<string>(BASE_NUMS) : new List<string>();
        #endregion
    }
}
