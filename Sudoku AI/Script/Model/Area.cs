using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Area
    {
        #region Properties
        internal List<string> ExistValues;
        internal Cell[,] Cells { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }
        internal bool IsBreak { get; set; } = false;
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        public void Prcs()
        {
            ChkBrk();
            if (!IsBreak)
            {
                for (var i = 0; i < WA; i++)
                {
                    for (var j = 0; j < HA; j++)
                    {
                        if (string.IsNullOrWhiteSpace(Cells[i, j].Value))
                        {
                            IsCompleted = false;
                            return;
                        }
                    }
                }
            }
            else
            {
                IsCompleted = false;
            }
        }

        // Break check
        private void ChkBrk()
        {
            ExistValues = new List<string>();
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var val = Cells[i, j].Value;
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (ExistValues.Contains(val))
                        {
                            IsBreak = true;
                            return;
                        }
                        ExistValues.Add(val);
                    }
                }
            }
        }
        #endregion
    }
}
