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
        internal Cell[,] Cells { get; set; } = new Cell[MAX_W, MAX_H];
        internal Area[,] Areas { get; set; } = new Area[WB, HB];
        internal bool IsBreak { get; set; } = false;
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Constructors
        public Board(Area[,] arrAreas)
        {
            Areas = arrAreas.Copy();
            UpdCells();
        }
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
                if (!IsCompleted)
                {
                    var listCells = new List<List<Cell>>();
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
                                        // rip x
                                        var xRoot = cell.X;
                                        _rows[xRoot].ForEach(x => nums.Remove(x));
                                        NearPath(xRoot, out var xbfr, out var xAft);
                                        // rip y
                                        var yRoot = cell.Y;
                                        _cols[yRoot].ForEach(x => nums.Remove(x));
                                        NearPath(yRoot, out var yBfr, out var yAft);
                                        // exception
                                        if (nums.Count == 0)
                                        {
                                            IsBreak = true;
                                            IsCompleted = false;
                                            return;
                                        }
                                        // choosen one x
                                        if (!string.IsNullOrWhiteSpace(Cells[xRoot, yBfr].Value) && !string.IsNullOrWhiteSpace(Cells[xRoot, yAft].Value))
                                        {
                                            foreach (var num in nums)
                                            {
                                                if (_rows[xbfr].Contains(num) && _rows[xAft].Contains(num))
                                                {
                                                    nums = new List<string>()
                                                    {
                                                        num
                                                    };
                                                    break;
                                                }
                                            }
                                        }
                                        // choosen one y
                                        if (!string.IsNullOrWhiteSpace(Cells[xbfr, yRoot].Value) && !string.IsNullOrWhiteSpace(Cells[xAft, yRoot].Value))
                                        {
                                            foreach (var num in nums)
                                            {
                                                if (_cols[yBfr].Contains(num) && _cols[yAft].Contains(num))
                                                {
                                                    nums = new List<string>()
                                                    {
                                                        num
                                                    };
                                                    break;
                                                }
                                            }
                                        }
                                        // check continous
                                        var cnt = nums.Count;
                                        if (cnt == 1)
                                        {
                                            UpdAreas(new Cell
                                            {
                                                X = cell.X,
                                                Y = cell.Y,
                                                Value = nums[0]
                                            });
                                            UpdCells();
                                            Prcs();
                                        }
                                        else if (cnt > 1)
                                        {
                                            var cells = new List<Cell>();
                                            nums.ForEach(x =>
                                            {
                                                cells.Add(new Cell
                                                {
                                                    X = cell.X,
                                                    Y = cell.Y,
                                                    Value = x
                                                });
                                            });
                                            listCells.Add(cells);
                                        }
                                    }
                                }
                            }
                            index++;
                        }
                    }
                    listCells.Select(x => x).OrderBy(x => x.Count).ToList().ForEach(TrScsBoard);
                }
            }
            else
            {
                IsCompleted = false;
            }
        }

        // Near path
        private void NearPath(int coord, out int bfr, out int aft)
        {
            switch (coord % 3)
            {
                case 0:
                {
                    bfr = coord + 2;
                    aft = coord + 1;
                    break;
                }
                case 1:
                {
                    bfr = coord - 1;
                    aft = coord + 1;
                    break;
                }
                default:
                {
                    bfr = coord - 1;
                    aft = coord - 2;
                    break;
                }
            }
        }

        // Update cells
        private void UpdCells()
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
                            Cells[cell.X, cell.Y] = cell;
                        }
                    }
                }
            }
        }

        // Update areas
        private void UpdAreas(Cell cell)
        {
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            var focCell = Areas[i, j].Cells[k, l];
                            if (focCell.X == cell.X && focCell.Y == cell.Y)
                            {
                                Areas[i, j].Cells[k, l] = cell;
                            }
                        }
                    }
                }
            }
        }

        // Break check
        private void ChkBrk()
        {
            _tbls = new List<List<string>>();
            _rows = new List<List<string>>();
            _cols = new List<List<string>>();
            if (!UnitScan() || !LineScan())
            {
                IsBreak = true;
            }
        }

        // Unit scan
        private bool UnitScan()
        {
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    Areas[i, j].Prcs();
                    if (Areas[i, j].IsBreak)
                    {
                        return false;
                    }
                    else
                    {
                        IsCompleted = IsCompleted && Areas[i, j].IsCompleted;
                    }
                    _tbls.Add(Areas[i, j].ExistValues);
                }
            }
            return true;
        }

        // Line scan
        private bool LineScan()
        {
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
            return _rows.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Count() <= 0 && _cols.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Count() <= 0;
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

        // Create flow
        private Board Cr8Flow(Cell cell)
        {
            var board = Cr8Board(new Cell
            {
                X = cell.X,
                Y = cell.Y,
                Value = cell.Value
            });
            board.Prcs();
            return board.IsCompleted ? board : null;
        }

        // Trace success board
        private void TrScsBoard(List<Cell> cells)
        {
            var tasks = new List<Task<Board>>();
            cells.ForEach(x =>
            {
                var task = Run(() => Cr8Flow(x));
                tasks.Add(task);
            });
            WaitAll(tasks.ToArray());
            var cmpls = tasks.Select(z => z).Where(z => z.Result != null);
            if (cmpls.Count() > 0)
            {
                var newBoard = cmpls.First().Result;
                Areas = newBoard.Areas.Copy();
                IsCompleted = true;
            }
        }
        #endregion
    }
}
