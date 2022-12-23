using System.Collections.Generic;
using System.Linq;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Area
    {
        #region Properties
        internal List<string> ExistValues;
        internal Cell[,] Cells { get; set; }
        internal int X { get; set; } = 0;
        internal int Y { get; set; } = 0;
        internal bool IsBreak { get; set; } = false;
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Constructors
        internal Area(Cell[,] cells) => Cells = cells;
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        internal void Prcs()
        {
            GetListCells();
            ChkBrk();
            ChkCplt();
            if (!IsBreak && !IsCompleted)
            {
                LastFreeCell();
                ObviousPrs();
                Obvious3b();
                H1b();
                Reboot();
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

        // Completed check
        private void ChkCplt()
        {
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

        // Last free cell
        private void LastFreeCell() => ExistValues.ForEach(x =>
                                                {
                                                    for (var i = 0; i < WA; i++)
                                                    {
                                                        for (var j = 0; j < HA; j++)
                                                        {
                                                            Cells[i, j].AvailableValues.Remove(x);
                                                        }
                                                    }
                                                });

        // Get list available values
        private IEnumerable<List<string>> GetListAvailVas()
        {
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var availVas = Cells[i, j].AvailableValues;
                    if (availVas.Count == 2)
                    {
                        yield return availVas;
                    }
                }
            }
        }

        // Obvious pairs
        private void ObviousPrs()
        {
            var cells = GetListCells().ToList();
            var cc = cells.GroupBy(x => x.AvailableValues.Count == 2).Where(g => g.Count() > 1).SelectMany(g => g.Distinct()).ToList();
            var prs = GetListAvailVas().ToList();
            var obviousPrs = new List<List<string>>();
            var cnt = prs.Count;
            // find obvious pairs
            for (var i = 0; i < cnt; i++)
            {
                for (var j = i; j < cnt; j++)
                {
                    if (prs[i].SequenceEqual(prs[j]) && !obviousPrs.Contains(prs[i]))
                    {
                        obviousPrs.Add(prs[i]);
                    }
                }
            }
            // reboot
            obviousPrs.ForEach(x =>
            {
                for (var i = 0; i < WA; i++)
                {
                    for (var j = 0; j < HA; j++)
                    {
                        var availVas = Cells[i, j].AvailableValues;
                        if (availVas.Count > 2 && x.All(y => availVas.Contains(y)))
                        {
                            x.ForEach(y => availVas.Remove(y));
                        }
                    }
                }
            });
        }

        // Obvious triples
        private void Obvious3b()
        {
            var prs = GetListAvailVas().ToList();
            var obvious3d = new List<List<string>>();
            var cnt = prs.Count;
            // find obvious pairs
            for (var i = 0; i < cnt; i++)
            {
                for (var j = i; j < cnt; j++)
                {
                    prs[i].ForEach(x =>
                    {
                        if (prs[j].Contains(x))
                        {
                            prs[j].ForEach(y =>
                            {
                                if (y != x)
                                {
                                    for (var k = j; k < cnt; k++)
                                    {
                                        if (prs[k].Contains(y))
                                        {
                                            prs[k].ForEach(z =>
                                            {
                                                if (z == x)
                                                {
                                                    obvious3d.Add(prs[i]);
                                                    obvious3d.Add(prs[j]);
                                                    obvious3d.Add(prs[k]);
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    });
                }
            }
            // reboot
            obvious3d.ForEach(x =>
            {
                for (var i = 0; i < WA; i++)
                {
                    for (var j = 0; j < HA; j++)
                    {
                        var availVas = Cells[i, j].AvailableValues;
                        if (availVas.Count > 2 && !availVas.Except(x).Any())
                        {
                            x.ForEach(y => availVas.Remove(y));
                        }
                    }
                }
            });
        }

        // Get list cells
        private IEnumerable<Cell> GetListCells()
        {
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    yield return Cells[i, j];
                }
            }
        }

        // Hidden singles
        private void H1b()
        {
            var cells = GetListCells().ToList();
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var availVas = Cells[i, j].AvailableValues;
                    foreach (var val in availVas)
                    {
                        var isH1b = true;
                        // scan
                        foreach (var cell in cells)
                        {
                            if (!isH1b)
                            {
                                break;
                            }
                            else
                            {
                                if (cell.X != Cells[i, j].X && cell.Y != Cells[i, j].Y)
                                {
                                    foreach (var item in cell.AvailableValues)
                                    {
                                        if (item == val)
                                        {
                                            isH1b = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        // reboot
                        if (isH1b)
                        {
                            availVas = new List<string>()
                            {
                                val
                            };
                            break;
                        }
                    }
                }
            }
        }

        // Hidden pairs
        private void HPrs()
        {
            var cells = GetListCells().ToList();
            var listCpls = new List<List<(string, string)>>();
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var cpls = new List<(string, string)>();
                    var availVas = Cells[i, j].AvailableValues;
                    var availVasCnt = availVas.Count;
                    for (var k = 0; k < availVasCnt; k++)
                    {
                        for (var l = k; l < availVasCnt; l++)
                        {
                            cpls.Add((availVas[k], availVas[l]));
                        }
                    }
                    listCpls.Add(cpls);
                }
            }
            listCpls.ForEach(x =>
            {
                x.GroupBy
            });
        }

        // Update Cell
        private void Reboot()
        {
            var isUpd = false;
            // update value
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    if (Cells[i, j].AvailableValues.Count == 1 && string.IsNullOrWhiteSpace(Cells[i, j].Value))
                    {
                        isUpd = true;
                        Cells[i, j].Value = Cells[i, j].AvailableValues[0];
                    }
                }
            }
            // restart
            if (isUpd)
            {
                Prcs();
            }
        }
        #endregion
    }
}
