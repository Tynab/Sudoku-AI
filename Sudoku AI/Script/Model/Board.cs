using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sudoku_AI.Script.Constant;
using static System.Threading.Tasks.Task;

namespace Sudoku_AI.Script.Model
{
    internal class Board
    {
        #region Fields
        private bool _isBreak = false;
        #endregion

        #region Properties
        internal Area[,] Areas { get; set; }
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

        #region Constructors
        public Board(Area[,] arrAreas) => Areas = arrAreas.Copy();
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        public void Prcs()
        {
            ChkBrk();
            if (!_isBreak && !IsCompleted)
            {
                for (var i = 0; i < WB; i++)
                {
                    for (var j = 0; j < HB; j++)
                    {
                        for (var k = 0; k < WA; k++)
                        {
                            for (var l = 0; l < HA; l++)
                            {
                                var cell = Areas[i, j].Cells[k, l];
                                if (string.IsNullOrWhiteSpace(cell.Value))
                                {
                                    for (var index = 0; index < MAX_NUM; index++)
                                    {
                                        var newCell = new Cell
                                        {
                                            X = cell.X,
                                            Y = cell.Y,
                                            Value = (index + 1).ToString()
                                        };
                                        var board = Cr8Board(newCell);
                                        board.Prcs();
                                        if (board != null && board.IsCompleted)
                                        {
                                            Areas = board.Areas.Copy();
                                            IsCompleted = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Break check
        private void ChkBrk()
        {
            // areas scan
            var arr = new string[MAX_W, MAX_H];
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    Areas[i, j].Prcs();
                    if (Areas[i, j].IsBreak)
                    {
                        _isBreak = true;
                        return;
                    }
                    else
                    {
                        IsCompleted = IsCompleted && Areas[i, j].IsCompleted;
                    }
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            arr[Areas[i, j].Cells[k, l].X, Areas[i, j].Cells[k, l].Y] = Areas[i, j].Cells[k, l].Value;
                        }
                    }
                }
            }
            // line scan
            for (var i = 0; i < MAX_W; i++)
            {
                var valHs = new List<string>();
                var valVs = new List<string>();
                for (var j = 0; j < MAX_H; j++)
                {
                    valHs.Add(arr[j, i]);
                    valVs.Add(arr[i, j]);
                }
                if (valHs.Count(x => x != "") != valHs.Distinct().Count(x => x != "") || valVs.Count(x => x != "") != valVs.Distinct().Count(x => x != ""))
                {
                    _isBreak = true;
                    return;
                }
            }
        }

        // Create board
        private Board Cr8Board(Cell cell)
        {
            var arrAreas = Areas.Copy();
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            var focCell = arrAreas[i, j].Cells[k, l];
                            if (focCell.X == cell.X && focCell.Y == cell.Y)
                            {
                                arrAreas[i, j].Cells[k, l] = cell;
                                return new Board(arrAreas);
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
