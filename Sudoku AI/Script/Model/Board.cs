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
        private List<List<string>> _rows;
        private List<List<string>> _cols;
        private List<List<string>> _tbls;
        #endregion

        #region Properties
        internal Cell[,] Cells { get; set; }
        internal Area[,] Areas { get; set; }
        internal bool IsBreak { get; set; } = false;
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Constructors
        public Board(Area[,] arrAreas)
        {
            Cells = new Cell[MAX_W, MAX_H];
            Areas = arrAreas.Copy();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        public async Task Prcs()
        {
            ChkBrk();
            if (!IsBreak && !IsCompleted)
            {
                var index = 0;
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
                                    var nums = new List<string>(BASE_NUMS);
                                    _tbls[index].ForEach(x => nums.Remove(x));
                                    _rows[cell.X].ForEach(x => nums.Remove(x));
                                    _cols[cell.Y].ForEach(x => nums.Remove(x));
                                    Parallel.ForEach(nums, async num =>
                                    {
                                        var newCell = new Cell
                                        {
                                            X = cell.X,
                                            Y = cell.Y,
                                            Value = num
                                        };
                                        var board = Cr8Board(newCell);
                                        await board.Prcs();
                                        if (board != null && board.IsCompleted)
                                        {
                                            Areas = board.Areas.Copy();
                                            IsCompleted = true;
                                            return;
                                        }
                                    });
                                }
                            }
                        }
                        index++;
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
            // areas scan
            _tbls = new List<List<string>>();
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    //
                    Areas[i, j].Prcs();
                    var area = Areas[i, j];
                    if (area.IsBreak)
                    {
                        IsBreak = true;
                        return;
                    }
                    else
                    {
                        IsCompleted = IsCompleted && area.IsCompleted;
                    }
                    _tbls.Add(area.ExistValues);
                    //
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            var cell = Areas[i, j].Cells[k, l];
                            Cells[cell.X, cell.Y] = cell;
                        }
                    }
                }
            }
            // line scan
            _rows = new List<List<string>>();
            _cols = new List<List<string>>();
            for (var i = 0; i < MAX_W; i++)
            {
                var row = new List<string>();
                var col = new List<string>();
                for (var j = 0; j < MAX_H; j++)
                {
                    var rowVal = Cells[i, j].Value;
                    if (!string.IsNullOrWhiteSpace(rowVal))
                    {
                        row.Add(rowVal);
                    }
                    var colVal = Cells[j, i].Value;
                    if (!string.IsNullOrWhiteSpace(colVal))
                    {
                        col.Add(colVal);
                    }
                }
                _rows.Add(row);
                _cols.Add(col);
            }
            if (_rows.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Count() > 0)
            {
                IsBreak = true;
                return;
            }
            if (_cols.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Count() > 0)
            {
                IsBreak = true;
                return;
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
