using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Area
    {
        #region Fields
        private bool _isBreak = false;
        #endregion

        #region Properties
        internal Cell[,] Cells { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }
        internal bool IsBreak
        {
            get => _isBreak;
            set
            {
                _isBreak = value;
                if (_isBreak)
                {
                    IsCompleted = false;
                }
            }
        }
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        public void Prcs()
        {
            ChkBrk();
            if (!_isBreak)
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
        }

        // Break check
        private void ChkBrk()
        {
            var xMin = X * WA;
            var xMax = xMin + WA;
            var yMin = Y * HA;
            var yMax = yMin + HA;
            var vals = new List<string>();
            for (var i = xMin; i < xMax; i++)
            {
                for (var j = yMin; j < yMax; j++)
                {
                    var val = Cells[i, j].Value;
                    if (!string.IsNullOrWhiteSpace(val) && vals.Contains(val))
                    {
                        _isBreak = true;
                        return;
                    }
                    vals.Add(val);
                }
            }
        }
        #endregion
    }
}
